using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemDuraction : ItemObject
{
    public int duractionTime;
    public bool drinkOn;
    public float coolTime;
    public Enum_PotionType duractionPostionType;

    public override void Setting()
    {
        state = new State(() => 
        {
            if (!drinkOn)
            {
                switch (duractionPostionType)
                {
                    case Enum_PotionType.Defenes:
                        player.SumDefense += item.PortionStat;
                        break;
                    case Enum_PotionType.Attack:
                        player.SumAttack += item.PortionStat;
                        break;
                }

                drinkOn = true;
            }            
            coolTime = duractionTime;        
        }, () => { }, 
        () => 
        {
            if (coolTime > 0)
            {
                coolTime -= Time.deltaTime;
                print(coolTime);
            }
            else
            {
                stateMachine.ExitState();
            }
        }, 
        () => 
        { 
            switch (duractionPostionType)
            {
                case Enum_PotionType.Defenes:
                    player.SumDefense -= item.PortionStat;
                    break;
                case Enum_PotionType.Attack:
                    player.SumAttack -= item.PortionStat;
                    break;
            }
            coolTime = 0;
            drinkOn = false;
        });
    }
    public override void Check()
    {
      
    } 
}
