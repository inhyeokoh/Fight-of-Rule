using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public string gender;

    public int level;
    public int maxHP;
    public int maxMP;
    public int maxEXP;

    public int hp;
    public int mp;
    public int exp;
    public int attack;
    public int attackSpeed;
    public int defense;
    public int speed;
    public int skillDamage;
}

