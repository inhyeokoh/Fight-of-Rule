using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
}

[Serializable]
public class CharOption : Data
{
    public int slotNum = 0;
    public string charName;
    public int level = 1;
    public string job;
    public string gender;
}