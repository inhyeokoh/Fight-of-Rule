using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : SubMono<MonsterController>
{

    private int maxHP;
    private int maxMP;

    private int hp;
    private int mp;
    private int exp;

    private int damage;

    private int attackNumber;

    private int attack;
    private float attackSpeed;
    private float delay;
    protected float abliltyDelay;
    private int defense;
    private int speed;
    protected int level;


    protected int detectDistance;
    protected int attackDistance;


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

    public int DetectDistance
    {
        get
        {
            return detectDistance;
        }
        set
        {
            detectDistance = value;
        }
    }

    public int AttackDistance
    {
        get
        {
            return attackDistance;
        }
        set
        {
            attackDistance = value;
        }
    }

    public int AttackNumber
    {
        get
        {
            return attackNumber;
        }
        set
        {
            attackNumber = value;
        }
    }

    public float AbliltyDelay
    {
        get
        {
            return abliltyDelay;
        }
        set
        {
            abliltyDelay = value;
        }
    }

    public float Delay
    {
        get
        {
            return delay;
        }
        set
        {
            delay = value;
        }
    }
    protected override void _Clear()
    {
   
    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        maxHP = 1;
        maxMP = 200;
        hp = maxHP;
        mp = maxMP;
        exp = 42;
        attack = 50;
        attackSpeed = 3f;
        delay = 1f;
        abliltyDelay = 20f;
        defense = 20;
        speed = 7;
        level = 8;

        detectDistance = 20;
        attackDistance = 10;
    }


    //�������� �ް� �׾����� ���׾����� üũ���ִ� �޼���
    public void DeadCheck(int damage, CharacterStatus expCharacter, float addforce)
    {
       
        hp -= damage;

        print(hp);

        if (hp <= 0)
        {
            hp = 0;
            _board._monsterState.ChangeState((int)Enum_MonsterState.Dead);
        }
        else
        {
            _board._monsterMovement.characterPosition = expCharacter.transform;
            _board._animationController.RootMotion(false);
            _board._monsterMovement.AddForce(addforce);
            _board._monsterState.ChangeState((int)Enum_MonsterState.Hit);
        }

        //print(hp);
    }


    //���� ����� ���� �������� ������ ����Ʈ������ �Ѱ��ִ� �޼���
    public int EffectDamage(int EffectDamage = 1)
    {
        return damage = attack * EffectDamage;
    }


}
