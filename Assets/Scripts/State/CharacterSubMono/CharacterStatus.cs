using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : SubMono<PlayerController>
{
    private int maxHP;
    private int maxMP;
    private int maxEXP;

    private int hp;
    private int mp;
    private int exp;
    private int attack;
    private int attackSpeed;
    private int defense;
    private int speed;
    private int level;
    private int skillDamage;

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

            if (hp == 0)
            {
                _board._playerState.Dead();
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
        set
        {
            skillDamage = value;
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

    public void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defense, bool firstLevel)
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
        return SkillDamage = Attack * EffectDamage;
    }

    protected override void _Init()
    {
        level = 1;
        maxHP = 50;
        maxMP = 50;
        maxEXP = 100;

        hp = 50;
        mp = 50;
        exp = 0;
        attack = 5;
        attackSpeed = 1;
        defense = 3;
        speed = 10;
        skillDamage = 1;

/*        maxHP = GameManager.Data.character.maxHP;
        maxMP = GameManager.Data.character.maxMP;
        maxEXP = GameManager.Data.character.maxEXP;

        hp = GameManager.Data.character.hp;
        mp = GameManager.Data.character.mp;
        exp = GameManager.Data.character.exp;
        attack = GameManager.Data.character.attack;
        defense = GameManager.Data.character.defense;
        speed = GameManager.Data.character.speed;
        level = GameManager.Data.character.level;*/
    }

    protected override void _Excute()
    {
    }

    protected override void _Clear()
    {
    }
}
