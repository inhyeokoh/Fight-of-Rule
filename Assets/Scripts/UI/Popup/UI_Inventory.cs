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
    List<Item> _items;
    int _totalSlotCount;

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

    private void OnDisable()
    {
        GameManager.UI.PointerOnUI(false);
    }

    protected override void Init()
    {
        base.Init();
        _invenPanel = _entities[(int)Enum_UI_Inventory.Panel].gameObject;
        _items = GameManager.Inven.items;
        _totalSlotCount = GameManager.Inven.totalSlot;

        _DrawItemSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
            };
        }

        // 인벤토리 창 드래그
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;
        };

        // 인벤토리 닫기
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };

        // UI위에 커서 있을 시 캐릭터 행동 제약
        _entities[(int)Enum_UI_Inventory.Panel].PointerEnterAction = (PointerEventData data) =>
        {
            Debug.Log("ui 위");
            GameManager.UI.PointerOnUI(true);
        };

        _entities[(int)Enum_UI_Inventory.Panel].PointerExitAction = (PointerEventData data) =>
        {
            Debug.Log("ui 밖");
            GameManager.UI.PointerOnUI(false);
        };

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;               
        gameObject.SetActive(false);
    }


    // 인벤토리 내 슬롯 번호에 맞게 아이템 배치
    void _DrawItemSlots()
    {
        // 슬롯 생성
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().index = i;
        }
    }

    public void UpdateInvenInfo(int slotIndex) // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    {
        UI_ItemSlot slot = _invenPanel.transform.GetChild(slotIndex).GetComponent<UI_ItemSlot>();
        slot._ItemRender();
    }

    // TODO : 인벤 확장

    void _ExtendSlot(int newSlot = 5)
    {
        for (int i = _totalSlotCount; i < _totalSlotCount + newSlot; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().index = i;
        }
        GameManager.Inven.totalSlot += newSlot;
        _totalSlotCount = GameManager.Inven.totalSlot;
        GameManager.Inven.ExtendItemList();
    }
}