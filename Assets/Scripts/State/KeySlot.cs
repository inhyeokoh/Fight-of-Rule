using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlot : MonoBehaviour
{
    [SerializeField]
    Skill slotSkill;

    [SerializeField]
    CoolTimeCheck coolTimeCheck;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (slotSkill != null)
        {
            return;
        }

        if (other.tag == "ActiveSkill")
        {
            slotSkill = other.GetComponent<ActiveSkill>();
            coolTimeCheck.MaxCoolTime = slotSkill.SkillCoolTime;
        }
        else if (other.tag == "PassiveSkill")
        {
            slotSkill = other.GetComponent<PassivSkill>();
            coolTimeCheck.MaxCoolTime = slotSkill.SkillCoolTime;
        }
    }    
    public void OnTriggerExit2D(Collider2D other)
    {
        
        print("������");
        if (other.tag == "ActiveSkill")
        {
            coolTimeCheck.CoolTimeStop();
            
            slotSkill = null;
        }
        else if (other.tag == "PassiveSkill")
        {
            coolTimeCheck.CoolTimeStop();
            slotSkill = null;
        }
    }
    public void QuickSlot(Skill skill, CharacterState player) 
    {
        if (skill == null)
        {
            player.ChangeState((int)Enum_CharacterState.Idle);
            print("��ų�� �����ϴ�.");
            return;
        }
        else
        {
            if (player.SkillUseCheck)
            {
                print("��ų ����� �Դϴ�.");
            }
            else if (coolTimeCheck.CoolTime > 0)
            {
                print("��ų�� ���� ��Ÿ�� �Դϴ�.");
            }
            else if (player.MP < skill.SKillMP)
            {
                print("������ �����մϴ�.");
            }
            else
            {
                player.MP -= skill.SKillMP;
                PlayerController.instance._effector.EffectBurstStop();
                skill.Use();
                coolTimeCheck.SkillUse();
            }
        }
      
    }
    public void Use(CharacterState player)
    {
        QuickSlot(slotSkill, player);
    }
}
