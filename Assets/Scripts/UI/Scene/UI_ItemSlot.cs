using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Entity
{
    GameObject _dragImg;
    Image _highlightImg;
    UI_Inventory _inven;
    List<Item> _invenItems;

    // 현재 슬롯
    Image _iconImg;
    public int index;

    // 드롭 시 위치한 슬롯
    int _otherIndex;

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
        _inven = transform.GetComponentInParent<UI_Inventory>();
        _invenItems = GameManager.Inven.items;
        _dragImg = _inven.dragImg;

        //드래그 시작
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null)
            {
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;
            }
        };

        //드래그 중
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null)
            {
                _dragImg.transform.position = data.position;
            }
        };

        //드래그 끝
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null && CheckCorrectDrop(data)) // 드래그 드롭한 오브젝트가 슬롯이어야함
            {
                _otherIndex = data.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_ItemSlot>().index;
                GameManager.Inven.DragAndDropItems(index, _otherIndex);
            }
            _dragImg.SetActive(false);
        };

        // 슬롯 하이라이트
        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
        };
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
