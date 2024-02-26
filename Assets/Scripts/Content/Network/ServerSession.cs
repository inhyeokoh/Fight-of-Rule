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

        //傈价过 1
        //var sdata = PacketHandler.Instance.SerializePacket(login_ask_pkt);
        //Send(sdata);

        //傈价过 2
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
