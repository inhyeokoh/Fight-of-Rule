using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// ���� ��ųâ�ȿ� �ִ� ��ų���Ե�
/// </summary>
public class UI_SkillUISlot : UI_Entity
{

    // ���� ��ų���� UI�� �ʿ��� ������Ʈ��
    UI_Entity IconObject;
    UI_SkillWindow SkillUIObject;
    TMP_Text[] skillText;
    [SerializeField]
    SelectSkillMove MoveSkillObject;

    


    // ����� �巡�׸� ������ ��ġ�� �����Ű�� ���� Ʈ������
    Transform canvas;
    Transform previousParent;


   
    // ���� ���Ծȿ� �ִ� ��ų
    Skill skill;

    enum Enum_UI_SkillUISolt
    {
        Panel = 0,
        SkillIcon,      
    }
    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_SkillUISolt);
    }

    private void Update()
    {
       // print($"���� ��ų ������ ��ġ {imageRect.position}");
    }


    protected override void Init()
    {
        // ���� ��ų�� ����� Ű���Կ��� ��ų�� �޸����� ���� ��ų���Կ� �ִ� �̹����� ���ƿ;� �ϱ� ������
        // ���� ��ų���Ծȿ� �ִ� �������� �����ϴ� ���

        base.Init();
        IconObject = _entities[(int)Enum_UI_SkillUISolt.SkillIcon];       
        
        canvas = gameObject.transform.root.GetComponent<Canvas>().transform;
        
        skill = IconObject.GetComponent<ActiveSkill>();
        IconObject.GetComponent<Image>().sprite = skill.Icon;
        SkillUIObject = transform.parent.parent.parent.parent.GetComponent<UI_SkillWindow>();
        skillText = _entities[(int)Enum_UI_SkillUISolt.Panel].GetComponentsInChildren<TMP_Text>();
        skillText[0].text = skill.SkillName;

        // ���� ��ų ���� ����
        SkillInfo();
         
        // ��ų������ �������� ��ųâ�� ���� ��ų �������� ������
        _entities[(int)Enum_UI_SkillUISolt.Panel].ClickAction = (PointerEventData data) =>
        {
            SkillUIObject.SelctSkill(skill);
            SkillTexts(this);
        };

        _entities[(int)Enum_UI_SkillUISolt.SkillIcon].BeginDragAction = (PointerEventData data) =>
        {
            if (skill.Level == 0)
            {
                data.pointerEnter = null;
                return;
            }
            /*_entities[(int)Enum_UI_SkillUISolt.SkillIcon].gameObject.transform.SetParent(canvas);
            _entities[(int)Enum_UI_SkillUISolt.SkillIcon].gameObject.transform.SetAsLastSibling();*/

            //print("Click");

            MoveSkillObject.gameObject.SetActive(true);
            MoveSkillObject.ClickSkill(skill);
            MoveSkillObject.GetComponent<Image>().raycastTarget = false;

        };
        _entities[(int)Enum_UI_SkillUISolt.SkillIcon].DragAction = (PointerEventData data) =>
        {            
            if (skill.Level == 0)
            {
                data.pointerDrag = null;
                return;
            }

            MoveSkillObject.gameObject.transform.position = data.position;        

        };
        _entities[(int)Enum_UI_SkillUISolt.SkillIcon].EndDragAction = (PointerEventData data) =>
        {
            if (skill.Level == 0)
            {
                data.pointerDrag = null;
                return;
            }
            MoveSkillObject.GetComponent<Image>().raycastTarget = true;
            MoveSkillObject.gameObject.SetActive(false);
    
        };
    }

    // ���� ��ų ���� ����
    public void SkillInfo()
    {
        skillText[1].text = $"���� ���� : {skill.Level}";
    }


    // ��ų�� �������� �Ͽ����� ������ �����ϱ����� ��ų ���� �������� �����ִ� �޼���
    public void SkillTexts(UI_SkillUISlot texts)
    {
        SkillUIObject.SelectSkillUIText(texts);
        return;
    }
    


                                   





}
