using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_SkillWindow : UI_Entity
{  
    /// <summary>
    /// 스킬창 오브젝트 들 
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
    /// 키슬롯과 스킬연동을 위한 클래스들
    /// </summary>
    [SerializeField]
    UI_SkillKeySlot[] skillChecks;

    public Skill selectSkill;

    CharacterStatus player;

    enum Enum_UI_SkillWindow
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
    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_SkillWindow); 
    }

    protected override void Init()
    {      
         base.Init();
        _skillPanal = _entities[(int)Enum_UI_SkillWindow.BackGroundPanel].gameObject;

        _entities[(int)Enum_UI_SkillWindow.Close].ClickAction =(PointerEventData data) =>
        {
            // GameManager.UI.ClosePopup(GameManager.UI.SkillWindow);
        };
        //현재 클릭 메서드들
        _entities[(int)Enum_UI_SkillWindow.LearnBtn].ClickAction = (PointerEventData data) =>
        {
            if (selectSkill == null)
            {
                print("배울 스킬이 없습니다");
                return;
            }

            if (selectSkill.Level == selectSkill.MAXLevel)
            {
                print("최대레벨 입니다.");
            }
            /*else if (selectSkill.SkillLevelCondition > player.Level)
            {
                print("레벨이 부족합니다.");
            }*/
            else
            {
                selectSkill.LevelUp();
                SkillInpomation();
                selectSkillInfo.SkillInfo();
            }
        };

        _entities[(int)Enum_UI_SkillWindow.ResetBtn].ClickAction = (PointerEventData data) =>
        {
            if (selectSkill == null)
            {
                print("리셋할 스킬이 없습니다");
                return;
            }

            if (selectSkill.Level == 0)
            {
                print("리셋할 포인트가 없습니다.");
            }
            else
            {
                for(int i =0; i < skillChecks.Length; i++)
                {
                    if(skillChecks[i].skill == selectSkill)
                    {
                        skillChecks[i].skill = null;
                        Color tempColor1 = skillChecks[i].SkillIcon.color;
                        tempColor1.g = tempColor1.b = tempColor1.r = 0;
                        skillChecks[i].SkillIcon.color = tempColor1;
                        skillChecks[i].SkillIcon.sprite = null;
                    }
                }

                selectSkill.Level = 0;
                selectSkill.SkillInfo(); 
                SkillInpomation();
                selectSkillInfo.SkillInfo();
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

    // 스킬 인포창을 갱신하기 위한 메서드
    public void SelctSkill(Skill skill)
    {
        this.selectSkill = skill;
        SkillInpomation();
    }


    // 스킬 인포창 갱신중
    private void SkillInpomation()
    {
        skillToolTip.text = selectSkill.SKillDESC;
        skillName.text = selectSkill.SkillName;
        skillLevel.text = $"레벨 {selectSkill.SkillLevelCondition} 이상";
        skillPoint.text = $"필요 포인트 \n {selectSkill.SkillPoint}";
        skillReset.text = "리셋하기";


        if (selectSkill.Level == 0)
        {
            skillLearn.text = "배우기";
        }
        else if (selectSkill.Level == selectSkill.MAXLevel)
        {
            skillLearn.text = "최대입니다";
            skillPoint.text = "MAX";
            skillLevel.text = "MAX";
        }
        else
        {
            skillLearn.text = "레벨업";
        }

    }


    // 스킬이 업했거나 리셋을 했으면 위에 인포창들을 갱신하기위해 스킬에 있는 정보들을 가져온다
    public void SelectSkillUIText(UI_SkillUISlot texts)
    {
        if (selectSkillInfo != null)
        {
            selectSkillInfo = null;
        }

        selectSkillInfo = texts;
    }


}
