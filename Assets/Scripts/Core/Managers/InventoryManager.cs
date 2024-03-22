using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public List<Item> items = new List<Item>(); // 아이템
    public Dictionary<int, Item> itemDict = new Dictionary<int, Item>();
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
        _inven = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_Inventory>();
    }

    //로컬로부터 내 인벤 아이템 정보 가져와서 인벤 리스트에 집어넣음
    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text파일로 된 DB의 Line
        for (int i = 0; i < lines.Length; i++) // 0번부터 차례로 넣음
        {
            string[] row = lines[i].Split('\t'); // Tab 기준으로 나누기
            // List자료형에 아이템 집어 넣기
            items.Add(new Item(null, Convert.ToInt32(row[0]), row[2], row[3], 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6])));
        }

        foreach (var item in items)
        {
            if (itemDict.ContainsKey(item.SlotNum))
            {
                Debug.Log($"Error in {item.SlotNum}");
            }
            itemDict.Add(item.SlotNum, item);
            // Debug.Log($"아이템 ID: {item.ItemId}, 아이템 이름: {item.ItemName}, 아이템 최대수: {item.MaxCount}, 아이템 수: {item.Count}, 아이템 슬롯 번호: {item.SlotNum}");
        }
    }

    public void ConnectInvenUI()
    {
        _inven = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_Inventory>();
    }

    public void DragAndDropItems(int a, int b)
    {     
        if (!itemDict.ContainsKey(b)) // b가 비어있는 슬롯이면 옮기기
        {
            _ChangeKeyAndSlotNum(a, b);
        }
        else if (_CheckItemIsSame(a, b)) // 같은 아이템이면 앞에꺼에 수량 합치기
        {
            _AddUpItems(a, b);
        }
        else  // 다른 아이템이면 위치 교환
        {
            _ChangeKeyAndSlotNum(a, b); // 아이템의 슬롯 번호 스위칭
            _ChangeKeyAndSlotNum(b, a);
        }
        _inven.UpdateInvenInfo(a); // 이미지 갱신
        _inven.UpdateInvenInfo(b);

        // 바뀐 내용 서버로 전송
    }

    void _ChangeKeyAndSlotNum(int oldKey, int newKey)
    {
        if (oldKey == newKey)
        {
            return;
        }

        itemDict[oldKey].SlotNum = newKey;
        itemDict.Add(newKey, itemDict[oldKey]);
        itemDict.Remove(oldKey);
    }

    bool _CheckItemIsSame(int a, int b)
    {
        if (itemDict[a].ItemId == itemDict[b].ItemId)
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
