using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    [Header("ActiveSkill")]
    
    [SerializeField]
    private int[] skillDamage; // 데미지   

    [SerializeField]
    WarriorSkill skillNumber;
    public override void LevelUp()
    {
        level++;
        SkillStat();        
    }
    public override void Use()
    {
        print($"스킬 데미지 : {SkillDamage}");
        print($"스킬 마나 : {SKillMP}");
        print($"스킬 쿨타임 : {SkillCoolTime}");
        SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage);
        SkillManager.Skill.PlayerState.ChangeState((int)skillNumber);      
    }
    public override Skill Init()
    {
        
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = SkillManager.Skill.transform.GetChild(0);       
        clone.SetActive(false);

        return clone.GetComponent<ActiveSkill>();
    }

    public override void SkillStat()
    {
        SkillDamage = skillDamage[level];    
        SKillMP = skillMP[level];
        SkillCoolTime = cool[level];
    }
}
