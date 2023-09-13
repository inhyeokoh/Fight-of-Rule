using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State<T>  where T : class
{
    /// <summary>
    /// 이 클래스에 들어왔을때 한번 호출
    /// </summary>
    public abstract void Enter(T entity);

    /// <summary>
    /// 프레임마다 호출
    /// </summary>
    public abstract void Stay(T entity);

    /// <summary>
    /// Fixed프레임 마다 호출
    /// </summary>
    public abstract void FixedStay(T entity);

    /// <summary>
    /// 이 클래스에서 나왔을때 한번 호출
    /// </summary>
    public abstract void Exit(T entity);
}
