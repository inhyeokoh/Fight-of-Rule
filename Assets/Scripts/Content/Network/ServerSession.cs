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

        GameManager.ThreadPool.UniAsyncJob(() => { GameManager.Resources.Instantiate("Prefabs/UI/Popup/UI_Login", GameObject.Find("Canvas").transform); });


        //傈价过 1
        //var sdata = PacketHandler.Instance.SerializePacket(login_ask_pkt);
        //Send(sdata);

        //傈价过 2
        //Send(PacketHandler.Instance.SerializePacket(login_ask_pkt));
    }

    public override void OnDisconnected(EndPoint endpoint)
    {
        
    }

    public override void OnPacketRecv(ArraySegment<byte> buffer)
    {
        PacketHandler.Instance.HandlePacket(this, buffer);
    }

    public override void OnSend(int bytetransferred)
    {
        
    }
}
