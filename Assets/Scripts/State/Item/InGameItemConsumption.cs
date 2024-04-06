using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemConsumption : ItemObject
{   
    //현재 포션 타입
    //public Enum_PotionType postionType;

    //타입마다 다른 포션세팅
    public override void Setting()
    {     
        state = new State(() => 
        {
            /*switch(postionType)
            {
                *//*case Enum_PotionType.Heal:
                    player.HP += item.PortionStat;
                    break;

                case Enum_PotionType.Mana:
                    player.MP += item.PortionStat;
                    break;

                case Enum_PotionType.Exp:
                    player.EXP += item.PortionStat;
                    break;

                case Enum_PotionType.Defenes:
                    player.Defense += item.PortionStat;
                    break;

                case Enum_PotionType.Attack:
                    player.Attack += item.PortionStat;
                    break;*//*
            }     */ 
        }, () => { }, () => { }, () => 
        { 
            gameObject.SetActive(false);       
        });
    }

    public override void Check()
    {
      
    }
}
