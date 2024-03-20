using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : Skill
{
    [Header("ActiveSkill")]
    // �������� �ٸ� ������
    [SerializeField]
    private int[] skillDamage; // ������   

   
   
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

    // ��ų�� ���ϸ� ������ �ö󰡰� ��ų�鵵 ���Ѵ�
    public override void LevelUp()
    {
        level++;
        SkillStat();        
    }

    // ��ų�� ����ϸ� �÷��̾ �ִ� �� ��ų ������������ ������ ��ų�������Ѵ�
    public override void Use()
    {
        print($"��ų ������ : {SkillDamage}");
        print($"��ų ���� : {SKillMP}");
        print($"��ų ��Ÿ�� : {SkillCoolTime}");
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


    // ���� ��ų�� 1���� �̻��϶� ������
    public override void SkillStat()
    {
        SkillEffectIndex = skillEffectIndex;
        SkillDamage = skillDamage[level];    
        SKillMP = skillMP[level];
        SkillCoolTime = cool[level];
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }


    // ��ų�� 0�����ϋ�
    public override void SkillInfo()
    {
        SkillLevelCondition = skillLevelCondition[level];
        SkillPoint = skillPoint[level];
    }

}
