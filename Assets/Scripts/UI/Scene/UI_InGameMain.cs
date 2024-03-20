using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_InGameMain : UI_Entity
{
    //������Ʈ���� �ʿ��� ������Ʈ��

    Slider HPSbr;
    Slider MPSbr;
    TMP_Text HPText;
    TMP_Text MPText;

    // ���� �÷��̾� ����
    CharacterStatus Player;



    enum Enum_UI_ingameMain
    {
        SettingPanel = 0,
        SkillPanel,
        SkillKeySlot,
        SkillKeySlot2,
        SkillKeySlot3,
        SkillKeySlot4,
        HPSdr,
        MPSdr,
    }
    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_ingameMain);
    }


    /// <summary>
    ///  ���� �ڽĵ��� ������Ʈ���� ������
    /// </summary>
    protected override void Init()
    {
        base.Init();

        print("���� �Է� �Ϸ�");

        HPSbr = _entities[(int)Enum_UI_ingameMain.HPSdr].GetComponent<Slider>();
        MPSbr = _entities[(int)Enum_UI_ingameMain.MPSdr].GetComponent<Slider>();
           
        HPSbr = _entities[(int)Enum_UI_ingameMain.HPSdr].GetComponent<Slider>();
        MPSbr = _entities[(int)Enum_UI_ingameMain.MPSdr].GetComponent<Slider>();

        HPText = HPSbr.GetComponentInChildren<TMP_Text>();
        MPText = MPSbr.GetComponentInChildren<TMP_Text>();      
    }

    /// <summary>
    /// ���� �÷��̾ Ž��
    /// </summary>
    
    public void PlayerCheck()
    {
        Player = PlayerController.instance._playerStat;
    }


    public void HPCheck()
    {

    }
    
    public void MPCheck()
    {

    }   
}

