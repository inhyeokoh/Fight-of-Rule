using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : UI_Entity
{
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
        

        ItemRender();

        //드래그 시작
        _entities[(int)Enum_UI_ItemSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _inven.dragImg.SetActive(true);
                _inven.dragImg.GetComponent<Image>().sprite = _iconImg.sprite;  // 드래그 이미지를 현재 이미지로
            }
        };

        //드래그 중
        _entities[(int)Enum_UI_ItemSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _inven.dragImg.transform.position = data.position;
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
            _inven.dragImg.SetActive(false);
        };

        // 커서가 들어오면 아이템 설명 이미지 띄우기 + 하이라이트 효과
        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _inven.descrPanel.SetActive(true);
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
                ShowItemInfo();
                _inven.RestrictItemDescrPos();
            }
        };

        // 커서가 나갔을때 아이템 설명 내리기 + 하이라이트 효과 끄기
        _entities[(int)Enum_UI_ItemSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
                _inven.descrPanel.SetActive(false);
                _inven.StopRestrictItemDescrPos(data);
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

    void ShowItemInfo()
    {
        _inven.descrPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = GameManager.Inven.items[index].ItemName; // 아이템 이름
        _inven.descrPanel.transform.GetChild(1).GetComponent<Image>().sprite = _iconImg.sprite; // 아이콘 이미지
        _inven.descrPanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = GameManager.Inven.items[index].ItemDescription; // 아이템 설명

        // TODO 장비 아이템일 경우 추가 비교 이미지
    }
}
