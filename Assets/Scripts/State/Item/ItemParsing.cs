//#define List
#define Dictionary
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemParsing : MonoBehaviour
{
#if List
    public List<ItemData> itemDatas = new List<ItemData>();
#endif

#if Dictionary
    public static Dictionary<string, int> DropItems = new Dictionary<string, int>();
    public static Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();
#endif
    void Awake()
    {
        ItemDataParsing();
    }

    private void ItemDataParsing()
    {
        List<Dictionary<string, string>> consumptionData = CSVReader.Read("Data/ItemsConsumptionItemDB");
        List<Dictionary<string, string>> equipmentData = CSVReader.Read("Data/ItemsEquipmentDB");
        List<Dictionary<string, string>> etcData = CSVReader.Read("Data/ItemsETCDB");


        for (int i = 0; i < consumptionData.Count; i++)
        {
            StateItemData ItemData;

            int item_id = int.Parse(consumptionData[i]["item_id"]);
            string item_name = consumptionData[i]["item_name"];
            string item_desc = consumptionData[i]["item_desc"];
            Sprite item_icon = GameManager.Resources.Load<Sprite>(consumptionData[i]["item_icon"]);

            Enum_Class item_class = (Enum_Class)Enum.Parse(typeof(Enum_Class), consumptionData[i]["item_class"]);
            Enum_Grade item_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), consumptionData[i]["item_grade"]);
            Enum_ItemType item_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), consumptionData[i]["item_type"]);
            Enum_DetailType item_detailtype = (Enum_DetailType)Enum.Parse(typeof(Enum_DetailType), consumptionData[i]["item_detailtype"]);
            long item_purchaseprice = long.Parse(consumptionData[i]["item_purchaseprice"]);
            long item_sellingprice = long.Parse(consumptionData[i]["item_sellingprice"]);
            int item_level = int.Parse(consumptionData[i]["item_level"]);
            int item_attack = int.Parse(consumptionData[i]["item_attack"]);
            int item_defense = int.Parse(consumptionData[i]["item_defense"]);
            int item_speed = int.Parse(consumptionData[i]["item_speed"]);
            int item_attackspeed = int.Parse(consumptionData[i]["item_attackspeed"]);
            int item_hp = int.Parse(consumptionData[i]["item_hp"]);
            int item_mp = int.Parse(consumptionData[i]["item_mp"]);
            int item_exp = int.Parse(consumptionData[i]["item_exp"]);
            int item_maxhp = int.Parse(consumptionData[i]["item_maxhp"]);
            int item_maxmp = int.Parse(consumptionData[i]["item_maxmp"]);
            int item_maxcount = int.Parse(consumptionData[i]["item_maxcount"]);



            ItemData = new StateItemData(item_id, item_name, item_desc, item_icon, item_class, item_grade, item_type, item_detailtype, item_purchaseprice, item_sellingprice, item_level,
                item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp, item_maxcount);


#if List
            itemDatas.Add(ItemData);
#endif
#if Dictionary

            DropItems.Add(item_name, item_id);
            itemDatas.Add(item_id, ItemData);
#endif
        }

        for (int i = 0; i < equipmentData.Count; i++)
        {
            StateItemData ItemData;

            int item_id = int.Parse(equipmentData[i]["item_id"]);
            string item_name = equipmentData[i]["item_name"];
            string item_desc = equipmentData[i]["item_desc"];
            Sprite item_icon = GameManager.Resources.Load<Sprite>(equipmentData[i]["item_icon"]);
            Enum_Class item_class = (Enum_Class)Enum.Parse(typeof(Enum_Class), equipmentData[i]["item_class"]);
            Enum_Grade item_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), equipmentData[i]["item_grade"]);
            Enum_ItemType item_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), equipmentData[i]["item_type"]);
            Enum_DetailType item_detailtype = (Enum_DetailType)Enum.Parse(typeof(Enum_DetailType), equipmentData[i]["item_detailtype"]);
            long item_purchaseprice = long.Parse(equipmentData[i]["item_purchaseprice"]);
            long item_sellingprice = long.Parse(equipmentData[i]["item_sellingprice"]);
            int item_level = int.Parse(equipmentData[i]["item_level"]);
            int item_attack = int.Parse(equipmentData[i]["item_attack"]);
            int item_defense = int.Parse(equipmentData[i]["item_defense"]);
            int item_speed = int.Parse(equipmentData[i]["item_speed"]);
            int item_attackspeed = int.Parse(equipmentData[i]["item_attackspeed"]);
            int item_hp = int.Parse(equipmentData[i]["item_hp"]);
            int item_mp = int.Parse(equipmentData[i]["item_mp"]);
            int item_exp = int.Parse(equipmentData[i]["item_exp"]);
            int item_maxhp = int.Parse(equipmentData[i]["item_maxhp"]);
            int item_maxmp = int.Parse(equipmentData[i]["item_maxmp"]);
            int item_maxcount = int.Parse(equipmentData[i]["item_maxcount"]);

            ItemData = new StateItemData(item_id, item_name, item_desc, item_icon, item_class, item_grade, item_type, item_detailtype, item_purchaseprice, item_sellingprice, item_level,
             item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp, item_maxcount);


