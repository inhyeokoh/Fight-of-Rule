using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InputName : UI_Entity
{
    // TODO : 생성가능,중복닉네임,생성불가한 케이스로 나뉘어야함
    bool canCreate = true;
    string nickName = "";
    TMP_Text msg;

    enum Enum_UI_InputName
    {
        Panel,
        InputField,
        Create,
        Accept,
        Cancel,
        CheckName
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_InputName);
    }


    protected override void Init()
    {
        base.Init();
        msg = _entities[(int)Enum_UI_InputName.CheckName].GetComponentInChildren<TMP_Text>();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.InputName);
            };
        }


        // 생성 가능한 이름인지 체크 후, 저장 및 씬 이동
        _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => {            
            if (canCreate)
            {
                msg.text = "This name can be created.";
                GameManager.Data.characters[GameManager.Data.selectedSlotNum].charName = nickName;

                _entities[(int)Enum_UI_InputName.Accept].ClickAction = (PointerEventData data) => {
                    GameManager.UI.OpenChildPopup(GameManager.UI.Confirm);
                    GameManager.UI.Confirm.GetComponent<UI_Confirm>().ChangeText("Would you like to create?");
                };
            }
            else
            {
                msg.text = "This name cannot be created.";
            }
        };

        _entities[(int)Enum_UI_InputName.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.InputName);
        };

        gameObject.SetActive(false);
    }

    private void Update()
    {
        nickName = _entities[(int)Enum_UI_InputName.InputField].GetComp<TMP_InputField>().text;
        if (String.IsNullOrEmpty(nickName))
        {
            msg.text = "Please enter your name.";
        }
        else if (nickName.Length < 4 || nickName.Length > 12)
        {
            msg.text = "Please enter at least 4 characters and no more than 12 characters.";
        }
        else
        {
            msg.text = "";
        }
    }
}
