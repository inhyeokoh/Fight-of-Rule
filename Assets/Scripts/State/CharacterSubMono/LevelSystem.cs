using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : SubMono<PlayerController>
{
   
    // 레벨업 하면 데이터를 불러와 다음 레벨 스텟 정보들을 보내주는 메서드
    public void LevelUpCheck(int level)
    {
        LevelData currentLeveldata = GameManager.Data.CurrentLevelData(_board._playerStat.Level, _board._class);


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
