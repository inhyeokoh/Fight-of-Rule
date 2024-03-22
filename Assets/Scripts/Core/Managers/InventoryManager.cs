using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public List<Item> items = new List<Item>(); // ������
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

    //���÷κ��� �� �κ� ������ ���� �����ͼ� �κ� ����Ʈ�� �������
    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text���Ϸ� �� DB�� Line
        for (int i = 0; i < lines.Length; i++) // 0������ ���ʷ� ����
        {
            string[] row = lines[i].Split('\t'); // Tab �������� ������
            // List�ڷ����� ������ ���� �ֱ�
            items.Add(new Item(null, Convert.ToInt32(row[0]), row[2], row[3], 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6])));
        }

        foreach (var item in items)
        {
            if (itemDict.ContainsKey(item.SlotNum))
            {
                Debug.Log($"Error in {item.SlotNum}");
            }
            itemDict.Add(item.SlotNum, item);
            // Debug.Log($"������ ID: {item.ItemId}, ������ �̸�: {item.ItemName}, ������ �ִ��: {item.MaxCount}, ������ ��: {item.Count}, ������ ���� ��ȣ: {item.SlotNum}");
        }
    }

    public void ConnectInvenUI()
    {
        _inven = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_Inventory>();
    }

    public void DragAndDropItems(int a, int b)
    {     
        if (!itemDict.ContainsKey(b)) // b�� ����ִ� �����̸� �ű��
        {
            _ChangeKeyAndSlotNum(a, b);
        }
        else if (_CheckItemIsSame(a, b)) // ���� �������̸� �տ����� ���� ��ġ��
        {
            _AddUpItems(a, b);
        }
        else  // �ٸ� �������̸� ��ġ ��ȯ
        {
            _ChangeKeyAndSlotNum(a, b); // �������� ���� ��ȣ ����Ī
            _ChangeKeyAndSlotNum(b, a);
        }
        _inven.UpdateInvenInfo(a); // �̹��� ����
        _inven.UpdateInvenInfo(b);

        // �ٲ� ���� ������ ����
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
