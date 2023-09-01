
public class NetworkManager : SubClass<GameManager>
{
    protected override void _Init()
    {
        //초기화(연결설정 등등)
    }

    protected override void _Excute()
    {
        //Update와 같이 매 프레임 해야하는 작업
    }

    protected override void _Clear()
    {
        //자원 정리
    }
}
