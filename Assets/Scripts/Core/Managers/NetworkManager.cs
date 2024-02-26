using ServerCore;
using System.Net;
using System.Net.Sockets;

public class NetworkManager : SubClass<GameManager>
{
    static readonly string SERVER_IP = "211.105.26.250";
    static readonly int SERVER_POTR = 28889;

    protected override void _Init()
    {
        //�ʱ�ȭ(���ἳ�� ���)
        //���� ip or ������ ����
        IPAddress ipaddr = IPAddress.Parse(SERVER_IP);
        IPEndPoint ipendpoint = new IPEndPoint(ipaddr, SERVER_POTR);  //ipendpoint�� �ڵ����� ipv4 �йи��� ������
        Connector connector = new Connector();                  //���� ������ �״�� �޾Ƽ� ����
        GameManager.ThreadPool.EnqueueJob(this, () => { connector.Connect(ipendpoint, () => { return new ServerSession(); }); });
    }
    protected override void _Excute()
    {
    }

    protected override void _Clear()
    {
    }
}
