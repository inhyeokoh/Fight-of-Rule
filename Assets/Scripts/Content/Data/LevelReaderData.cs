#define Dictionary
//#define List
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReaderData : MonoBehaviour
{
#if List
    public List<LevelData> warriorLevelDatas = new List<LevelData>();
    public List<LevelData> archerLevelDatas = new List<LevelData>();
    public List<LevelData> wizardLevelDatas = new List<LevelData>();
#endif

#if Dictionary
    public static Dictionary<int, LevelData> warriorLevelDatas = new Dictionary<int, LevelData>();
    public static Dictionary<int, LevelData> archerLevelDatas = new Dictionary<int, LevelData>();
    public static Dictionary<int, LevelData> wizardLevelDatas = new Dictionary<int, LevelData>();
#endif


    private void Awake()
    {
        Reader("Data/WarriorLevelDB");
    }

    //레벨 CSV 불러오기
    private void Reader(string characterClass)
    {
        List<Dictionary<string,string>> levelDatas = CSVReader.Read(characterClass);

        for (int i = 0; i < levelDatas.Count; i++)
        {
            LevelData levelData;

            int level = int.Parse(levelDatas[i]["level"]);
            int maxhp = int.Parse(levelDatas[i]["maxhp"]);
            int maxmp = int.Parse(levelDatas[i]["maxmp"]);
            int maxexp = int.Parse(levelDatas[i]["maxexp"]);
            int attack = int.Parse(levelDatas[i]["attack"]);
            int defense = int.Parse(levelDatas[i]["defense"]);

            levelData = new LevelData(level, maxhp, maxmp, maxexp, attack, defense);

            switch (characterClass)
            {
                case "Data/WarriorLevelDB":
#if List
                    warriorLevelDatas.Add(levelData);
#endif
#if Dictionary                
                    warriorLevelDatas.Add(level, levelData);               
#endif
                    break;             
            }
        }
    }

    // 직업마다 맞는 레벨 데이터 불러오기
    // 데이터를 깊은 복사 방식으로 리턴하게함
    public static LevelData CurrentLevelData(int level, Enum_Class characterClass)
    {
        LevelData currentLevelData;
        currentLevelData = null;

        switch (characterClass)
        {
            case Enum_Class.Default:             
                break;
            case Enum_Class.Warrior:
                currentLevelData = warriorLevelDatas[level];
                break;
            case Enum_Class.Archer:
                currentLevelData = archerLevelDatas[level];
                break;
            case Enum_Class.Wizard:
                currentLevelData = wizardLevelDatas[level];
                break;          
        }
  
        return currentLevelData;
    }
}
