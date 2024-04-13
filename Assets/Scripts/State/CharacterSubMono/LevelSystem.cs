using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : SubMono<PlayerController>
{
   
    public void LevelUpCheck(int level)
    {
        LevelData currentLeveldata = LevelReaderData.CurrentLevelData(_board._playerStat.Level, _board._class);


        int maxHP = currentLeveldata.maxhp;
        int maxMP = currentLeveldata.maxmp;
        int maxEXP = currentLeveldata.maxexp;
        int attack = currentLeveldata.attack;
        int defense = currentLeveldata.defense;
        _board.LevelStatUP(maxEXP, maxHP, maxMP, attack, defense);
      
    }

    protected override void _Clear()
    {
      
    }

    protected override void _Excute()
    {
     
    }

    protected override void _Init()
    {
       
    }
}
