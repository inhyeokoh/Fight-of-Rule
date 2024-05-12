using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopSlot : UI_Entity
{
    UI_Shop shopUI;

    // 현재 슬롯
    Image _iconImg;
    GameObject _amountText;
    public int index;

    enum Enum_UI_ShopSlot
    {
        SlotImg,
        IconImg
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_ShopSlot);
    }

    protected override void Init()
    {
        base.Init();
        _iconImg = _entities[(int)Enum_UI_ShopSlot.IconImg].GetComponent<Image>();
        shopUI = transform.GetComponentInParent<UI_Shop>();

        ItemRender();

        // 커서가 들어오면 아이템 설명 이미지 띄우기
        _entities[(int)Enum_UI_ShopSlot.IconImg].PointerEnterAction = (PointerEventData data) =>
        {
            if (!CheckItemNull())
            {
                shopUI.descrPanel.SetActive(true);
                ShowItemInfo();
                shopUI.RestrictItemDescrPos();
            }
        };

        // 커서가 나갔을때 아이템 설명 내리기
        _entities[(int)Enum_UI_ShopSlot.IconImg].PointerExitAction = (PointerEventData data) =>
        {
            shopUI.descrPanel.SetActive(false);
            shopUI.StopRestrictItemDescrPos(data);
        };

        // 우클릭으로 아이템 장바구니 담기
        _entities[(int)Enum_UI_ShopSlot.IconImg].ClickAction = (PointerEventData data) =>
        {
            if (CheckItemNull()) return;

            if (data.button == PointerEventData.InputButton.Right)
            {
                // 장바구니에 담기
                // shopUI.basket.transform.GetChild()
            }
        };     
    }

    // 슬롯 번호에 맞게 아이템 그리기
    public void ItemRender()
    {
        if (index < shopUI.shopItemCount)
        {
            _iconImg.color = new Color32(255, 255, 255, 255);
            _iconImg.sprite = shopUI.shopItems[index].icon;
        }
        else
        {
            _iconImg.sprite = null;
            _iconImg.color = new Color32(56, 58, 72, 230);
            shopUI.descrPanel.SetActive(false);
        }
    }

    bool CheckItemNull()
    {
        return shopUI.shopItems[index] == null;
    }

    void ShowItemInfo()
    {
        shopUI.descrPanel.transform.GetChild(0).GetComponentInChildren<TMP_Text>().text = shopUI.shopItems[index].name; // 아이템 이름
        shopUI.descrPanel.transform.GetChild(1).GetComponent<Image>().sprite = _iconImg.sprite; // 아이콘 이미지

        if (shopUI.shopItems[index].itemType == Enum_ItemType.Equipment) // 장비아이템 설명
        {
            StateItemData itemData = ItemParsing.itemDatas[shopUI.shopItems[index].id] as StateItemData;
            int[] stats = { itemData.level, itemData.attack, itemData.defense, itemData.speed, itemData.attackSpeed, itemData.maxHp, itemData.maxMp };
            string descLines = string.Format(shopUI.shopItems[index].desc, $"{itemData.level}\n", $"{itemData.attack}\n", $"{itemData.defense}\n", $"{itemData.speed}\n", $"{itemData.attackSpeed}\n", $"{itemData.maxHp}\n", $"{itemData.maxMp}\n");
            string[] lines = descLines.Split("\n");

            string desc = $"{lines[0]} \n";
            for (int i = 1; i < lines.Length - 1; i++)
            {
                if (stats[i] == 0)
                {
                    continue;
                }
                desc += $"{lines[i]} \n";
            }

            shopUI.descrPanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text = desc;
        }
        else
        {
            shopUI.descrPanel.transform.GetChild(2).GetComponentInChildren<TMP_Text>().text =
                shopUI.shopItems[index].desc; // 아이템 설명
        }
    }
}
