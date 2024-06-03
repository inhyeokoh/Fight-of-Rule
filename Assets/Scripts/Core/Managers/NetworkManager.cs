using ServerCore;
using System.Net;
using System.Net.Sockets;

public enum NetState
{
    NONE = 0,
    PRE_LOGIN,
    NEED_VRF,
    VERIFIED,
    MAX
}


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

    public void Connect(string ip, int port, NetState nstate = NetState.PRE_LOGIN, Vrf? vrf = null)
    {
        mainSession = null;

        //초기화(연결설정 등등)
        //서버 ip or 도메인 설정
        IPAddress ipaddr = IPAddress.Parse(ip);
        IPEndPoint ipendpoint = new IPEndPoint(ipaddr, port);  //ipendpoint로 자동으로 ipv4 패밀리로 지정됨
        Connector connector = new Connector();                  //연결 설정만 그대로 받아서 연결
        GameManager.ThreadPool.EnqueueJob(() => { connector.Connect(ipendpoint, () => { return new ServerSession(nstate, vrf); }); });
    }
}
