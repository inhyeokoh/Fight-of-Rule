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
    List<Item> _invenItems;
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
        _invenItems = GameManager.Inven.items;

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

        gameObject.SetActive(false);
    }


    // �κ��丮 �� ���԰� ������ ����
    void _SetItemSlots()
    {
        int slotIndex = 0;
        for (int i = 0; i < _totalSlotCount; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _invenPanel.transform);
            _itemSlot.name = "ItemSlot_" + slotIndex;
            _itemSlot.GetComponent<UI_ItemSlot>().index = slotIndex;

/*            if (_invenItems[slotIndex].ItemName == null)
            {
                slotIndex++;
                continue;
            }*/

            GameObject icon = _itemSlot.transform.GetChild(2).gameObject;  // IconImage (=���� ������Ʈ �ڽ� 2��)
            GameObject amountText = icon.transform.GetChild(0).gameObject; // Amount Text (=������ ������Ʈ �ڽ� 0��)

            icon.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            icon.GetComponent<Image>().sprite
                = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[slotIndex].ItemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
            amountText.SetActive(true);
            // amountText.GetComponent<TMP_Text>().text = $"{_invenItems[slotIndex].count}";        
            slotIndex++;
        }
    }   

    public void UpdateInvenInfo(int slotIndex) // ������ �迭 ������ �°� UI ���� ��Ű�� �޼���
    {
        GameObject slotInfo = _invenPanel.transform.GetChild(slotIndex).GetChild(2).gameObject; // ��ȣ�� �´� ������ IconImg ������Ʈ 
        Image iconImg= slotInfo.GetComponent<Image>();
        TMP_Text amountText = slotInfo.transform.GetChild(0).GetComponent<TMP_Text>();

        if (_invenItems[slotIndex] == null)
        {
            iconImg.sprite = null;
            iconImg.color = new Color32(12, 15, 29, 0);
            amountText.gameObject.SetActive(false);
        }
        else
        {
            iconImg.sprite = GameManager.Resources.Load<Sprite>($"Materials/ItemIcons/{_invenItems[slotIndex].ItemName}"); // �ش� ������ �̸��� ��ġ�ϴ� �̹��� �ε�
            iconImg.color = new Color32(255, 255, 255, 255);
            amountText.gameObject.SetActive(true);
            // amountText.text = $"{(_invenItems[slotIndex].count)}"; //  �ش� ������ �̸��� ��ġ�ϴ� ���� �ε�
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