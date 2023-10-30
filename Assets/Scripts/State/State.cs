using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class State//<T> where T : class
{
    //���¸� Ȯ���ϰ� ���¸� ȣ������ִ� �븮�� ������
    Action _enterAction;
    Action _stayAction;
    Action _fixedStayAction;
    Action _exitAction;

    //ó���� ��ü ������ �Ҷ� �����ִ� Action ���µ��� �Ҵ��ϱ� ���� �Ķ���͸� �־���
    public State(Action enterAction, Action fixedStayAction, Action stayAction, Action exitAction)
    {
        _enterAction = enterAction;
        _fixedStayAction = fixedStayAction;
        _stayAction = stayAction;
        _exitAction = exitAction;
    }

    

    /// <summary>
    /// �� Ŭ������ �������� �ѹ� ȣ��
    /// </summary>
    public virtual void Enter()
    {
        _enterAction?.Invoke();
    }
  
    /// <summary>
    /// Fixed������ ���� ȣ��
    /// </summary>
    public virtual void FixedStay()
    {
        _fixedStayAction?.Invoke();
    }

    /// <summary>
    /// �����Ӹ��� ȣ��
    /// </summary>
    public virtual void Stay()
    {
        _stayAction?.Invoke();
    }
    /// <summary>
    /// �� Ŭ�������� �������� �ѹ� ȣ��
    /// </summary>
    public virtual void Exit()
    {
        _exitAction?.Invoke();
    }
    
}
