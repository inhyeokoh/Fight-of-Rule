using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

            GameManager.Data.setting = JsonUtility.FromJson<SettingsData>(GameManager.Data.LoadData("Setting"));
            var loadAsync = SceneManager.LoadSceneAsync("StatePattern"); // 테스트 위해서 인게임 씬으로 바로 이동
            GameManager.ThreadPool.UniAsyncLoopJob(() =>
            {             
                return loadAsync.progress < 0.9f;
            });
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            GameManager.Scene.ExitGame();
        };
    }
}
