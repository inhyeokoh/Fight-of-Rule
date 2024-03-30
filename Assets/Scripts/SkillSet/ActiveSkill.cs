using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    /*[SerializeField]
    private int[] skillDamage; 
 
    [SerializeField]
    WarriorSkill skillNumber;*/
    private void Start()
    {
        if (level == 0)
        {
            SkillZeroStat();
        }
        else
        {
            SkillStat();
        }
    }

    // ��ų�� ���ϸ� ������ �ö󰡰� ��ų�鵵 ���Ѵ�

    public override Skill Init()
    {
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = SkillManager.Skill.transform.GetChild(0);       
        return clone.GetComponent<ActiveSkill>();
    }
}
