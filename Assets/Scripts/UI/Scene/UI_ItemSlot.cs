using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Entity
{
    GameObject _dragImg;
    Image _highlightImg;
    UI_Inventory _inven;

    // 현재 슬롯
    Image _iconImg;
    int _currentSlotIndex;

    // 드롭 시 위치한 슬롯
    int _otherSlotIndex;

    enum Enum_UI_ItemSlot
    {
        SlotImg,
        HighlightImg,
        IconImg
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_ItemSlot);
    }

    protected override void Init()
    {
        base.Init();

        _iconImg = _entities[(int)Enum_UI_ItemSlot.IconImg].GetComponent<Image>();
        _highlightImg = _entities[(int)Enum_UI_ItemSlot.HighlightImg].GetComponent<Image>();
        _currentSlotIndex = GetSlotIndex(gameObject.name);
        _inven = transform.GetComponentInParent<UI_Inventory>();

        //드래그 시작
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null)
            {
                _dragImg = _inven.dragImg;
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;
            }
        };

        //드래그 중
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null)
            {
                _dragImg.transform.position = data.position;
            }
        };

        //드래그 끝
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null && CheckCorrectDrop(data)) // 드래그 드롭한 오브젝트가 슬롯이어야함
            {
                _otherSlotIndex = GetSlotIndex(data.pointerCurrentRaycast.gameObject.transform.parent.name);
                // 같은 아이템이면 앞에꺼에 수량 합치기, 다른 아이템이면 위치 교환
                if (_inven.CheckItemType(_currentSlotIndex, _otherSlotIndex))
                {
                    _inven.AddUpItems(_currentSlotIndex, _otherSlotIndex);
                }
                else
                {
                    _inven.SwitchItems(_currentSlotIndex, _otherSlotIndex); // 아이템 배열 스위칭
                }
                _inven.UpdateInvenInfo(_currentSlotIndex);
                _inven.UpdateInvenInfo(_otherSlotIndex);
            }
            _dragImg.SetActive(false);
        };

        // 슬롯 하이라이트
        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
            if (_inven.items[_currentSlotIndex] != null)
            {
                Debug.Log(_inven.items[_currentSlotIndex].itemName);
            }
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
        };
    }

    int GetSlotIndex(string name)
    {
        string[] objName = name.Split('_');
        return Convert.ToInt32(objName[1]);
    }

    bool CheckCorrectDrop(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject.name == "IconImg")
        {
            return true;
        }
        return false;
    }
}