#if List
            itemDatas.Add(ItemData);
#endif
#if Dictionary
            DropItems.Add(item_name, item_id);
            itemDatas.Add(item_id, ItemData);
#endif
        }
        for (int i = 0; i < etcData.Count; i++)
        {
            ItemData ItemData;

            int etcItem_id = int.Parse(etcData[i]["item_id"]);
            string etcItem_name = etcData[i]["item_name"];
            string etcItem_desc = etcData[i]["item_desc"];
            Sprite etcItem_icon = GameManager.Resources.Load<Sprite>(etcData[i]["item_icon"]); ;
            Enum_Grade etcItem_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), etcData[i]["item_grade"]);
            Enum_ItemType etcItem_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), etcData[i]["item_type"]);
            long etcItem_purchaseprice = long.Parse(etcData[i]["item_purchaseprice"]);
            long etcItem_sellingprice = long.Parse(etcData[i]["item_sellingprice"]);
            int etcItem_maxcount = int.Parse(etcData[i]["item_maxcount"]);

            ItemData = new ItemData(etcItem_id, etcItem_name, etcItem_desc, etcItem_icon, etcItem_type, etcItem_grade, etcItem_purchaseprice, etcItem_sellingprice,
                etcItem_maxcount);


#if List
            itemDatas.Add(ItemData);
#endif
#if Dictionary
            DropItems.Add(etcItem_name, etcItem_id);
            itemDatas.Add(etcItem_id, ItemData);
#endif
        }
    }


#if Dictionary
    public static ItemData StateItemDataReader(int item_id)
    {
        StateItemData itemData = itemDatas[item_id] as StateItemData;

        if (itemData != null)
        {
            StateItemData itemDataPasing;

            itemDataPasing = new StateItemData(itemData.id, itemData.name, itemData.desc, itemData.icon, itemData.itemClass, itemData.itemGrade, itemData.itemType,
                itemData.detailType, itemData.purchaseprice, itemData.sellingprice, itemData.level, itemData.attack, itemData.defense, itemData.speed, itemData.attackSpeed
                , itemData.hp, itemData.mp, itemData.exp, itemData.maxHp, itemData.maxMp, itemData.maxCount);

            return itemDataPasing;
        }
        else
        {
            ItemData itemDataPasing;
            itemDataPasing = new ItemData(itemDatas[item_id].id, itemDatas[item_id].name, itemDatas[item_id].desc, itemDatas[item_id].icon, itemDatas[item_id].itemType,
                itemDatas[item_id].itemGrade, itemDatas[item_id].purchaseprice, itemDatas[item_id].sellingprice, itemDatas[item_id].maxCount);

            return itemDataPasing;
        }
    }

    public static ItemData MonsterDropItem(string itemName)
    {
        ItemData item;

        try
        {
            int itemID = DropItems[itemName];
            item = StateItemDataReader(itemID);

            return item;
        }
        catch
        {
            print("아이템 정보가 없습니다.");
        }

        return null;
    }
}

  /*  public static ETCItemData ETCItemDataReader(int item_id)
    {
        return etcItemDatas[item_id];
    }*/

#endif