using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillKeySlot : UI_Entity
{
    // ���� Ű���Կ� ������ִ� ��ų
    public Skill skill;
    Image skillIcon;
    RectTransform imageRect;

    // ���� ���� Ű������ ������ �ٸ� Ű���Ե� (���� ��ų�� ������ Ȯ��)
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
       // print($"���� ��ų Ű���� ��ġ{imageRect.position}");
    }

    protected override void Init()
    {
        base.Init();

        skillIcon = _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].GetComponent<Image>();
        imageRect = _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].GetComponent<RectTransform>();
       
       
        
        // ���� ��ų ������ �������
        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].DropAction = (PointerEventData data) =>
        {
            if (data.pointerDrag != null)
            {
                print("�߻���");
            }
            else
            {
                print("�߻� ����");
            }
       
            // ��Ƽ�� ��ų�̶� �нú� ��ų�̸�
            if (data.pointerDrag.gameObject.tag == "ActiveSkill" || data.pointerDrag.gameObject.tag == "PassiveSkill")
            {
                print(data.pointerDrag.gameObject.name);
                // �Ʊ� ��Ҵ� ��ų ������ ��������
                skill = data.pointerDrag.GetComponent<Skill>();

               /* if (skill.Level == 0)
                {
                    return;
                }*/
                
                // ���� ���� ��ų�� ������ ��ų�� �ִ� Ű���� Ȯ��
                for (int i =0; i < keySlots.Length; i++)
                {

                    // ���࿡ �ִٸ� ���� ��ų�� �ִ� Ű������ �ν�Ű�� �ٽ� ��ų�� ���� ȭ������ �ٲ�
                    if(keySlots[i].skill == skill)
                    {
                        keySlots[i].skill = null;
                        Color tempColor1 = keySlots[i].SkillIcon.color;
                        tempColor1.g = tempColor1.b = tempColor1.r = 0;
                        keySlots[i].SkillIcon.color = tempColor1;
                        keySlots[i].SkillIcon.sprite = null;
                    }
                }

                // �� ��ų�� Ű���Կ� ����

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
            print("��ų�� �����ϴ�.");
            return;
        }
        else
        {
            if (playerState.SkillUseCheck)
            {
                print("��ų ����� �Դϴ�.");
            }
            else if (coolTimeCheck.CoolTime > 0)
            {
                print("��ų�� ���� ��Ÿ�� �Դϴ�.");
            }
            else if (playerStat.MP < skill.SKillMP)
            {
                print("������ �����մϴ�.");
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
