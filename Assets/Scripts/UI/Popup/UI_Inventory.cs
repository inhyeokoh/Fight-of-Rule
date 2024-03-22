using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _invenPanel;
    public GameObject dragImg;
    List<Item> _invenItems;
    int _totalSlotCount = 30;

    enum Enum_UI_Inventory
    {
        Interact,
        Panel,
        DragImg,
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
        _invenItems = GameManager.Inven.items;

        _DrawItemSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
            };
        }

        // �κ��丮 â �巡��
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;
        };

        // �κ��丮 �ݱ�
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };

        // UI���� Ŀ�� ���� �� ĳ���� �ൿ ����
        _entities[(int)Enum_UI_Inventory.Panel].PointerEnterAction = (PointerEventData data) =>
        {
            GameManager.UI.PointerOnUI(true);
        };

        _entities[(int)Enum_UI_Inventory.Panel].PointerExitAction = (PointerEventData data) =>
        {
            GameManager.UI.PointerOnUI(false);
        };

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;

        gameObject.SetActive(false);
    }


    // �κ��丮 �� ���� ��ȣ�� �°� ������ ��ġ
    void _DrawItemSlots()
    {
        // ���� ����
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().index = i;
        }
    }

    public void UpdateInvenInfo(int slotIndex) // ������ �迭 ������ �°� UI ���� ��Ű�� �޼���
    {
        UI_ItemSlot slot = _invenPanel.transform.GetChild(slotIndex).GetComponent<UI_ItemSlot>();
        slot._ItemRender();
    }
}