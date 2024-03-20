using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcWarrior : Orc
{
    public override void StateAdd()
    {
        //���� ����

        //ĳ���Ͱ� ������ �������� ��������

        //�׷��� ���������� �̿��� Ȯ���� ǥ��

        //������ �̰� Update���� �س����� ���������� ��� �ߵ��ɰ�

        //�׷��� ó�� ȣ���Ҷ��� Ȯ���� ǥ����

        //�ٵ� �׷��� ���������� �ѹ��� ȣ���� �ɰ��̰� Update���� �Ȱ��� �� �������ϸ� �����Եɰ�

        //�׷��� ������ �ϰ� ����ī��Ʈ�� �ٵǰ� ĳ���Ͱ� �����ÿ� �ٽ� �ϴ°� ���� �ʳ� �̰���

        //���ݿ� �ٰ������� ������ �ִٰ� ������



        // ���Ͱ� �����̾��� �ٷ� �ൿ���� ���ϰ� ������ ��������
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

        // ��ũ�� ��ų
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
