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

    public Sprite ItemImage { get { return itemImage; } }

    public int ItemId { get { return itemId; } }

    public string ItemName { get { return itemName; } }

    public string ItemDescription { get { return itemDescription; } }

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

    public Item(Sprite itemImage, int itemId,string itemName, string itemDescription, int level, int portionStat, int attack,int defense,int speed,
        int attackSpeed,int maxHp,int maxMp, int maxCount, int count, bool countable, int slotNum)
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
    }
}
