using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// warning
/// 상위 클래스에 접근이 가능하도록 설계했지만
/// 공통 기능을 가지고, 공유해야하는 클래스는 최대한 상위 클래스 접근을 금지
/// 따라서 Mount 함수는 필수 호출이 아님(비 추상함수)
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SubClass<T> : ISubClass where T : class
{
    protected T _board;
    public List<ISubClass> SubClasses = new List<ISubClass>();

    /// <summary>
    /// T1: T
    /// if(T1 is not T) assert!
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <param name="board">upper class</param>
    public void Mount<T1>(T1 board)
    {
        if(board is not T)
        {
#if UNITY_EDITOR
            Debug.Log($"SubClass {WhoIAm()}.Mount() Error. T1 is not T");
            Debug.Assert(false);
#endif
        }

        _board = board as T;
        for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Mount(this);
    }

    public System.Type WhoIAm()
    {
        return GetType();
    }

    /// <summary>
    /// sub: subClass
    /// </summary>
    /// <param name="sub"></param>
    public void Connect(ISubClass sub)
    {
        sub.Mount(this);
        SubClasses.Add(sub);
    }

    public Action GetAction()
    {
        return () => { Excute(); };
    }

    //초기화
    public void Init() { _Init(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Init(); }
    //실행
    public void Excute() { _Excute(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Excute(); }
    //정리
    public void Clear() { _Clear(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Clear(); }


    /*======================
     * 
     *     반드시 정의
     * 
    =======================*/
    protected abstract void _Init();
    protected abstract void _Excute();
    protected abstract void _Clear();
}
