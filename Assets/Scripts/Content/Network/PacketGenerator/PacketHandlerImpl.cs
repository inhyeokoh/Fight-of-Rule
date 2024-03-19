using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandlerImpl : MonoBehaviour
{
    internal static bool Handle_S_OPTION(Session session, S_OPTION message)
    {
        //�����κ��� �޾ƿ� ȯ�漳�� �������� �޸𸮿� �ø��� 
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
            //��쿡 ���� ��α��� ���� (������ �׳� return)
            Debug.Log("�α��� ����");
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
                            //������ ���� => �ű� ����
                            //�ű����� ���� ó�� ( ��쿡 ���� �ٸ� ��Ŷ�� �����ؾ� �� �� ����)
                            GameManager.Data.selectedSlotNum = 0; // 0�� ���� �����ϵ���
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
                            //���� ��ȸ�ϸ鼭 ���� ���� ���� ó��
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
                        //ĳ���� ���þ����� �̵�
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
            Debug.Log("�̹� �����ϴ� ���̵�");
            return false;
        }

        if (message.SignupResult == S_SIGNUP.Types.SIGNUP_FLAGS.SignupErrorExist)
        {
            Debug.Log("�̹� ���Ե� ȸ���Դϴ�");
            return false;
        }

        Debug.Log("���������� ���� �Ǿ����ϴ�");
        return true;
    }

    internal static bool Handle_S_CHARACTER(Session session, S_CHARACTER message)
    {

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        // message.NicknameSuccess; �� bool���� �޾Ƽ� UI_Login ��ũ��Ʈ ������ �޾ƾ�����?

        return true;
    }
}
