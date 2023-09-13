using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Enum_WarriorState
{
    Idle,
    Move,
    SkillAction,
    Avoid,
    Hit,
    Fall,
    Die,
}
public class Warrior : BaseGameEntity
{
    private int hp;
    private int damage;
    private int defense;
    private int speed;
    private Enum_WarriorState warriorState;


    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value < 0 ? 0 : value;
        }
    }
    public  int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
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

    public Enum_WarriorState WarriorState
    {
        get
        {
            return warriorState;
        }
        set
        {
            warriorState = value;
        }
    }



    private State<Warrior>[] states;
    private StateMachine<Warrior> stateMachine;

    private void OnCollisionEnter(Collision collision)
    {
       /* if (collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Boss")
        {
            ChangeState(Enum_WarriorState.Fall);
        }*/
            
            

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }


    public override void Setup(string name)
    {
        base.Setup(name);

        states = new State<Warrior>[6];
        states[(int)Enum_WarriorState.Idle] = new WarriorOwnedState.Idle();
        states[(int)Enum_WarriorState.Move] = new WarriorOwnedState.Move();
        states[(int)Enum_WarriorState.SkillAction] = new WarriorOwnedState.SkillAction();
        states[(int)Enum_WarriorState.Avoid] = new WarriorOwnedState.Avoid();
        states[(int)Enum_WarriorState.Fall] = new WarriorOwnedState.Fall();
        states[(int)Enum_WarriorState.Die] = new WarriorOwnedState.Die();

        stateMachine = new StateMachine<Warrior>();

        stateMachine.Setup(this, states[(int)Enum_WarriorState.Idle]);
    }

    public override void Updated()
    {
        stateMachine.Stay();
    }

    public void ChangeState(Enum_WarriorState newState)
    {
        WarriorState = newState;
        stateMachine.ChangeState(states[(int)newState]);
    }


}
