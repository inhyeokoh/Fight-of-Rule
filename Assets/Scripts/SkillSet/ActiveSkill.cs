using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActiveSkill : Skill
{
    /*[SerializeField]
    private int[] skillDamage; 
    [SerializeField]
    WarriorSkill skillNumber;*/

    static bool settingComplete;

    private void Start()
    {
        if (level == 0)
        {
            SkillZeroStat();
        }
        else
        {
            SkillStat();
        }
    }

    // 스킬을 업하면 레벨이 올라가고 스킬들도 업한다

    public override Skill Init()
    {
        GameObject clone = Instantiate(gameObject);
        clone.transform.parent = SkillManager.Skill.transform.GetChild(0);
        ActiveSkill cloneCp = clone.GetComponent<ActiveSkill>();
        cloneCp.controller = PlayerController.instance;
        cloneCp.playerCapability = cloneCp.controller._playerCapability;
        cloneCp.playerStatus = cloneCp.controller._playerStat;

        return cloneCp;
        //return this;
    }
    public override void Setting()
    {
        if (!settingComplete)
        {
            //    print("패시브 스킬 세팅 완료");
            ActiveSkillSetting();
        }
        stateMachine = new StateMachine();
        state = new State(() => { skills[skillID].enterAction?.Invoke(this); }, () => { skills[skillID].fixedStay?.Invoke(this); }, () => { skills[skillID].stay?.Invoke(this); }, () => { skills[skillID].exit?.Invoke(this); });
        settingComplete = true;
    }
    public override void LevelUp()
    {
        level++;
        SkillStat();
    }
    public override void LevelReset()
    {
        level = 0;
        SkillStat();
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

    public override void Use()
    {
        SkillManager.Skill.PlayerStat.EffectDamage(SkillDamage);
        //SkillManager.Skill.PlayerState.ChangeState((int)skillNumber);
        Enter();
        base.Use();       
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

    public void ActiveSkillSetting()
    {
        skills.Add(0, new SkillState((skill) => {
            controller._animationController.ChangeMoveAnimation(0);
            controller._playerMovement.Direction(controller._playerMovement.TargetPosition);
            controller._playerMovement.IsKinematic(true);
            controller._playerState.UseSkill(true);
            controller._playerMovement.Stop();
            controller._animationController.ChangeSkillAnimation(0);
        }, (skill) => { }, (skill) => { },
        (skill) =>
        {
            controller._playerState.UseSkill(false);
            controller._playerMovement.IsKinematic(false);
        }));

        skills.Add(1, new SkillState((skill) => {
            controller._animationController.ChangeMoveAnimation(0);
            controller._playerMovement.Direction(controller._playerMovement.TargetPosition);
            controller._playerState.UseSkill(true);
            controller._playerMovement.IsKinematic(true);
            controller._animationController.RootMotion(true);
            controller._animationController.ChangeSkillAnimation(1);
        }, (skill) => { }, (skill) => { },
         (skill) =>
         {
             controller._animationController.RootMotion(false);
             controller._playerMovement.IsKinematic(false);
             controller._playerState.UseSkill(false);
         }));

        skills.Add(2, new SkillState((skill) => {
            controller._animationController.ChangeMoveAnimation(0);
            controller._playerMovement.Direction(PlayerController.instance._playerMovement.TargetPosition);
            controller._playerMovement.IsKinematic(true);
            controller._playerState.UseSkill(true);
            controller._playerMovement.Stop();
            controller._animationController.ChangeSkillAnimation(3);
        }, (skill) => { }, (skill) => { },
       (skill) =>
       {
           controller._playerMovement.IsKinematic(false);
           controller._playerState.UseSkill(false);
       }));

        skills.Add(3, new SkillState((skill) => {
            controller._animationController.ChangeMoveAnimation(0);
            controller._playerMovement.Direction(controller._playerMovement.TargetPosition);
            controller._playerState.UseSkill(true);
            controller._playerMovement.IsKinematic(true);
            controller._animationController.RootMotion(true);
            controller._animationController.ChangeSkillAnimation(2);
        }, (skill) => { }, (skill) => { },
       (skill) =>
       {
           controller._animationController.RootMotion(false);
           controller._playerMovement.IsKinematic(false);
           controller._playerState.UseSkill(false);
       }));

        skills.Add(4, new SkillState((skill) => {
            controller._animationController.ChangeMoveAnimation(0);
            controller._playerMovement.Direction(controller._playerMovement.TargetPosition);
            controller._playerState.UseSkill(true);
            controller._playerMovement.IsKinematic(false);
            controller._animationController.RootMotion(false);
            controller._animationController.ChangeSkillAnimation(4);
        }, (skill) => { },
        (skill) =>
        {
            if (Vector3.Distance(controller._playerMovement.playerTransform.position, controller._playerMovement.TargetPosition) > 0.1f)
            {
                //print(Vector3.Distance(_board._playerMovement.playerTransform.position, _board._playerMovement.TargetPosition));
                /*  _board._playerMovement.targetPosition.y += 0;
                  Vector3 direction = _board._playerMovement.TargetPosition - gameObject.transform.position;
                  _board._playerMovement.Rb.velocity = direction.normalized * Speed;*/

                controller._playerMovement.Move(controller._playerStat.Speed);


            }
            else
            {
                controller._playerMovement.Stop();
            }
        },
     (skill) =>
     {
         controller._playerMovement.TargetPosition = gameObject.transform.position;
         controller._playerMovement.Stop();

     }));
    }
}
