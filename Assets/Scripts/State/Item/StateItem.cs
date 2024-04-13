using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateItem
{
    Action<StateItemData> enterAction;
    Action<StateItemData> fixedstayAction;
    Action<StateItemData> stayAction;
    Action<StateItemData> exitAction;

    public StateItem(Action<StateItemData> enterAction, Action<StateItemData> fixedstayAction, Action<StateItemData> stayAction, Action<StateItemData> exitAction)
    {
        this.enterAction = enterAction;
        this.fixedstayAction = fixedstayAction;
        this.stayAction = stayAction;
        this.exitAction = exitAction;
    }

    // 굳이 생성자에 넣지말고 그냥 메서드를 하나 만들어서 매개변수를 넣을까 그냥 그래도 될거같은데
    public void Enter(StateItemData item)
    {
        enterAction?.Invoke(item);
    }
    
    public void FixedStay(StateItemData item)
    {
        fixedstayAction?.Invoke(item);   
    }

    public void Stay(StateItemData item)
    {
        stayAction?.Invoke(item);
    }

    public void Exit(StateItemData item)
    {
        exitAction?.Invoke(item);
    }
}
