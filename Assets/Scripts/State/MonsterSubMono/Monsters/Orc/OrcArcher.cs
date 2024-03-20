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
                _board._monsterMovement.Delay(_board._monsterStatus.Delay);
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
                 if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) > _board._monsterStatus.DetectDistance)
                 {
                     ChangeState((int)Enum_MonsterState.Return);
                 }
                 else
                 {
                     if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < _board._monsterStatus.AttackDistance &&
                     isAttack)
                     {
                         AttackNumber();
                         ChangeState((int)Enum_MonsterState.Attack);
                     }
                     else if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < _board._monsterStatus.AttackDistance &&
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
        base._Init();
    }

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

}
