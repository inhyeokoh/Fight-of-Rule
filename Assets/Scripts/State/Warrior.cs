using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Enum_WarriorState
{
    Idle,
    Move,
    Attack,
    SkillActionQ,
    SkillActionW,
    SkillActionE,
    SkillActionR,
    Avoid,
    Hit,
    Fall,
    Die,
}
public class Warrior : BaseGameEntity
{
    private int hp;
    private int mp;
    private float exp;
    private int damage;
    private int defense;
    private int speed = 10;
    private Enum_WarriorState warriorState;

    public Animator animator;

    [SerializeField]
    private WarriorSkill warriorSkill;

    [SerializeField]
    private Vector3 inputVec;


    [SerializeField]
    private Rigidbody rigid;


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

    public int MP { 
        get 
        { 
            return mp; 
        } 
        set 
        {
            hp = value < 0 ? 0 : value;
        } 
    }
    public float EXP
    {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
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
    public Vector3 InputVec
    {
        get
        {
            return inputVec;
        }
        set
        {
            inputVec = value;
        }
    }
    public Rigidbody Rigid { get { return rigid; } set { rigid = value; } }


  

    public State<Warrior>[] states;
    private StateMachine<Warrior> stateMachine;

    

    private void OnCollisionEnter(Collision collision)
    {
        /* if (collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Boss")
         {
             warriorSkill.StopAllCoroutines();
             ChangeState(Enum_WarriorState.Fall);
         }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }


    public override void Setup(string name)
    {
        base.Setup(name);

        states = new State<Warrior>[11];
        states[(int)Enum_WarriorState.Idle] = new WarriorOwnedState.Idle();
        states[(int)Enum_WarriorState.Move] = new WarriorOwnedState.Move();
        states[(int)Enum_WarriorState.Attack] = new WarriorOwnedState.Attack();
        states[(int)Enum_WarriorState.SkillActionQ] = new WarriorOwnedState.SkillActionQ();
        states[(int)Enum_WarriorState.SkillActionW] = new WarriorOwnedState.SkillActionW();
        states[(int)Enum_WarriorState.SkillActionE] = new WarriorOwnedState.SkillActionE();
        states[(int)Enum_WarriorState.SkillActionR] = new WarriorOwnedState.SkillActionR();
        states[(int)Enum_WarriorState.Avoid] = new WarriorOwnedState.Avoid();
        states[(int)Enum_WarriorState.Hit] = new WarriorOwnedState.Hit();
        states[(int)Enum_WarriorState.Fall] = new WarriorOwnedState.Fall();
        states[(int)Enum_WarriorState.Die] = new WarriorOwnedState.Die();

        WarriorOwnedState.SkillActionQ inputQ = (WarriorOwnedState.SkillActionQ)states[(int)Enum_WarriorState.SkillActionQ];
        WarriorOwnedState.SkillActionW inputW = (WarriorOwnedState.SkillActionW)states[(int)Enum_WarriorState.SkillActionW];
        WarriorOwnedState.SkillActionE inputE = (WarriorOwnedState.SkillActionE)states[(int)Enum_WarriorState.SkillActionE];
        WarriorOwnedState.SkillActionR inputR = (WarriorOwnedState.SkillActionR)states[(int)Enum_WarriorState.SkillActionR];

        inputQ.InputQ += warriorSkill.Skill0;
        inputW.InputW += warriorSkill.Skill1;
        inputE.InputE += warriorSkill.Skill2;
        inputR.InputR += warriorSkill.Skill3;

        stateMachine = new StateMachine<Warrior>();

        

        stateMachine.Setup(this, states[(int)Enum_WarriorState.Idle]);
    }

    public override void FixedUpdated()
    {
        stateMachine.FixedStay();
    }
    public override void Updated()
    {

        stateMachine.Stay();
    }

    public void LevelUp()
    {

    }

    public void ChangeState(Enum_WarriorState newState)
    {
        WarriorState = newState;
        stateMachine.ChangeState(states[(int)newState]);
        
    }

    public void Coroutine()
    {

    }


}
