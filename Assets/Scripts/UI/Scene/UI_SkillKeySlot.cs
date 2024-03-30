using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillKeySlot : UI_Entity
{

    bool skillChange;


    // ���� Ű���Կ� ������ִ� ��ų
    public Skill skill;
    public CoolTimeCheck coolTimeCheck;
        
    [SerializeField]
    KeySlotUISetting keySlotUISetting;

    Image skillIcon;
    RectTransform keySlotImageRect;

    // ����� �巡�׸� ������ ��ġ�� �����Ű�� ���� Ʈ������
    Transform canvas;
    Transform previousParent;


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
        keySlotImageRect = _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].GetComponent<RectTransform>();

        canvas = gameObject.transform.root;
        previousParent = transform.parent;
        /*keySlotUISetting =*/



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
                skill = data.pointerDrag.transform.parent.GetComponent<UI_SkillUISlot>().SkillReturn();
                coolTimeCheck.CoolTimeChanage(skill);

                keySlotUISetting.KeySlotCheck(this, skill);
            }  
            
            if (data.pointerDrag.gameObject.tag == "KeySlotSkill")
            {
                UI_SkillKeySlot dataUIKeySlot = data.pointerDrag.transform.parent.GetComponent<UI_SkillKeySlot>();
                //if (dataUIKeySlot.skill == null) return;

                if (skill == null)
                {
                    keySlotUISetting.KeySlotNullChanage(this, dataUIKeySlot);
                   
                    MoveSkillObject.GetComponent<Image>().raycastTarget = true;
                    MoveSkillObject.gameObject.SetActive(false);
                }
                else
                {
                    keySlotUISetting.KeySlotChanage(this, dataUIKeySlot);                   
                }

                skillChange = true;
            }
        };
        _entities[(int)Enum_UI_SKillKeySlot.SkillIcon].BeginDragAction = (PointerEventData data) =>
        {
            if (skill == null)
            {
                data.pointerEnter = null;
                return;
            }

            MoveSkillObject.gameObject.transform.SetParent(canvas);
            MoveSkillObject.transform.SetAsLastSibling();
            MoveSkillObject.gameObject.SetActive(true);
            MoveSkillObject.SkillClick(skill);
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

           /* if (data.pointerDrag)*/

            MoveSkillObject.gameObject.transform.SetParent(previousParent);
            MoveSkillObject.transform.SetAsLastSibling();
            MoveSkillObject.GetComponent<Image>().raycastTarget = true;
            MoveSkillObject.gameObject.SetActive(false);

            if (data.pointerCurrentRaycast.gameObject == null || 
            data.pointerCurrentRaycast.gameObject.tag != "KeySlotSkill")
            {
                if (MoveSkillObject.transform.position.x > (keySlotImageRect.position.x + 96f)        
                || MoveSkillObject.transform.position.x < (keySlotImageRect.position.x - 96f)        
                || MoveSkillObject.transform.position.y > (keySlotImageRect.position.y + 52.50f)         
                || MoveSkillObject.transform.position.y < (keySlotImageRect.position.y + -52.50f))
                {
                    keySlotUISetting.KeySlotSkillReset(skill);
                }
            }         
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
            else if (skill.CoolTime > 0)
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
            }
        }
    }
    public void Use(CharacterState playerState, CharacterStatus playerStat)
    {
        QuickSlot(skill, playerState, playerStat);
    }

}
