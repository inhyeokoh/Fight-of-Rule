using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// 확인 버튼만 있는 팝업
public class UI_ConfirmY : UI_Entity
{
    TMP_Text _mainText;

    enum Enum_UI_Confirm
    {
        Panel,
        Interact,
        MainText,
        Accept,
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Confirm);
    }

    protected override void Init()
    {
        base.Init();

        _mainText = _entities[(int)Enum_UI_Confirm.MainText].GetComponent<TMP_Text>();
        _mainText.text = "Default";

        _entities[(int)Enum_UI_Confirm.Accept].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.ConfirmY);
        };

        gameObject.SetActive(false);
    }

    public void ChangeText(string contents)
    {
        _mainText.text = contents;
    }
}
