using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SignUp : UI_Entity
{
    // TODO : 생성가능,중복닉네임,생성불가한 케이스로 나뉘어야함
    bool canCreate = true;
    public TMP_Text msg;


    enum Enum_UI_SignUp
    {
        Panel,
        IDField,
        CheckResult,
        PWField,
        Create,
        Cancel        
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_SignUp);
    }

    protected override void Init()
    {
        base.Init();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.SignUp);
            };
        }


        msg = _entities[(int)Enum_UI_SignUp.CheckResult].GetComponentInChildren<TMP_Text>();

        //서버에 회원가입 요청
        _entities[(int)Enum_UI_SignUp.Create].ClickAction = (PointerEventData data) => {
            C_SIGNUP signup_ask_pkt = new C_SIGNUP();
            signup_ask_pkt.SignupId = _entities[(int)Enum_UI_SignUp.IDField].GetComponent<TMP_InputField>().text;
            signup_ask_pkt.SignupPw = CryptoLib.BytesToString(CryptoLib.EncryptSHA256(_entities[(int)Enum_UI_SignUp.PWField].GetComponent<TMP_InputField>().text), encoding: "ascii");

            GameManager.Network.mainSession.Send(PacketHandler.Instance.SerializePacket(signup_ask_pkt));

            if (!canCreate)
            {
                msg.text = "ID is duplicated";
            }
            else
            {
                GameManager.UI.ClosePopup(GameManager.UI.SignUp);
            }
        };

        _entities[(int)Enum_UI_SignUp.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.SignUp);
        };

        gameObject.SetActive(false);
    }
}
