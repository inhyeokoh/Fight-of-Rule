using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        ActiveSkill cloneCp = clone.GetComponent<ActiveSkill>();
        cloneCp.controller = PlayerController.instance;
        cloneCp.playerCapability = cloneCp.controller._playerCapability;
        cloneCp.playerStatus = cloneCp.controller._playerStat;

        return cloneCp;
        //return this;
    }
    public override void Setting()
    {
       
    }
    public override void LevelUp()
    {
        level++;
        SkillStat();
    }
    public override void LevelReset()
    {
        level = 0;
        SkillStat();
    }
    public override void SKillDB(WarriorSkillData data)
    {
        skillID = data.id;
        skillName = data.name;
        skillDESC = data.desc;
        icon = data.icon;
        skillType = (Enum_SkillType)Enum.Parse(typeof(Enum_SkillType), data.skillType);
        skillNumber = data.number;
        maxLevel = data.maxLevel;
        skillLevelCondition = data.levelCondition;
        skillPoint = data.skillPoint;
        skillDuration = data.skillDuration;
        skillMaxHP = data.skillMaxHP;
        skillAttack = data.skillAttack;
        skillDefense = data.skillDefense;
        skillSpeed = data.skillSpeed;
        skillAttackSpeed = data.skillAttackSpeed;
        skillMaxMP = data.skillMaxMP;
        skillMP = data.skillMP;
        cool = data.skillCool;
        skillDamage = data.skillDamage;
    }

}
