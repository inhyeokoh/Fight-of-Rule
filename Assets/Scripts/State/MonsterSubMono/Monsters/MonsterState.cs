using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_MonsterState
{
    Idle,
    Delay,
    Move,
    Return,
    Attack,   
    Hit,
    GetUp,
    Fall,
    Dead,
}

public abstract class MonsterState : SubMono<MonsterController>
{
    protected bool isAttack = true;
    [SerializeField]
    protected bool isDelay = true;
    [SerializeField]
    protected bool isAbliltyDelay = true;
    [SerializeField]
    protected bool isHitCheck;
    [SerializeField]
    protected bool isDeadCheck;
   
    protected Dictionary<int, State> state;
    [SerializeField]
    public CharacterStatus expCharacter;
    StateMachine stateMachine;
    [SerializeField]
    protected Enum_MonsterState monsterState;
    public Enum_MonsterState EnumMonsterState { get { return monsterState; } }
    public static event Action<int> OnMonsterKilled;

    public bool IsAttack
    {
        get
        {
            return isAttack;
        }
        set
        {
            isAttack = value;
        }
    }

    public bool IsDelay
    {
        get
        {
            return isDelay;
        }
        set
        {
            isDelay = value;
        }
    }

    public bool IsAbliltyDelay
    {
        get
        {
            return isAbliltyDelay;
        }
        set
        {
            isAbliltyDelay = value;
        }
    }

    public bool IsHitCheck
    {
        get
        {
            return isHitCheck;
        }
        set
        {
            isHitCheck = value;
        }
    }

