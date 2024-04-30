using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : UI_Entity
{
    UI_PlayerInfo _playerInfoUI;
    Image _highlightImg;

    // 현재 슬롯
    Image _iconImg;
    public int index;

    // 드롭 시 위치한 슬롯
    int _otherIndex;

    enum Enum_UI_EquipSlot
    {
        SlotImg,
        HighlightImg,
        IconImg
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_EquipSlot);
    }

    protected override void Init()
    {
        base.Init();
        _iconImg = _entities[(int)Enum_UI_EquipSlot.IconImg].GetComponent<Image>();
        _highlightImg = _entities[(int)Enum_UI_EquipSlot.HighlightImg].GetComponent<Image>();
        _playerInfoUI = transform.GetComponentInParent<UI_PlayerInfo>();
        
        //드래그 시작
        _entities[(int)Enum_UI_EquipSlot.IconImg].BeginDragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _playerInfoUI.dragImg.SetActive(true);
                _playerInfoUI.dragImg.GetComponent<Image>().sprite = _iconImg.sprite;  // 드래그 이미지를 현재 이미지로
            }
        };

        //드래그 중
        _entities[(int)Enum_UI_EquipSlot.IconImg].DragAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _playerInfoUI.dragImg.transform.position = data.position;
            }
        };

        //드래그 끝
        _entities[(int)Enum_UI_EquipSlot.IconImg].EndDragAction = (PointerEventData data) =>
        {
            if (CheckItemNull())
            {
                return;
            }

            if (CheckSlotDrop(data)) // 드롭한 위치가 장비 슬롯
            {
                _otherIndex = data.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<UI_ItemSlot>().index;
                GameManager.Inven.DragAndDropItems(index, _otherIndex);
            }
            // 플레이어 정보 UI 밖에 드롭할 경우
            else if (CheckSceneDrop(data))
            {
                if (true)  // 드롭한 위치가 인벤 슬롯
                {

                }
                else
                {
                    // 버릴지 되묻는 팝업
/*                    _playerInfoUI.dropConfirmPanel.SetActive(true);
                    _playerInfoUI.dropConfirmPanel.transform.GetChild(0).GetComponent<UI_DropConfirm>().ChangeText(index);*/
                }
            }


            _playerInfoUI.dragImg.SetActive(false);
        };

        // 커서가 들어오면 아이템 설명 이미지 띄우기 + 하이라이트 효과
        _entities[(int)Enum_UI_EquipSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _playerInfoUI.descrPanel.SetActive(true);
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0.4f);
                ShowItemInfo();
                // _playerInfoUI.RestrictItemDescrPos();
            }
        };

        // 커서가 나갔을때 아이템 설명 내리기 + 하이라이트 효과 끄기
        _entities[(int)Enum_UI_EquipSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
                _playerInfoUI.descrPanel.SetActive(false);
                // _playerInfoUI.StopRestrictItemDescrPos(data);
            }
        };

        // 우클릭으로 아이템 장착
        _entities[(int)Enum_UI_EquipSlot.IconImg].ClickAction = (PointerEventData data) =>
        {
            if (CheckItemNull())
            {
                return;
            }

            if (data.button == PointerEventData.InputButton.Right && GameManager.Inven.equips[index].ItemType == Item.Enum_ItemType.Equipment) // 장비에 우클릭 한 경우
            {
                // TODO 장착 불가 경우

                GameManager.Inven.UnEquipItem(index);
            }
        };

        if (index == GameManager.Inven.equipSlotCount - 1)
        {
            _playerInfoUI.gameObject.SetActive(false);
        }
    }

    // 슬롯 번호에 맞게 아이템 그리기
    public void ItemRender()
    {
        if (GameManager.Inven.equips[index] != null)
        {
            _iconImg.color = new Color32(255, 255, 255, 255);
            _iconImg.sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{GameManager.Inven.equips[index].ItemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
        }
        else
        {
            _iconImg.sprite = null;
            _iconImg.color = new Color32(12, 15, 29, 0);
            _highlightImg.color = new Color(_highlightImg.color.r, _highlightImg.color.g, _highlightImg.color.b, 0f);
            _playerInfoUI.descrPanel.SetActive(false);
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
        return GameManager.Inven.equips[index] == null;
    }

    bool CheckSceneDrop(PointerEventData data)
    {
        _playerInfoUI.GetUIPos();

/*        if (data.position.x < _playerInfoUI. || data.position.y < _playerInfoUI.invenUI_leftBottom.y ||
            data.position.x > _playerInfoUI.invenUI_rightTop.x || data.position.y > _playerInfoUI.invenUI_rightTop.y)
        {
            return true;
        }*/

        return false;
    }

    // 드롭 시 슬롯에 벗어나지 않았는지 확인
    bool CheckSlotDrop(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject == null)
        {
            return false;
        }

        return data.pointerCurrentRaycast.gameObject.name == "IconImg";
    }

    void ShowItemInfo()
    {
        _playerInfoUI.descrPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = GameManager.Inven.equips[index].ItemName; // 아이템 이름
        _playerInfoUI.descrPanel.transform.GetChild(1).GetComponent<Image>().sprite = _iconImg.sprite; // 아이콘 이미지
        _playerInfoUI.descrPanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = GameManager.Inven.equips[index].ItemDescription; // 아이템 설명

        // TODO 장비 아이템일 경우 추가 비교 이미지
    }
}
