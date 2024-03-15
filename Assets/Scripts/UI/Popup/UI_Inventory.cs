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
    public ItemBase[] items; // 인벤 아이템 배열

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
        _GetInvenDataFromDB();
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
    }

    //로컬로부터 내 인벤 아이템 정보 가져와서 배열에 집어넣음
    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line
        items = new ItemBase[_totalSlotCount + 1]; // 마지막 칸은 아이템 스위칭용
        for (int i = 0; i < lines.Length; i++) // 배열 0번부터 차례로 넣음
        {
            string[] row = lines[i].Split('\t'); // Tab 기준으로 나누기           
            // 아이템 집어 넣기
            if (row[0] == "Equipment")
            {
                items[i] = new InGameItemEquipment();
            }
            else if (row[0] == "Consumption")
            {
                items[i] = new InGameItemConsumption();
                //items[i] = new InGameItemDuraction();
            }
            else
            {
                // Etc
            }
            // items[i].itemID
            // items[i].itemType =
            items[i].itemName = row[1];
            items[i].itemDescription = row[2];
            items[i].count = Convert.ToInt32(row[3]);
            // Debug.Log($"아이템 명: {items[i].itemName}, 아이템 수량: {items[i].count}");
        }        
    }

    // 인벤토리 내 슬롯과 아이템 생성
    void _SetItemSlots()
    {
        int slotIndex = 0;
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + slotIndex;

            if (items[slotIndex].itemName == null)
            {
                slotIndex++;
                continue;
            }

            GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage (=슬롯 오브젝트 자식 2번)
            GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text (=아이콘 오브젝트 자식 0번)

            icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            icon.GetComponent<Image>().sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
            amountText.SetActive(true);
            amountText.GetComponent<TMP_Text>().text = $"{items[slotIndex].count}";        
            slotIndex++;
        }
    }   


    public void SwitchItems(int a, int b)
    {        
        items[_totalSlotCount] = items[b];
        items[b] = items[a];
        items[a] = items[_totalSlotCount];
    }

    public bool CheckItemType(int a, int b)
    {
        if (items[a].itemType == items[b].itemType)
        {
            return true;
        }

        return false;
    }


    public void AddUpItems(int a, int b)
    {
        if (items[a].count <= 100)
        {

        }
    }


    public void UpdateInvenInfo(int slotIndex) // 아이템 배열 정보에 맞게 갱신 시키는 메서드 사용.
    {
        GameObject slotInfo = _invenPanel.transform.GetChild(slotIndex).GetChild(2).gameObject; // 번호에 맞는 슬롯의 IconImg 오브젝트 
        Image iconImg= slotInfo.GetComponent<Image>();
        TMP_Text amountText = slotInfo.transform.GetChild(0).GetComponent<TMP_Text>();

        if (items[slotIndex] == null)
        {
            iconImg.sprite = null;
            iconImg.color = new Color32(12, 15, 29, 0);
            amountText.gameObject.SetActive(false);
        }
        else
        {
            iconImg.sprite = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
            iconImg.color = new Color32(255, 255, 255, 255);
            amountText.gameObject.SetActive(true);
            amountText.text = $"{(items[slotIndex].count)}"; //  해당 아이템 이름과 일치하는 수량 로드
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