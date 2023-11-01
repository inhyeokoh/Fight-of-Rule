using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : SubMono<PlayerController>
{
    //Event∏¶ ≈Î«ÿ callback
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
        throw new System.NotImplementedException();
    }

    protected override void _Excute()
    {
        throw new System.NotImplementedException();
    }

    protected override void _Clear()
    {
        throw new System.NotImplementedException();
    }
}