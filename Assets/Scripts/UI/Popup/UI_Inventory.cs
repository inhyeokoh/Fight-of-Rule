using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _invenPanel;
    public GameObject dragImg;
    List<Item> _invenItems;
    int _totalSlotCount = 30;

    enum Enum_UI_Inventory
    {
        Interact,
        Panel,
        DragImg,
        Close
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Inventory);
    }

    protected override void Init()
    {
        base.Init();
        _invenPanel = _entities[(int)Enum_UI_Inventory.Panel].gameObject;
        _invenItems = GameManager.Inven.items;

        _SetItemSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
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

        // UI위에 커서 있을 시 캐릭터 행동 제약
        _entities[(int)Enum_UI_Inventory.Panel].PointerEnterAction = (PointerEventData data) =>
        {
            GameManager.UI.PointerOnUI(true);
        };

        _entities[(int)Enum_UI_Inventory.Panel].PointerExitAction = (PointerEventData data) =>
        {
            GameManager.UI.PointerOnUI(false);
        };

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;

        gameObject.SetActive(false);
    }


    // 인벤토리 내 슬롯과 아이템 생성
    void _SetItemSlots()
    {
        int slotIndex = 0;
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + slotIndex;
            _itemSlot.GetComponent<UI_ItemSlot>().index = slotIndex;

/*            if (_invenItems[slotIndex].ItemName == null)
            {
                slotIndex++;
                continue;
            }*/

            GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage (=슬롯 오브젝트 자식 2번)
            GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text (=아이콘 오브젝트 자식 0번)

            icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            icon.GetComponent<Image>().sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[slotIndex].ItemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
            amountText.SetActive(true);
            // amountText.GetComponent<TMP_Text>().text = $"{_invenItems[slotIndex].count}";        
            slotIndex++;
        }
    }   

    public void UpdateInvenInfo(int slotIndex) // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    {
        GameObject slotInfo = _invenPanel.transform.GetChild(slotIndex).GetChild(2).gameObject; // 번호에 맞는 슬롯의 IconImg 오브젝트 
        Image iconImg= slotInfo.GetComponent<Image>();
        TMP_Text amountText = slotInfo.transform.GetChild(0).GetComponent<TMP_Text>();

        if (_invenItems[slotIndex] == null)
        {
            iconImg.sprite = null;
            iconImg.color = new Color32(12, 15, 29, 0);
            amountText.gameObject.SetActive(false);
        }
        else
        {
            iconImg.sprite = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[slotIndex].ItemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
            iconImg.color = new Color32(255, 255, 255, 255);
            amountText.gameObject.SetActive(true);
            // amountText.text = $"{(_invenItems[slotIndex].count)}"; //  해당 아이템 이름과 일치하는 수량 로드
        }
    }

    // 아이템 습득
    /*    void AddItem/Item data, int amount)
        {
            if (수량 있는 아이템) -> 동일 아이템 찾아서 수량 합산

            if (data._countable == true)
            {

            }




            else if (동일 아이템 없거나, 이미 최대수량 이거나, 수량이 없는 아이템) -> 배열의 앞부터 빈 슬롯 찾아서 넣기
        }*/

}