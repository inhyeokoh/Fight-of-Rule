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

    private int skillPoint;

    public int SkillPoint { get { return skillPoint; } set { skillPoint = value; } }

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
            GameManager.UI.PlayerInfo.UpdateStatus();
            GameManager.Quest.UpdateAvailableQuests();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
        }
    }

    public int Attack { get { return characterAttack; } set { characterAttack = value; } }

    public int Defense { get { return characterDefense; } set { characterDefense = value; } }

    public int AttackSpped { get { return characterAttackSpeed; } set { characterAttackSpeed = value; } }
    public int Speed { get { return characterSpeed; } set { characterSpeed = value; } }
    public int MaxHP { get { return characterMaxHP; } set { characterMaxHP = value; } }
    public int MaxMP { get { return characterMaxMP; } set { characterMaxMP = value; } }

    public int SumMaxHP { get { return sumMaxHP; } set { sumMaxHP = value; GameManager.UI.PlayerInfo.UpdateStatus(); } }

    public int SumMaxMP { get { return sumMaxMP; } set { sumMaxMP = value; GameManager.UI.PlayerInfo.UpdateStatus(); } }
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
            SkillManager.Skill.SkillDamageUpdate();
            GameManager.UI.PlayerInfo.UpdateStatus();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
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
            GameManager.UI.PlayerInfo.UpdateStatus();
        }
    }


    //레벨업할때 능력치를 상승시키는 메서드
    public void LevelStatUP(int maxEXP, int maxHP, int maxMP, int attack, int defense)
    {
        int previousEXP = this.characterMaxEXP;
   
        this.characterMaxEXP = maxEXP;       

        this.characterMaxHP += maxHP;
        this.characterMaxMP += maxMP;
        this.characterAttack += attack;
        this.characterDefense += defense;

      

        SumMaxHP += maxHP;
        SumMaxMP += maxMP;
        SumAttack += attack;  
        SumDefense += defense;

        HP = SumMaxHP;
        MP = SumMaxMP;

        if (level == 50)
        {
            EXP = maxEXP;
        }
        else
        {
            EXP -= previousEXP;
        }
    }

    //데미지 계산
    public int EffectDamage(int EffectDamage = 1)
    {
        return SkillDamage = sumAttack * EffectDamage;
    }

    protected override void _Init()
    {
        Level = GameManager.Data.CurrentCharacter.Stat.Level;
        characterMaxHP = GameManager.Data.CurrentCharacter.Stat.MaxHP;
        characterMaxMP = GameManager.Data.CurrentCharacter.Stat.MaxMP;
        characterMaxEXP = GameManager.Data.CurrentCharacter.Stat.MaxEXP;

        hp = GameManager.Data.CurrentCharacter.Stat.Hp;
        mp = GameManager.Data.CurrentCharacter.Stat.Mp;
        exp = GameManager.Data.CurrentCharacter.Stat.Exp;
        characterAttack = GameManager.Data.CurrentCharacter.Stat.Attack;
        characterAttackSpeed = GameManager.Data.CurrentCharacter.Stat.AttackSpeed;
        characterDefense = GameManager.Data.CurrentCharacter.Stat.Defense;
        characterSpeed = GameManager.Data.CurrentCharacter.Stat.Speed;

        sumMaxHP = characterMaxHP;
        sumMaxMP = characterMaxMP;
        sumAttack = characterAttack;
        sumAttackSpeed = characterAttackSpeed;
        sumDefense = characterDefense;
        sumSpeed = characterSpeed;
    }

    protected override void _Excute()
    {
    }

    protected override void _Clear()
    {
    }
}
