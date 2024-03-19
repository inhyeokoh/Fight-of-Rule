using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandlerImpl : MonoBehaviour
{
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

    internal static bool Handle_S_LOGIN(Session session, S_LOGIN message)
    {
        if (false == message.LoginSuccess)
        {
            //경우에 따라서 재로그인 유도 (지금은 그냥 return)
            Debug.Log("로그인 실패");
            return false;
        }

        Debug.Log(message.Ip);
        Debug.Log(message.Port);
        Debug.Log(message.Uid);

        GameManager.ThreadPool.UniAsyncJob(() =>
        {
            var loadAsync = SceneManager.LoadSceneAsync("create");
            GameManager.ThreadPool.UniAsyncLoopJob(() => { return loadAsync.progress < 0.9f; });
        }
        );
        /*                // var field_list = message.Slots;
                        AsyncOperation loadAsync;

                        if (field_list.Count == 0)
                        {
                            //슬롯이 없음 => 신규 유저
                            //신규유저 로직 처리 ( 경우에 따라 다른 패킷을 전송해야 할 수 있음)
                            GameManager.Data.selectedSlotNum = 0; // 0번 슬롯 생성하도록
                            loadAsync = SceneManager.LoadSceneAsync("Create");
                            GameManager.ThreadPool.UniAsyncLoopJob(() =>
                            {
                                return loadAsync.progress < 0.9f;
                            });

                            //TODO
                            return true;
                        }

                        GameManager.Data.characters = new CharData[field_list.Count];
                        for (int i = 0; i < field_list.Count; i++)
                        {
                            var slot = field_list[i];
                            //슬롯 순회하면서 유저 스탯 정보 처리
                            GameManager.Data.characters[i].charName = slot.Nickname;
                            GameManager.Data.characters[i].job = slot.Job;
                            GameManager.Data.characters[i].gender = slot.Gender;

                            GameManager.Data.characters[i].level = slot.Stat.Level;
                            GameManager.Data.characters[i].maxHP = slot.Stat.MaxHP;
                            GameManager.Data.characters[i].hp = slot.Stat.Hp;
                            GameManager.Data.characters[i].maxMP = slot.Stat.MaxMP;
                            GameManager.Data.characters[i].mp = slot.Stat.Mp;
                            GameManager.Data.characters[i].maxEXP = slot.Stat.MaxEXP;
                            GameManager.Data.characters[i].exp = slot.Stat.Exp;
                            GameManager.Data.characters[i].attack = slot.Stat.Attack;
                            GameManager.Data.characters[i].attackSpeed = slot.Stat.AttackSpeed;
                            GameManager.Data.characters[i].defense = slot.Stat.Defense;
                            GameManager.Data.characters[i].speed = slot.Stat.Speed;

                        }
                        //캐릭터 선택씬으로 이동
                        loadAsync = SceneManager.LoadSceneAsync("Select");
                        GameManager.ThreadPool.UniAsyncLoopJob(() =>
                        {
                            return loadAsync.progress < 0.9f;
                        });*/

        return true;
    }

    internal static bool Handle_S_SIGNUP(Session session, S_SIGNUP message)
    {
        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorDup)
        {
            Debug.Log("이미 존재하는 아이디");
            return false;
        }

        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorExist)
        {
            Debug.Log("이미 가입된 회원입니다");
            return false;
        }

        Debug.Log("성공적으로 가입 되었습니다");
        return true;
    }

    internal static bool Handle_S_CHARACTER(Session session, S_CHARACTER message)
    {

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        // message.NicknameSuccess; 이 bool값을 받아서 UI_Login 스크립트 변수에 받아야할지?

        return true;
    }
}
