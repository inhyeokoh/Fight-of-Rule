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
            //��쿡 ���� ��α��� ���� (������ �׳� return)
            Debug.Log("�α��� ����");
            return false;
        }


        var feild_list = message.Slots;

        if(feild_list.Count == 0)
        {
            //������ ���� => �ű� ����
            //�ű����� ���� ó�� ( ��쿡 ���� �ٸ� ��Ŷ�� �����ؾ� �� �� ����)

            //TODO
            return true;
        }

        for(int i = 0; i < feild_list.Count; i++)
        {
            var slot = feild_list[i];
            //���� ���� ��ȸ�ϸ鼭 ���� ���� ó��
        }

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        throw new NotImplementedException();
    }
}
