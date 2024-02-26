using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : SubMono<PlayerController>
{
    private int characterMaxHP;
    private int characterMaxMP;
    private int characterMaxEXP;
    private int characterAttack;
    private int characterAttackSpeed;
    private int characterDefense;
    private int characterSpeed;

    private int hp;
    private int mp;
    private int exp;
    private int level;
    private int skillDamage;

    private int sumMaxHP;
    private int sumMaxMP;
    private int sumAttack;
    private int sumAttackSpeed;
    private int sumDefense;
    private int sumSpeed;

    public int MaxEXP { get { return characterMaxEXP; } }

    // ���� ���� ������ �״�� �ΰ�
    // �ִ�ü��, �ִ븶��, ���ݷ�, ����, ���ǵ�, ���ݼӵ�


    // ������ ���� ��� ����

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
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value < 0 ? 0 : value;

            if (hp >= sumMaxHP)
            {
                hp = sumMaxHP;
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

            if (mp >= sumMaxMP)
            {
                mp = sumMaxMP;
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

                if (exp >= characterMaxEXP)
                {
                    {
                        level++;
                        _board.LevelUpCheck(level);
                    }
                }
            }
        }
    }

    public int Attack { get { return characterAttack; } set { characterAttack = value; } }

    public int Defense { get { return characterDefense; } set { characterDefense = value; } }

    public int AttackSpped { get { return characterAttackSpeed; } set { characterAttackSpeed = value; } }
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

    public int Speed { get { return characterSpeed; } set { characterSpeed = value; } }
    public int MaxHP { get { return characterMaxHP; } set { characterMaxHP = value; } }
    public int MaxMP { get { return characterMaxMP; } set { characterMaxMP = value; } }

    public int SumMaxHP { get { return sumMaxHP; } set { sumMaxHP = value; } }

    public int SumMaxMP { get { return sumMaxMP; } set { sumMaxMP = value; } }

    public int SumAttack
    {
        get
        {
            return sumAttack;
        }
        set
        {
            sumAttack = value;
        }
    }

    public int SumDefense
    {
        get
        {
            return sumDefense;
        }
        set
        {
            sumDefense = value;
        }
    }

    public int SumSpeed
    {
        get
        {
            return sumSpeed;
        }
        set
        {
            sumSpeed = value;
        }
    }


    public int SumAttackSpeed
    {
        get
        {
            return sumAttackSpeed;
        }
        set
        {
            sumAttackSpeed = value;
        }
    }

    public void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defense, bool firstLevel)
    {
        int previousEXP = this.characterMaxEXP;

        if (firstLevel)
        {
            this.characterMaxEXP = maxEXP;
        }
        else
        {
            this.characterMaxEXP += maxEXP;
        }

        this.characterMaxHP += maxHP;
        this.characterMaxMP += maxMP;
        this.characterAttack += attack;
        this.characterDefense += defense;

        HP = this.characterMaxHP;
        MP = this.characterMaxMP;
        EXP -= previousEXP;
    }

    public int EffectDamage(int EffectDamage = 1)
    {
        return SkillDamage = sumAttack * EffectDamage;
    }

    protected override void _Init()
    {
        level = 50;
        characterMaxHP = 50;
        characterMaxMP = 50;
        characterMaxEXP = 100;

        hp = 20;
        mp = 50;
        exp = 0;
        characterAttack = 5;
        characterAttackSpeed = 1;
        characterDefense = 3;
        characterSpeed = 10;
      
        sumMaxHP = characterMaxHP;
        sumMaxMP = characterMaxMP;
        sumAttack = characterAttack;
        sumAttackSpeed = characterAttackSpeed;
        sumDefense = characterDefense;
        sumSpeed = characterSpeed;


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
