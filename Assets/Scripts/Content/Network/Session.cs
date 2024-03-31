using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

public abstract class PacketSession : Session
{
    public static readonly ushort HeaderLen = 2; //2bytes
    public abstract void OnPacketRecv(ArraySegment<byte> buffer);

    public override sealed int OnRecv(ArraySegment<byte> buffer)
    {

        int processedLen = 0;
        while (true)
        {
            if (buffer.Count < HeaderLen)   //최소한 헤더사이즈 만큼은 있어야 검증이 가능
                break;

            int dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);

            if (dataSize > buffer.Count)
                break;

            //복사하면 thread 분리 가능 => 지금은 그냥 사용 (OnPacketRecv에서 고민)
            OnPacketRecv(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
            processedLen += dataSize;
            buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
        }

        return processedLen;
    }
}


public abstract class Session
{
    public abstract void OnConnected(EndPoint endpoint);
    public abstract void OnDisconnected(EndPoint endpoint);
    public abstract int OnRecv(ArraySegment<byte> buffer);
    public abstract void OnSend(int bytetransferred);

    static int BUFSIZE = (int)Math.Pow(2, sizeof(ushort) * 8) - 1;
    RecvBuffer _recvBuffer = new RecvBuffer(BUFSIZE);

    protected Socket _socket;
    int disconnected = 0;

    SocketAsyncEventArgs _sendargs = new SocketAsyncEventArgs();
    SocketAsyncEventArgs _recvargs = new SocketAsyncEventArgs();
    Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
    List<ArraySegment<byte>> PendingList = new List<ArraySegment<byte>>();

    object _sendlock = new object();
    //object _recvlock = new object();

    void DisconnectedClear()
    {
        _sendQueue.Clear();
        PendingList.Clear();
    }

    public void Start(Socket socket)
    {
        _socket = socket;
        //비동기 송신
        _sendargs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
        //비동기 수신
        _recvargs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

        //1개만 등록
        GameManager.ThreadPool.EnqueueJob(() => { RecvRegister(_recvargs); });
    }
    public void Send(ArraySegment<byte> buffer)
    {
        GameManager.ThreadPool.EnqueueJob(() => { _Send(buffer); });
    }
    public void Send(List<ArraySegment<byte>> sendingList)
    {
        GameManager.ThreadPool.EnqueueJob(() => { _Send(sendingList); });
    }

    void _Send(ArraySegment<byte> buffer)
    {
        if (buffer == null)
            return;

        lock(_sendlock)
        {
            _sendQueue.Enqueue(buffer);
        }

        SendRegister();
    }

    void _Send(List<ArraySegment<byte>> sendingList) //모아쏘기
    {
        if (sendingList.Count == 0) return;     //PendingList에 아무거도 없게 하지 않기 위해.

        lock (_sendlock)
        {
            for (int i = 0; i < sendingList.Count; i++)
                _sendQueue.Enqueue(sendingList[i]);
        }

        SendRegister();
    }

    void SendRegister()
    {
        if (disconnected == 1) 
            return;

        lock (_sendlock)
        {
            //전송중이거나, 전송할게 없는 경우
            if (_sendQueue.Count == 0 || PendingList.Count > 0)
                return;

            while (_sendQueue.Count > 0)
                PendingList.Add(_sendQueue.Dequeue()); 

            _sendargs.BufferList = PendingList; //반드시 복사로 넘겨줘야 고장안남
        }

        try    //전송중 Disconnect일 경우 처리
        {
            bool pending = _socket.SendAsync(_sendargs);//비동기 송신
            if (pending == false)
                OnSendCompleted(null, _sendargs);
        }
        catch (Exception ex)     /*지금은 콘솔에다 그냥 출력하지만 오류 내용은 파일에다가 출력하는게 좋다.*/
        {
            _PrintLog($"Socket handle Failed. Exception : {ex.Message} \n {ex.StackTrace}");
        }
    }


    //iocp thread가 물었을 가능성이 있으므로, 다시 스레드로 전달(stack overflow방지)
    void OnSendCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
        {
            try
            {

                OnSend(args.BytesTransferred);
                args.BufferList = null;     //굳이 안해도 되는데 깔끔하게 하려고

                lock( _sendlock)
                {
                    PendingList.Clear();    //초기화
                }

                GameManager.ThreadPool.EnqueueJob(() => { SendRegister(); }); //재등록
            }
            catch (Exception ex)
            {
                _PrintLog("OnSendCompleted");
                _PrintLog(ex.StackTrace);
            }
        }
        else
        {
            Disconnect();
        }
    }
    #region Network(Receive)
    //단 하나만 Receive를 수행한다고 생각해보자.
    void RecvRegister(SocketAsyncEventArgs args)
    {
        if (disconnected > 0) return;

        _recvBuffer.Clear();    //일단 초기화
        ArraySegment<byte> segment = _recvBuffer.RecvSegment;   //가용 세그먼트 참조 반환
        args.SetBuffer(segment.Array, segment.Offset, segment.Count);

        try
        {
            bool pending = _socket.ReceiveAsync(args);  //비동기 수신.

            if (pending == false)      //바로 수신데이터가 있다면
                OnRecvCompleted(null, args);
        }
        catch (Exception ex)
        {
            _PrintLog($"Socket handle Failed. Exception : {ex.StackTrace}");
        }
    }

    void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
    {

        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)   //0보다 큰 데이터가 왔는가(유효한가). 연결해제등의 요청은 0이므로 유효한거만 추출
        {
            //OnRecv 패킷을 분석해서 어떤종류의 데이터인지 확인. 그에맞는 작업시작
            //만약 의미없는 패킷들이 몰려서 오거나
            //디도스 공격처럼 작정하고 서버를 부수려고 하면 그를 거르는 작업이 필요하다.
            try
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false) //비정상적이라면(일반적이라면 절대 안일어남)
                {
                    Disconnect();   //종료
                    return;
                }

                //컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지를 받는다.
                int processLen = OnRecv(_recvBuffer.DataSegment);
                if (processLen < 0)
                {
                    Disconnect();
                    return;
                }

                //Read커서 이동
                if (_recvBuffer.OnRead(processLen) == false) //제대로 안옮겨졌으면.
                {
                    Disconnect();
                    return;
                }
                    
                GameManager.ThreadPool.EnqueueJob(() => { RecvRegister(args); });
            }
            catch (Exception ex)    //유효하지 않은경우( Disconect 와 같은 메세지를 받은 경우)
            {
                _PrintLog($"OnRecvCompleteRecursive {ex.Message}" + "\n" + ex.StackTrace);
                Disconnect();
            }

        }
        else
        {
            _PrintLog("0 byte recv");
            _PrintLog(args.BytesTransferred + args.SocketError.ToString());
            _PrintLog(_recvBuffer.DataSegment.Count + "\t" + _recvBuffer.RecvSegment.Count);

            Disconnect();
        }
    }
    #endregion

    public void Disconnect()
    {
        if (Interlocked.Exchange(ref disconnected, 1) == 1)
            return;

        OnDisconnected(_socket.RemoteEndPoint);
        _socket.Shutdown(SocketShutdown.Both);  //FIN & FINACK
        _socket.Close();                        //END

        DisconnectedClear();
    }

    void _PrintLog(string msg)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.Log(msg);
#endif
    }
}