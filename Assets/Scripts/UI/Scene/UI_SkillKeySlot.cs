using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillKeySlot : UI_Entity
{
    // 현재 키슬롯에 저장되있는 스킬
    public Skill skill;
    Image skillIcon;
    RectTransform imageRect;

    // 현재 나의 키슬롯을 제외한 다른 키슬롯들 (같은 스킬을 꼈는지 확인)
    [SerializeField]
    UI_SkillKeySlot[] keySlots;
    [SerializeField]
    CoolTimeCheck coolTimeCheck;

    [SerializeField]
    SelectSkillMove MoveSkillObject;

    public Image SkillIcon { get { return skillIcon; } set { skillIcon = value; } }
    enum Enum_UI_SKillKeySlot
    {
        SkillIcon = 0,
        CoolTime,
        Fill,
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_SKillKeySlot);
    }


    private void Update()
    {
       // print($"현재 스킬 키슬롯 위치{imageRect.position}");
    }

    protected override void Init()
    {
        base.Init();

        skillIcon = _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].GetComponent<Image>();
        imageRect = _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].GetComponent<RectTransform>();
       
       
        
        // 현재 스킬 정보가 닿았을때
        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].DropAction = (PointerEventData data) =>
        {
            if (data.pointerDrag != null)
            {
                print("발생함");
            }
            else
            {
                print("발생 안함");
            }
       
            // 엑티브 스킬이랑 패시브 스킬이면
            if (data.pointerDrag.gameObject.tag == "ActiveSkill" || data.pointerDrag.gameObject.tag == "PassiveSkill")
            {
                print(data.pointerDrag.gameObject.name);
                // 아까 닿았던 스킬 정보를 가져오고
                skill = data.pointerDrag.GetComponent<Skill>();

               /* if (skill.Level == 0)
                {
                    return;
                }*/
                
                // 현재 나의 스킬과 동일한 스킬이 있는 키슬롯 확인
                for (int i =0; i < keySlots.Length; i++)
                {

                    // 만약에 있다면 원래 스킬이 있던 키슬롯은 널시키고 다시 스킬이 없던 화면으로 바꿈
                    if(keySlots[i].skill == skill)
                    {
                        keySlots[i].skill = null;
                        Color tempColor1 = keySlots[i].SkillIcon.color;
                        tempColor1.g = tempColor1.b = tempColor1.r = 0;
                        keySlots[i].SkillIcon.color = tempColor1;
                        keySlots[i].SkillIcon.sprite = null;
                    }
                }

                // 그 스킬을 키슬롯에 적용

                Color tempColor = skillIcon.color;
                tempColor.g = tempColor.b = tempColor.r = 255f;
                skillIcon.color = tempColor;
                skillIcon.sprite = skill.Icon;
                coolTimeCheck.MaxCoolTime = skill.SkillCoolTime;
            }  
            
            if (data.pointerDrag.gameObject.tag == "KeySlotSkill")
            {
                UI_SkillKeySlot dataUIKeySlot = data.pointerDrag.transform.parent.GetComponent<UI_SkillKeySlot>();
                //if (dataUIKeySlot.skill == null) return;

                if (skill == null)
                {                    
                    skill = dataUIKeySlot.skill;

                    dataUIKeySlot.skill = null;

                    Color tempColor1 = dataUIKeySlot.SkillIcon.color;
                    tempColor1.g = tempColor1.b = tempColor1.r = 0;
                    dataUIKeySlot.SkillIcon.color = tempColor1;
                    dataUIKeySlot.SkillIcon.sprite = null;

                    Color tempColor = skillIcon.color;
                    tempColor.g = tempColor.b = tempColor.r = 255f;
                    skillIcon.color = tempColor;
                    skillIcon.sprite = skill.Icon;
                    coolTimeCheck.MaxCoolTime = skill.SkillCoolTime;

                    MoveSkillObject.GetComponent<Image>().raycastTarget = true;
                    MoveSkillObject.gameObject.SetActive(false);
                }
                else
                {                  
                    Skill tempSkill = skill;
                    skill = dataUIKeySlot.skill;
                    dataUIKeySlot.skill = tempSkill;

                    skillIcon.sprite = skill.Icon;
                    coolTimeCheck.MaxCoolTime = skill.SkillCoolTime;
                    
                    dataUIKeySlot.SkillIcon.sprite = dataUIKeySlot.skill.Icon;
                    dataUIKeySlot.coolTimeCheck.MaxCoolTime = skill.SkillCoolTime;
                }
            }
        };
        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].BeginDragAction = (PointerEventData data) =>
        {
            if (skill == null)
            {
                data.pointerEnter = null;
                return;
            }
          
            MoveSkillObject.gameObject.SetActive(true);
            MoveSkillObject.ClickSkill(skill);
            MoveSkillObject.GetComponent<Image>().raycastTarget = false;
        };
        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].DragAction = (PointerEventData data) =>
        {
            if (skill == null)
            {
                data.pointerDrag = null;
                return;
            }

            MoveSkillObject.gameObject.transform.position = data.position;
        };

        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].EndDragAction = (PointerEventData data) =>
        {
            if (skill == null)
            {
                data.pointerDrag = null;
                return;
            }

            if (data.pointerDrag)

            MoveSkillObject.GetComponent<Image>().raycastTarget = true;
            MoveSkillObject.gameObject.SetActive(false);
        };

    }


    public void QuickSlot(Skill skill, CharacterState playerState, CharacterStatus playerStat)
    {
        if (skill == null)
        {
            playerState.ChangeState((int)Enum_CharacterState.Idle);
            print("스킬이 없습니다.");
            return;
        }
        else
        {
            if (playerState.SkillUseCheck)
            {
                print("스킬 사용중 입니다.");
            }
            else if (coolTimeCheck.CoolTime > 0)
            {
                print("스킬이 아직 쿨타임 입니다.");
            }
            else if (playerStat.MP < skill.SKillMP)
            {
                print("마나가 부족합니다.");
            }
            else
            {
                playerStat.MP -= skill.SKillMP;
                PlayerController.instance._effector.EffectBurstStop();
                skill.Use();
                coolTimeCheck.SkillUse();
            }
        }

    }
    public void Use(CharacterState playerState, CharacterStatus playerStat)
    {
        QuickSlot(skill, playerState, playerStat);
    }

}
