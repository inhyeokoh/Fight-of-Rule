using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubMono<T> : MonoBehaviour, ISubClass where T : class
{
    T _board;
    public List<ISubClass> SubClasses = new List<ISubClass>();

    public void Mount<T1>(T1 board)
    {
        if (board is not T)
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

    //�ʱ�ȭ
    public void Init() { _Init(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Init(); }
    //����
    public void Excute() { _Excute(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Excute(); }
    //����
    public void Clear() { _Clear(); for (int i = 0; i < SubClasses.Count; i++) SubClasses[i].Clear(); }


    /*======================
     * 
     *     �ݵ�� ����
     * 
    =======================*/
    protected abstract void _Init();
    protected abstract void _Excute();
    protected abstract void _Clear();
}