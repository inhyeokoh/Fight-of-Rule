using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Google.Protobuf;

public class PacketHandlerImpl : MonoBehaviour
{
    internal static bool Handle_S_SIGNUP(Session session, S_SIGNUP message)
    {
        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorDup)
        {
#if UNITY_EDITOR
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                Utils.Log("이미 존재하는 아이디");
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistID);
            });
            return false;
#endif
        }

        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorExist)
        {
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                Utils.Log("이미 가입된 회원입니다");
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistUser);
            });
            return false;
        }

        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            Utils.Log("성공적으로 가입 되었습니다");
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
            GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.SignUpSuccess);
        });
        return true;
    }

    internal static bool Handle_S_LOGIN(Session session, S_LOGIN message)
    {
        if (false == message.LoginSuccess)
        {
            //경우에 따라서 재로그인 유도 (지금은 그냥 return)
            Debug.Log("로그인 실패");
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.LoginFail);
            });
            return false;
        }

        Utils.Log(message.Ip);
        Utils.Log(message.Port);
        Utils.Log(message.Uid);
        Utils.Log(message.Token);

        if (message.Ip.Length <= 0 || message.Port > 65535 || message.Port <= 0)
        {
            //이건 명확히 재시도 해야함
            //todo
            Utils.Log("로그인 실패");
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.LoginFail);
            });
            return false;
        }

        GameManager.Network.Connect(message.Ip, message.Port, NetState.NEED_VRF, new Vrf() { ip = message.Ip, port = message.Port, token = message.Token, unique_id = message.Uid });
        return true;
    }

    internal static bool Handle_S_ASK_VERF(Session session, S_ASK_VERF message)
    {
        return true;
    }

    internal static bool Handle_S_VERIFYING(Session session, S_VERIFYING message)
    {
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.BlockAll);
        });

        if (message.Sucess == false)
        {
            //TODO 게임 종료시키기
            Utils.Log("cannot verifying");
            return false;
        }

        // 신규 유저        
        if (message.Character.Count == 0)
        {
            GameManager.Data.SelectedSlotNum = 0; // 0번 슬롯 생성하도록

            // 캐릭터 생성씬 이동
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                var loadAsync = SceneManager.LoadSceneAsync("Create");
                GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
            });
            return true;
        }

        // 기존 유저
        foreach (var charInfo in message.Character)
        {
            GameManager.Data.characters[charInfo.BaseInfo.SlotNum] = charInfo;
        }

        // 캐릭터 선택씬 이동
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("Select");
            GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
        });

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.BlockAll);
        });

        // 생성 불가
        if (message.Success == false)
        {            
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistNickName);
            });
            return false;
        }

        // 해당 닉네임 생성 가능하면
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.Data.CurrentCharacter.BaseInfo.Nickname = ByteString.CopyFrom(GameManager.UI.InputName.nickname, System.Text.Encoding.Unicode);
            GameManager.UI.InputName.childPopups.Add(GameManager.UI.ConfirmYN);
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmYN);
            GameManager.UI.ConfirmYN.ChangeText(UI_ConfirmYN.Enum_ConfirmTypes.AskDecidingNickName);            
        });
        return true;
    }

    internal static bool Handle_S_CHARACTERS(Session session, S_CHARACTERS message)
    {
        return true;
    }

    internal static bool Handle_S_NEW_CHARACTER(Session session, S_NEW_CHARACTER message)
    {
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.BlockAll);
        });

        // 캐릭터 생성 불가 시
        if (message.Success == false)
        {
            GameManager.ThreadPool.UniAsyncJob(() =>
            {
                GameManager.UI.OpenPopup(GameManager.UI.ConfirmY);
                GameManager.UI.ConfirmY.ChangeText(UI_ConfirmY.Enum_ConfirmTypes.ExistNickName);
            });
            return false;
        }

        // 캐릭터 생성 가능 시
        Debug.Log(message.Character.BaseInfo.Job);
        GameManager.Data.characters[GameManager.Data.SelectedSlotNum] = message.Character;
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("Select");
            GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
        });

        return true;
    }

    internal static bool Handle_S_DELETE_CHARACTER(Session session, S_DELETE_CHARACTER message)
    {
        Debug.Log("캐릭터 삭제 패킷 수신");
        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.BlockAll);
        });

        if (message.Success == false)
        {
            Debug.Log("캐릭터 삭제 실패");
            return false;
        }
        // 캐릭터 데이터 삭제
        GameManager.Data.characters[message.SlotNum] = null;
        Debug.Log("캐릭터 삭제 완료");
        // TODO UI갱신

        return true;
    }

    internal static bool Handle_S_OPTION(Session session, S_OPTION message)
    {
        //서버로부터 받아온 환경설정 정보들을 메모리에 올리기 
        GameManager.Data.setting.totalVol = message.SettingOptions.TotalVol;
        GameManager.Data.setting.backgroundVol = message.SettingOptions.BackgroundVol;
        GameManager.Data.setting.effectVol = message.SettingOptions.EffectVol;

        GameManager.Data.setting.bTotalVol = message.SettingOptions.TotalVolOn;
        GameManager.Data.setting.bBackgroundVol = message.SettingOptions.BackgroundVolOn;
        GameManager.Data.setting.bEffectVol = message.SettingOptions.EffectVolOn;

        return true;
    }

    internal static bool Handle_S_ITEMINFO(Session session, S_ITEMINFO s_ITEMINFO)
    {
        return true;
    }
}
