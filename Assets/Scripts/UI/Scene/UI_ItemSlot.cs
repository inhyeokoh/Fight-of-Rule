using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Entity
{
    public Image iconImg;
    // GameObject _dragTemp;
    // Image drag;
    Image _highlightImg;

    enum Enum_UI_ItemSlot
    {
        IconImg,
        HighlightImg
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_ItemSlot);
    }

    protected override void Init()
    {
        base.Init();

        iconImg = _entities[(int)Enum_UI_ItemSlot.IconImg].GetComponent<Image>();
        _highlightImg = _entities[(int)Enum_UI_ItemSlot.HighlightImg].GetComponent<Image>();

        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
/*            _dragTemp = new GameObject();
            drag = _dragTemp.GetComponent<Image>();
            drag = iconImg;*/
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            // drag.rectTransform.position = data.position;
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            //_dragTemp = null;
            // ���� ���콺 �����Ϳ� ��ġ�� ����ĭ �̹���
            Image b =  data.pointerCurrentRaycast.gameObject.GetComponent<Image>();
            Sprite temp = iconImg.sprite; // temp�� �ӽ� ����
            iconImg.sprite = b.sprite;
            b.sprite = temp;
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
        };

        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
        };
    }

    void Swap(Image a, Image b)
    {
        // if ���� �̹���(or ������)��� ���� ��ġ��
        // else ��ġ��ȯ    
    }

}

// ��ĭ�� �Ű����°� ����

// A B C
// A B
// C = A
// A = B
// B = C