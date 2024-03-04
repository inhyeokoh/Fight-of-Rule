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
            if (buffer.Count < HeaderLen)   //�ּ��� ��������� ��ŭ�� �־�� ������ ����
                break;

            int dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);

            if (dataSize > buffer.Count)
                break;

            //�����ϸ� thread �и� ���� => ������ �׳� ��� (OnPacketRecv���� ���)
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
        //�񵿱� �۽�
        _sendargs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
        //�񵿱� ����
        _recvargs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

        //1���� ���
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

    void _Send(List<ArraySegment<byte>> sendingList) //��ƽ��
    {
        if (sendingList.Count == 0) return;     //PendingList�� �ƹ��ŵ� ���� ���� �ʱ� ����.

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
            //�������̰ų�, �����Ұ� ���� ���
            if (_sendQueue.Count == 0 || PendingList.Count > 0)
                return;

            while (_sendQueue.Count > 0)
                PendingList.Add(_sendQueue.Dequeue()); 

            _sendargs.BufferList = PendingList; //�ݵ�� ����� �Ѱ���� ����ȳ�
        }

        try    //������ Disconnect�� ��� ó��
        {
            bool pending = _socket.SendAsync(_sendargs);//�񵿱� �۽�
            if (pending == false)
                OnSendCompleted(null, _sendargs);
        }
        catch (Exception ex)     /*������ �ֿܼ��� �׳� ��������� ���� ������ ���Ͽ��ٰ� ����ϴ°� ����.*/
        {
            _PrintLog($"Socket handle Failed. Exception : {ex.Message} \n {ex.StackTrace}");
        }
    }


    //iocp thread�� ������ ���ɼ��� �����Ƿ�, �ٽ� ������� ����(stack overflow����)
    void OnSendCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
        {
            try
            {

                OnSend(args.BytesTransferred);
                args.BufferList = null;     //���� ���ص� �Ǵµ� ����ϰ� �Ϸ���

                lock( _sendlock)
                {
                    PendingList.Clear();    //�ʱ�ȭ
                }

                GameManager.ThreadPool.EnqueueJob(() => { SendRegister(); }); //����
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
    //�� �ϳ��� Receive�� �����Ѵٰ� �����غ���.
    void RecvRegister(SocketAsyncEventArgs args)
    {
        if (disconnected > 0) return;

        _recvBuffer.Clear();    //�ϴ� �ʱ�ȭ
        ArraySegment<byte> segment = _recvBuffer.RecvSegment;   //���� ���׸�Ʈ ���� ��ȯ
        args.SetBuffer(segment.Array, segment.Offset, segment.Count);

        try
        {
            bool pending = _socket.ReceiveAsync(args);  //�񵿱� ����.

            if (pending == false)      //�ٷ� ���ŵ����Ͱ� �ִٸ�
                OnRecvCompleted(null, args);
        }
        catch (Exception ex)
        {
            _PrintLog($"Socket handle Failed. Exception : {ex.StackTrace}");
        }
    }

    void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
    {

        if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)   //0���� ū �����Ͱ� �Դ°�(��ȿ�Ѱ�). ������������ ��û�� 0�̹Ƿ� ��ȿ�ѰŸ� ����
        {
            //OnRecv ��Ŷ�� �м��ؼ� ������� ���������� Ȯ��. �׿��´� �۾�����
            //���� �ǹ̾��� ��Ŷ���� ������ ���ų�
            //�𵵽� ����ó�� �����ϰ� ������ �μ����� �ϸ� �׸� �Ÿ��� �۾��� �ʿ��ϴ�.
            try
            {
                if (_recvBuffer.OnWrite(args.BytesTransferred) == false) //���������̶��(�Ϲ����̶�� ���� ���Ͼ)
                {
                    Disconnect();   //����
                    return;
                }

                //������ ������ �����͸� �Ѱ��ְ� �󸶳� ó���ߴ����� �޴´�.
                int processLen = OnRecv(_recvBuffer.DataSegment);
                if (processLen < 0)
                {
                    Disconnect();
                    return;
                }

                //ReadĿ�� �̵�
                if (_recvBuffer.OnRead(processLen) == false) //����� �ȿŰ�������.
                {
                    Disconnect();
                    return;
                }
                    
                GameManager.ThreadPool.EnqueueJob(() => { RecvRegister(args); });
            }
            catch (Exception ex)    //��ȿ���� �������( Disconect �� ���� �޼����� ���� ���)
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