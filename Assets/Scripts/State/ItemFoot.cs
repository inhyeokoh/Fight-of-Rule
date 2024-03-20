using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFoot : InGameItemEquipment
{
    public int speed;
    public int defenes;
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        level = 20;

        state = new State(() =>
        {
            player.SumSpeed += speed;
            player.SumDefense += defenes;

        }, () => { print(defenes); }, () => { },
        () =>
        {
            player.SumSpeed -= speed;
            player.SumDefense -= defenes;
        });
    }
}
