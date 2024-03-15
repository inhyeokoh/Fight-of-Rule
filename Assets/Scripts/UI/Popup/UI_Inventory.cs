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
    public ItemBase[] items; // �κ� ������ �迭

    int _totalSlotCount = 30;

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
        _GetInvenDataFromDB();
        _SetItemSlots();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
            };
        }

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

        // UI���� Ŀ�� ���� �� ĳ���� �ൿ ����
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

    //���÷κ��� �� �κ� ������ ���� �����ͼ� �迭�� �������
    void _GetInvenDataFromDB()
    {
        GameManager.Data.ItemDB = GameManager.Resources.Load<TextAsset>("Data/ItemDB");
        string[] lines = GameManager.Data.ItemDB.text.Substring(0, GameManager.Data.ItemDB.text.Length - 1).Split('\n'); // text���Ϸ� �� DB�� Line
        items = new ItemBase[_totalSlotCount + 1]; // ������ ĭ�� ������ ����Ī��
        for (int i = 0; i < lines.Length; i++) // �迭 0������ ���ʷ� ����
        {
            string[] row = lines[i].Split('\t'); // Tab �������� ������           
            // ������ ���� �ֱ�
            if (row[0] == "Equipment")
            {
                items[i] = new InGameItemEquipment();
            }
            else if (row[0] == "Consumption")
            {
                items[i] = new InGameItemConsumption();
                //items[i] = new InGameItemDuraction();
            }
            else
            {
                // Etc
            }
            // items[i].itemID
            // items[i].itemType =
            items[i].itemName = row[1];
            items[i].itemDescription = row[2];
            items[i].count = Convert.ToInt32(row[3]);
            // Debug.Log($"������ ��: {items[i].itemName}, ������ ����: {items[i].count}");
        }        
    }

    // �κ��丮 �� ���԰� ������ ����
    void _SetItemSlots()
    {
        int slotIndex = 0;
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + slotIndex;

            if (items[slotIndex].itemName == null)
            {
                slotIndex++;
                continue;
            }

            GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage (=���� ������Ʈ �ڽ� 2��)
            GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text (=������ ������Ʈ �ڽ� 0��)

            icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            icon.GetComponent<Image>().sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
            amountText.SetActive(true);
            amountText.GetComponent<TMP_Text>().text = $"{items[slotIndex].count}";        
            slotIndex++;
        }
    }   


    public void SwitchItems(int a, int b)
    {        
        items[_totalSlotCount] = items[b];
        items[b] = items[a];
        items[a] = items[_totalSlotCount];
    }

    public bool CheckItemType(int a, int b)
    {
        if (items[a].itemType == items[b].itemType)
        {
            return true;
        }

        return false;
    }


    public void AddUpItems(int a, int b)
    {
        if (items[a].count <= 100)
        {

        }
    }


    public void UpdateInvenInfo(int slotIndex) // ������ �迭 ������ �°� ���� ��Ű�� �޼��� ���.
    {
        GameObject slotInfo = _invenPanel.transform.GetChild(slotIndex).GetChild(2).gameObject; // ��ȣ�� �´� ������ IconImg ������Ʈ 
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
            iconImg.sprite = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{items[slotIndex].itemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
            iconImg.color = new Color32(255, 255, 255, 255);
            amountText.gameObject.SetActive(true);
            amountText.text = $"{(items[slotIndex].count)}"; //  �ش� ������ �̸��� ��ġ�ϴ� ���� �ε�
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