using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _content;
    public GameObject dragImg;
    TMP_Text[] togNames;
    Toggle[] toggles;

    List<Item> _items;
    int _totalSlotCount;

    enum Enum_UI_Inventory
    {
        Interact,
        Panel,
        Panel_U,
        ScrollView,
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
        _content = _entities[(int)Enum_UI_Inventory.ScrollView].transform.GetChild(0).GetChild(0).gameObject; // Content 담기는 오브젝트
        togNames = _entities[(int)Enum_UI_Inventory.Panel_U].GetComponentsInChildren<TMP_Text>();
        toggles = _entities[(int)Enum_UI_Inventory.Panel_U].GetComponentsInChildren<Toggle>();
        _items = GameManager.Inven.items;
        _totalSlotCount = GameManager.Inven.totalSlot;

        _SetPanel_U();
        _DrawSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
            };

            // UI위에 커서 있을 시 캐릭터 행동 제약
            _subUI.PointerEnterAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(true);
            };

            _subUI.PointerExitAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(false);
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

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;               
        gameObject.SetActive(false);
    }

    void _SetPanel_U()
    {
        togNames[0].text = "All";
        togNames[1].text = "Equipment";
        togNames[2].text = "Consumption";
        togNames[3].text = "Material";
        togNames[4].text = "Etc";

        toggles[0].onValueChanged.AddListener((value) => _ToggleValueChanged(value, "All"));
        for (int i = 1; i < toggles.Length; i++) // 전체보기는 제외
        {
            string typeName = togNames[i].text;
            toggles[i].onValueChanged.AddListener((value) => _ToggleValueChanged(value, typeName));
        }
    }

    void _ToggleValueChanged(bool value, string typeName)
    {
        if (value)
        {
            if (typeName == "All")
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i] == null)
                    {
                        continue;
                    }
                    UI_ItemSlot slot = _content.transform.GetChild(i).GetComponent<UI_ItemSlot>();
                    slot.RenderBright();
                }
            }
            else
            {
                _RenderByType(typeName);
            }
        }
    }

    // 해당 아이템은 색 밝게, 나머지는 약간 어둡게
    void _RenderByType(string typeName)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null)
            {
                continue;
            }

            UI_ItemSlot slot = _content.transform.GetChild(i).GetComponent<UI_ItemSlot>();
            // Debug.Log($"{i}번째 아이템  {_items[i].Type} , {typeName}");
            if (_items[i].Type != typeName) // 다른 타입은 어둡게 그리기
            {
                slot.RenderDark();
            }
            else
            {
                slot.RenderBright();
            }
        }
    }


    // 인벤토리 내 슬롯 번호에 맞게 아이템 배치
    void _DrawSlots()
    {
        // 슬롯 생성
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _content.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().index = i;
        }
    }

    public void UpdateInvenInfo(int slotIndex) // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    {
        UI_ItemSlot slot = _content.transform.GetChild(slotIndex).GetComponent<UI_ItemSlot>();
        slot.ItemRender();
    }

    // TODO : 인벤 확장

    void _ExtendSlot(int newSlot = 5)
    {
        for (int i = _totalSlotCount; i < _totalSlotCount + newSlot; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _content.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().index = i;
        }
        GameManager.Inven.totalSlot += newSlot;
        _totalSlotCount = GameManager.Inven.totalSlot;
        GameManager.Inven.ExtendItemList();
    }
}