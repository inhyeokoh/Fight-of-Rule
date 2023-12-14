using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Login : UI_Entity
{
    int slotLength = 4;

    enum Enum_UI_Logins
    {
        Panel,
        IDField,
        PWField,
        Login,
        Setting,
        Quit
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Logins);
    }

    protected override void Init()
    {
        base.Init();

        _entities[(int)Enum_UI_Logins.Login].ClickAction = (PointerEventData data) => {
            // InputField에 입력한 id와 pw를 각각 LoginData 클래스에 저장
            GameManager.Data.login.id = _entities[(int)Enum_UI_Logins.IDField].GetComp<TMP_InputField>().text;
            GameManager.Data.login.pw = _entities[(int)Enum_UI_Logins.PWField].GetComp<TMP_InputField>().text;

            // LoginData 클래스를 LoginData라는 Json형식의 파일로 변환하여 저장
            GameManager.Data.SaveData("LoginData", GameManager.Data.login);


            if (CheckSlotsNull())
            {
                GameManager.Data.fileName = "Slot0";
                GameManager.Scene.GetNextScene();
            }
            else
            {
                GameManager.Scene.GetNextScene(2);
            }
        };

        _entities[(int)Enum_UI_Logins.Setting].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            GameManager.Scene.ExitGame();
        };
    }

    bool CheckSlotsNull()
    {
        for (int i = 0; i<slotLength; i++)
        {
            if (GameManager.Data.CheckData($"Slot{i}")) // 슬롯 데이터가 하나라도 있으면
            {
                return false;
            }
        }
        return true;
    }
}
