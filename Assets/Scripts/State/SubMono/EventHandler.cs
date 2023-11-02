using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : SubMono<PlayerController>
{

    public class WarriorEventHandler
    {
        //워리어의 애니메이션 이벤트 콜백

    }

    public class ArcherEventHandler
    {
        //아처의 애니메이션 이벤트 콜백
    }
    
    public class WizardEventHandler
    {
        //마법사의 애니메이션 이벤트 콜백
    }
    //Event를 통해 callback함수 대부분 애니메이션 이벤트에다 넣기 위해 만든 클래스
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