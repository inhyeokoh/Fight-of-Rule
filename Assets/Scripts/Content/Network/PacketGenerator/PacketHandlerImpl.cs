using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketHandlerImpl : MonoBehaviour
{

    internal static bool Handle_S_OPTION(Session session, S_OPTION message)
    {
        throw new NotImplementedException();
    }

    internal static bool Handle_S_LOGIN(Session session, S_LOGIN message)
    {
        if(false == message.LoginSuccess)
        {
            //경우에 따라서 재로그인 유도 (지금은 그냥 return)
            Debug.Log("로그인 실패");
            return false;
        }


        var feild_list = message.Slots;

        if(feild_list.Count == 0)
        {
            //슬롯이 없음 => 신규 유저
            //신규유저 로직 처리 ( 경우에 따라 다른 패킷을 전송해야 할 수 있음)

            //TODO
            return true;
        }

        for(int i = 0; i < feild_list.Count; i++)
        {
            var slot = feild_list[i];
            //대충 슬롯 순회하면서 유저 정보 처리
        }

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        throw new NotImplementedException();
    }
}
