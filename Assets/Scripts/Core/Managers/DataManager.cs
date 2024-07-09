using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Google.Protobuf;

public class DataManager : SubClass<GameManager>
{
    // 환경설정 정보
    public VOL_OPTIONS volOptions;

    // 캐릭터 정보
    public CHARACTER_INFO[] characters;
    int selectedSlotNum = 0;
    public int SelectedSlotNum
    {
        get { return selectedSlotNum; }
        set { selectedSlotNum = value; CurrentCharacter = characters[selectedSlotNum]; }
    }

    public CHARACTER_INFO CurrentCharacter { get; set; }

    // 어딘가엔 들고 있어야함
    /*    public long CharId { get; set; }
        string charName;
        public string CharName
        {
            get { return charName; }
            set { charName = value; GameManager.UI.PlayerInfo.UpdateStatus(); }
        }
        int job;
        public int Job
        {
            get { return job; }
            set { job = value; GameManager.UI.PlayerInfo.UpdateStatus(); }
        }
        bool gender;
        public bool Gender
        {
            get { return gender; }
            set { gender = value; GameManager.UI.PlayerInfo.UpdateStatus(); }
        }
        Vector3 pos;*/

    /// <summary>
    /// 아이템 데이터
    /// </summary>
    public Dictionary<string, int> DropItems = new Dictionary<string, int>();
    public Dictionary<int, ItemData> itemDatas = new Dictionary<int, ItemData>();
    /////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 몬스터 데이터
    /// </summary>
    List<MonsterData> monstersData = new List<MonsterData>();
    public Dictionary<int, MonsterData> monsterDatas = new Dictionary<int, MonsterData>();

    Dictionary<int, MonsterItemDropData> monsterItemDrops = new Dictionary<int, MonsterItemDropData>();
    /////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// 레벨 데이터
    /// </summary>
    public Dictionary<int, LevelData> warriorLevelDatas = new Dictionary<int, LevelData>();
    public Dictionary<int, LevelData> archerLevelDatas = new Dictionary<int, LevelData>();
    public Dictionary<int, LevelData> wizardLevelDatas = new Dictionary<int, LevelData>();
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 스킬 데이터
    /// </summary>
    public List<WarriorSkillData> warriorSkillData = new List<WarriorSkillData>();
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 퀘스트 데이터. ( key = questID, value = QuestData )
    /// </summary>
    public Dictionary<int, QuestData> questDict = new Dictionary<int, QuestData>();
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// NPC 데이터. { key = npcID, value = Npc }
    /// </summary>
    public Dictionary<int, Npc> npcDict = new Dictionary<int, Npc>();
    /////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// NPC 데이터. { key = npcID, value = List<ShopItem></ShopItem> }
    /// </summary>
    public Dictionary<int, ShopItem[]> shopDict = new Dictionary<int, ShopItem[]>();
    /////////////////////////////////////////////////////////////////////////////////////

    string tableFolderPath;

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        volOptions = new VOL_OPTIONS();
        characters = new CHARACTER_INFO[4];
        CurrentCharacter = new CHARACTER_INFO();

        tableFolderPath = "Data/SheetsToCSV/bin/Debug/TableFiles/";
        DBDataLoad();
#if SERVER || CLIENT_TEST_TITLE
#elif CLIENT_TEST_PROPIM || CLIENT_TEST_HYEOK
        CurrentCharacter = new CHARACTER_INFO();
        CurrentCharacter.BaseInfo = new CHARACTER_BASE();
        CurrentCharacter.BaseInfo.CharacterId = 0;
        CurrentCharacter.BaseInfo.Nickname = ByteString.CopyFrom("HelloWorld", System.Text.Encoding.Unicode);
        CurrentCharacter.BaseInfo.CharacterClass = 0;
        CurrentCharacter.BaseInfo.Gender = true;

        CurrentCharacter.Stat = new CHARACTER_STATUS();
        CurrentCharacter.Stat.Level = 1;
        CurrentCharacter.Stat.MaxHP = 100;
        CurrentCharacter.Stat.Hp = 100;
        CurrentCharacter.Stat.MaxMP = 100;
        CurrentCharacter.Stat.Mp = 100;
        CurrentCharacter.Stat.MaxEXP = 100;
        CurrentCharacter.Stat.Exp = 0;
        CurrentCharacter.Stat.Attack = 5;
        CurrentCharacter.Stat.AttackSpeed = 1;
        CurrentCharacter.Stat.Defense = 3;
        CurrentCharacter.Stat.Speed = 10;

