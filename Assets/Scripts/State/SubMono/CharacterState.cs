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
    //ĳ���Ϳ����� ĳ���Ϳ� ���°� ���� �߽������� �־���ϴ� ���
    //ĳ������ ��Ʈ��ũ�� �������ؼ� hp mp exp damage defense speed level ������ ���Ͱ� ���������� �޾ƿ´�.

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


