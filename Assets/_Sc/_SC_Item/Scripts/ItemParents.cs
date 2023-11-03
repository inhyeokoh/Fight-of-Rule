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
        // player의 stat을 조작하는 스크립트를 가지고 와서 stat 관련 조작이 가능하게끔
        // 기본 세팅을 해주는 메소드.
    }
    public abstract void DoubleClickAction();

    // 고민인 지점 - 아이템이 어떻게 적용되도록 할 것인가??
    // 특정 카테고리의 아이템을 만들고, 데이터를 정리해 놓으면 한번에 들어갈 수 있도록 정리가 되어야 함.
    protected abstract void Use();

    protected void ToolTip(string toolTip)
    {
        Debug.Log(toolTip);
    }

    // HP, 능력치 등등을 변경할 수 있는 아이템의 효과들을 담아둠.
    public abstract class ItemEffect { }

    public void CoolTime(float coolTime)
    {

    }

}
