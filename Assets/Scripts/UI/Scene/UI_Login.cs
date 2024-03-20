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

            //로그인 성공시 실행될 내용. 테스트 용도
            var loadAsync = SceneManager.LoadSceneAsync("Create");
            GameManager.ThreadPool.UniAsyncLoopJob(() =>
            {
                return loadAsync.progress < 0.9f;
            });
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            GameManager.Scene.ExitGame();
        };
    }

    public void StartLogin()
    {
        C_LOGIN login_ask_pkt = new C_LOGIN();
        login_ask_pkt.LoginId = _entities[(int)Enum_UI_Logins.IDField].GetComponent<TMP_InputField>().text;
        login_ask_pkt.LoginPw = _entities[(int)Enum_UI_Logins.PWField].GetComponent<TMP_InputField>().text;
        login_ask_pkt.LoginPw = CryptoLib.BytesToString(CryptoLib.EncryptSHA256(login_ask_pkt.LoginPw));

        GameManager.Network.mainSession.Send(PacketHandler.Instance.SerializePacket(login_ask_pkt));
    }
}