        CurrentCharacter.Vector3 = new VECTOR3();
        CurrentCharacter.Vector3.X = 0;
        CurrentCharacter.Vector3.Y = 0;
        CurrentCharacter.Vector3.Z = 0;

        volOptions.MasterVol = 0.7f;
        volOptions.BgmVol = 0.6f;
        volOptions.EffectVol = 0.5f;
        volOptions.VoiceVol = 0.5f;
#endif
    }

    void DBDataLoad()
    {
        ItemTableParsing();
        SkillTableParsing("WarriorSkillTable");
        MonstersTableParsing();
        LevelTableParsing("WarriorLevelTable");
        QuestTableParsing("QuestTable");
        //NpcTableParsing("NpcTable");
        ShopTableParsing("ShopTable");
    }

    private void ItemTableParsing()
    {
        List<Dictionary<string, string>> consumptionData = CSVReader.Read($"{tableFolderPath}ItemsConsumptionItemTable");
        List<Dictionary<string, string>> equipmentData = CSVReader.Read($"{tableFolderPath}ItemsEquipmentTable");
        List<Dictionary<string, string>> etcData = CSVReader.Read($"{ tableFolderPath}ItemsETCTable");


        for (int i = 0; i < consumptionData.Count; i++)
        {
            ConsumptionItemData ItemData;

            int item_id = int.Parse(consumptionData[i]["item_id"]);
            string item_name = consumptionData[i]["item_name"];
            string item_desc = consumptionData[i]["item_desc"];
            Sprite item_icon = GameManager.Resources.Load<Sprite>(consumptionData[i]["item_icon"]);

            Enum_Grade item_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), consumptionData[i]["item_grade"]);
            Enum_ConsumptionDetailType item_detailtype = (Enum_ConsumptionDetailType)Enum.Parse(typeof(Enum_ConsumptionDetailType), consumptionData[i]["item_detailtype"]);           
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
            bool item_durationbool = bool.Parse(consumptionData[i]["item_durationbool"]);
            float item_duration = float.Parse(consumptionData[i]["item_duration"]);



            ItemData = new ConsumptionItemData(item_id, item_name, item_desc, item_icon, item_grade, Enum_ItemType.Consumption, item_detailtype, item_sellingprice, item_level,
                item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp, item_durationbool, item_duration, item_maxcount);

            DropItems.Add(item_name, item_id);
            itemDatas.Add(item_id, ItemData);
        }

        for (int i = 0; i < equipmentData.Count; i++)
        {
            EquipmentItemData ItemData;

            int item_id = int.Parse(equipmentData[i]["item_id"]);
            string item_name = equipmentData[i]["item_name"];
            string item_desc = equipmentData[i]["item_desc"];
            Sprite item_icon = GameManager.Resources.Load<Sprite>(equipmentData[i]["item_icon"]);
            Enum_Class item_class = (Enum_Class)Enum.Parse(typeof(Enum_Class), equipmentData[i]["item_class"]);
            Enum_Grade item_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), equipmentData[i]["item_grade"]);
            Enum_EquipmentDetailType item_equipmenttype = (Enum_EquipmentDetailType)Enum.Parse(typeof(Enum_EquipmentDetailType), equipmentData[i]["item_detailtype"]);   
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
            int item_maxreinforcement = int.Parse(equipmentData[i]["item_maxreinforcement"]);
            bool item_durationbool = bool.Parse(equipmentData[i]["item_durationbool"]);
            float item_duration = float.Parse(equipmentData[i]["item_duration"]);

            ItemData = new EquipmentItemData(item_id, item_name, item_desc, item_icon, item_class, item_grade, Enum_ItemType.Equipment, item_equipmenttype, item_sellingprice, item_level,
             item_attack, item_defense, item_speed, item_attackspeed, item_hp, item_mp, item_exp, item_maxhp, item_maxmp, item_maxreinforcement, item_durationbool, item_duration);

            DropItems.Add(item_name, item_id);
            itemDatas.Add(item_id, ItemData);

        }

        for (int i = 0; i < etcData.Count; i++)
        {
            ItemData ItemData;

            int etcItem_id = int.Parse(etcData[i]["item_id"]);
            string etcItem_name = etcData[i]["item_name"];
            string etcItem_desc = etcData[i]["item_desc"];
            Sprite etcItem_icon = GameManager.Resources.Load<Sprite>(etcData[i]["item_icon"]);
            Enum_Grade etcItem_grade = (Enum_Grade)Enum.Parse(typeof(Enum_Grade), etcData[i]["item_grade"]);     
            long etcItem_sellingprice = long.Parse(etcData[i]["item_sellingprice"]);
            int etcItem_maxcount = int.Parse(etcData[i]["item_maxcount"]);

            ItemData = new ItemData(etcItem_id, etcItem_name, etcItem_desc, etcItem_icon, Enum_ItemType.ETC, etcItem_grade, etcItem_sellingprice,
                etcItem_maxcount);


            DropItems.Add(etcItem_name, etcItem_id);
            itemDatas.Add(etcItem_id, ItemData);
        }
    }

    public ItemData StateItemDataReader(int item_id)
    {
        EquipmentItemData itemData = itemDatas[item_id] as EquipmentItemData;

        if (itemData != null)
        {
            EquipmentItemData itemDataPasing;

            itemDataPasing = new EquipmentItemData(itemData.id, itemData.name, itemData.desc, itemData.icon, itemData.itemClass, itemData.itemGrade, itemData.itemType,
                itemData.detailType,  itemData.sellingprice, itemData.level, itemData.attack, itemData.defense, itemData.speed, itemData.attackSpeed
                , itemData.hp, itemData.mp, itemData.exp, itemData.maxHp, itemData.maxMp, itemData.maxReinforcement, itemData.durationBool, itemData.duration);

            return itemDataPasing;
        }

        ConsumptionItemData secondItemData = itemDatas[item_id] as ConsumptionItemData;

        if (secondItemData != null)
        {
            ConsumptionItemData itemDataPasing;

            itemDataPasing = new ConsumptionItemData(secondItemData.id, secondItemData.name, secondItemData.desc, secondItemData.icon, secondItemData.itemGrade, secondItemData.itemType,secondItemData.detailType,
              secondItemData.sellingprice, secondItemData.level, secondItemData.attack, secondItemData.defense, secondItemData.speed, secondItemData.attackSpeed
                , secondItemData.hp, secondItemData.mp, secondItemData.exp, secondItemData.maxHp, secondItemData.maxMp, secondItemData.durationBool, secondItemData.duration, secondItemData.maxCount);

            return itemDataPasing;
        }
        else
        {
            ItemData itemDataPasing;
            itemDataPasing = new ItemData(itemDatas[item_id].id, itemDatas[item_id].name, itemDatas[item_id].desc, itemDatas[item_id].icon, itemDatas[item_id].itemType,
                itemDatas[item_id].itemGrade, itemDatas[item_id].sellingprice, itemDatas[item_id].maxCount);

            return itemDataPasing;
        }
    }

    public ItemData MonsterDropItem(string itemName)
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
            Debug.Log("아이템 정보가 없습니다.");

        }

        return null;
    }
    public ItemData MonsterDropItem(int itemID)
    {
        ItemData item;

        try
        {
            item = StateItemDataReader(itemID);

            return item;
        }
        catch
        {
            Debug.Log("아이템 정보가 없습니다.");

        }

        return null;
    }
    private void MonstersTableParsing()
    {
        List<Dictionary<string, string>> data = CSVReader.Read($"{tableFolderPath}MonsterTable");
        List<Dictionary<string, string>> dropData = CSVReader.Read($"{tableFolderPath}MonsterItemTable");

        for (int i = 0; i < data.Count; i++)
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
           /* int[] monster_stateitem = Array.ConvertAll(data[i]["monster_stateitem"].Split(","), int.Parse);
            int[] moinster_etcitem = Array.ConvertAll(data[i]["moinster_etcitem"].Split(","), int.Parse);*/

            monsterData = new MonsterData(monster_id, monster_object, monster_name, monster_type, monster_level, monster_exp, monster_maxhp, monster_maxmp, monster_attack, monster_attackspeed, monster_delay,
                monster_abliltydelay, monster_defense, monster_speed, monster_detectdistance, monster_attackdistance/*, monster_stateitem, moinster_etcitem*/);

            monstersData.Add(monsterData);
            monsterDatas.Add(monster_id, monsterData);
        }

        for (int i = 0; i < dropData.Count; i++)
        {
            MonsterItemDropData monsterItemDropData;

            int monster_id = int.Parse(dropData[i]["monster_id"]);
            int[] monster_itemdrop = Array.ConvertAll(dropData[i]["monster_itemdrop"].Split(","), int.Parse);
            float[] monster_itempercent = Array.ConvertAll(dropData[i]["monster_itempercent"].Split(","), float.Parse);
            int monster_mingold = int.Parse(dropData[i]["monster_mingold"]);
            int monster_maxgold = int.Parse(dropData[i]["monster_maxgold"]);

            monsterItemDropData = new MonsterItemDropData(monster_id, monster_itemdrop, monster_itempercent, monster_mingold, monster_maxgold);

            monsterItemDrops.Add(monster_id, monsterItemDropData);
        }


    }

    public GameObject MonsterInstance(int monsterID)
    {
        GameObject monster = Resources.Load<GameObject>(monsterDatas[monsterID].monster_object);

        MonsterController monsterStatus = monster.GetComponent<MonsterController>();
        monsterStatus.MonsterDBReader(monsterDatas[monsterID], monsterItemDrops[monsterID]);


        return monster;
    }

    private void LevelTableParsing(string characterClass)
    {
        List<Dictionary<string, string>> levelDatas = CSVReader.Read(tableFolderPath + characterClass);

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
                case "WarriorLevelTable":
                    warriorLevelDatas.Add(level, levelData);
                    break;
            }
        }
    }
    public LevelData CurrentLevelData(int level, Enum_Class characterClass)
    {
        LevelData currentLevelData;
        currentLevelData = null;

        switch (characterClass)
        {
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

    public void SkillTableParsing(string fileName)
    {
        List<Dictionary<string, string>> skill = CSVReader.Read(tableFolderPath + fileName);

        for (int i = 0; i < skill.Count; i++)
        {
            WarriorSkillData warrirSkill;

            int id = int.Parse(skill[i]["skill_id"]);
            string skillType = skill[i]["skill_type"];
            string name = skill[i]["skill_name"];
            string desc = skill[i]["skill_desc"];
            string iconString = skill[i]["skill_icon"];
            float[] duration = Check<float>("skill_duration", i, skill);

            //print(Resources.Load(iconString).name);
            Sprite icon = Resources.Load<Sprite>(iconString);

            WarriorSkill skillNumber = (WarriorSkill)Enum.Parse(typeof(WarriorSkill), skill[i]["skill_number"]);
            int skillMaxLevel = int.Parse(skill[i]["skill_maxlevel"]);

            int[] skillLevelCondition = Check<int>("skill_levelcondition", i, skill);
            int[] skillPoint = Check<int>("skill_point", i, skill);
            int[] skillMaxHP = Check<int>("skill_maxhp", i, skill);
            int[] skillMaxMP = Check<int>("skill_maxmp", i, skill);
            int[] skillAttack = Check<int>("skill_attack", i, skill);
            int[] skillDefnse = Check<int>("skill_defense", i, skill);
            int[] skillSpeed = Check<int>("skill_speed", i, skill);
            int[] skillAttackSpeed = Check<int>("skill_attackspeed", i, skill);
            int[] skillMP = Check<int>("skill_mp", i, skill);
            float[] skillCool = Check<float>("skill_cool", i, skill);
            int[] skillDamage = Check<int>("skill_damage", i, skill);


            warrirSkill = new WarriorSkillData(id, skillType, name, desc, icon, skillNumber, duration, skillMaxLevel, skillLevelCondition, skillPoint, skillMaxHP, skillMaxMP, skillAttack, skillDefnse, skillSpeed, skillAttackSpeed, skillMP, skillCool, skillDamage);


            warriorSkillData.Add(warrirSkill);
        }
    }

    public void QuestTableParsing(string fileName)
    {
        List<Dictionary<string, string>> quest = CSVReader.Read(tableFolderPath + fileName);

        for (int i = 0; i < quest.Count; i++)
        {
            QuestData questData;

            int questID = int.Parse(quest[i]["quest_id"]);
            string title = quest[i]["title"];
            int npcID = int.Parse(quest[i]["npc_id"]);
            Enum_QuestType questType = (Enum_QuestType)Enum.Parse(typeof(Enum_QuestType), quest[i]["quest_type"]);
            string[] conversationText = quest[i]["conversation_text"].Split("/");
            string summaryText = quest[i]["summary_text"];
            string ongoingText = quest[i]["ongoing_text"];
            string[] completeText = quest[i]["complete_text"].Split("/");
            int requiredLevel = int.Parse(quest[i]["required_level"]);
            int? nextQuestID = int.TryParse(quest[i]["next_quest_id"], out int tempNextQuestID) ? tempNextQuestID : null;
            int[] questObj = String.IsNullOrEmpty(quest[i]["quest_obj"]) ? null : Array.ConvertAll(quest[i]["quest_obj"].Split(","), int.Parse);
            int[] questObjRequiredCount = String.IsNullOrEmpty(quest[i]["quest_obj_required_count"]) ? null : Array.ConvertAll(quest[i]["quest_obj_required_count"].Split(","), int.Parse);
            int[] questMonster = String.IsNullOrEmpty(quest[i]["quest_monster"]) ? null : Array.ConvertAll(quest[i]["quest_monster"].Split(","), int.Parse);
            int[] questMonsterRequiredCount = String.IsNullOrEmpty(quest[i]["quest_monster_required_count"]) ? null : Array.ConvertAll(quest[i]["quest_monster_required_count"].Split(","), int.Parse);
            int expReward = String.IsNullOrEmpty(quest[i]["exp_reward"]) ? -1 : int.Parse(quest[i]["exp_reward"]);
            long goldReward = String.IsNullOrEmpty(quest[i]["gold_reward"]) ? -1L : int.Parse(quest[i]["gold_reward"]);
            int[] itemRewardIDs = String.IsNullOrEmpty(quest[i]["item_reward_id"]) ? null : Array.ConvertAll(quest[i]["item_reward_id"].Split(","), int.Parse);
            int[] itemRewardCounts = String.IsNullOrEmpty(quest[i]["item_reward_count"]) ? null : Array.ConvertAll(quest[i]["item_reward_count"].Split(","), int.Parse);

            List<QuestGoal> goals = new List<QuestGoal>();
            if (questObj != null)
            {
                for (int j = 0; j < questObj.Length; j++)
                {
                    ObjectGoal objGoal = new ObjectGoal(questObj[j], questObjRequiredCount[j]);
                    goals.Add(objGoal);
                }
            }

            if (questMonster != null)
            {
                for (int j = 0; j < questMonster.Length; j++)
                {
                    MonsterGoal monsterGoal = new MonsterGoal(questMonster[j], questMonsterRequiredCount[j]);

                    goals.Add(monsterGoal);
                }
            }

            List<ItemData> itemRewards = new List<ItemData>();
            if (itemRewardIDs != null)
            {
                for (int j = 0; j < itemRewardIDs.Length; j++)
                {
                    ItemData item = GameManager.Data.StateItemDataReader(itemRewardIDs[j]);
                    item.count = itemRewardCounts[j];
                    itemRewards.Add(item);
                }
            }

            questData = new QuestData(questID, title, npcID, questType, conversationText, summaryText, ongoingText, completeText, requiredLevel, nextQuestID, goals,
                expReward, goldReward, itemRewards);

            questDict.Add(questID, questData);
        }
    }

    public void NpcTableParsing(string fileName)
    {
        List<Dictionary<string, string>> npcTable = CSVReader.Read(tableFolderPath + fileName);

        GameObject[] npcArray = GameObject.FindGameObjectsWithTag("Npc");
        foreach (GameObject npcObj in npcArray)
        {
            Npc npc = npcObj.GetComponent<Npc>();
            if (npc != null)
            {
                npcDict.Add(npc.NpcID, npc);
            }
        }

        for (int i = 0; i < npcTable.Count; i++)
        {
            int npcID = int.Parse(npcTable[i]["npc_id"]);
            string name = npcTable[i]["name"];
            string type = npcTable[i]["type"];
            string defaultText = npcTable[i]["default_text"];
            npcDict[npcID].name = name; // 오브젝트명 변경
            npcDict[npcID].NpcName = name;
            npcDict[npcID].npcType = (Enum_NpcType)Enum.Parse(typeof(Enum_NpcType), type);
            npcDict[npcID].DefaultText = defaultText;
        }
    }

    public void ShopTableParsing(string fileName)
    {
        List<Dictionary<string, string>> shopTable = CSVReader.Read(tableFolderPath + fileName);

        for (int i = 0; i < shopTable.Count; i++)
        {
            int npcID = int.Parse(shopTable[i]["npc_id"]);

            int[] sellingItemIDs = Array.ConvertAll(shopTable[i]["selling_item_id"].Split(","), int.Parse);
            int[] sellingPrices = Array.ConvertAll(shopTable[i]["selling_price"].Split(","), int.Parse);
            ShopItem[] shopItems = new ShopItem[sellingItemIDs.Length];
            for (int j = 0; j < sellingItemIDs.Length; j++)
            {
                ShopItem item = new ShopItem
                {
                    itemID = sellingItemIDs[j],
                    itemPrice = sellingPrices[j],
                    iconSprite = itemDatas[sellingItemIDs[j]].icon
                };
                shopItems[j] = item;
            }

            shopDict.Add(npcID, shopItems);
        }
    }

    public T[] Check<T>(string dataName, int index, List<Dictionary<string, string>> skill)
    {
        if (skill[index][dataName] == "null")
        {
            return null;
        }

        string[] data = skill[index][dataName].Split(",");
        T[] array = new T[data.Length];

        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (T)Convert.ChangeType(data[i], typeof(T));
        }

        return array;

    }
}