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

    // ���� ����
    Image _iconImg;
    public int index;

    // ��� �� ��ġ�� ����
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

        //�巡�� ����
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null)
            {
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null)
            {
                _dragImg.transform.position = data.position;
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (_invenItems[index] != null && CheckCorrectDrop(data)) // �巡�� ����� ������Ʈ�� �����̾����
            {
                _otherIndex = data.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_ItemSlot>().index;
                GameManager.Inven.DragAndDropItems(index, _otherIndex);
            }
            _dragImg.SetActive(false);
        };

        // ���� ���̶���Ʈ
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
