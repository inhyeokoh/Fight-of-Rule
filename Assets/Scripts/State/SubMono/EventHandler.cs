using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : SubMono<PlayerController>
{

    public class WarriorEventHandler
    {
        //�������� �ִϸ��̼� �̺�Ʈ �ݹ�

    }

    public class ArcherEventHandler
    {
        //��ó�� �ִϸ��̼� �̺�Ʈ �ݹ�
    }
    
    public class WizardEventHandler
    {
        //�������� �ִϸ��̼� �̺�Ʈ �ݹ�
    }
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