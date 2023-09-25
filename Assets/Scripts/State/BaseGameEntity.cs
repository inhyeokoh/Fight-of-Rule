using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameEntity : MonoBehaviour
{
    //�÷��̾� �г���
    private string ninkName;
    //�÷��̾� ���� ��ȣ
    private static int ID;



    public virtual void Setup(string name)
    {
        ninkName = name;
        ++ID;
    }



    public abstract void FixedUpdated();

    /// <summary>
    /// GameController�� �ִ� ��� Ŭ������ ������Ʈ�� ����
    /// </summary>
    public abstract void Updated();

    /// <summary>
    /// �ϴ� �׽�Ʈ�� �� ���� �ؽ�Ʈ ���
    /// </summary>
    public void Print(string text)
    {
        print($"{name}�� : {text}  ");
    }

}
