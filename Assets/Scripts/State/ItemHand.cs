using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHand : InGameItemEquipment
{
    public int attack;
    public int maxMP;
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        level = 20;

        state = new State(() =>
        {
            player.SumAttack += attack;
            player.SumMaxMP += maxMP;
            if (player.SumMaxMP < player.MP)
            {
                player.MP = player.SumMaxMP;
            }
        }, () => { print(attack); }, () => { },
        () =>
        {
            player.SumAttack -= attack;
            player.SumMaxMP -= maxMP;
          
        });
    }
}
