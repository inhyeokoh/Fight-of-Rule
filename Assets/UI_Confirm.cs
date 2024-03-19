using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Confirm : UI_Entity
{
    public TMP_Text mainText;

    enum Enum_UI_Confirm
    {
        Panel,
        Interact,
        MainText,
        Accept,
        Cancel
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Confirm);
    }

    protected override void Init()
    {
        base.Init();

        mainText = _entities[(int)Enum_UI_Confirm.MainText].GetComponent<TMP_Text>();
        mainText.text = "Default";

        _entities[(int)Enum_UI_Confirm.Accept].ClickAction = (PointerEventData data) => {
            GameManager.UI.CloseLinkedPopup();
            GameManager.Scene.GetNextScene();
        };

        _entities[(int)Enum_UI_Confirm.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Confirm);
        };

        gameObject.SetActive(false);
    }

    public void ChangeText(string contents)
    {
        Debug.Log(mainText.text);
        mainText.text = contents;
    }
}
