using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemParents : MonoBehaviour
{
    protected string toolTip;
    protected int coolTime;



    public void BasicSettings()
    {
        // player�� stat�� �����ϴ� ��ũ��Ʈ�� ������ �ͼ� stat ���� ������ �����ϰԲ�
        // �⺻ ������ ���ִ� �޼ҵ�.
    }
    public abstract void DoubleClickAction();

    // ����� ���� - �������� ��� ����ǵ��� �� ���ΰ�??
    // Ư�� ī�װ��� �������� �����, �����͸� ������ ������ �ѹ��� �� �� �ֵ��� ������ �Ǿ�� ��.
    protected abstract void Use();

    protected void ToolTip(string toolTip)
    {
        Debug.Log(toolTip);
    }

    // HP, �ɷ�ġ ����� ������ �� �ִ� �������� ȿ������ ��Ƶ�.
    public abstract class ItemEffect { }

    public void CoolTime(float coolTime)
    {

    }

}
