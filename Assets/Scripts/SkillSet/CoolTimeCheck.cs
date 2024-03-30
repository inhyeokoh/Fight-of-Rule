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
        // ��ų ��Ÿ�� fixedUpdate�� üũ
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
    // ���� ��ų ��ü ����
    public void CoolTimeChanage(Skill currentSKill)
    {      
        skill = currentSKill;     
    }
    // ��ų�� ���� null �ٲ�
    public void SkillReset()
    {
        skill = null;
    } 
}
