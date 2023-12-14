using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
}

[Serializable]
public class CharData : Data
{
    public string charName;
    public int level = 1;
    public string job;
    public string gender;
}

[Serializable]
public class SettingsData : Data
{
    public float totalVol = 0.5f;
    public float backgroundVol = 0.5f;
    public float effectVol = 0.5f;

    public bool bTotalVol;
    public bool bBackgroundVol;
    public bool bEffectVol;
}

[Serializable]
public class LoginData : Data
{
    public string id;
    public string pw;
}