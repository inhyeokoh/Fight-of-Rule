using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWarrior : Orc
{
    public override void StateAdd()
    {
        //어택 로직

        //캐릭터가 공격이 여러개가 있을거임

        //그래서 랜덤랭지를 이용해 확률을 표현

        //하지만 이걸 Update에다 해놓으면 랜덤랭지는 계속 발동될것

        //그래서 처음 호출할때만 확률을 표시함

        //근데 그러면 랜덤랭지는 한번만 호출이 될것이고 Update에선 똑같은 그 공격패턴만 나오게될것

        //그래서 공격을 하고 공격카운트가 다되고 캐릭터가 있을시에 다시 하는게 낫지 않나 이거임

        //공격에 다가왔을땐 가만히 있다가 떄리기



        // 몬스터가 딜레이없이 바로 행동하지 못하게 딜레이 상태패턴
        state.Add((int)Enum_MonsterState.Delay, new State(() =>
        {
            if (isDeadCheck)
            {
                print("Delay Enter");
                _board._animationController.ChangeTrrigerAnimation(Enum_MonsterState.Dead.ToString());
                return;
            }

            monsterState = Enum_MonsterState.Delay;
            _board._animationController.ChangeMoveAnimation(0);
            _board._monsterMovement.Stop();
           // _board._monsterMovement.IsKinematic(true);

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
                print("Delay Update");             
                return;
            }

            if (!isDelay)
            {
                _board._monsterMovement.Rotation();
                _board._monsterMovement.Stop();
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
                        print("delay");
                      

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
                        float rush = Random.Range(0, 100.1f);
                        if (rush <= 50f && isAbliltyDelay)
                        {
                            ChangeState((int)Enum_OrcState.Rush);
                        }
                        else
                        {
                            ChangeState((int)Enum_MonsterState.Move);
                        }                   
                    }
                }
            }
        }, () => { /*_board._monsterMovement.IsKinematic(false);*/ }));

        // 오크의 스킬
        state.Add((int)Enum_OrcState.Rush, new State(() => 
        {
            _board._monsterMovement.Stop();        
            _board._animationController.ChangeMoveAnimation(0);
            _board._monsterStatus.EffectDamage(4);
            _board._effector.InstanceEffect = 0;
            _board._monsterMovement.Ablilty(_board._monsterStatus.AbliltyDelay);
            _board._animationController.ChangeAbliltyAnimation(0);
        }, () => { }, 
        () => 
        { 
            _board._monsterMovement.Slerp();
        }, () => {  }));
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

    public override void AttackNumber()
    {
        float attack = Random.Range(0, 100.1f);

        if (attack <= 20f)
        {
            _board._monsterStatus.AttackNumber = 1;
            _board._monsterStatus.EffectDamage(2);
            //print(damage);
        }
        else
        {
            _board._monsterStatus.AttackNumber = 0;
            _board._monsterStatus.EffectDamage();
            //print(damage);
        }

        //print(attack);
    }
}
