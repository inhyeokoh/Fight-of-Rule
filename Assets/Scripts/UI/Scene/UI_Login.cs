//#define SERVER
#define CLIENT_TEST
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
        SignUp,
        Quit
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Logins);
    }

    protected override void Init()
    {
        base.Init();

        _entities[(int)Enum_UI_Logins.SignUp].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.SignUp);
        };

        _entities[(int)Enum_UI_Logins.Login].ClickAction = (PointerEventData data) => {
#if SERVER
            C_LOGIN login_ask_pkt = new C_LOGIN();
            login_ask_pkt.LoginId = _entities[(int)Enum_UI_Logins.IDField].GetComponent<TMP_InputField>().text;
            login_ask_pkt.LoginPw = CryptoLib.BytesToString(CryptoLib.EncryptSHA256(_entities[(int)Enum_UI_Logins.PWField].GetComponent<TMP_InputField>().text), encoding:"ascii");

            GameManager.Network.Send(PacketHandler.Instance.SerializePacket(login_ask_pkt)); 

#elif CLIENT_TEST
            // 서버 없이 씬 넘어가기
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                var loadAsync = SceneManager.LoadSceneAsync("Create");
                GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
            });
#endif
        };

        _entities[(int)Enum_UI_Logins.Quit].ClickAction = (PointerEventData data) => {
            SceneController.instance.ExitGame();
        };
    }
}
