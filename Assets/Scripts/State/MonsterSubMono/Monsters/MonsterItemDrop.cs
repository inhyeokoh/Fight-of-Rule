using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterItemDrop : SubMono<MonsterController>
{
    public int minGold;
    public int maxGold;
    public string[] Item;
    float[] stateItemPersent;
    public ItemData[] items;

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
            Item = _board.monsterItemDropDB.monster_itemdrop;
            items = new ItemData[Item.Length];

            for (int i = 0; i < Item.Length; i++)
            {
                items[i] = ItemParsing.MonsterDropItem(Item[i]);
            }
        }
        //stateItem = _board.monsterDB.monster_stateitem;
        //etcItem = _board.monsterDB.moinster_etcitem;
    }

}
