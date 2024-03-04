using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketHandlerImpl : MonoBehaviour
{
    internal static bool Handle_S_OPTION(Session session, S_OPTION message)
    {
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
        if(false == message.LoginSuccess)
        {
            //경우에 따라서 재로그인 유도 (지금은 그냥 return)
            Debug.Log("로그인 실패");
            return false;
        }

        GameManager.Data.setting = new SettingsData();
        var field_list = message.Slots;

        if(field_list.Count == 0)
        {
            //슬롯이 없음 => 신규 유저
            //신규유저 로직 처리 ( 경우에 따라 다른 패킷을 전송해야 할 수 있음)
            GameManager.Data.selectedSlotNum = 0; // 0번 슬롯 생성하도록
            SceneManager.LoadScene("Create");            
            
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
        SceneManager.LoadScene("Select"); // 밑에 return true랑 상관없으려나? 반환값이 true면 다음씬 이동?

        return true;
    }

    internal static bool Handle_S_NICKNAME(Session session, S_NICKNAME message)
    {
        // message.NicknameSuccess;

        return true;
    }
}
