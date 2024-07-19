using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Enum_SkillType
{
    PassiveSkill,
    ActiveSkill,
}
public abstract class Skill : MonoBehaviour
{
    protected static Dictionary<int, SkillState> skills = new Dictionary<int, SkillState>();
    protected State state;
    protected StateMachine stateMachine;
   
    public class SkillState
    {
        public Action<Skill> enterAction;
        public Action<Skill> fixedStay;
        public Action<Skill> stay;
        public Action<Skill> exit;

        public SkillState(Action<Skill> _enterAction, Action<Skill> _fixedStay, Action<Skill> _stay, Action<Skill> _exit)
        {
            enterAction = _enterAction;
            fixedStay = _fixedStay;
            stay = _stay;
            exit = _exit;
        }
    }

    public static bool playerInfo;


    public PlayerController controller;
    public CharacterStatus playerStatus;
    public CharacterCapability playerCapability;


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
    protected float[] skillDuration;
    [SerializeField]
    protected int[] skillMaxHP;
    [SerializeField]
    protected int[] skillMaxMP;
    [SerializeField]
    protected int[] skillAttack;
    [SerializeField]
    protected int[] skillDefense;
    [SerializeField]
    protected int[] skillSpeed;
    [SerializeField]
    protected int[] skillAttackSpeed;
    [SerializeField]
    protected int[] skillMP;// 드는 마나  
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

    [SerializeField]
    protected Enum_SkillType skillType; 

    private float maxCoolTime; // 현재 스킬 레벨 쿨타임
    protected float coolTime; // 현재 돌아가고 있는 쿨타임
   
    public int Level { get { return level; }}
    public int SkillPoint { get; protected set; }
    public int SkillLevelCondition { get; protected set; }
    public int MAXLevel { get { return maxLevel; } }
    public int SkillMP { get; protected set; }
    public int SkillDamage { get; protected set; }

    public float SkillDuration { get; protected set; }

    public int SkillMaxHP { get; protected set; }
    public int SkillMaxMP { get; protected set; }

    public int SkillAttack { get; protected set; }

    public int SkillDefense { get; protected set; }

    public int SkillSpeed { get; protected set; }

    public int SkillAttackSpeed { get; protected set; }



    public float CoolTime { get { return coolTime; } }

    public float MaxCoolTime { get { return maxCoolTime; } }

    public float SkillCoolTime { get; protected set; }

    public float SkillEffectIndex { get; protected set; }

    public Sprite Icon { get { return icon; } }

    public string SkillName { get { return skillName; } }

    public string SkillDESCSkillText { get { return skillDESCSkillText; } }

    public Enum_SkillType SkillType { get { return skillType; } }
    public abstract void SKillDB(WarriorSkillData data);
    // 현재 스킬이 1레벨 이상일때 정보들
    public void SkillStat()
    {
        Check();
        DESCUpdate();
    }

    public void Check()
    {
        SkillDuration = NullCheck(skillDuration);
        SkillMaxHP = NullCheck(skillMaxHP);
        SkillMaxMP = NullCheck(skillMaxMP);
        SkillAttack = NullCheck(skillAttack);
        SkillDefense = NullCheck(skillDefense);
        SkillSpeed = NullCheck(skillSpeed);
        SkillAttackSpeed = NullCheck(skillAttackSpeed);
        SkillDamage = NullCheck(skillDamage);
        SkillMP = NullCheck(skillMP);
        SkillCoolTime = NullCheck(cool);
        SkillLevelCondition = NullCheck(skillLevelCondition);
        SkillPoint = NullCheck(skillPoint);
    }

    public T NullCheck<T>(T[] checks)
    {
        if (checks == null)
        {
            return default(T);
        }

        T stat = checks[level];

        return stat;
    }

    // 데이터를 받은 스킬 설명과 다른 자세한 부분들을 다른 string 변수에다 할당
    public void DESCUpdate()
    {
        if (level == 0)
        {
            skillDESCSkillText = string.Format(skillDESC, level, SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage), SkillMP, SkillCoolTime, SkillDuration, SkillMaxHP,
                SkillMaxMP, SkillAttack, SkillDefense, SkillSpeed, SkillAttackSpeed);
        }
        else
        {
            skillDESCSkillText = string.Format(skillDESC, level, SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage), SkillMP, SkillCoolTime, SkillDuration, SkillMaxHP,
               SkillMaxMP, SkillAttack, SkillDefense, SkillSpeed, SkillAttackSpeed);
        }

    }
    public abstract void LevelReset();
    public virtual void Use() 
    {
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

    public abstract Skill Init();

    public abstract void LevelUp();
    public abstract void Setting();   
}
