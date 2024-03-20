using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody : InGameItemEquipment
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
            player.SumAttack += item.Attack;

        }, () => { print(item.MaxHp); }, () => { },
        () =>
        {
            player.SumMaxHP -= item.MaxHp;         
            player.SumDefense -= item.Defense;
            player.SumAttack -= item.Attack;
        });
    }
}
