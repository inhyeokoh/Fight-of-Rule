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


public abstract class Character : BasePlayerEntity
{

    private int hp;
    private int mp;
    private int exp;
    private int damage;
    private int defense;
    private int speed;
    private int level;
    private Enum_CharacterState characterState;

    public Animator animator;
  
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
    
    //처음에 이런식으로 적용해야할 클래스를 제네릭에다 넣고 StateMachine을 생성
    private StateMachine stateMachine = new StateMachine();

    //딕셔너리나 배열이나 리스트를 이용해 상태들을 담는 인덱스 생성
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



    public override void Setup(string name)
    {
        
        base.Setup(name);


        hp = SaveHp; mp = SaveMp; exp = SaveExp; damage = SaveDamage; 
        defense = SaveDefense; speed = SaveSpeed; level = SaveLevel;

        print(hp);
          
        //stateMachine = new StateMachine<Character>();

        //StateMachine을 생성하고 자기 자신(Character.cs)와 처음에 실행해야하는 상태를 넣고있는 모습
        stateMachine.Setup(states[(int)Enum_CharacterState.Idle]);
    }



    //FixedStay나 //Stay를 사용할땐 상속이든 다른곳에서 연결을 하든 Update와 FixedUpdate에 연결해야함
    public override void FixedUpdated()
    {
        stateMachine.FixedStay();
    }

    public override void Updated()
    {
        stateMachine.Stay();
    }

    // ChangeState 함수를 가져와 호출하면 상태가 바뀜 Enum이나 int형이나 파라미터는 자기가 쓰고싶은대로 변경하면됌 
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


