using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemConsumption : ItemParents, IPointerClickHandler
{
    // ���������� �ް� ���� �۾��ϴ°� ���� ��.
    private void Start()
    {
        // �⺻������ �������� hp, mp���� ������ �� �ְԲ�
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
