using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WarriorSkill
{
    AssaultBlade = 9,
    DubleSlash,
    DivingSlash,
    SwordStorm,
    WheelWind,
}
public class Warrior : CharacterState
{
    public int a;

    public override void StateAdd()
    {
        state.Add((int)WarriorSkill.AssaultBlade, new State(() => {
           // _board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            _board._playerMovement.IsKinematic(true);
            skillUseCheck = true;
            _board._playerMovement.Stop();
            _board._animationController.ChangeSkillAnimation(0);         
        }, () => { }, () => { }, 
        () => 
        {
            _board._playerMovement.TargetPosition = gameObject.transform.position;       
            skillUseCheck = false;
            _board._playerMovement.IsKinematic(false);
        }));
     
        state.Add((int)WarriorSkill.DivingSlash, new State(() => {
            //_board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            skillUseCheck = true;
            _board._playerMovement.IsKinematic(true);
            _board._animationController.RootMotion(true);
            _board._animationController.ChangeSkillAnimation(1);
        }, () => { }, () => { },
         () =>
         {
             _board._playerMovement.TargetPosition = gameObject.transform.position;             
             _board._animationController.RootMotion(false);
             _board._playerMovement.IsKinematic(false);
             skillUseCheck = false;
         }));
        
        state.Add((int)WarriorSkill.DubleSlash, new State(() => {
            //_board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            _board._playerMovement.IsKinematic(true);
            skillUseCheck = true;
            _board._playerMovement.Stop();        
            _board._animationController.ChangeSkillAnimation(3);
        }, () => { }, () => { },
       () =>
       {
           _board._playerMovement.TargetPosition = gameObject.transform.position;       
           _board._playerMovement.IsKinematic(false);
           skillUseCheck = false;
       }));

        state.Add((int)WarriorSkill.SwordStorm, new State(() => {
            //_board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            skillUseCheck = true;
            _board._playerMovement.IsKinematic(true);
            _board._animationController.RootMotion(true);
            _board._animationController.ChangeSkillAnimation(2);
        }, () => { }, () => { },
       () =>
       {
           _board._playerMovement.TargetPosition = gameObject.transform.position;
           _board._animationController.RootMotion(false);
           _board._playerMovement.IsKinematic(false);
           skillUseCheck = false;
       }));

        state.Add((int)WarriorSkill.WheelWind, new State(() => {
            //_board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            skillUseCheck = true;
            _board._playerMovement.IsKinematic(false); 
            _board._animationController.RootMotion(false);
            _board._animationController.ChangeSkillAnimation(4);
        }, () => { }, 
        () => 
        {
            if (Vector3.Distance(_board._playerMovement.playerTransform.position, _board._playerMovement.TargetPosition) > 0.1f)
            {
                print(Vector3.Distance(_board._playerMovement.playerTransform.position, _board._playerMovement.TargetPosition));
                /*  _board._playerMovement.targetPosition.y += 0;
                  Vector3 direction = _board._playerMovement.TargetPosition - gameObject.transform.position;
                  _board._playerMovement.Rb.velocity = direction.normalized * Speed;*/

                _board._playerMovement.Move(Speed);


            }
            else
            {
                _board._playerMovement.Stop();
            }
        },
     () =>
     {
         _board._playerMovement.TargetPosition = gameObject.transform.position;
         _board._playerMovement.Stop();
         
     }));



        base.StateAdd();
        ChangeState((int)Enum_CharacterState.Idle);

    }

    public override void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defenes, bool firstLevel)
    {
        base.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenes, firstLevel);
    }

    protected override void _Init()
    {
        Attack = 700;
        attackCombo = 2;      
        base._Init();
    }

    protected override void _Excute()
    {

    }

    protected override void _Clear()
    {

    }

}
