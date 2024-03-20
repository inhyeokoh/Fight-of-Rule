using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SubClass<GameManager>
{
    public List<Item> items; // ������
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

    //���÷κ��� �� �κ� ������ ���� �����ͼ� �κ� ����Ʈ�� �������
    void _GetInvenDataFromDB()
    {
        items = new List<Item>();
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text���Ϸ� �� DB�� Line
        for (int i = 0; i < lines.Length; i++) // �迭 0������ ���ʷ� ����
        {
            string[] row = lines[i].Split('\t'); // Tab �������� ������           
            // ������ ���� �ֱ�
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
            // Debug.Log($"������ ��: {items[i].itemName}, ������ ����: {items[i].count}");
        }
    }

    public void DragAndDropItems(int a, int b)
    {
        // ���� �������̸� �տ����� ���� ��ġ��, �ٸ� �������̸� ��ġ ��ȯ
        if (_CheckItemIsSame(a, b))
        {
            _AddUpItems(a, b);
        }
        else
        {
            _SwitchItems(a, b); // ������ �迭 ����Ī
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
