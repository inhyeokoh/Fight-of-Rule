using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeCheck : MonoBehaviour
{
    [SerializeField]
    Skill skill;
    
    public Text skillCoolTimeText;
    public Image skillFillAmount;
 

    private void FixedUpdate()
    {
        // 스킬 쿨타임 fixedUpdate로 체크
        if (skill != null)
        {
            if (skill.CoolTime > 0)
            {
                skillFillAmount.fillAmount = skill.CoolTime / skill.MaxCoolTime;

                if (skill.CoolTime >= 1)
                {
                    skillCoolTimeText.text = $"{(int)skill.CoolTime}";
                }
                else
                {
                    skillCoolTimeText.text = $"{skill.CoolTime:F1}";
                }
            }
            else
            {
                skillFillAmount.fillAmount = 0;
                skillCoolTimeText.text = " ";
            }
        }
        else
        {
            skillFillAmount.fillAmount = 0;
            skillCoolTimeText.text = " ";
        }
    }
    // 현재 스킬 객체 정보
    public void CoolTimeChanage(Skill currentSKill)
    {      
        skill = currentSKill;     
    }
    // 스킬을 뺄때 null 바꿈
    public void SkillReset()
    {
        skill = null;
    } 
}
