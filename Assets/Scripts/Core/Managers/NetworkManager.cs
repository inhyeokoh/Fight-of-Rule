using ServerCore;
using System.Net;
using System.Net.Sockets;

public class NetworkManager : SubClass<GameManager>
{
    static readonly string SERVER_IP = "211.105.26.250";
    static readonly int SERVER_POTR = 28889;

    public ServerSession mainSession = null;

    protected override void _Init()
    {
        Connect(SERVER_IP, SERVER_POTR);
    }
    protected override void _Excute()
    {
    }

    protected override void _Clear()
    {
    }

    public void Connect(string ip, int port)
    {
        mainSession = null;

        //�ʱ�ȭ(���ἳ�� ���)
        //���� ip or ������ ����
        IPAddress ipaddr = IPAddress.Parse(ip);
        IPEndPoint ipendpoint = new IPEndPoint(ipaddr, port);  //ipendpoint�� �ڵ����� ipv4 �йи��� ������
        Connector connector = new Connector();                  //���� ������ �״�� �޾Ƽ� ����
        GameManager.ThreadPool.EnqueueJob(() => { connector.Connect(ipendpoint, () => { return new ServerSession(); }); });
    }
}
