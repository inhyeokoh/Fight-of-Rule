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
public class StateItemData : Data
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public Enum_ItemType itemType;
    public Enum_EquipmentType equipmentType;
    public long purchaseprice;
    public long sellingprice;
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
    public int maxCount;

   
    public StateItemData(int id,string name, string desc, Sprite icon, Enum_ItemType itemType, Enum_EquipmentType equipmentType, long purchaseprice, long sellingprice, int level, int attack, int defense
        , int speed, int attackSpeed, int hp, int mp, int exp, int maxHp, int maxMp, int maxCount)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.icon = icon;
        this.itemType = itemType;
        this.equipmentType = equipmentType;
        this.purchaseprice = purchaseprice;
        this.sellingprice = sellingprice;
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
public class ETCItemData
{
    public int id;
    public string name;
    public string desc;
    public Sprite icon;
    public Enum_ItemType itemType;
    public long purchaseprice;
    public long sellingprice;
    public int maxCount;
    public ETCItemData(int id, string name, string desc, Sprite icon, Enum_ItemType itemType, long purchaseprice, long sellingprice, int maxCount)
    {
        this.id = id;
        this.name = name;
        this.desc = desc;
        this.icon = icon;
        this.itemType = itemType;
        this.purchaseprice = purchaseprice;
        this.sellingprice = sellingprice;
        this.maxCount = maxCount;
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


