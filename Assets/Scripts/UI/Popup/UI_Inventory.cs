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

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;
    }


    // 인벤토리 내 슬롯 생성
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
                    GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage(=슬롯 오브젝트 자식 2번)
                    GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text 활성화(=아이콘 오브젝트 자식 0번)

                    icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    icon.GetComponent<Image>().sprite
                        = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // 해당 아이템 이름과 일치하는 이미지 로드
                    amountText.SetActive(true);
                    amountText.GetComponent<TMP_Text>().text = $"{items[slotIndex].itemNumber}";

                    slotIndex++;
                }
                else
                {
                    slotIndex++;
                }
            }
        }
    }

    // 로컬이나 서버로부터 내 인벤 아이템 정보 가져오기
    void GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line 수
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

    void SaveInvenData()
    {
        GameManager.Data.SaveData("Inven", items);
    }

    void LoadInvenData()
    {
        string data = GameManager.Data.LoadData("Inven");
        items = JsonUtility.FromJson<Item[]>(data);
    }

    void DrageItem()
    {
    
    
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