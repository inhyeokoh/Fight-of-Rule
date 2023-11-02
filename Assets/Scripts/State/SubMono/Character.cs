using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum Enum_CharacterState
{
    Idle,
    Move,
    Attack,  
    Avoid,
    Hit,
    Fall,
    Die,
}


public abstract class Character : SubMono<PlayerController>
{
    //Caracter
    //ĳ���Ϳ����� ĳ���Ϳ� ���°� ���� �߽������� �־���ϴ� ���
    //ĳ������ ��Ʈ��ũ�� �������ؼ� hp mp exp damage defense speed level ������ ���Ͱ� ���������� �޾ƿ´�.

    private int hp;
    private int mp;
    private int exp;
    private int damage;
    private int defense;
    private int speed = 30;
    private int level;
    private Enum_CharacterState characterState;
    [SerializeField]
    protected Vector3 inputVec;

    [SerializeField]
    protected Rigidbody rigid;



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

    public int MP
    {
        get
        {
            return mp;
        }
        set
        {
            hp = value < 0 ? 0 : value;
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
           // if (exp )            
        }
    }

    public int Damage
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



    public Enum_CharacterState CharacterState
    {
        get
        {
            return characterState;
        }
        set
        {
            characterState = value;
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
    
    //ó���� �̷������� �����ؾ��� Ŭ������ ���׸����� �ְ� StateMachine�� ����
    private StateMachine stateMachine = new StateMachine();

    //��ųʸ��� �迭�̳� ����Ʈ�� �̿��� ���µ��� ��� �ε��� ����
    protected Dictionary<int, State> states = new Dictionary<int, State>();

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

    protected override void _Init()
    {
        stateMachine.Setup(states[(int)Enum_CharacterState.Idle]);
    }

    //FixedStay�� //Stay�� ����Ҷ� ����̵� �ٸ������� ������ �ϵ� Update�� FixedUpdate�� �����ؾ���
    public void FixedUpdated()
    {
        stateMachine.FixedStay();
    }

    public void Updated()
    {
        stateMachine.Stay();
    }

    // ChangeState �Լ��� ������ ȣ���ϸ� ���°� �ٲ� Enum�̳� int���̳� �Ķ���ʹ� �ڱⰡ ���������� �����ϸ�� 
    public void ChangeState(int newState)
    {      
        //CharacterState = newState;       
        //stateMachine.ChangeState(states[(int)newState]);
        stateMachine.ChangeState(states[newState]);
    }


    public virtual void LevelUp()
    {

    }



  


    














}


