using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // ���� ��ų ����
    [SerializeField]
    protected int level = 1;

    // �ִ� ����
    [SerializeField]
    protected int maxLevel;
    public int Level { get { return level; } set { level = value; } }
    
    [Header("Skill ID")]
    [SerializeField]
    protected int skillID; //��ų ��ȣ
    
    [Header("Skill String")]
    [SerializeField]
    protected string skillName; //��ų �̸�
    [SerializeField]
    protected string skillDESC; // ��ų ���� 
    [SerializeField]
    protected Sprite icon; // ��ų ������

    [Header("Skill Array")]
    [SerializeField]
    protected int[] skillMP; // ��� ����  
    [SerializeField]
    protected int[] skillPoint; // �ʿ� ��ų ����Ʈ
    [SerializeField]
    protected int[] skillLevelCondition; // �ʿ� ����
    [SerializeField]
    protected float[] cool; // ��Ÿ��  

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

  
    
    
    // ����� ��� �Ǵ� �޼���
    public void SkillReset()
    {
        level = -1;
    }

   

}
