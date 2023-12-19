using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : SubMono<PlayerController>
{
    public List<LevelCollection> levelCollections;
    private int index = 0;


    public void LevelUpCheck(int level)
    {
        if (level > levelCollections[index].lastLevel)
        {
            index++;
        }

        if (level == levelCollections[index].firstLevel)
        {
            int maxEXP = levelCollections[index].exp;
            int maxHP = levelCollections[index].hp;
            int maxMP = levelCollections[index].mp;
            int attack = levelCollections[index].attack;
            int defenese = levelCollections[index].defense;
            _board.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenese, true);
        }
        else
        {
            int maxEXP = levelCollections[index].exp;
            int maxHP = levelCollections[index].hp;
            int maxMP = levelCollections[index].mp;
            int attack = levelCollections[index].attack;
            int defenese = levelCollections[index].defense;
            _board.LevelStatUP(maxEXP, maxHP, maxMP, attack, defenese,false);
        }      
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
