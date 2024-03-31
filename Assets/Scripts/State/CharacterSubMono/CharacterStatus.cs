using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : SubMono<PlayerController>
{

    //데이터를 전달받는 캐릭터의 순수능력치
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

    //인게임상에 캐릭터 능력치
    private int sumMaxHP;
    private int sumMaxMP;
    private int sumAttack;
    private int sumAttackSpeed;
    private int sumDefense;
    private int sumSpeed;

    public int MaxEXP { get { return characterMaxEXP; } }

    // 현재 순수 스텟은 그대루 두고
    // 최대체력, 최대마나, 공격력, 방어력, 스피드, 공격속도


    // 아이템 적용 장비 적용

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
    public int Speed { get { return characterSpeed; } set { characterSpeed = value; } }
    public int MaxHP { get { return characterMaxHP; } set { characterMaxHP = value; } }
    public int MaxMP { get { return characterMaxMP; } set { characterMaxMP = value; } }

    public int SumMaxHP { get { return sumMaxHP; } set { sumMaxHP = value; } }

    public int SumMaxMP { get { return sumMaxMP; } set { sumMaxMP = value; } }
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


    //레벨업할때 능력치를 상승시키는 메서드
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

    //데미지 계산
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
