using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;

namespace ServerCore
{
    public class Listener
    {
        // Ŭ���̾�Ʈ�� ������ ó���� ����
        Socket listenSocket;

        Action<Socket> onAcceptHandler;

        // ������ ���� �ϴ� ���� ����
        public void Init(IPEndPoint endPoint, Action<Socket> _onAcceptHandler)
        {
            listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // TCP �������ݷ� ����
            onAcceptHandler += _onAcceptHandler;

            listenSocket.Bind(endPoint); // ���������� ��ġ ���

            listenSocket.Listen(10); // backlog�� �ִ�� ��� ������ ��

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted); // callback ������� OnAcceptCompleted ����
            RegisterAccept(args);
        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null; // �ʱ�ȭ �� ���ָ� ������ ���� ��

            bool pending = listenSocket.AcceptAsync(args); // �񵿱�
            if (pending == false)  // �ٷ� Ȯ�ε�����
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

            // ���� �������� ������ �� �ٽ� ���
            RegisterAccept(args);
        }


        public Socket Accept()
        {
            return listenSocket.Accept();
        }
    }

}

