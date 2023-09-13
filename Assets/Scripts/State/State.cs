using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State<T>  where T : class
{
    /// <summary>
    /// �� Ŭ������ �������� �ѹ� ȣ��
    /// </summary>
    public abstract void Enter(T entity);

    /// <summary>
    /// �����Ӹ��� ȣ��
    /// </summary>
    public abstract void Stay(T entity);

    /// <summary>
    /// Fixed������ ���� ȣ��
    /// </summary>
    public abstract void FixedStay(T entity);

    /// <summary>
    /// �� Ŭ�������� �������� �ѹ� ȣ��
    /// </summary>
    public abstract void Exit(T entity);
}
