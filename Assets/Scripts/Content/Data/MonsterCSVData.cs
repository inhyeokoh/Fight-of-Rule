using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCSVData : MonoBehaviour
{
    public static MonsterCSVData instance;

    [SerializeField]
    List<MonsterData> monstersData = new List<MonsterData>();
    Dictionary<int, MonsterData> monsterDatas = new Dictionary<int, MonsterData>();

    Dictionary<int, MonsterItemDropData> monsterItemDrops = new Dictionary<int, MonsterItemDropData>();

    private void Awake()
    {
        instance = this;

        MonstersDBReader();
        MonsterInstance(0);
    }
    private void MonstersDBReader()
    {
        List<Dictionary<string, string>> data = CSVReader.Read("Data/MonstersDB");
        List<Dictionary<string, string>> dropData = CSVReader.Read("Data/MonsterItemDropDB");

        for(int i = 0; i < data.Count; i++)
        {
            MonsterData monsterData;

            int monster_id = int.Parse(data[i]["monster_id"]);
            string monster_object = data[i]["monster_object"];
            string monster_name = data[i]["monster_name"];
            Enum_MonsterType monster_type = (Enum_MonsterType)Enum.Parse(typeof(Enum_MonsterType), data[i]["monster_type"]);
            int monster_level = int.Parse(data[i]["monster_level"]);
            int monster_exp = int.Parse(data[i]["monster_exp"]);
            int monster_maxhp = int.Parse(data[i]["monster_maxhp"]); 
            int monster_maxmp = int.Parse(data[i]["monster_maxmp"]); 
            int monster_attack = int.Parse(data[i]["monster_attack"]);
            float monster_attackspeed = float.Parse(data[i]["monster_attackspeed"]);
            float monster_delay = float.Parse(data[i]["monster_delay"]);
            float monster_abliltydelay = float.Parse(data[i]["monster_abliltydelay"]);
            int monster_defense = int.Parse(data[i]["monster_defense"]);
            int monster_speed = int.Parse(data[i]["monster_speed"]);
            float monster_detectdistance = float.Parse(data[i]["monster_detectdistance"]);
            float monster_attackdistance = float.Parse(data[i]["monster_attackdistance"]);
            int[] monster_stateitem = Array.ConvertAll(data[i]["monster_stateitem"].Split(","), int.Parse);
            int[] moinster_etcitem = Array.ConvertAll(data[i]["moinster_etcitem"].Split(","), int.Parse);
            int monster_mingold = int.Parse(data[i]["monster_mingold"]);
            int monster_maxgold = int.Parse(data[i]["monster_maxgold"]);

            monsterData = new MonsterData(monster_id, monster_object, monster_name, monster_type, monster_level, monster_exp, monster_maxhp, monster_maxmp, monster_attack, monster_attackspeed, monster_delay,
                monster_abliltydelay, monster_defense, monster_speed, monster_detectdistance, monster_attackdistance, monster_stateitem, moinster_etcitem, monster_mingold, monster_maxgold);

            monstersData.Add(monsterData);
            monsterDatas.Add(monster_id, monsterData);
        }

        for (int i = 0; i < dropData.Count; i++)
        {
            MonsterItemDropData monsterItemDropData;

            int monster_id = int.Parse(dropData[i]["monster_id"]);
            string[] monster_itemdrop = dropData[i]["monster_itemdrop"].Split(",");
            int monster_mingold = int.Parse(dropData[i]["monster_mingold"]);
            int monster_maxgold = int.Parse(dropData[i]["monster_maxgold"]);

            monsterItemDropData = new MonsterItemDropData(monster_id, monster_itemdrop, monster_mingold, monster_maxgold);

            monsterItemDrops.Add(monster_id, monsterItemDropData);
        }


    }

    public  GameObject MonsterInstance(int monsterID)
    {
        GameObject monster = Resources.Load<GameObject>(monsterDatas[monsterID].monster_object);

        MonsterController monsterStatus = monster.GetComponent<MonsterController>();
        monsterStatus.MonsterDBReader(monsterDatas[monsterID],monsterItemDrops[monsterID]);


        return monster;
    }
            
            
}
