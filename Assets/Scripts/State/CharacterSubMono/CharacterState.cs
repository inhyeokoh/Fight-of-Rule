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
    //ĳ���Ϳ����� ĳ���Ϳ� ���°� ���� �߽������� �־���ϴ� ���
    //ĳ������ ��Ʈ��ũ�� �������ؼ� hp mp exp damage defense speed level ������ ���Ͱ� ���������� �޾ƿ´�.
    Collider playerCollider;

    protected int attackCombo;
    [SerializeField]
    protected bool skillUseCheck;

    [SerializeField]
    private bool isAvoid = false;

    [SerializeField]
    private bool moveStateCheck;


    [SerializeField]
    private Enum_CharacterState characterState;

    public bool IsAvoid { get { return isAvoid; } set { isAvoid = value; } }
 

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
                _board._playerMovement.Move(_board._playerStat.Speed);
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
            _board._playerStat.EffectDamage();
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
                _board._playerMovement.Avoid(_board._playerStat.Speed);
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
        //print($"������� {(Enum_CharacterState)newState}");
        stateMachine.ChangeState(state[newState]);
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


