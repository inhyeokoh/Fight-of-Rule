using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace ServerCore
{
    public class Client : MonoBehaviour
    {
        static void Main(string[] args)
        {
/*            try
            {
                string host = Dns.GetHostName();
                IPHostEntry ipHost = Dns.GetHostEntry(host);
                IPAddress ipAddr = ipHost.AddressList[0]; // ip �ּҰ� ������ ���� �� ������ ù��°
                IPEndPoint endPoint = new IPEndPoint(ipAddr, 7000);

                Connector connector = new Connector();

                // connector.Connect(endPoint, () => { return new GameSession(); });

                // ���� ����
                socket.Connect(endPoint);
                Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

                // ���� ����
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World");
                int sendBytes = socket.Send(sendBuff);

                // ���� ����
                byte[] recvBuff = new byte[1024]; // ��� ������ ũ�Ⱑ ������ �𸣹Ƿ� 1024�� ����
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"[From Server] {recvData}");

                socket.Close();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log(e.ToString());
#endif
            }*/

        }
    }
}