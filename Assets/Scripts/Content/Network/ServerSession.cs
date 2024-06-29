using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;


public struct Vrf
{
    public Vrf(string ip, int port, long unique_id, string token)
    {
        this.ip = ip;
        this.port = port;
        this.unique_id = unique_id;
        this.token = token;

        this._verified = false;
    }

    public void Verifying() { _verified = true; }
    public bool Verified() { return this._verified; }

    private bool _verified;
    public string ip;
    public int port;
    public long unique_id;
    public string token;
}

public class ServerSession : PacketSession
{
    public NetState nowstate { get; private set; } = NetState.NONE;
    Vrf? _vrf = null;

    public ServerSession(NetState nowstate, Vrf? vrf = null)
    {
        this.nowstate = nowstate;
        this._vrf = vrf;
    }

    
    public override void OnConnected(EndPoint endpoint)
    {
        switch(nowstate)
        {
            case NetState.NONE:
                break;
            case NetState.PRE_LOGIN:
                GameManager.Network.mainSession = this;
                GameManager.ThreadPool.UniAsyncJob(() => { GameManager.UI.OpenPopup(GameManager.UI.Login); });
                break;
            case NetState.NEED_VRF:
                GameManager.Network.mainSession = this;
                var ret = Utils.Dynamic_Assert(_vrf != null, "no vrf for vrf-session");
                if(ret == false)
                    Disconnect();

                C_VERIFYING vrf = new C_VERIFYING();
                vrf.Token =  _vrf?.token;
                if(_vrf != null)
                    vrf.Uid = _vrf.Value.unique_id;

                GameManager.Network.Send(PacketHandler.Instance.SerializePacket(vrf));
                break;
            default:
                break;
        }
        


        //전송법 1
        //var sdata = PacketHandler.Instance.SerializePacket(login_ask_pkt);
        //Send(sdata);

        //전송법 2
        //Send(PacketHandler.Instance.SerializePacket(login_ask_pkt));
    }

    public override void OnDisconnected(EndPoint endpoint)
    {
        
    }

    public override void OnPacketRecv(ArraySegment<byte> buffer)
    {
        GameManager.Network.Recv(buffer);
        PacketHandler.Instance.HandlePacket(this, buffer);
    }

    public override void OnSend(int bytetransferred)
    {
    }
}
