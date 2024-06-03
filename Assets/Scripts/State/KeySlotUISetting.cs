using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlotUISetting : MonoBehaviour
{
   
    [SerializeField]
    UI_SkillKeySlot[] keySlots;
    
    public void KeySlotAllReset()
    {
        for (int i = 0; i < keySlots.Length; i++)
        {                
            keySlots[i].skill = null;               
            keySlots[i].coolTimeCheck.SkillReset();               
            Color tempColor1 = keySlots[i].SkillIcon.color;               
            tempColor1.g = tempColor1.b = tempColor1.r = 0;              
            keySlots[i].SkillIcon.color = tempColor1;            
            keySlots[i].SkillIcon.sprite = null;             
            return;
            
        }
    }

    public void KeySlotCheck(UI_SkillKeySlot currentKeySlot, Skill skill)
    { 
        Color tempColor = currentKeySlot.SkillIcon.color;
        tempColor.g = tempColor.b = tempColor.r = 255f;
        currentKeySlot.SkillIcon.color = tempColor;
        currentKeySlot.SkillIcon.sprite = skill.Icon;

        for (int i = 0; i < keySlots.Length; i++)
        {
            // 만약에 있다면 원래 스킬이 있던 키슬롯은 널시키고 다시 스킬이 없던 화면으로 바꿈
            if (keySlots[i].skill == skill)
            {
                if (keySlots[i] == currentKeySlot)
                {
                    continue;
                }

                keySlots[i].skill = null;
                keySlots[i].coolTimeCheck.SkillReset();
                Color tempColor1 = keySlots[i].SkillIcon.color;
                tempColor1.g = tempColor1.b = tempColor1.r = 0;
                keySlots[i].SkillIcon.color = tempColor1;
                keySlots[i].SkillIcon.sprite = null;                     
                return;
            }
        }      
    }


    public void KeySlotSkillReset(Skill resetSkill)
    {
        for (int i = 0; i < keySlots.Length; i++)
        {
            if (keySlots[i].skill == resetSkill)
            {          
                keySlots[i].skill = null;
                keySlots[i].coolTimeCheck.SkillReset();
                Color tempColor1 = keySlots[i].SkillIcon.color;
                tempColor1.g = tempColor1.b = tempColor1.r = 0;
                keySlots[i].SkillIcon.color = tempColor1;
                keySlots[i].SkillIcon.sprite = null;             
                break;
            }
        }

        /*return true;*/
    }

    public void KeySlotNullChanage(UI_SkillKeySlot currentKeySlot, UI_SkillKeySlot changeKeySlot)
    {     
        currentKeySlot.skill = changeKeySlot.skill;
       
        changeKeySlot.skill = null;
        changeKeySlot.coolTimeCheck.SkillReset();

    
    
        Color tempColor1 = changeKeySlot.SkillIcon.color;
        tempColor1.g = tempColor1.b = tempColor1.r = 0;
        changeKeySlot.SkillIcon.color = tempColor1;
        changeKeySlot.SkillIcon.sprite = null;
      

               
        Color tempColor = currentKeySlot.SkillIcon.color;
        tempColor.g = tempColor.b = tempColor.r = 255f;
        currentKeySlot.SkillIcon.color = tempColor;
        currentKeySlot.SkillIcon.sprite = currentKeySlot.skill.Icon;
        currentKeySlot.coolTimeCheck.CoolTimeChanage(currentKeySlot.skill);
    }

    public void KeySlotChanage(UI_SkillKeySlot currentKeySlot, UI_SkillKeySlot changeKeySlot)
    {
        Skill tempSkill = currentKeySlot.skill;
        currentKeySlot.skill = changeKeySlot.skill;
        changeKeySlot.skill = tempSkill;

        currentKeySlot.SkillIcon.sprite = currentKeySlot.skill.Icon;
        currentKeySlot.coolTimeCheck.CoolTimeChanage(currentKeySlot.skill);

        changeKeySlot.SkillIcon.sprite = changeKeySlot.skill.Icon;
        changeKeySlot.coolTimeCheck.CoolTimeChanage(changeKeySlot.skill);
    }

}
