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
            // InputField�� �Է��� id�� pw�� ���� LoginData Ŭ������ ����
            GameManager.Data.login.id = _entities[(int)Enum_UI_Logins.IDField].GetComp<TMP_InputField>().text;
            GameManager.Data.login.pw = _entities[(int)Enum_UI_Logins.PWField].GetComp<TMP_InputField>().text;

            // LoginData Ŭ������ LoginData��� Json������ ���Ϸ� ��ȯ�Ͽ� ����
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
            if (GameManager.Data.CheckData($"Slot{i}")) // ���� �����Ͱ� �ϳ��� ������
            {
                return false;
            }
        }
        return true;
    }
}
