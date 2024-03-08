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

    // ���� ����
    Image _iconImg;
    int _currentSlotIndex;
    GameObject _amountText;

    // ��ӽ� ��ġ�� ����
    Image b;
    int _otherSlotIndex;
    GameObject _amountTextB;

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
        _amountText = _entities[(int)Enum_UI_ItemSlot.IconImg].transform.GetChild(0).gameObject;

        //�巡�� ����
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null)
            {
                _dragImg = _inven.dragImg;
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null)
            {
                _dragImg.transform.position = data.position;
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (_inven.items[_currentSlotIndex] != null) // �巡�� ����� ������Ʈ�� �����̾����
            {
                if (CheckCorrectDrop(data))
                {
                    _otherSlotIndex = GetSlotIndex(data.pointerCurrentRaycast.gameObject.transform.parent.name);
                    _inven.SwitchItem(_currentSlotIndex, _otherSlotIndex); // ������ �迭 ����Ī
                    _inven.UpdateInvenInfo(_currentSlotIndex);
                    _inven.UpdateInvenInfo(_otherSlotIndex);
                }

                _dragImg.SetActive(false);
            }
        };

        // ���� ���̶���Ʈ
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
            b = data.pointerCurrentRaycast.gameObject.GetComponent<Image>(); // ���� ���콺 �����Ϳ� ��ġ�� ����ĭ �̹���
            _amountTextB = data.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject; // ����� ���� Amount Text
            return true;
        }
        return false;
    }

    void Swap(Image a, Image b)
    {
        // if ���� �̹���(or ������)��� ���� ��ġ��
        // else ��ġ��ȯ    
    }
}
