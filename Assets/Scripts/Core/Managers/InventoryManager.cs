using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public List<Item> items; // 아이템
    UI_Inventory _inven;

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
        
    }

    protected override void _Init()
    {
        _GetInvenDataFromDB();
    }

    //로컬로부터 내 인벤 아이템 정보 가져와서 인벤 리스트에 집어넣음
    void _GetInvenDataFromDB()
    {
        items = new List<Item>();
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line
        for (int i = 0; i < lines.Length; i++) // 배열 0번부터 차례로 넣음
        {
            string[] row = lines[i].Split('\t'); // Tab 기준으로 나누기           
            // 아이템 집어 넣기
            if (row[0] == "Equipment")
            {
                // items[i] = new InGameItemEquipment();
            }
            else if (row[0] == "Consumption")
            {
                // items[i] = new InGameItemConsumption();
                //items[i] = new InGameItemDuraction();
            }
            else
            {
                // Etc
            }
            // items[i].itemID
            // items[i].itemType =
/*            items[i].itemName = row[1];
            items[i].itemDescription = row[2];
            items[i].count = Convert.ToInt32(row[3]);*/
            // Debug.Log($"아이템 명: {items[i].itemName}, 아이템 수량: {items[i].count}");
        }
    }

    public void DragAndDropItems(int a, int b)
    {
        // 같은 아이템이면 앞에꺼에 수량 합치기, 다른 아이템이면 위치 교환
        if (_CheckItemIsSame(a, b))
        {
            _AddUpItems(a, b);
        }
        else
        {
            _SwitchItems(a, b); // 아이템 배열 스위칭
        }
        _inven.UpdateInvenInfo(a);
        _inven.UpdateInvenInfo(b);
    }

    void _SwitchItems(int a, int b)
    {
        items[items.Count] = items[b];
        items[b] = items[a];
        items[a] = items[items.Count];
    }

    bool _CheckItemIsSame(int a, int b)
    {
        if (items[a].ItemId == items[b].ItemId)
        {
            return true;
        }

        return false;
    }


    void _AddUpItems(int a, int b)
    {
/*        if (items[a] <= 100)
        {
            items[a].count + items[b].count;
        }*/
    }
}
