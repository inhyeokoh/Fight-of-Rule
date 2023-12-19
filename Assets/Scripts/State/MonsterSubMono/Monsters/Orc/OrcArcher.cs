using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcArcher : Orc
{
    public override void StateAdd()
    {
        state.Add((int)Enum_MonsterState.Delay, new State(() =>
        {
            if (isDeadCheck)
            {
                _board._animationController.ChangeTrrigerAnimation(Enum_MonsterState.Dead.ToString());
                return;
            }

            _board._animationController.ChangeMoveAnimation(0);
            _board._monsterMovement.Stop();
            //_board._monsterMovement.IsKinematic(true);

            if (isDelay)
            {
                _board._monsterMovement.Delay(delay);
            }
        },
         () =>
         {

         },
         () =>
         {
             if (isDeadCheck)
             {                
                 return;
             }

             if (!isDelay)
             {
                 _board._monsterMovement.Stop();
                 _board._monsterMovement.Rotation();

             }
             else if (isDelay && !isHitCheck)
             {
                 if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) > detectDistance)
                 {
                     ChangeState((int)Enum_MonsterState.Return);
                 }
                 else
                 {
                     if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < attackDistance &&
                     isAttack)
                     {
                         AttackNumber();
                         ChangeState((int)Enum_MonsterState.Attack);
                     }
                     else if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < attackDistance &&
                     !isAttack)
                     {
                         ChangeState((int)Enum_MonsterState.Idle);
                     }
                     else
                     {                                                 
                         ChangeState((int)Enum_MonsterState.Move);                        
                     }
                 }
             }
         }, () => {/* _board._monsterMovement.IsKinematic(false);*/ }));
        base.StateAdd();
        ChangeState((int)Enum_CharacterState.Idle);
    }
    protected override void _Init()
    {
        maxHP = 500;
        maxMP = 300;
        hp = maxHP;
        mp = maxMP;
        exp = 30;
        attack = 20;
        attackNumber = 0;
        attackSpeed = 5f;
        delay = 1f;
        defense = 5;
        speed = 10;
        level = 5;



        detectDistance = 30;
        attackDistance = 15;
        
        base._Init();
    }

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

}
