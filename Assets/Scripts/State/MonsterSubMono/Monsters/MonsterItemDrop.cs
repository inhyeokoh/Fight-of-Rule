using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterItemDrop : SubMono<MonsterController>
{
    public int minGold;
    public int maxGold;
    public string[] ItemName;
    private bool[] ItemProduce;
    float[] ItemPercent;
    public ItemData[] items;
    public List<GameObject> itemObject;

    // 아이템 테이블을 만들어서 db를 불러올때 Dictionary를 이용해서 데이터베이스 name id를 딕셔너리로 넣고
    // 몬스터 테이블에서 아이템 이름을 써서 몬스터 DB를 불러올때 몬스터 드랍하는 스트링 배열을 가져와서 name으로 저장되어있는
    // dictionary를 이용해 아이템 id를 불러와 아이템 id로 저장되있는 아이템 객체들을 가져오자
    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
        
    }

    protected override void _Init()
    {
        ItemDataDB();
    }   

    public void ItemDataDB()
    {
        if (!_board.DBRederOn)
        {
            minGold = _board.monsterItemDropDB.monster_mingold;
            maxGold = _board.monsterItemDropDB.monster_maxgold;
            ItemPercent = _board.monsterItemDropDB.monster_itempercent;
            ItemName = _board.monsterItemDropDB.monster_itemdrop;
            items = new ItemData[ItemName.Length];

            for (int i = 0; i < ItemName.Length; i++)
            {
                items[i] = GameManager.Data.MonsterDropItem(ItemName[i]);
            }
        }

        ItemProduce = new bool[items.Length];

        for (int i = 0; i < ItemProduce.Length; i++)
        {
            ItemProduce[i] = ItemResult(ItemPercent[i]);

            if (ItemProduce[i])
            {
                GameObject clone = ItemManager._item.ItemInstance(items[i], Vector3.one, Quaternion.identity);
                itemObject.Add(clone);
                clone.SetActive(false);
            }
        }

    }

    public bool ItemResult(float percent)
    {
        float result = Random.Range(0, 100f);

        if (result <= percent)
        {
            return true;
        }
        else
        {
            return false;
        }       
    }
    public void ItemDrop()
    {
        for (int i = 0; i < itemObject.Count; i++)
        {
            itemObject[i].SetActive(true);
            itemObject[i].transform.position = gameObject.transform.position;
            print(itemObject[i].GetComponent<ItemObject>().item.name);
        }
    }
}
