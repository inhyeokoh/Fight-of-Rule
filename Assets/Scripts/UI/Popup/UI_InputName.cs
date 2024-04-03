#define CLIENTONLY
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InputName : UI_Entity
{
    public string nickname;
    TMP_Text _instruction;

    enum Enum_UI_InputName
    {
        Panel,
        Instruction,
        InputField,
        Accept,
        Cancel,
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_InputName);
    }


    protected override void Init()
    {
        base.Init();
        _instruction = _entities[(int)Enum_UI_InputName.Instruction].GetComponentInChildren<TMP_Text>();
        _instruction.text = "Up to 12 characters can be entered in Korean, English, and numbers.";

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.InputName);
            };
        }

        _entities[(int)Enum_UI_InputName.Accept].ClickAction = (PointerEventData data) => {
            nickname = _entities[(int)Enum_UI_InputName.InputField].GetComponent<TMP_InputField>().text;
            string nickChecker = Regex.Replace(nickname, @"[^0-9a-zA-Z가-R]{1,12}", "", RegexOptions.Singleline);

            // 특수문자 안되게
            if (nickname.Equals(nickChecker) == false)
            {
                GameManager.UI.OpenChildPopup(GameManager.UI.ConfirmY, true);
                GameManager.UI.ConfirmY.GetComponent<UI_ConfirmY>().ChangeText("Special characters and spaces are not allowed.");
            }
            else if (nickname.Length < 2 || nickname.Length > 12)
            {
                GameManager.UI.OpenChildPopup(GameManager.UI.ConfirmY, true);
                GameManager.UI.ConfirmY.GetComponent<UI_ConfirmY>().ChangeText("Please enter at least 2 characters and no more than 12 characters.");
            }
            else
            {
#if CLIENTONLY
                GameManager.Data.characters[GameManager.Data.selectedSlotNum].charName = GameObject.Find("PopupCanvas").GetComponentInChildren<UI_InputName>().nickname;
                GameManager.UI.OpenChildPopup(GameManager.UI.ConfirmYN, true);
                GameManager.UI.ConfirmYN.GetComponent<UI_ConfirmYN>().choice = 0;
                GameManager.UI.ConfirmYN.GetComponent<UI_ConfirmYN>().ChangeText($"Would you like to decide on this character name ?\n Character name : {GameManager.Data.characters[GameManager.Data.selectedSlotNum].charName}");
#else
                C_NICKNAME nick_DupAsk_pkt = new C_NICKNAME();
                nick_DupAsk_pkt.Nickname = nickname;
                GameManager.Network.mainSession.Send(PacketHandler.Instance.SerializePacket(nick_DupAsk_pkt));            
#endif
            };
        };

        _entities[(int)Enum_UI_InputName.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.InputName);
        };

        gameObject.SetActive(false);
    }
}
