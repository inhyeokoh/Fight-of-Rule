using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBody : InGameItemEquipment
{
    public int maxHp;
    public int defenes;
    public int attack;
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
            player.SumAttack += attack;

        }, () => { print(maxHp); }, () => { },
        () =>
        {
            player.SumMaxHP -= maxHp;         
            player.SumDefense -= defenes;
            player.SumAttack -= attack;
        });
    }
}
