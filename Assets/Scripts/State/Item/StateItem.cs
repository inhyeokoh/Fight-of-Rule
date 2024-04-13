using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateItem
{
    // 어렵게 생각하지 말고 아이템 스테이트와 아이템 스테이트 머신을 따로 분류해서 만들었음
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
