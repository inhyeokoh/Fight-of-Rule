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
            // InputField에 입력한 id와 pw를 각각 LoginData 클래스에 저장
            GameManager.Data.login.id = _entities[(int)Enum_UI_Logins.IDField].GetComp<TMP_InputField>().text;
            GameManager.Data.login.pw = _entities[(int)Enum_UI_Logins.PWField].GetComp<TMP_InputField>().text;

            // LoginData 클래스를 LoginData라는 Json형식의 파일로 변환하여 저장
            GameManager.Data.SaveData("LoginData", GameManager.Data.login);

            // 환경설정, 캐릭터 데이터 생성하고 Json 파일로 된 내용 (추후 서버 통신으로 변경) 받아옴 
            GameManager.Data.setting = JsonUtility.FromJson<SettingsData>(GameManager.Data.LoadData("Setting"));
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            GameManager.Scene.ExitGame();
        };
    }
}
