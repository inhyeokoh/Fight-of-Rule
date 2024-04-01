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
    GameObject _amountText;
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
        _amountText = _iconImg.transform.GetChild(0).gameObject;

        _inven = transform.GetComponentInParent<UI_Inventory>();
        _invenItems = GameManager.Inven.items;
        _dragImg = _inven.dragImg;

        ItemRender();

        //드래그 시작
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;  // 드래그 이미지를 현재 이미지로
            }
        };

        //드래그 중
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _dragImg.transform.position = data.position;
            }
        };

        //드래그 끝
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull() && CheckCorrectDrop(data)) // 드래그 드롭한 오브젝트가 슬롯이어야함
            {
                _otherIndex = data.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_ItemSlot>().index;
                GameManager.Inven.DragAndDropItems(index, _otherIndex);
            }
            _dragImg.SetActive(false);
        };

        // 슬롯 하이라이트
        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
            }
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
            }
        };
    }

    // 슬롯 번호에 맞게 아이템 그리기
    public void ItemRender()
    {
        if (_invenItems[index] != null)
        {
            _iconImg.color = new Color32(255, 255, 255, 255);
            _iconImg.sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[index].ItemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
            _amountText.SetActive(true);
            _amountText.GetComponent<TMP_Text>().text = $"{_invenItems[index].Count}";            
        }
        else
        {
            _iconImg.sprite = null;
            _iconImg.color = new Color32(12, 15, 29, 0);
            _amountText.gameObject.SetActive(false);
        }
    }
    public void RenderBright()
    {
        _iconImg.color = new Color32(255, 255, 255, 255);
    }

    public void RenderDark()
    {
        _iconImg.color = new Color32(50, 50, 50, 255);
    }

    bool CheckItemNull()
    {
        return _iconImg.sprite == null;
    }


    // 드롭 시 슬롯에 벗어나지 않았는지 확인
    bool CheckCorrectDrop(PointerEventData data)
    {
        return data.pointerCurrentRaycast.gameObject.name == "IconImg";
    }
}
