using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InGameNotify : UI_Entity
{
    TMP_Text _mainText;
    Enum_NotifyParent UIParent;

    enum Enum_UI_InGameNotify
    {
        MainText,
        Interact,
        Accept
    }
    public enum Enum_NotifyParent
    {
        Shop
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_InGameNotify);
    }

    protected override void Init()
    {
        base.Init();
        UIParent = Enum_NotifyParent.Shop;
        _mainText = _entities[(int)Enum_UI_InGameNotify.MainText].transform.GetChild(0).GetComponent<TMP_Text>();

        _entities[(int)Enum_UI_InGameNotify.Accept].ClickAction = (PointerEventData data) => {
            transform.parent.gameObject.SetActive(false);
        };

        transform.parent.gameObject.SetActive(false);
    }

    public void ChangeText(Enum_NotifyParent UIName)
    {
        UIParent = UIName;

        switch (UIParent)
        {
            case Enum_NotifyParent.Shop:
                _mainText.text = $"장바구니가 가득 찼습니다!";
                break;
            default:
                break;
        }
    }
}
