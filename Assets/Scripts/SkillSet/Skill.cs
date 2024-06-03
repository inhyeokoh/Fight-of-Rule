using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // 현재 스킬 레벨
    [SerializeField]
    protected int level = 0;

    // 최대 레벨
    [SerializeField]
    protected int maxLevel;

    [SerializeField]
    protected WarriorSkill skillNumber;

    [Header("Skill ID")]
    [SerializeField]
    protected int skillID; //스킬 번호
    
    [Header("Skill String")]
    [SerializeField]
    protected string skillName; //스킬 이름
    [SerializeField]
    protected string skillDESC; // 스킬 설명

    [SerializeField]
    protected string skillDESCSkillText; // 스킬 설명 및 현재 스킬 정보
    [SerializeField]
    protected Sprite icon; // 스킬 아이콘

    [Header("Skill Array")]
    [SerializeField]
    protected int[] skillMP; // 드는 마나  
    [SerializeField]
    protected int[] skillPoint; // 필요 스킬 포인트
    [SerializeField]
    protected int[] skillLevelCondition; // 필요 레벨
    [SerializeField]
    protected float[] cool; // 스킬 레벨 쿨타임  
   /* [SerializeField]
    protected bool IsInstanceEffect;*/
   /* [SerializeField]
    protected int skillEffectIndex;*/
    [SerializeField]
    protected int[] skillDamage;


    private float maxCoolTime; // 현재 스킬 레벨 쿨타임
    protected float coolTime; // 현재 돌아가고 있는 쿨타임
   
    public int Level { get { return level; }}
    public int SkillPoint { get; protected set; }
    public int SkillLevelCondition { get; protected set; }
    public int MAXLevel { get { return maxLevel; } }
    public int SkillMP { get; protected set; }
    public int SkillDamage { get; protected set; }

    public float CoolTime { get { return coolTime; } }

    public float MaxCoolTime { get { return maxCoolTime; } }

    public float SkillCoolTime { get; protected set; }

    public float SkillEffectIndex { get; protected set; }

    public Sprite Icon { get { return icon; } }

    public string SkillName { get { return skillName; } }

    public string SkillDESCSkillText { get { return skillDESCSkillText; } }


    public abstract Skill Init();

    public abstract void SKillDB(WarriorSkillData data);
    // 현재 스킬이 1레벨 이상일때 정보들
    public void SkillStat()
    {
      //  SkillEffectIndex = skillEffectIndex;
        SkillDamage = skillDamage[level];
        SkillMP = skillMP[level];
        SkillCoolTime = cool[level];
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
        DESCUpdate();
    }

    // 데이터를 받은 스킬 설명과 다른 자세한 부분들을 다른 string 변수에다 할당
    public void DESCUpdate()
    {
        if (level == 0)
        {
            skillDESCSkillText = string.Format(skillDESC, level, SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage), SkillMP, SkillCoolTime);

        }
        else
        {
            skillDESCSkillText = string.Format(skillDESC, level, SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage), SkillMP, SkillCoolTime);      
        }

    }
    public void LevelUp()
    {
        level++;
        SkillStat();
    }

    public void LevelReset()
    {
        level = 0;
        SkillStat();
       //SkillZeroStat();
    }
    
    public void Use()
    {
       // print($"스킬 데미지 : {SkillDamage}");
       // print($"스킬 마나 : {SkillMP}");
      //  print($"스킬 쿨타임 : {SkillCoolTime}");
       // PlayerController.instance._effector.InstanceEffect = skillEffectIndex;
        SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage);

     
        SkillManager.Skill.PlayerState.ChangeState((int)skillNumber);

        maxCoolTime = SkillCoolTime;
        coolTime = maxCoolTime;
        StartCoroutine(CoolTimeTimer());
    }

    // 스킬 쿨타임
    IEnumerator CoolTimeTimer()
    {
        while (coolTime > 0)
        {          
            coolTime -= Time.deltaTime;
            yield return null;
        }

        coolTime = 0;
    }

    // 스킬이 0레벨일떄
    public void SkillZeroStat()
    {
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }


    public virtual void BuffOn(int statsUp) { }
   
    public virtual void BuffOff(int statsUp) { }

    
}
