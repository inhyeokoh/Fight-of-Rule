using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public int totalSlot = 30;
    public List<Item> items; // slot index�� ���� ������ ����Ʈ

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

    //���÷κ��� �� �κ� ������ ���� �����ͼ� �κ� ����Ʈ�� �������
    public void ConnectInvenUI()
    {
        items = new List<Item>(new Item[totalSlot]);

        _inven = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_Inventory>();
        _GetInvenDataFromDB();
    }

    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text���Ϸ� �� DB�� Line

        for (int i = 0; i < lines.Length; i++) // 0������ ���ʷ� ����
        {
            string[] row = lines[i].Split('\t'); // Tab �������� ������
            // ������ ���� �ֱ�
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

        if (items[newPos] == null) // newPos�� ����ִ� �����̸� �ű��
        {
            _ChangeSlotNum(oldPos, newPos);
        }
        // �� �� �ִ� �������̰� ���� �������̸� ���� ��ġ��
        else if (_CheckSameAndCountable(oldPos, newPos))
        {
            _AddUpItems(oldPos, newPos);
        }
        else  // �ٸ� �������̸� ��ġ ��ȯ
        {
            _ExchangeSlotNum(oldPos, newPos);
        }
        _inven.UpdateInvenInfo(oldPos); // �̹��� ����
        _inven.UpdateInvenInfo(newPos);

        // TODO �ٲ� ���� ������ ����
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
        if (items[b].Count == items[b].MaxCount) // �ٲ� ��ġ�� �̹� �� ���ִ� ���, ������ ��ȯ
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
    // ������ ����
    /*    void AddItem/Item data, int amount)
        {
            if (���� �ִ� ������) -> ���� ������ ã�Ƽ� ���� �ջ�

            if (data._countable == true)
            {

            }




            else if (���� ������ ���ų�, �̹� �ִ���� �̰ų�, ������ ���� ������) -> �迭�� �պ��� �� ���� ã�Ƽ� �ֱ�
        }*/

}
