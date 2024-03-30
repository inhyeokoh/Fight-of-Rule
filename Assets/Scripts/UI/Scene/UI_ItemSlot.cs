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
    GameObject _amountText;
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
        _amountText = _iconImg.transform.GetChild(0).gameObject;

        _inven = transform.GetComponentInParent<UI_Inventory>();
        _invenItems = GameManager.Inven.items;
        _dragImg = _inven.dragImg;

        _ItemRender();

        //�巡�� ����
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _dragImg.SetActive(true);
                _dragImg.GetComponent<Image>().sprite = _iconImg.sprite;  // �巡�� �̹����� ���� �̹�����
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _dragImg.transform.position = data.position;
            }
        };

        //�巡�� ��
        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull() && CheckCorrectDrop(data)) // �巡�� ����� ������Ʈ�� �����̾����
            {
                _otherIndex = data.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_ItemSlot>().index;
                GameManager.Inven.DragAndDropItems(index, _otherIndex);
            }
            _dragImg.SetActive(false);
        };

        // ���� ���̶���Ʈ
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

    // ���� ��ȣ�� �°� ������ �׸���
    public void _ItemRender()
    {
        if (_invenItems[index] != null)
        {
            _iconImg.color = new Color32(255, 255, 255, 255);
            _iconImg.sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[index].ItemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
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

    bool CheckItemNull()
    {
        return _iconImg.sprite == null;
    }


    // ��� �� ���Կ� ����� �ʾҴ��� Ȯ��
    bool CheckCorrectDrop(PointerEventData data)
    {
        return data.pointerCurrentRaycast.gameObject.name == "IconImg";
    }
}
