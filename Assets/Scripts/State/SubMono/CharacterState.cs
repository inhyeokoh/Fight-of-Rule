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


public abstract class CharacterState : SubMono<PlayerController>
{
    //Caracter
    //캐릭터에서는 캐릭터에 상태가 가장 중심적으로 있어야하는 기능
    //캐릭터의 네트워크에 연결을해서 hp mp exp damage defense speed level 렌더러 백터값 물리엔진을 받아온다.

    private int hp;
    private int mp;
    private int exp;
    private int attack;
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

    
  
    protected override void _Init()
    {
       
    }


    public virtual void LevelUp()
    {

    }



  


    














}


