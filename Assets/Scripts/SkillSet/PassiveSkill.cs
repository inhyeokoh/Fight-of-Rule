using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PassiveSkill : Skill
{
    static Dictionary<int, PassiveSkillState> passiveSkills = new Dictionary<int, PassiveSkillState>();
    static bool settingComplete;
    State state;
    StateMachine stateMachine;


    private bool passiveOn;
    public bool PassiveOn { get { return passiveOn; } }

    public class PassiveSkillState
    {
        public Action<PassiveSkill> enterAction;
        public Action<PassiveSkill> fixedStay;
        public Action<PassiveSkill> stay;
        public Action<PassiveSkill> exit;

        public PassiveSkillState(Action<PassiveSkill> _enterAction, Action<PassiveSkill> _fixedStay, Action<PassiveSkill> _stay, Action<PassiveSkill> _exit )
        {
            enterAction = _enterAction;
            fixedStay = _fixedStay;
            stay = _stay;
            exit = _exit;
        }
    }



    [SerializeField]
    private int[] statsUp;

    public override Skill Init()
    {
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = SkillManager.Skill.transform.GetChild(0);
        PassiveSkill cloneCp = clone.GetComponent<PassiveSkill>();
        cloneCp.controller = PlayerController.instance;
        cloneCp.playerCapability = cloneCp.controller._playerCapability;
        cloneCp.playerStatus = cloneCp.controller._playerStat;

        return cloneCp;

        // return this;
    }
    public override void Setting()
    {
        if (!settingComplete)
        {
        //    print("패시브 스킬 세팅 완료");
            PassiveSkillSetting();
        }
        stateMachine = new StateMachine();
        state = new State(() => { passiveSkills[skillID].enterAction?.Invoke(this); }, () => { passiveSkills[skillID].fixedStay?.Invoke(this); }, () => { passiveSkills[skillID].stay?.Invoke(this); }, () => { passiveSkills[skillID].exit?.Invoke(this); });
    }

    public override void LevelUp()
    {
        if (level == 0)
        {
            passiveOn = true;
        }
        else
        {
            Exit();
        }
        level++;
        SkillStat();
        Enter();
    }
    public override void LevelReset()
    {
        Exit();
        level = 0;
        SkillStat();

        passiveOn = false;
    }
    public void Enter()
    {
        stateMachine.EnterState(state);
    }

    public void FixedStay()
    {
        stateMachine.FixedStay();
    }
    public void Stay()
    {
        stateMachine.Stay();
    }
    public void Exit()
    {
        stateMachine.SkillExitState();
    }
    public override void SKillDB(WarriorSkillData data)
    {
        skillID = data.id;
        skillName = data.name;
        skillDESC = data.desc;
        icon = data.icon;
        skillType = (Enum_SkillType)Enum.Parse(typeof(Enum_SkillType), data.skillType);
        skillNumber = data.number;
        maxLevel = data.maxLevel;
        skillLevelCondition = data.levelCondition;
        skillPoint = data.skillPoint;
        skillDuration = data.skillDuration;
        skillMaxHP = data.skillMaxHP;
        skillAttack = data.skillAttack;
        skillDefense = data.skillDefense;
        skillSpeed = data.skillSpeed;
        skillAttackSpeed = data.skillAttackSpeed;
        skillMaxMP = data.skillMaxMP;
        skillMP = data.skillMP;
        cool = data.skillCool;
        skillDamage = data.skillDamage;
    }
   
    public void PassiveSkillSetting() 
    {
        passiveSkills.Add(5, new PassiveSkillState((skill) =>
        {
            playerStatus.SumAttack += skill.SkillAttack;
            print($"공격력{skill.SkillAttack}만큼 오름 ");
        },
        (skill) => { }, (skill) => { print("작동중"); }, (skill) => { playerStatus.SumAttack -= skill.SkillAttack; }
        ));

    }

}
