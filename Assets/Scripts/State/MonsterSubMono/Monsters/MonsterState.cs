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
    protected int maxHP;
    protected int maxMP;

    protected int attackCombo;

    protected int hp;
    protected int mp;
    protected int exp;

    protected int damage;

    protected int attackNumber;


    protected int attack;
    protected float attackSpeed;
    protected float delay;
    protected float abliltyDelay;
    protected int defense;
    protected int speed;
    protected int level;

    protected bool isAttack = true;
    [SerializeField]
    protected bool isDelay = true;
    [SerializeField]
    protected bool isAbliltyDelay = true;
    [SerializeField]
    protected bool isHitCheck;

    [SerializeField]
    protected bool isDeadCheck;

    protected int detectDistance;
    protected int attackDistance;


    protected Dictionary<int, State> state;
    [SerializeField]
    protected CharacterStatus expCharacter;
    StateMachine stateMachine;
    [SerializeField]
    protected Enum_MonsterState monsterState;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value < 0 ? 0 : value;

            if (hp >= maxHP)
            {
                hp = maxHP;
            }         
        }
    }

    public int MP
    {
        get
        {
            return mp;
        }
        set
        {
            mp = value < 0 ? 0 : value;

            if (mp >= maxMP)
            {
                mp = maxMP;
            }
        }
    }
    public int Attack
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
        }
    }

    public int Defense
    {
        get
        {
            return defense;
        }
        set
        {
            defense = value;
        }
    }

    public int Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }

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


    public Enum_MonsterState EnumMonsterState { get { return monsterState; } }

    public float AttackSpeed
    {
        get
        {
            return attackSpeed;
        }
        set
        {
            attackSpeed = value;
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
                    if (Vector3.Distance(_board.players[i].transform.position, gameObject.transform.position) < detectDistance)
                    {
                        _board._monsterMovement.characterPosition = _board.players[i].transform;
                    }
                }

            }
            else if (_board._monsterMovement.characterPosition != null)
            {
                if (!isHitCheck)
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
              if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) > detectDistance)
              {
                  ChangeState((int)Enum_MonsterState.Delay);
              }
              else if (!isHitCheck)
              {
                  if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < attackDistance &&
                  isAttack /*&& monsterState != Enum_MonsterState.Hit && monsterState != Enum_MonsterState.Dead*/)
                  {
                     

                      AttackNumber();
                      ChangeState((int)Enum_MonsterState.Attack);
                  }
                  else if (Vector3.Distance(_board._monsterMovement.characterPosition.position, gameObject.transform.position) < attackDistance &&
                  !isAttack)
                  {
                      ChangeState((int)Enum_MonsterState.Delay);
                  }
                  else
                  {
                      _board._monsterMovement.Move(speed);
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
                    if (Vector3.Distance(_board.players[i].transform.position, gameObject.transform.position) < detectDistance)
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
                        _board._monsterMovement.Return(speed);
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
                _board._monsterMovement.Attack(attackSpeed);
                _board._animationController.ChanageAttackAnimation(attackNumber);
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
            expCharacter.EXP += exp;
            gameObject.GetComponent<Collider>().enabled = false;        
            Invoke("SetActive", 3);
            gameObject.GetComponent<MonsterState>().enabled = false;
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

    public void DeadCheck(int damage, CharacterStatus expCharacter, float addforce)
    {

       // _board.DistributeEffectBurstStop();
        this.expCharacter = expCharacter;
        hp -= damage / defense;

        if (hp <= 0)
        {
            hp = 0;
            ChangeState((int)Enum_MonsterState.Dead);
        }
        else
        {
            _board._monsterMovement.characterPosition = expCharacter.transform;
            _board._animationController.RootMotion(false);
            _board._monsterMovement.AddForce(addforce);
            ChangeState((int)Enum_MonsterState.Hit);
        }

        //print(hp);
    }
    public int EffectDamage(int EffectDamage = 1)
    {
        return damage = attack * EffectDamage;
    }

    public void SetActive()
    {
        gameObject.SetActive(false);
    }

    public abstract void AttackNumber();
   
}
