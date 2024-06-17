using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : SubMono<MonsterController>
{
    private int maxHP;
    private int maxMP;

    public int hp;
    private int mp;
    public int exp;

    private int attack;
    private float attackSpeed;
    private float delay;
    protected float abliltyDelay;
    private int defense;
    private int speed;
    protected int level;

    protected float detectDistance;
    protected float attackDistance;

    private int damage;



    private int attackNumber;

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

    public float DetectDistance
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

    public float AttackDistance
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
        MonsterStatusApply();
    }

    private void MonsterStatusApply()
    {
        maxHP = _board.monsterDB.monster_maxhp;
        maxMP = _board.monsterDB.monster_maxmp;
        hp = maxHP;
        mp = maxMP;
        exp = _board.monsterDB.monster_exp;
        attack = _board.monsterDB.monster_attack;
        attackSpeed = _board.monsterDB.monster_attackspeed;
        delay = _board.monsterDB.monster_delay;
        abliltyDelay = _board.monsterDB.monster_abliltydelay;
        defense = _board.monsterDB.monster_defense;
        speed = _board.monsterDB.monster_speed;
        level = _board.monsterDB.monster_level;

        detectDistance = _board.monsterDB.monster_detectdistance;
        attackDistance = _board.monsterDB.monster_attackdistance;
    }

    //데미지를 받고 죽었는지 안죽었는지 체크해주는 메서드
    public void DeadCheck(int damage, CharacterStatus expCharacter, float addforce)
    {
       
        hp -= damage;

      //  print(hp);

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


    //현재 기술을 쓸때 데미지의 정보를 이펙트들한테 넘겨주는 메서드
    public int EffectDamage(int EffectDamage = 1)
    {
        return damage = attack * EffectDamage;
    }


}
