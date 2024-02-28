using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
    public override void OnConnected(EndPoint endpoint)
    {
        GameManager.Network.mainSession = this;

        C_LOGIN login_ask_pkt = new C_LOGIN();
        login_ask_pkt.LoginId = "hyeok";
        login_ask_pkt.LoginPw = "123123";

/*        // 이건 로그인 버튼 클릭할때 입력 내용들을 메모리에 담고 담은걸 또 다시 login_ask_pkt에..
        login_ask_pkt.LoginId = GameManager.Data.login.id;
        login_ask_pkt.LoginPw = GameManager.Data.login.pw;*/

        //전송법 1
        //var sdata = PacketHandler.Instance.SerializePacket(login_ask_pkt);
        //Send(sdata);

        //전송법 2
        Send(PacketHandler.Instance.SerializePacket(login_ask_pkt));
    }

    public override void OnDisconnected(EndPoint endpoint)
    {
        
    }

    public override void OnPacketRecv(ArraySegment<byte> buffer)
    {
        
    }

    public override void OnSend(int bytetransferred)
    {
        
    }
}
