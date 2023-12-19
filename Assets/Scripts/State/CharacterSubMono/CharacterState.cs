using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Enum_CharacterState
{
    Idle,
    Move,
    Delay,
    Attack,
    Avoid,
    Hit,
    GetUp,
    Fall,
    Dead,
}


public abstract class CharacterState : SubMono<PlayerController>
{
    //Caracter
    //캐릭터에서는 캐릭터에 상태가 가장 중심적으로 있어야하는 기능
    //캐릭터의 네트워크에 연결을해서 hp mp exp damage defense speed level 렌더러 백터값 물리엔진을 받아온다.
    Collider playerCollider;

    private int maxHP = 50;
    private int maxMP = 50;
    private int maxEXP = 100;

    protected int attackCombo;
    [SerializeField]
    protected bool skillUseCheck;

    private int hp = 50;
    private int mp = 50;
    private int exp = 0;
    private int attack = 5;
    private int attackSpeed;
    private int defense = 3;
    private int speed = 10;
    private int level = 1;
    private int skillDamage;

    [SerializeField]
    private bool isAvoid = false;

    [SerializeField]
    private bool moveStateCheck;


    [SerializeField]
    private Enum_CharacterState characterState;


    public bool IsAvoid { get { return isAvoid; } set { isAvoid = value; } }

    public int MaxHP { get { return maxHP; } }
    public int MaxMP { get { return maxMP; } }

    public int MaxEXP { get { return maxEXP; } }

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

            if(hp == 0)
            {
                Dead();
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

    public int EXP
    {
        get
        {
            return exp;
        }
        set
        {
            if (level == 50)
            {
                return;
            }
            else
            {
                exp = value;                            
               
                if (exp >= maxEXP)             
                {                             
                    {                    
                        level++;                    
                        _board.LevelUpCheck(level);                 
                    }
                }
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

    public int SkillDamage
    {
        get
        {
            return skillDamage;
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

    public int AttackSpeed
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

    public bool SkillUseCheck { get { return skillUseCheck; } }

    public Enum_CharacterState CharacterStates { get { return characterState; } }

    private StateMachine stateMachine = new StateMachine();
    public Dictionary<int, State> state = new Dictionary<int, State>();


    public virtual void StateAdd()
    {
      
        state.Add((int)Enum_CharacterState.Idle, new State(() => 
        {
            moveStateCheck = _board._animationController.MoveCheck();

            if (moveStateCheck)
            {
                ChangeState((int)Enum_CharacterState.Move);
            }
            else
            {
                _board._animationController.RootMotion(false);
                _board._playerMovement.Stop();
                _board._playerMovement.IsKinematic(true);
                characterState = Enum_CharacterState.Idle;
                //_board._animationController.ChangeMoveAnimation(0);
            }      
        }, 
        () => { }, 
        () => { _board._playerMovement.Stop(); }, 
        () => { _board._playerMovement.IsKinematic(false); }));

        state.Add((int)Enum_CharacterState.Move, new State(() =>
        {
            _board._animationController.RootMotion(false);
            characterState = Enum_CharacterState.Move; 
            _board._animationController.ChangeMoveAnimation(1); 
        },
        () => { },
        () =>
        {
            if (Vector3.Distance(_board._playerMovement.playerTransform.position, _board._playerMovement.targetPosition) > 0.1f)
            {
                _board._playerMovement.Move(speed);
            }
            else
            {
                _board._playerMovement.Stop();
                _board._playerState.ChangeState((int)Enum_CharacterState.Idle);
            }
        },
        () => 
        {
            _board._animationController.ChangeMoveAnimation(0);
        }));
        state.Add((int)Enum_CharacterState.Attack, new State(() => 
        {
            EffectDamage();
            _board._playerMovement.IsKinematic(true);
            _board._animationController.ChangeMoveAnimation(0);
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition); 
            characterState = Enum_CharacterState.Attack; 
            _board._playerMovement.Stop(); 
            _board._playerMovement.AttackCombo(attackCombo);
        },
        () => { }, 
        () => { }, 
        () => { _board._playerMovement.IsKinematic(false); }));

        state.Add((int)Enum_CharacterState.Avoid, new State(() =>
        {
            //_board._animationController.ChangeMoveAnimation(0);            
            isAvoid = true;
            _board._playerMovement.Direction(_board._playerMovement.TargetPosition);
            characterState = Enum_CharacterState.Avoid; playerCollider.enabled = false;
            _board._animationController.ChangeTrrigerAnimation(Enum_CharacterState.Avoid.ToString());
        },
        () => { },
        () =>
        { 
            if (isAvoid)
            {
                _board.DistributeEffectBurstStop();
                _board._playerMovement.Avoid(speed);
            }
            else
            {
                _board._playerState.ChangeState((int)Enum_CharacterState.Idle);
            }        
        },
        () => 
        { 
            playerCollider.enabled = true; 
        }));

        state.Add((int)Enum_CharacterState.Dead, new State(() =>
        { 
            characterState = Enum_CharacterState.Dead;
            _board._animationController.ChangeTrrigerAnimation(Enum_CharacterState.Dead.ToString());
            _board._playerMovement.Stop();
        },
        () => { },
        () => { },
        () => { }));

        state.Add((int)Enum_CharacterState.Delay, new State(() =>
        {
            characterState = Enum_CharacterState.Delay;         
            _board._playerMovement.Stop();
        },
() => { },

() => { _board._playerMovement.Stop(); },
() => {   skillUseCheck = false;}));

    }

    protected override void _Init()
    {
        playerCollider = gameObject.GetComponent<Collider>();
    }

    public void FixedUpdated()
    {
        stateMachine.FixedStay();
    }
    public void Updated()
    {
        stateMachine.Stay();
    }

    public void ChangeState(int newState)
    {
        //print($"현재상태 {(Enum_CharacterState)newState}");
        stateMachine.ChangeState(state[newState]);
    }


    public virtual void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defense, bool firstLevel)
    {
        int previousEXP = this.maxEXP;

        if (firstLevel)
        {
            this.maxEXP = maxEXP;
        }
        else
        {
            this.maxEXP += maxEXP;
        }

        this.maxHP += maxHP;
        this.maxMP += maxMP;
        this.attack += attack;
        this.defense += defense;

        HP = this.maxHP;
        MP = this.maxMP;
        EXP -= previousEXP;
    }

    public int EffectDamage(int EffectDamage = 1)
    {
        return skillDamage = attack * EffectDamage;
    }

    public void Alive()
    {
        ChangeState((int)Enum_CharacterState.Idle);
    }
    public void Dead()
    {               
        ChangeState((int)Enum_CharacterState.Dead);
    }


}


