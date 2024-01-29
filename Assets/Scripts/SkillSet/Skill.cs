using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{

    protected int level = 2;

    public int Level { get { return level; } }
    
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


    public int SKillMP { get; protected set; }
    public int SkillDamage { get; protected set; }

    public float SkillCoolTime { get; protected set; }


    public abstract Skill Init();
    public abstract void Use();

    public abstract void LevelUp();

    public abstract void SkillStat();


    public virtual void BuffOn(int statsUp) { }
   
    
    public virtual void BuffOff(int statsUp) { }

    public void SkillReset()
    {
        level = -1;
    }

}
