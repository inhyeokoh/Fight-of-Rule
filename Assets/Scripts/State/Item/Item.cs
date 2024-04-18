using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    private Sprite itemImage;
    private int itemId;
    private string itemName;
    private string itemDescription; 

    private int level;

    private int portionStat;

    private int attack;
    private int defense;
    private int speed;
    private int attackSpeed;
    private int maxHp;
    private int maxMp;
    private int maxCount;
    private int count;
    private int slotNum;
    private bool countable;
    private int stateIndex;

    public Enum_ItemType ItemType { get; private set; }
    public Enum_ItemGrade ItemGrade { get; private set; }
    public Enum_DetailType DetailType { get; private set; }

    public enum Enum_ItemType // 아이템 대분류
    {
        Equipment,
        Consumption,
        Materials,
        Etc
    }

    public enum Enum_DetailType // 상세타입
    {
        Weapon,
        Head,
        Body,
        Belt,
        Hand,
        Foot,
        Potion,
        Box,
        None
    }

    public enum Enum_ItemGrade // 아이템 등급
    {
        Normal,
        Rare,
        Unique,
        Legendary
    }

    public Sprite ItemImage { get { return itemImage; } }

    public int ItemId { get { return itemId; } }

    public string ItemName { get { return itemName; } }

    public string ItemDescription { get { return itemDescription; } }

    public int StateIndex { get { return stateIndex; } }

    public int Level { get { return level; } set { level = value; } }

    public int PortionStat { get { return portionStat; } set { portionStat = value; } }
    public int Attack { get { return attack; } set { attack = value; } }
    public int Defense { get { return defense; } set { defense = value; } }
    public int Speed { get { return speed; } set { speed = value; } }
    public int AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public int MaxHp { get { return maxHp; } set { maxHp = value; } }
    public int MaxMp { get { return maxMp; } set { maxMp = value; } }
    public int MaxCount { get { return maxCount; } set { maxCount = value; } }
    public int Count { get { return count; } set { count = value; } }

    public int SlotNum { get { return slotNum; } set { slotNum = value; } }

    public bool Countable { get { return countable; } set { countable = value; } }

    public Item(Sprite itemImage, int itemId, string type ,string itemName, string itemDescription, int level, int portionStat, int attack,int defense,int speed,
        int attackSpeed,int maxHp,int maxMp, int maxCount, int count, bool countable, int slotNum, int stateIndex, string grade, string detailType)
    {
        this.itemImage = itemImage;
        this.itemId = itemId;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.level = level;
        this.portionStat = portionStat;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.maxCount = maxCount;
        this.count = count;
        this.countable = countable;
        this.slotNum = slotNum;
        this.stateIndex = stateIndex;

        ItemType = GetItemType(type);
        ItemGrade = GetItemGrade(grade);
        DetailType = GetItemDetailType(detailType);
    }

    // 아이템의 타입을 반환하는 메서드
    Enum_ItemType GetItemType(string itemType)
    {
        switch (itemType)
        {
            case "Equipment":
                return Enum_ItemType.Equipment;
            case "Consumption":
                return Enum_ItemType.Consumption;
            case "Materials":
                return Enum_ItemType.Materials;
            default:
                return Enum_ItemType.Etc;
        }
    }

    Enum_ItemGrade GetItemGrade(string itemGrade)
    {
        switch (itemGrade)
        {
            case "Rare":
                return Enum_ItemGrade.Rare;
            case "Unique":
                return Enum_ItemGrade.Unique;
            case "Legendary":
                return Enum_ItemGrade.Legendary;
            default:
                return Enum_ItemGrade.Normal;
        }
    }

    Enum_DetailType GetItemDetailType(string detailType)
    {
        switch (detailType)
        {
            case "Weapon":
                return Enum_DetailType.Weapon;
            case "Helmet":
                return Enum_DetailType.Head;
            case "Armor":
                return Enum_DetailType.Body;
            case "Belt":
                return Enum_DetailType.Belt;
            case "Boots":
                return Enum_DetailType.Foot;
            case "Gloves":
                return Enum_DetailType.Hand;
            case "Potion":
                return Enum_DetailType.Potion;
            case "Box":
                return Enum_DetailType.Box;
            default:
                return Enum_DetailType.None;
        }
    }
}