    public bool IsHDeadCheck
    {
        get
        {
            return isDeadCheck;
        }
        set
        {
            isDeadCheck = value;
        }
    }

    
    protected override void _Init()
    {
        state = new Dictionary<int, State>();
        stateMachine = new StateMachine(); 
    }

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
        
    }
    public void FixedUpdated()
    {
        stateMachine.FixedStay();
    }

    public void Updated()
    {
        stateMachine.Stay();
    }
    public virtual void StateAdd()
    {
        state.Add((int)Enum_MonsterState.Idle, new State(() =>
        {
            if (isDeadCheck)
            {
                print("Idle Enter");
                _board._animationController.ChangeTrrigerAnimation(Enum_MonsterState.Dead.ToString());
                return;
            }

            _board._monsterMovement.Stop();
            //_board._monsterMovement.IsKinematic(true);
            monsterState = Enum_MonsterState.Idle;
            _board._animationController.ChangeMoveAnimation(0);           
        },
        () => { },
        () =>
        {
            if (isDeadCheck)
            {
                print("Idle Update");
                return;
            }

            if (_board._monsterMovement.characterPosition == null)
            {
                for (int i = 0; i < _board.players.Length; i++)
                {
                    if (Vector3.Distance(_board.players[i].transform.position, gameObject.transform.position) < _board._monsterStatus.DetectDistance)
                    {
                        _board._monsterMovement.characterPosition = _board.players[i].transform;
                    }
                }

            }
            else if (_board._monsterMovement.characterPosition != null)
            {
                if (!isHitCheck)
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
                        _board._monsterMovement.Rotation();
                    }
                    else
                    {
                        ChangeState((int)Enum_MonsterState.Delay);
                    }
                }                              
            }
        },
        () => { /*_board._monsterMovement.IsKinematic(false);*/ }));
        state.Add((int)Enum_MonsterState.Move, new State(() => { _board._animationController.ChangeMoveAnimation(1); }, () => { },
          () =>
          {
              if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) > _board._monsterStatus.DetectDistance)
              {
                  ChangeState((int)Enum_MonsterState.Delay);
              }
              else if (!isHitCheck)
              {
                  if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < _board._monsterStatus.AttackDistance &&
                  isAttack /*&& monsterState != Enum_MonsterState.Hit && monsterState != Enum_MonsterState.Dead*/)
                  {
                     

                      AttackNumber();
                      ChangeState((int)Enum_MonsterState.Attack);
                  }
                  else if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < _board._monsterStatus.AttackDistance &&
                  !isAttack)
                  {
                      ChangeState((int)Enum_MonsterState.Delay);
                  }
                  else
                  {
                      _board._monsterMovement.Move(_board._monsterStatus.Speed);
                  }
              }
          }, () =>
          {
               
           }));
        state.Add((int)Enum_MonsterState.Return, new State(() =>
        {
            monsterState = Enum_MonsterState.Return;
            _board._monsterMovement.characterPosition = null;
            _board._animationController.ChangeMoveAnimation(1);
        }, () => { },
            () =>
            {
                for (int i = 0; i < _board.players.Length; i++)
                {
                    if (Vector3.Distance(_board.players[i].transform.position, gameObject.transform.position) < _board._monsterStatus.DetectDistance)
                    {
                        _board._monsterMovement.characterPosition = _board.players[i].transform;              
                        ChangeState((int)Enum_MonsterState.Delay);
                    }                                               
                }

                if (_board._monsterMovement.characterPosition == null)
                {
                    if (Vector3.Distance(_board._monsterMovement.spawn, gameObject.transform.position) < 0.4f)
                    {
                        ChangeState((int)Enum_MonsterState.Idle);
                    }
                    else
                    {                       
                        _board._monsterMovement.Return(_board._monsterStatus.Speed);
                    }
                }


            }, () =>
            {

            })
        {

        });
        state.Add((int)Enum_MonsterState.Attack, new State(() =>
        { 

            if (!isHitCheck)
            {
           
                monsterState = Enum_MonsterState.Attack;
                _board._animationController.ChangeMoveAnimation(0);
                _board._monsterMovement.Stop();
                //_board._monsterMovement.IsKinematic(true);
                _board._monsterMovement.Attack(_board._monsterStatus.AttackSpeed);
                _board._animationController.ChanageAttackAnimation(_board._monsterStatus.AttackNumber);
                _board._effector.InstanceEffect = 0;
            }              
        },
      () => { },
      () => {  },
      () =>
      {
         // _board._monsterMovement.IsKinematic(false);
      }));
        state.Add((int)Enum_MonsterState.Hit, new State(() => 
        {
            _board._monsterMovement.HitDelay(1.5f);        
            isHitCheck = true;
            monsterState = Enum_MonsterState.Hit;

            _board._monsterMovement.Stop();
            _board._animationController.ChangeTrrigerAnimation(Enum_MonsterState.Hit.ToString());
        }, () => { }, 
        () => 
        {
            if (!isHitCheck && !isDeadCheck)
            {
                ChangeState((int)Enum_MonsterState.Idle);
            }
        }, 
        () => 
        {        
        }));
        state.Add((int)Enum_MonsterState.GetUp, new State(() => { }, () => { }, () => { }, () => { }));
        state.Add((int)Enum_MonsterState.Fall, new State(() => { }, () => { }, () => { }, () => { }));
        
        state.Add((int)Enum_MonsterState.Dead, new State(() =>            
        {
            isDeadCheck = true;
            _board._animationController.ChangeTrrigerAnimation(Enum_MonsterState.Dead.ToString());
            _board._monsterMovement.Stop();
            _board._monsterMovement.Dead();
            monsterState = Enum_MonsterState.Dead;
            expCharacter.EXP += _board._monsterStatus.exp;

            gameObject.GetComponent<Collider>().enabled = false;        
            Invoke("SetActive", 3);
            gameObject.GetComponent<MonsterState>().enabled = false;

            _board._monsterItemDrop.ItemDrop();
            OnMonsterKilled?.Invoke(_board.monsterDB.monster_id);
        }, () => { }, () => {  }, 
        () => 
        {
            print("Dead에서 빠져나옴");       
        }));
    }
    public void ChangeState(int newState)
    {
        stateMachine.ChangeState(state[newState]);
    }

    private void Alive()
    {

    }

    public void SetActive()
    {
        gameObject.SetActive(false);
    }

    public abstract void AttackNumber();
   
}
