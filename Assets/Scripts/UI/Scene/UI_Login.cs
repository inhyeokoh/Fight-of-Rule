using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Login : UI_Entity
{
    enum Enum_UI_Logins
    {
        Panel,
        IDField,
        PWField,        
        Login,
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

            // ȯ�漳��, ĳ���� ������ �����ϰ� Json ���Ϸ� �� ���� (���� ���� ������� ����) �޾ƿ� 
            GameManager.Data.setting = JsonUtility.FromJson<SettingsData>(GameManager.Data.LoadData("Setting"));
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            GameManager.Scene.ExitGame();
        };
    }
}
