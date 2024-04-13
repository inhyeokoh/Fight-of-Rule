//#define List
#define Dictionary
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
#if List
    public List<StateItemData> stateItemDatas = new List<StateItemData>();
    public List<ETCItemData> etcItemDatas = new List<ETCItemData>();
#endif

#if Dictionary
     public static Dictionary<int, StateItemData> stateItemDatas = new Dictionary<int, StateItemData>();
     public static Dictionary<int, ETCItemData> etcItemDatas = new Dictionary<int, ETCItemData>();
#endif
    void Awake()
    {
        StateItemDataParsing();
        ETCItemDataParsing();
    }

    private void StateItemDataParsing()
    {
        List<Dictionary<string, string>> consumptionData = CSVReader.Read("Data/ItemsConsumptionItemDB");
        List<Dictionary<string, string>> equipmentData = CSVReader.Read("Data/ItemsEquipmentDB");
        for (int i = 0; i < consumptionData.Count; i++)
        {
            StateItemData stateItemData;

            int item_id = int.Parse(consumptionData[i]["item_id"]);
            string item_name = consumptionData[i]["item_name"];
            string item_desc = consumptionData[i]["item_desc"];
            Sprite item_icon = null;
            Enum_ItemType item_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), consumptionData[i]["item_type"]);
            Enum_EquipmentType item_equipmenttype = (Enum_EquipmentType)Enum.Parse(typeof(Enum_EquipmentType), consumptionData[i]["item_equipmenttype"]);
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

            stateItemData = new StateItemData(item_id, item_name, item_desc, item_icon, item_type, item_equipmenttype, item_purchaseprice, item_sellingprice, item_level,
                item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp ,item_maxcount);


#if List
            stateItemDatas.Add(stateItemData);
#endif
#if Dictionary
            stateItemDatas.Add(item_id, stateItemData);
#endif
        }

        for (int i = 0; i < equipmentData.Count; i++)
        {
            StateItemData stateItemData;

            int item_id = int.Parse(equipmentData[i]["item_id"]);
            string item_name = equipmentData[i]["item_name"];
            string item_desc = equipmentData[i]["item_desc"];
            Sprite item_icon = null;
            Enum_ItemType item_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), equipmentData[i]["item_type"]);
            Enum_EquipmentType item_equipmenttype = (Enum_EquipmentType)Enum.Parse(typeof(Enum_EquipmentType), equipmentData[i]["item_equipmenttype"]);
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

            stateItemData = new StateItemData(item_id, item_name, item_desc, item_icon, item_type, item_equipmenttype, item_purchaseprice, item_sellingprice, item_level,
                item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp, item_maxcount);

#if List
            stateItemDatas.Add(stateItemData);
#endif
#if Dictionary
            stateItemDatas.Add(item_id, stateItemData);
#endif
        }
    }

#if Dictionary
    public static StateItemData StateItemDataReader(int item_id)
    {
        StateItemData item = new StateItemData(stateItemDatas[item_id].id, stateItemDatas[item_id].name, stateItemDatas[item_id].desc,
            stateItemDatas[item_id].icon, stateItemDatas[item_id].itemType, stateItemDatas[item_id].equipmentType, stateItemDatas[item_id].purchaseprice,
            stateItemDatas[item_id].sellingprice, stateItemDatas[item_id].level, stateItemDatas[item_id].attack, stateItemDatas[item_id].defense,
            stateItemDatas[item_id].speed, stateItemDatas[item_id].attackSpeed, stateItemDatas[item_id].hp, stateItemDatas[item_id].mp, stateItemDatas[item_id].exp,
            stateItemDatas[item_id].maxHp, stateItemDatas[item_id].maxMp, stateItemDatas[item_id].maxCount);

        return item;
    }

    public static ETCItemData ETCItemDataReader(int item_id)
    {
        return etcItemDatas[item_id];
    }

#endif

    public void ETCItemDataParsing()
    {
        List<Dictionary<string, string>> ectData = CSVReader.Read("Data/ItemsETCDB");

        for (int i = 0; i < ectData.Count; i++)
        {
            ETCItemData etcItemData;

            int ectItem_id = int.Parse(ectData[i]["item_id"]);
            string ectItem_name = ectData[i]["item_name"];
            string ectItem_desc = ectData[i]["item_desc"];
            Sprite ectItem_icon = null;
            Enum_ItemType ectItem_type = (Enum_ItemType)Enum.Parse(typeof(Enum_ItemType), ectData[i]["item_type"]);   
            long ectItem_purchaseprice = long.Parse(ectData[i]["item_purchaseprice"]);
            long ectItem_sellingprice = long.Parse(ectData[i]["item_sellingprice"]);                
            int ectItem_maxcount = int.Parse(ectData[i]["item_maxcount"]);

            etcItemData = new ETCItemData(ectItem_id, ectItem_name, ectItem_desc, ectItem_icon, ectItem_type, ectItem_purchaseprice, ectItem_sellingprice,
                ectItem_maxcount);


#if List
            etcItemDatas.Add(etcItemData);
#endif
#if Dictionary
            etcItemDatas.Add(ectItem_id, etcItemData);
#endif
        }
    } 
}
