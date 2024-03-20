using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFoot : InGameItemEquipment
{ 
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        item.Level = 20;

        state = new State(() =>
        {
            player.SumSpeed += item.Speed;
            player.SumDefense += item.Defense;

        }, () => { print(item.Defense); }, () => { },
        () =>
        {
            player.SumSpeed -= item.Speed;
            player.SumDefense -= item.Defense;
        });
    }
}
