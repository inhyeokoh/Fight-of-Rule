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
    public Item[] items; // �κ� ������ �迭

    int _slotCountHor = 6;  // ���� ���� ��
    int _slotCountVer = 5;  // ���� ���� ��

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

        // �κ��丮 â �巡��
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            transform.position = data.position;
        };

        // �κ��丮 �ݱ�
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };

        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;
    }


    // �κ��丮 �� ���� ����
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
                    GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage(=���� ������Ʈ �ڽ� 2��)
                    GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text Ȱ��ȭ(=������ ������Ʈ �ڽ� 0��)

                    icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    icon.GetComponent<Image>().sprite
                        = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
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

    // �����̳� �����κ��� �� �κ� ������ ���� ��������
    void GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text���Ϸ� �� DB�� Line ��
        items = new Item[_slotCountHor * _slotCountVer + 1]; // ������ ĭ�� ������ ����Ī��
        for (int i = 0; i < lines.Length; i++) // �迭 0������ ���ʷ� ����
        {
            string[] row = lines[i].Split('\t'); // Tab �������� ������
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