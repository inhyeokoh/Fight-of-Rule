using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemConsumption : ItemParents, IPointerClickHandler
{
    // 상태패턴을 받고 나서 작업하는게 편할 것.
    private void Start()
    {
        // 기본세팅을 바탕으로 hp, mp등을 조작할 수 있게끔
        BasicSettings();   
    }

    private float lastClickTime = 0f;
    private float doubleClickDelay = 0.3f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickDelay)
        {
            DoubleClickAction();
        }
        lastClickTime = Time.time;
    }

    public override void DoubleClickAction()
    {
        PlayerStats.Inst.hp += 10;

    }

    protected override void Use()
    {
        
    }

}
