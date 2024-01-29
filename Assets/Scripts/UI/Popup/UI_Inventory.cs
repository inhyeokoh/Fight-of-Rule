using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _invenPanel;
    GameObject _itemSlot;     // ���� ������
    // Item[] _items;

    int _slotCountHor = 6;  // ���� ���� ��
    int _slotCountVer = 5;  // ���� ���� ��
    int _slotIndex;



    enum Enum_UI_Inventory
    {
        Interact,
        Panel,
        Close
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Inventory);
    }

    protected override void Init()
    {
        base.Init();
        _invenPanel = _entities[(int)Enum_UI_Inventory.Panel].gameObject;
        SetItemSlots();

        // _items = new Item[_slotCountHor * _slotCountVer];

        UI_ItemSlot[] itemSlot = _entities[(int)Enum_UI_Inventory.Panel].GetComponentsInChildren<UI_ItemSlot>();
        Debug.Log(itemSlot.Length);

        // �κ��丮 â �巡��
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;
        };

        // �κ��丮 �ݱ�
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            Debug.Log("�κ� �ݱ�");
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };
    }

    // �κ��丮 �� ���� ����
    private void SetItemSlots()
    {
        for (int i = 0; i < _slotCountVer; i++)
        {
            for (int j = 0; j < _slotCountHor; j++)
            {
                _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            }
        }
    }

    // ������ ����
/*    void AddItem/Item data, int amount)
    {
        if (���� �ִ� ������) -> ���� ������ ã�Ƽ� ���� �ջ�

        if (data._countable == true)
        {

        }




        else if (���� ������ ���ų�, �̹� �ִ���� �̰ų�, ������ ���� ������) -> �迭�� �պ��� �� ���� ã�Ƽ� �ֱ�
    }*/

}