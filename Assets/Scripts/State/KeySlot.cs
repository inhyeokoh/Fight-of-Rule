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
        
        print("나갔음");
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
            print("스킬이 없습니다.");
            return;
        }
        else
        {
            if (player.SkillUseCheck)
            {
                print("스킬 사용중 입니다.");
            }
            else if (coolTimeCheck.CoolTime > 0)
            {
                print("스킬이 아직 쿨타임 입니다.");
            }
            else if (player.MP < skill.SKillMP)
            {
                print("마나가 부족합니다.");
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
