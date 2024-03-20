using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    [Header("ActiveSkill")]
    // 레벨마다 다른 데미지
    [SerializeField]
    private int[] skillDamage; // 데미지   

   
   
    [SerializeField]
    WarriorSkill skillNumber;
    private void Start()
    {
        if (level == 0)
        {
            SkillInfo();
        }
        else
        {
            SkillStat();
        }
    }

    // 스킬을 업하면 레벨이 올라가고 스킬들도 업한다
    public override void LevelUp()
    {
        level++;
        SkillStat();        
    }

    // 스킬을 사용하면 플레이어에 있는 그 스킬 상태패턴으로 접근해 스킬을쓰게한다
    public override void Use()
    {
        print($"스킬 데미지 : {SkillDamage}");
        print($"스킬 마나 : {SKillMP}");
        print($"스킬 쿨타임 : {SkillCoolTime}");
        PlayerController.instance._effector.InstanceEffect = skillEffectIndex;
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


    // 현재 스킬이 1레벨 이상일때 정보들
    public override void SkillStat()
    {
        SkillEffectIndex = skillEffectIndex;
        SkillDamage = skillDamage[level];    
        SKillMP = skillMP[level];
        SkillCoolTime = cool[level];
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }


    // 스킬이 0레벨일떄
    public override void SkillInfo()
    {
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }

}
