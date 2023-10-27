using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

namespace ServerCore
{
    public class Listener
    {
        // 클라이언트의 접속을 처리할 소켓
        Socket listenSocket;

        Action<Socket> onAcceptHandler;

        // 문지기 역할 하는 소켓 생성
        public void Init(IPEndPoint endPoint, Action<Socket> _onAcceptHandler)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // TCP 프로토콜로 설정
            onAcceptHandler += _onAcceptHandler;

            listenSocket.Bind(endPoint); // 문지기한테 위치 등록

            listenSocket.Listen(10); // backlog는 최대로 대기 가능한 수

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted); // callback 방식으로 OnAcceptCompleted 실행
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null; // 초기화 안 해주면 이전의 값이 들어감

            bool pending = listenSocket.AcceptAsync(args); // 비동기
            if (pending == false)  // 바로 확인됐을때
            {
                OnAcceptCompleted(null, args);
            }
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // TODO
                onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log(args.SocketError.ToString());
#endif
            }

            // 위의 과정들이 끝나면 또 다시 등록
            RegisterAccept(args);
        }


        public Socket Accept()
        {
            return listenSocket.Accept();
        }
    }

}

