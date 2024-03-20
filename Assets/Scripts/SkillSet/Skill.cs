using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // 현재 스킬 레벨
    [SerializeField]
    protected int level = 1;

    // 최대 레벨
    [SerializeField]
    protected int maxLevel;
    public int Level { get { return level; } set { level = value; } }
    
    [Header("Skill ID")]
    [SerializeField]
    protected int skillID; //스킬 번호
    
    [Header("Skill String")]
    [SerializeField]
    protected string skillName; //스킬 이름
    [SerializeField]
    protected string skillDESC; // 스킬 설명서 
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
    protected float[] cool; // 쿨타임  

    [SerializeField]
    protected bool IsInstanceEffect;

    [SerializeField]
    protected int skillEffectIndex;

    public int SkillPoint { get; protected set; }
    public int SkillLevelCondition { get; protected set; }
    public int MAXLevel { get { return maxLevel; } }
    public int SKillMP { get; protected set; }
    public int SkillDamage { get; protected set; }

    public float SkillCoolTime { get; protected set; }

    public float SkillEffectIndex { get; protected set; }

    public Sprite Icon { get { return icon; } }

    public string SkillName { get { return skillName; } }

    public string SKillDESC { get { return skillDESC; } }


    public abstract Skill Init();
    public abstract void Use();

    public abstract void LevelUp();

    public abstract void SkillStat();

    public abstract void SkillInfo();

    public virtual void BuffOn(int statsUp) { }
   
    
    public virtual void BuffOff(int statsUp) { }

  
    
    
    // 현재는 없어도 되는 메서드
    public void SkillReset()
    {
        level = -1;
    }

   

}
