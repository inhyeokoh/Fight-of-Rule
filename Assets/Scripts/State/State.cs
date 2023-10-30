using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class State//<T> where T : class
{
    //상태를 확인하고 상태를 호출시켜주는 대리자 변수들
    Action _enterAction;
    Action _stayAction;
    Action _fixedStayAction;
    Action _exitAction;

    //처음에 객체 생성을 할때 위에있는 Action 상태들을 할당하기 위해 파라미터를 넣어줌
    public State(Action enterAction, Action fixedStayAction, Action stayAction, Action exitAction)
    {
        _enterAction = enterAction;
        _fixedStayAction = fixedStayAction;
        _stayAction = stayAction;
        _exitAction = exitAction;
    }

    

    /// <summary>
    /// 이 클래스에 들어왔을때 한번 호출
    /// </summary>
    public virtual void Enter()
    {
        _enterAction?.Invoke();
    }
  
    /// <summary>
    /// Fixed프레임 마다 호출
    /// </summary>
    public virtual void FixedStay()
    {
        _fixedStayAction?.Invoke();
    }

    /// <summary>
    /// 프레임마다 호출
    /// </summary>
    public virtual void Stay()
    {
        _stayAction?.Invoke();
    }
    /// <summary>
    /// 이 클래스에서 나왔을때 한번 호출
    /// </summary>
    public virtual void Exit()
    {
        _exitAction?.Invoke();
    }
    
}
