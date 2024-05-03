using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Data
{
}

[Serializable]
public class LoginData : Data
{
    public string id;
    public string pw;
    public int slotCount;
}

[System.Serializable]
public class StateItemData : ItemData
{
    public Enum_Class itemClass;
    public Enum_DetailType detailType;
    public int level;
    public int attack;
    public int defense;
    public int speed;
    public int attackSpeed;
    public int exp;
    public int hp;
    public int mp;
    public int maxHp;
    public int maxMp;

    public StateItemData(int id, string name, string desc, Sprite icon, Enum_Class itemClass, Enum_Grade itemGrade, Enum_ItemType itemType, Enum_DetailType detailType, long purchaseprice, long sellingprice, int level, int attack, int defense
        , int speed, int attackSpeed, int hp, int mp, int exp, int maxHp, int maxMp, int maxCount, int count = 1) : base(id, name, desc, icon, itemType, itemGrade, purchaseprice, sellingprice, maxCount, count)
    {
        this.itemClass = itemClass;   
        this.detailType = detailType;      
        this.level = level;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.exp = exp;
        this.hp = hp;
        this.mp = mp;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.maxCount = maxCount;
    }
}

[System.Serializable]
public class ItemData : Data
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public Enum_ItemType itemType;
    public Enum_Grade itemGrade;
    public long purchaseprice;
    public long sellingprice;
    public int count;
    public int maxCount;
    public ItemData(int id, string name, string desc, Sprite icon, Enum_ItemType itemType, Enum_Grade itemGrade, long purchaseprice, long sellingprice, int maxCount, int count = 1)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.icon = icon;
        this.itemType = itemType;
        this.itemGrade = itemGrade;
        this.purchaseprice = purchaseprice;
        this.sellingprice = sellingprice;
        this.maxCount = maxCount;

        this.count = count;
    }
}

[System.Serializable]
public class LevelData : Data
{
    public int level;
    public int maxhp;
    public int maxmp;
    public int maxexp;
    public int attack;
    public int defense;

    public LevelData(int level, int maxhp, int maxmp, int maxexp,int attack, int defense)
    {
        this.level = level;
        this.maxhp = maxhp;
        this.maxmp = maxmp;
        this.maxexp = maxexp;
        this.attack = attack;
        this.defense = defense;
    }
}


[System.Serializable]
public class WarriorSkillData : Data
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public WarriorSkill number;
    public int maxLevel;
    public int[] levelCondition;
    public int[] skillPoint;
    public int[] skillMP;
    public float[] skillCool;
    public int[] skillDamage;

    public WarriorSkillData(int id, string name, string desc, Sprite icon, WarriorSkill number, int maxLevel, int[] levelCondition,
        int[] skillPoint, int[] skillMP, float[] skillCool, int[] skillDamage)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.icon = icon;
        this.number = number;
        this.maxLevel = maxLevel;
        this.levelCondition = levelCondition;
        this.skillPoint = skillPoint;
        this.skillMP = skillMP;
        this.skillCool = skillCool;
        this.skillDamage = skillDamage;
    }
}


[System.Serializable]
public class MonsterData : Data
{
    public int monster_id;
    public string monster_object;
    public string monster_name;
    public Enum_MonsterType monster_type;
    public int monster_level;
    public int monster_exp;
    public int monster_maxhp;
    public int monster_maxmp;
    public int monster_attack;
    public float monster_attackspeed;
    public float monster_delay;
    public float monster_abliltydelay;
    public int monster_defense;
    public int monster_speed;
    public float monster_detectdistance;
    public float monster_attackdistance;
    public int[] monster_stateitem;
    public int[] moinster_etcitem;
    public int monster_mingold;
    public int monster_maxgold;

    public MonsterData(int monster_id, string monster_object, string monster_name, Enum_MonsterType monster_type, int monster_level,
        int monster_exp, int monster_maxhp, int monster_maxmp, int monster_attack, float monster_attackspeed, float monster_delay, 
        float monster_abliltydelay, int monster_defense, int monster_speed, float monster_detectdistance, float monster_attackdistance, 
        int[] monster_stateitem, int[] moinster_etcitem, int monster_mingold, int monster_maxgold)
    {
        this.monster_id = monster_id;
        this.monster_object = monster_object;
        this.monster_name = monster_name;
        this.monster_type = monster_type;
        this.monster_level = monster_level;
        this.monster_exp = monster_exp;
        this.monster_maxhp = monster_maxhp;
        this.monster_maxmp = monster_maxmp;
        this.monster_attack = monster_attack;
        this.monster_attackspeed = monster_attackspeed;
        this.monster_delay = monster_delay;
        this.monster_abliltydelay = monster_abliltydelay;
        this.monster_defense = monster_defense;
        this.monster_speed = monster_speed;
        this.monster_detectdistance = monster_detectdistance;
        this.monster_attackdistance = monster_attackdistance;
        this.monster_stateitem = monster_stateitem;
        this.moinster_etcitem = moinster_etcitem;
        this.monster_mingold = monster_mingold;
        this.monster_maxgold = monster_maxgold;
    }
}

[System.Serializable]
public class MonsterItemDropData
{
    public int monster_id;
    public string[] monster_itemdrop;
    public float[] monster_itempercent;
    public int monster_mingold;
    public int monster_maxgold;

    public MonsterItemDropData(int monster_id, string[] monster_itemdrop,float[] monster_itempercent,int monster_mingold, int monster_maxgold)
    {
        this.monster_id = monster_id;
        this.monster_itemdrop = monster_itemdrop;
        this.monster_itempercent = monster_itempercent;
        this.monster_mingold = monster_mingold;
        this.monster_maxgold = monster_maxgold;
    }
}




[Serializable]
public class SettingsData : Data
{
    public float totalVol;
    public float backgroundVol;
    public float effectVol;

    public bool bTotalVol;
    public bool bBackgroundVol;
    public bool bEffectVol;
}

[Serializable]
public class CharData : Data
{
    public string charName;
    public string job;
    public bool gender;

    public int level;
    public int maxHP;
    public int hp;
    public int maxMP;
    public int mp;
    public int maxEXP;
    public int exp;
    public int attack;
    public int attackSpeed;
    public int defense;
    public int speed;

    public CharData()
    {
        charName = "";
        job = "Warrior";
        gender = true;

        level = 1;
        maxHP = 100;
        hp = 100;
        maxMP = 100;
        mp = 100;
        maxEXP = 100;
        exp = 0;
        attack = 5;
        attackSpeed = 1;
        defense = 3;
        speed = 10;
    }

    public CharData(string job) : this()
    {
        this.job = job;
    }
}


