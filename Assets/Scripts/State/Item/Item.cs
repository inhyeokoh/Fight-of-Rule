using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    private Sprite itemImage;
    private int itemId;

    private string itemName;
    private string itemDescription;

    private int purchasePrice;
    private int sellingPrice;


    private int level;
 
    private int attack;
    private int defense;
    private int speed;
    private int attackSpeed;
    private int hp;
    private int mp;
    private int maxHp;
    private int maxMp;
    private int maxCount;
    private int count;
    private int slotNum;
    private bool countable;
    //private int stateIndex;

    Enum_ItemType itemType; 
    Enum_EquipmentType equipmentType;



    public Sprite ItemImage { get { return itemImage; } }

    public int ItemId { get { return itemId; } }

    public string ItemName { get { return itemName; } }

    public string ItemDescription { get { return itemDescription; } }

    public int PurchasePrice { get { return purchasePrice; } }

    public int SellingPrice { get { return sellingPrice; } }

    public int Level { get { return level; } set { level = value; } }
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

    public Item(Sprite itemImage, int itemId,string itemName, string itemDescription, int level, int attack,int defense,int speed,
        int attackSpeed,int maxHp,int maxMp, int maxCount)
    {
        this.itemImage = itemImage;
        this.itemId = itemId;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.level = level;
        this.attack = attack;
        this.defense = defense;
        this.speed = speed;
        this.attackSpeed = attackSpeed;
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.maxCount = maxCount;
    }
}
