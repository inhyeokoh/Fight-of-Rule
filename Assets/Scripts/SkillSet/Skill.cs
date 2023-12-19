using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{

    protected int level = 2;

    public int Level { get { return level; } }
    
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
