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
    public Item[] items; // 인벤 아이템 배열

    int _slotCountHor = 6;  // 슬롯 가로 수
    int _slotCountVer = 5;  // 슬롯 세로 수

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
        GetInvenDataFromDB();
        SetItemSlots();

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


    // 인벤토리 내 슬롯과 아이템 생성
    private void SetItemSlots()
    {
        int slotIndex = 0;
        for (int i = 0; i < _slotCountVer; i++)
        {
            for (int j = 0; j < _slotCountHor; j++)
            {
                GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
                _itemSlot.name = "ItemSlot_" + slotIndex;
                if (items[slotIndex] != null)
                {
                    GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage (=슬롯 오브젝트 자식 2번)
                    GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text (=아이콘 오브젝트 자식 0번)

                    icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    icon.GetComponent<Image>().sprite
                        = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
                    amountText.SetActive(true);
                    amountText.GetComponent<TMP_Text>().text = $"{items[slotIndex].itemNumber}";
                }

                slotIndex++;
            }
        }
    }   

    //로컬로부터 내 인벤 아이템 정보 가져와서 배열에 집어넣음
    void GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line
        items = new Item[_slotCountHor * _slotCountVer + 1]; // 마지막 칸은 아이템 스위칭용
        for (int i = 0; i < lines.Length; i++) // 배열 0번부터 차례로 넣음
        {
            string[] row = lines[i].Split('\t'); // Tab 기준으로 나누기
            items[i] = new Item(row[0], row[1], row[2], row[3], row[4] == "TRUE");
        }        
    }

    public void SwitchItem(int a, int b)
    {        
        items[_slotCountHor * _slotCountVer] = items[b];
        items[b] = items[a];
        items[a] = items[_slotCountHor * _slotCountVer];
    }

    public void UpdateInvenInfo(int slotIndex) // 초기에는 교환할 두 아이템의 배열 재정령 후 이미지와 수량을 서로 교체하는 방식이 더 적은 로직을 사용하여 간단해 보였지만, 확장성 면에서 아이템 합산시에는 재사용이 어려워서 아이템 배열 정보에 맞게 갱신 시키는 메서드 사용.
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
            amountText.text = $"{items[slotIndex].itemNumber}"; //  해당 아이템 이름과 일치하는 수량 로드
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