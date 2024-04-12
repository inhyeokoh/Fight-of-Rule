using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public int totalSlot = 30;
    public List<Item> items; // slot index에 따른 아이템 리스트

    UI_Inventory _inven;

    enum Enum_Sort // 아이템 정렬 방법
    {
        Grade,
        Equipment,
        Consumption,
        ID
    }

    // 정렬 순서
    enum Enum_ItemType // 아이템 대분류
    {
        Equipment,
        Consumption,
        Materials,
        Etc
    }

    enum Enum_EquipType // 장비
    {
        Weapon,
        Head,
        Body,
        Belt,
        Hand,
        Foot
    }

    enum Enum_ConsumType  // 소비
    {
        Potion,
        Box,
        None
    }

    enum Enum_ItemGrade // 아이템 등급
    {
        Normal,
        Rare,
        Unique,
        Legendary
    }

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
            items[Convert.ToInt32(row[7])] = new Item(null, Convert.ToInt32(row[0]), row[1], row[2], row[3], 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), row[6] == "TRUE", Convert.ToInt32(row[7]),/*temp*/0, row[8], row[9]);
        }
    }

    public void ExtendItemList()
    {
        while (items.Count < totalSlot)
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
        _inven.UpdateInvenUI(oldPos); // 이미지 갱신
        _inven.UpdateInvenUI(newPos);

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
            if (items[b].Count > items[b].MaxCount) // 합쳤을 때 수가 MaxCount보다 크면
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

    // 아이템의 타입을 반환하는 메서드
    Enum_ItemType GetItemType(Item item)
    {
        switch (item.Type)
        {
            case "Equipment":
                return Enum_ItemType.Equipment;
            case "Consumption":
                return Enum_ItemType.Consumption;
            case "Materials":
                return Enum_ItemType.Materials;
            default:
                return Enum_ItemType.Etc;
        }
    }

    Enum_ItemGrade GetItemGrade(Item item)
    {
        switch (item.Grade)
        {
            case "Rare":
                return Enum_ItemGrade.Rare;
            case "Unique":
                return Enum_ItemGrade.Unique;
            case "Legendary":
                return Enum_ItemGrade.Legendary;
            default:
                return Enum_ItemGrade.Normal;
        }
    }

    Enum_EquipType GetItemEquipType(Item item)
    {
        switch (item.DetailType)
        {
            case "Weapon":
                return Enum_EquipType.Weapon;
            case "Helmet":
                return Enum_EquipType.Head;
            case "Armor":
                return Enum_EquipType.Body;
            case "Belt":
                return Enum_EquipType.Belt;
            case "Boots":
                return Enum_EquipType.Foot;
            case "Gloves":
                return Enum_EquipType.Hand;
            default:
                return Enum_EquipType.Weapon;
        }
    }

    Enum_ConsumType GetItemConsumType(Item item)
    {
        switch (item.DetailType)
        {
            case "Potion":
                return Enum_ConsumType.Potion;
            case "Box":
                return Enum_ConsumType.Box;
            default:
                return Enum_ConsumType.None;
        }
    }

    // 아이템 번호에 따라서 리스트 재정렬 + 앞부터 비어 있는 칸 채워야함 + slotNum 변경 + 같은 아이템이면 합쳐줌
    // 한번이라도 정렬 버튼 누른적 있을거고 아이템 번호에 맞게 정리 되어 있는 상태가 많기 때문에 병합 정렬로 가는게 가장 괜찮다고 판단
    public void SortItems()
    {
        // 아이템을 Enum_Itemtype에 따라 분류할 딕셔너리 생성
        Dictionary<Enum_ItemType, List<Item>> itemDictionary = new Dictionary<Enum_ItemType, List<Item>>();

        // 각 아이템을 해당하는 Enum_Itemtype의 리스트에 추가
        foreach (Item item in items)
        {
            if (item != null)
            {
                Enum_ItemType itemType = GetItemType(item); // 아이템의 타입 가져오기
                if (!itemDictionary.ContainsKey(itemType))
                {
                    itemDictionary[itemType] = new List<Item>();
                }
                itemDictionary[itemType].Add(item);
            }
        }

        // 대분류(장비,소비 등등)된 항목들 순회
        foreach (Enum_ItemType itemType in itemDictionary.Keys)
        {
            List<Item> itemList = itemDictionary[itemType];
            if (itemType == Enum_ItemType.Equipment)
            {
                // ID -> Equipment(Helmet,Boots,etc) -> Grade 순으로 정렬 시켜버리는게 빠르진 못해도 간단하지 않을까..
                MergeSortItems(itemList, Enum_Sort.ID); // 아이템 등급 순서대로 병합 정렬
                MergeSortItems(itemList, Enum_Sort.Equipment); // 아이템 등급 순서대로 병합 정렬
                MergeSortItems(itemList, Enum_Sort.Grade); // 아이템 등급 순서대로 병합 정렬
            }
            else if (itemType == Enum_ItemType.Consumption)
            {
                MergeSortItems(itemList, Enum_Sort.ID);
                MergeSortItems(itemList, Enum_Sort.Consumption);
            }
            else
            {
                MergeSortItems(itemList, Enum_Sort.ID);
            }
        }

        // Enum_Itemtype에 명시된 순서대로 아이템 리스트 재배열
        items.Clear();
        foreach (Enum_ItemType itemType in Enum.GetValues(typeof(Enum_ItemType)))
        {
            if (itemDictionary.ContainsKey(itemType))
            {
                List<Item> itemList = itemDictionary[itemType];
                items.AddRange(itemList);
            }
        }

        _CombineQuantities(items);
        ExtendItemList(); // 비어있는 칸 다시 null로 채우기

        // UI 갱신
        for (int i = 0; i < items.Count; i++)
        {
            _inven.UpdateInvenUI(i);
        }
    }

    // 특정 순서대로 병합 정렬하는 메서드
    void MergeSortItems(List<Item> itemList, Enum_Sort sort)
    {
        if (itemList.Count <= 1)
        {
            return;        
        }

        int mid = itemList.Count / 2;
        List<Item> leftList = new List<Item>();
        List<Item> rightList = new List<Item>();

        for (int i = 0; i < mid; i++)
        {
            leftList.Add(itemList[i]);
        }
        for (int i = mid; i < itemList.Count; i++)
        {
            rightList.Add(itemList[i]);
        }

        MergeSortItems(leftList, sort);
        MergeSortItems(rightList, sort);

        itemList.Clear();
        switch (sort)
        {
            case Enum_Sort.Grade:
                itemList.AddRange(MergeItemsByGrade(leftList, rightList));
                break;
            case Enum_Sort.Equipment:
                itemList.AddRange(MergeItemsByEquip(leftList, rightList));
                break;
            case Enum_Sort.Consumption:
                itemList.AddRange(MergeItemsByConsum(leftList, rightList));
                break;
            case Enum_Sort.ID:
                itemList.AddRange(MergeItemsById(leftList, rightList));
                break;
            default:
                break;
        }
    }

    List<Item> MergeItemsById(List<Item> leftList, List<Item> rightList)
    {
        List<Item> mergedList = new List<Item>();

        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < leftList.Count && rightIndex < rightList.Count)
        {
            if (leftList[leftIndex].ItemId < rightList[rightIndex].ItemId)
            {
                mergedList.Add(leftList[leftIndex]);
                leftIndex++;
            }
            else
            {
                mergedList.Add(rightList[rightIndex]);
                rightIndex++;
            }
        }

        while (leftIndex < leftList.Count)
        {
            mergedList.Add(leftList[leftIndex]);
            leftIndex++;
        }

        while (rightIndex < rightList.Count)
        {
            mergedList.Add(rightList[rightIndex]);
            rightIndex++;
        }

        return mergedList;
    }

    List<Item> MergeItemsByGrade(List<Item> leftList, List<Item> rightList)
    {
        List<Item> mergedList = new List<Item>();

        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < leftList.Count && rightIndex < rightList.Count)
        {
            Enum_ItemGrade left = GetItemGrade(leftList[leftIndex]);
            Enum_ItemGrade right = GetItemGrade(rightList[rightIndex]);
            // 낮은 등급일수록 왼쪽에 배치
            if (left < right)
            {
                mergedList.Add(leftList[leftIndex]);
                leftIndex++;
            }
            else
            {
                mergedList.Add(rightList[rightIndex]);
                rightIndex++;
            }
        }

        while (leftIndex < leftList.Count)
        {
            mergedList.Add(leftList[leftIndex]);
            leftIndex++;
        }

        while (rightIndex < rightList.Count)
        {
            mergedList.Add(rightList[rightIndex]);
            rightIndex++;
        }

        return mergedList;
    }

    List<Item> MergeItemsByEquip(List<Item> leftList, List<Item> rightList)
    {
        List<Item> mergedList = new List<Item>();

        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < leftList.Count && rightIndex < rightList.Count)
        {
            Enum_EquipType left = GetItemEquipType(leftList[leftIndex]);
            Enum_EquipType right = GetItemEquipType(rightList[rightIndex]);
            if (left < right)
            {
                mergedList.Add(leftList[leftIndex]);
                leftIndex++;
            }
            else
            {
                mergedList.Add(rightList[rightIndex]);
                rightIndex++;
            }
        }

        while (leftIndex < leftList.Count)
        {
            mergedList.Add(leftList[leftIndex]);
            leftIndex++;
        }

        while (rightIndex < rightList.Count)
        {
            mergedList.Add(rightList[rightIndex]);
            rightIndex++;
        }

        return mergedList;
    }

    List<Item> MergeItemsByConsum(List<Item> leftList, List<Item> rightList)
    {
        List<Item> mergedList = new List<Item>();

        int leftIndex = 0;
        int rightIndex = 0;

        while (leftIndex < leftList.Count && rightIndex < rightList.Count)
        {
            Enum_ConsumType left = GetItemConsumType(leftList[leftIndex]);
            Enum_ConsumType right = GetItemConsumType(rightList[rightIndex]);
            if (left < right)
            {
                mergedList.Add(leftList[leftIndex]);
                leftIndex++;
            }
            else
            {
                mergedList.Add(rightList[rightIndex]);
                rightIndex++;
            }
        }

        while (leftIndex < leftList.Count)
        {
            mergedList.Add(leftList[leftIndex]);
            leftIndex++;
        }

        while (rightIndex < rightList.Count)
        {
            mergedList.Add(rightList[rightIndex]);
            rightIndex++;
        }

        return mergedList;
    }

    void _CombineQuantities(List<Item> itemList)
    {
        if (itemList.Count < 2)
        {
            return;
        }

        int i = 1;
        while (i < itemList.Count)
        {
            // 같은 ItemID이고 더 앞쪽에 있는 아이템의 수량이 최대가 아닐 경우 앞에다가 합치기
            if (itemList[i].ItemId == itemList[i - 1].ItemId && itemList[i - 1].Count != itemList[i - 1].MaxCount)
            {
                _AddUpItems(i, i - 1);
                if (itemList[i] == null)
                {
                    itemList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
            else
            {
                i++;
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
