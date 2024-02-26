using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHead : InGameItemEquipment
{
    public int maxHp;
    public int defenes;
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();     
        level = 20;

        state = new State(() => 
        {
            player.SumMaxHP += maxHp;
            if (player.SumMaxHP < player.HP)
            {
                player.HP = player.SumMaxHP;
            }
            player.SumDefense += defenes;

        }, () => { print(maxHp); }, () => { }, 
        () => 
        {
            player.SumMaxHP -= maxHp;
                   
            player.SumDefense -= defenes;
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
