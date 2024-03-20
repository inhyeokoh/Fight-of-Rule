using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHand : InGameItemEquipment
{
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        item.Level = 20;

        state = new State(() =>
        {
            player.SumAttack += item.Attack;
            player.SumMaxMP += item.MaxMp;
            if (player.SumMaxMP < player.MP)
            {
                player.MP = player.SumMaxMP;
            }
        }, () => { print(item.Attack); }, () => { },
        () =>
        {
            player.SumAttack -= item.Attack;
            player.SumMaxMP -= item.MaxMp;
          
        });
    }
}
