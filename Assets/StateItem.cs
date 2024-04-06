using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateItem : State
{
    Action<int, int, string> action = (a, b, c) => { };   

    public StateItem(Action enterAction, Action/*<int, int>*/ stayAction, Action/*<int, int> */fixedAction, Action exitAction) :  base(enterAction, stayAction, fixedAction, exitAction)
    {

    }


    public void Enter(int item, int iteme, string ti)
    {
        action.Invoke(item,iteme, ti);
    }
}
