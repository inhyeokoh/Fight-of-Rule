using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public int totalSlot = 30;
    public List<Item> items; // slot index에 따른 아이템 리스트

    UI_Inventory _inven;
    protected override void _Clear()
    {        
    }

    protected override void _Excute()
    {        
    }

    protected override void _Init()
    {
    }

    //로컬로부터 내 인벤 아이템 정보 가져와서 인벤 리스트에 집어넣음
    public void ConnectInvenUI()
    {
        items = new List<Item>(new Item[totalSlot]);

        _inven = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_Inventory>();
        _GetInvenDataFromDB();
    }

    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line

        for (int i = 0; i < lines.Length; i++) // 0번부터 차례로 넣음
        {
            string[] row = lines[i].Split('\t'); // Tab 기준으로 나누기
            // 아이템 집어 넣기
            items[Convert.ToInt32(row[7])] = new Item(null, Convert.ToInt32(row[0]), row[2], row[3], 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), row[6] == "TRUE", Convert.ToInt32(row[7]));
        }
    }

    public void ExtendItemList()
    {
        while (items.Count <= totalSlot)
        {
            items.Add(null);
        }
    }

    public void DragAndDropItems(int oldPos, int newPos)
    {
        if (oldPos == newPos) return;

        if (items[newPos] == null) // newPos가 비어있는 슬롯이면 옮기기
        {
            _ChangeSlotNum(oldPos, newPos);
        }
        // 셀 수 있는 아이템이고 같은 아이템이면 수량 합치기
        else if (_CheckSameAndCountable(oldPos, newPos))
        {
            _AddUpItems(oldPos, newPos);
        }
        else  // 다른 아이템이면 위치 교환
        {
            _ExchangeSlotNum(oldPos, newPos);
        }
        _inven.UpdateInvenInfo(oldPos); // 이미지 갱신
        _inven.UpdateInvenInfo(newPos);

        // TODO 바뀐 내용 서버로 전송
    }

    void _ChangeSlotNum(int oldKey, int newKey)
    {
        items[newKey] = items[oldKey];
        items[oldKey] = null;
    }

    void _ExchangeSlotNum(int oldKey, int newKey)
    {
        Item temp = items[oldKey];
        items[oldKey] = items[newKey];
        items[newKey] = temp;
    }

    bool _CheckSameAndCountable(int a, int b)
    {
        return items[a].ItemId == items[b].ItemId && (items[a].Countable && items[b].Countable) == true;
    }

    void _AddUpItems(int a, int b)
    {
        if (items[b].Count == items[b].MaxCount) // 바꿀 위치에 이미 꽉 차있는 경우, 수량만 교환
        {
            int temp = items[b].Count;
            items[b].Count = items[a].Count;
            items[a].Count = temp;
        }
        else
        {
            items[b].Count = items[a].Count + items[b].Count;
            if (items[b].Count > items[b].MaxCount)
            {
                items[a].Count = items[b].Count - items[b].MaxCount;
                items[b].Count = items[b].MaxCount;
            }
            else
            {
                items[a] = null;
            }
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
