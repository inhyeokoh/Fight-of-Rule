#define CLIENTONLY
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// 확인 취소 버튼 있는 팝업
public class UI_ConfirmYN : UI_Entity
{
    TMP_Text _mainText;

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

        _mainText = _entities[(int)Enum_UI_Confirm.MainText].GetComponent<TMP_Text>();
        _mainText.text = "Default";

        _entities[(int)Enum_UI_Confirm.Accept].ClickAction = (PointerEventData data) => {
            _ExecuteAcceptAction();
        };

        _entities[(int)Enum_UI_Confirm.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.ConfirmYN);
        };

        gameObject.SetActive(false);
    }

    void _ExecuteAcceptAction()
    {        
/*                GameManager.UI.CloseLinkedPopup();
                GameManager.Scene.LoadScene("Select");*/
                GameObject.Find("CharacterCreate").GetComponent<UI_CharacterCreate>().SendCharacterPacket();
                GameManager.UI.CloseLinkedPopup();
                GameManager.Scene.LoadScene("Select");        
    }

    public void ChangeText(string contents)
    {
        _mainText.text = contents;
    }
}
