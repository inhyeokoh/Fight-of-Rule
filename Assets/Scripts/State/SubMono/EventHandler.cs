using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : SubMono<PlayerController>
{

    //Event�� ���� callback�Լ� ��κ� �ִϸ��̼� �̺�Ʈ���� �ֱ� ���� ���� Ŭ����
    public void OnStateEvent(int stateId)
    {
       // _board.EventDistributor(stateId);
    }

    public void OnChangeState(int stateId)
    {
       // _board.StateDistributor(stateId);
    }

    protected override void _Init()
    {
       
    }

    protected override void _Excute()
    {
       
    }

    protected override void _Clear()
    {
       
    }
}