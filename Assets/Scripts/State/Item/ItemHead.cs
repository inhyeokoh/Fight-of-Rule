using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHead : InGameItemEquipment
{
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        item.Level = 20;

        state = new State(() => 
        {
            player.SumMaxHP += item.MaxHp;
            if (player.SumMaxHP < player.HP)
            {
                player.HP = player.SumMaxHP;
            }
            player.SumDefense += item.Defense;

        }, () => { print(item.MaxHp); }, () => { }, 
        () => 
        {
            player.SumMaxHP -= item.MaxHp;
                   
            player.SumDefense -= item.Defense;
        });
    }

    public override void Data(int ID)
    {
        
    }


    /* public override void Enter()
     {

     }

     public override void Exit()
     {

     }*/
}
