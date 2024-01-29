using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _invenPanel;
    GameObject _itemSlot;     // 슬롯 프리팹
    // Item[] _items;

    int _slotCountHor = 6;  // 슬롯 가로 수
    int _slotCountVer = 5;  // 슬롯 세로 수
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

        // 인벤토리 창 드래그
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;
        };

        // 인벤토리 닫기
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            Debug.Log("인벤 닫기");
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };
    }

    // 인벤토리 내 슬롯 생성
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

    // 아이템 습득
/*    void AddItem/Item data, int amount)
    {
        if (수량 있는 아이템) -> 동일 아이템 찾아서 수량 합산

        if (data._countable == true)
        {

        }




        else if (동일 아이템 없거나, 이미 최대수량 이거나, 수량이 없는 아이템) -> 배열의 앞부터 빈 슬롯 찾아서 넣기
    }*/

}