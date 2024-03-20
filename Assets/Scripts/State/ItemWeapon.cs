using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : InGameItemEquipment
{
    public int attack;
    public override void Setting()
    {
        ap = gameObject.GetComponent<InGameItemEquipment>();
        level = 20;

        state = new State(() =>
        {
            player.SumAttack += attack;
        }, () => { print(attack); }, () => { },
        () =>
        {
            player.SumAttack -= attack;
        });
    }

    //현재 정진님이 원하시는 것
    //데이터에 있는 아이템들은 다 묶고 자식 데이터도 하나 만들어서 상속하게 만들고 하자

    public override void Data(int itemID)
    {
        /*ItemWeaponData data = GameManager.Data.warriorWeapons.warriorWeaponItems[itemID];

        this.itemID = data.itemID;
        this.itemType = (Enum_ItemType)System.Enum.Parse(typeof(Enum_ItemType), data.itemType);
        this.level = data.level;
        this.itemName = data.itemName;
        this.itemDescription = data.itemDescription;

        this.equipmentClass = (Enum_Class)System.Enum.Parse(typeof(Enum_Class), data.equipmentClass);
        this.equipmentType = (Enum_EquipmentType)System.Enum.Parse(typeof(Enum_EquipmentType), data.equipmentType);
        this.attack = data.attack;*/
    }
}
