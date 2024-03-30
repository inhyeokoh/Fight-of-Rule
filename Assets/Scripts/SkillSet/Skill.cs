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

    [SerializeField]
    protected WarriorSkill skillNumber;

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
    protected float[] cool; // ��ų ���� ��Ÿ��  
    [SerializeField]
    protected bool IsInstanceEffect;
    [SerializeField]
    protected int skillEffectIndex;
    [SerializeField]
    protected int[] skillDamage;


    private float maxCoolTime; // ���� ��ų ���� ��Ÿ��
    protected float coolTime; // ���� ���ư��� �ִ� ��Ÿ��
   
    public int Level { get { return level; }}
    public int SkillPoint { get; protected set; }
    public int SkillLevelCondition { get; protected set; }
    public int MAXLevel { get { return maxLevel; } }
    public int SKillMP { get; protected set; }
    public int SkillDamage { get; protected set; }

    public float CoolTime { get { return coolTime; } }

    public float MaxCoolTime { get { return maxCoolTime; } }

    public float SkillCoolTime { get; protected set; }

    public float SkillEffectIndex { get; protected set; }

    public Sprite Icon { get { return icon; } }

    public string SkillName { get { return skillName; } }

    public string SKillDESC { get { return skillDESC; } }


    public abstract Skill Init();
    // ���� ��ų�� 1���� �̻��϶� ������
    public void SkillStat()
    {
        SkillEffectIndex = skillEffectIndex;
        SkillDamage = skillDamage[level];
        SKillMP = skillMP[level];
        SkillCoolTime = cool[level];
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }
    public void LevelUp()
    {
        level++;
        SkillStat();
    }

    public void LevelReset()
    {
        level = 0;
        SkillZeroStat();
    }
    
    public void Use()
    {
        print($"��ų ������ : {SkillDamage}");
        print($"��ų ���� : {SKillMP}");
        print($"��ų ��Ÿ�� : {SkillCoolTime}");
        PlayerController.instance._effector.InstanceEffect = skillEffectIndex;
        SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage);
        SkillManager.Skill.PlayerState.ChangeState((int)skillNumber);

        maxCoolTime = SkillCoolTime;
        coolTime = maxCoolTime;
        StartCoroutine(CoolTimeTimer());
    }

    // ��ų ��Ÿ��
    IEnumerator CoolTimeTimer()
    {
        while (coolTime > 0)
        {          
            coolTime -= Time.deltaTime;
            yield return null;
        }

        coolTime = 0;
    }

    // ��ų�� 0�����ϋ�
    public void SkillZeroStat()
    {
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }


    public virtual void BuffOn(int statsUp) { }
   
    public virtual void BuffOff(int statsUp) { }


}
