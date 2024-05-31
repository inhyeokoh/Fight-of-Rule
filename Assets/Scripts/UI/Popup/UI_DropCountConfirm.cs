using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DropCountConfirm : UI_Entity
{
    TMP_Text _mainText;
    int _slotIndex;
    Enum_DropUIParent dropUIParent;

    enum Enum_UI_DropCountConfirm
    {
        MainText,
        InputField,
        Accept,
        Cancel
    }

    public enum Enum_DropUIParent
    {
        Inven,
        PlayerInfo,
        Shop
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_DropCountConfirm);
    }

    protected override void Init()
    {
        base.Init();
        dropUIParent = Enum_DropUIParent.Inven;
        _mainText = _entities[(int)Enum_UI_DropCountConfirm.MainText].transform.GetChild(0).GetComponent<TMP_Text>();

        // 입력한 수량에 맞게 버리기
        _entities[(int)Enum_UI_DropCountConfirm.Accept].ClickAction = (PointerEventData data) => {
            string input = _entities[(int)Enum_UI_DropCountConfirm.InputField].GetComponent<TMP_InputField>().text;
            int count = Convert.ToInt32(input);

            switch (dropUIParent)
            {
                case Enum_DropUIParent.Inven:
                    GameManager.Inven.DropInvenItem(_slotIndex, count);
                    break;
                case Enum_DropUIParent.Shop:

                    UI_ShopPurchase shopPurchase = transform.parent.parent.GetComponentInChildren<UI_ShopPurchase>();
                    shopPurchase.AddItemInShopBasket(_slotIndex, count);
                    shopPurchase.UpdateGoldPanel();
                    break;
                default:
                    break;
            }

            transform.parent.gameObject.SetActive(false);
        };

        _entities[(int)Enum_UI_DropCountConfirm.Cancel].ClickAction = (PointerEventData data) => {
            transform.parent.gameObject.SetActive(false);
        };

        transform.parent.gameObject.SetActive(false);
    }

    public void ChangeText(Enum_DropUIParent UIName, int slotIndex)
    {
        dropUIParent = UIName;

        _slotIndex = slotIndex;
        switch (dropUIParent)
        {
            case Enum_DropUIParent.Inven:
                _mainText.text = $"{GameManager.Inven.items[slotIndex].name} 아이템을 몇개나 버리시겠습니까?";
                break;
            case Enum_DropUIParent.Shop:
                UI_ShopPurchase shopPurchase = transform.parent.parent.GetComponentInChildren<UI_ShopPurchase>();
                _mainText.text = $"{shopPurchase.shopItems[slotIndex].name} 아이템을 몇개나 구매하시겠습니까?";
                break;
            default:
                break;
        }
    }
}

