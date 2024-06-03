#define CLIENTONLY
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// 확인,취소 버튼이 있는 팝업
/// </summary>
public class UI_ConfirmYN : UI_Entity
{
    bool _init;
    public bool _useBlocker = true;
    TMP_Text _mainText;
    Enum_ConfirmTypes confirmType;

    enum Enum_UI_Confirm
    {
        Panel,
        Interact,
        MainText,
        Accept,
        Cancel
    }

    public enum Enum_ConfirmTypes
    {
        AskDecidingNickName
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
            GameObject.Find("CharacterCreate").GetComponent<UI_CharacterCreate>().SendCharacterPacket();
            GameManager.UI.ClosePopupAndChildren(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_Confirm.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.ConfirmYN);
        };

        gameObject.SetActive(false);
    }

    public void ChangeText(Enum_ConfirmTypes type)
    {
        confirmType = type;

        switch (confirmType)
        {
            case Enum_ConfirmTypes.AskDecidingNickName:
                _mainText.text = $"해당 이름으로 결정하시겠습니까?\n 캐릭터명 : {GameManager.Data.characters[GameManager.Data.selectedSlotNum].charName}";
                break;
            default:
                break;
        }
    }
}
