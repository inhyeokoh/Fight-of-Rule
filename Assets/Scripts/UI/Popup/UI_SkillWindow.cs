using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public enum Enum_UI_SkillWindow
{
    BackGroundPanel = 0,
    Interact,
    Close,
    CurrentSkillPointPanel,
    SkillToolTipView,
    LearnBtn,
    ResetBtn,
    SkillNamePanel,
    NecessaryLevelPanel,
    NecessaryPointPanel,
    SkillView,
    SelctSkillObject
}

public class UI_SkillWindow : UI_Entity
{

    [SerializeField]
    GameObject skillContent;

    /// <summary>
    /// ��ųâ ������Ʈ �� 
    /// </summary>
    UI_SkillUISlot selectSkillInfo;
    GameObject _skillPanal;
    TMP_Text playerPoint;
    TMP_Text skillToolTip;
    TMP_Text skillName;
    TMP_Text skillLevel;
    TMP_Text skillPoint;
    TMP_Text skillLearn;
    TMP_Text skillReset;


    /// <summary>
    /// Ű���԰� ��ų������ ���� Ŭ������
    /// </summary>
 
    public Skill selectSkill;

    CharacterStatus player;

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_SkillWindow); 
    }

    protected override void Init()
    {      
         base.Init();
        _skillPanal = _entities[(int)Enum_UI_SkillWindow.BackGroundPanel].gameObject;

     
        // ��ų�Ŵ����� �ִ� ĳ���� ��ų �迭 �������� ��ų���� ����
        foreach(Skill skill in SkillManager.Skill.CharacterSkillSet())
        {
            UI_SkillUISlot skillClone = GameManager.Resources.Instantiate
               ("Prefabs/UI/Scene/SkillUISlot", skillContent.transform)
               .GetComponent<UI_SkillUISlot>();

            skillClone.CurrentSkill(skill);
        }
      
        _entities[(int)Enum_UI_SkillWindow.Close].ClickAction =(PointerEventData data) =>
        {
            // GameManager.UI.ClosePopup(GameManager.UI.SkillWindow);
        };
        //���� Ŭ�� �޼����
        _entities[(int)Enum_UI_SkillWindow.LearnBtn].ClickAction = (PointerEventData data) =>
        {
            if (selectSkill == null)
            {
                print("��� ��ų�� �����ϴ�");
                return;
            }

            if (selectSkill.Level == selectSkill.MAXLevel)
            {
                print("�ִ뷹�� �Դϴ�.");
            }
            /*else if (selectSkill.SkillLevelCondition > player.Level)
            {
                print("������ �����մϴ�.");
            }*/
            else
            {       
                SkillManager.Skill.SkillLevelUp(selectSkill);
                SkillInfomation();
                selectSkillInfo.SkillLevelInfo();
            }
        };

        _entities[(int)Enum_UI_SkillWindow.ResetBtn].ClickAction = (PointerEventData data) =>
        {
            if (selectSkill == null)
            {
                print("������ ��ų�� �����ϴ�");
                return;
            }

            if (selectSkill.Level == 0)
            {
                print("������ ����Ʈ�� �����ϴ�.");
            }
            else
            {
                SkillManager.Skill.SkillReset(selectSkill); 
                SkillInfomation();
                selectSkillInfo.SkillLevelInfo();
            }
        };


        playerPoint = _entities[(int)Enum_UI_SkillWindow.CurrentSkillPointPanel].GetComponentInChildren<TMP_Text>();
        skillToolTip = _entities[(int)Enum_UI_SkillWindow.SkillToolTipView].GetComponentInChildren<TMP_Text>();     
        skillLearn = _entities[(int)Enum_UI_SkillWindow.LearnBtn].GetComponentInChildren<TMP_Text>();
        skillReset = _entities[(int)Enum_UI_SkillWindow.ResetBtn].GetComponentInChildren<TMP_Text>();
        skillName = _entities[(int)Enum_UI_SkillWindow.SkillNamePanel].GetComponentInChildren<TMP_Text>();
        skillLevel = _entities[(int)Enum_UI_SkillWindow.NecessaryLevelPanel].GetComponentInChildren<TMP_Text>();
        skillPoint = _entities[(int)Enum_UI_SkillWindow.NecessaryPointPanel].GetComponentInChildren<TMP_Text>();
    }

    // ��ų ����â�� �����ϱ� ���� �޼���
    public void SelctSkill(Skill skill)
    {
        this.selectSkill = skill;
        SkillInfomation();
    }


    // ��ų ����â ������
    private void SkillInfomation()
    {
        skillToolTip.text = selectSkill.SKillDESC;
        skillName.text = selectSkill.SkillName;
        skillLevel.text = $"���� {selectSkill.SkillLevelCondition} �̻�";
        skillPoint.text = $"�ʿ� ����Ʈ \n {selectSkill.SkillPoint}";
        skillReset.text = "�����ϱ�";


        if (selectSkill.Level == 0)
        {
            skillLearn.text = "����";
        }
        else if (selectSkill.Level == selectSkill.MAXLevel)
        {
            skillLearn.text = "�ִ��Դϴ�";
            skillPoint.text = "MAX";
            skillLevel.text = "MAX";
        }
        else
        {
            skillLearn.text = "������";
        }

    }


    // ��ų�� ���߰ų� ������ ������ ���� ����â���� �����ϱ����� ��ų�� �ִ� �������� �����´�
    public void SelectSkillUIInfoMationText(UI_SkillUISlot texts)
    {
        if (selectSkillInfo != null)
        {
            selectSkillInfo = null;
        }

        selectSkillInfo = texts;
    }
}
