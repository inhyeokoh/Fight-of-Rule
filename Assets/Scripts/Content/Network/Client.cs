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
                IPAddress ipAddr = ipHost.AddressList[0]; // ip 주소가 여러개 있을 수 있으니 첫번째
                IPEndPoint endPoint = new IPEndPoint(ipAddr, 7000);

                Connector connector = new Connector();

                // connector.Connect(endPoint, () => { return new GameSession(); });

                // 연결 문의
                socket.Connect(endPoint);
                Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");

                // 보낼 내용
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World");
                int sendBytes = socket.Send(sendBuff);

                // 받을 내용
                byte[] recvBuff = new byte[1024]; // 어느 정도의 크기가 들어올지 모르므로 1024로 설정
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