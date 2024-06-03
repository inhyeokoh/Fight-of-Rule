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

    // 스킬을 업하면 레벨이 올라가고 스킬들도 업한다

    public override Skill Init()
    {
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = SkillManager.Skill.transform.GetChild(0);       
        return clone.GetComponent<ActiveSkill>();
    }

    public override void SKillDB(WarriorSkillData data)
    {
        skillID = data.id;
        skillName = data.name;
        skillDESC = data.desc;
        icon = data.icon;
        skillNumber = data.number;
        maxLevel = data.maxLevel;
        skillLevelCondition = data.levelCondition;
        skillPoint = data.skillPoint;
        skillMP = data.skillMP;
        cool = data.skillCool;
        skillDamage = data.skillDamage;
    } 
}
