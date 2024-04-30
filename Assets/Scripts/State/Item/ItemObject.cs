using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_DetailType
{
    Default,
    Weapon,
    Head,
    Body,
    Hand,
    Foot,
    Potion,
    Box
}

public enum Enum_Grade
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary,
}
  
public enum Enum_ItemType  
{    
    Consumption,    
    Equipment,   
    ETC
}
public class ItemObject : MonoBehaviour
{
    // 아이템 정보
    public ItemData item;

    private int itemID; // 아이템 ID들을 분류 해놨지만 캐릭터가 가지고있는 아이템 개별 ID들을 어떻게 저장해야할지 모르겠음
  
    //포션이나 장비등등 스텟 정보를 넘겨주기위한 플레이어
    [SerializeField]

    protected CharacterStatus player;   

    private void Awake()
    {       
      /*  try
        {
            Setting();
        }
        catch
        {
            print("현재 플레이어를 찾을수 없습니다");
        }*/
    }

    private void Start()
    {
        //현재 아이템정보를 깊은 복사같은 방식으로 데이터를 불러옴
        //item = ItemParsing.StateItemDataReader(itemID);

        //이건 아이템들 마다 정보들이 따로 적용되는지 확인
        //Setting();       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            player = PlayerController.instance._playerStat;
        }     
    }
 
    //아이템의 정보들
    //아이템의 효과
    //아이템의 이름
    //현재 아이템을 적용할 캐릭터

    //아이템 베이스에는 소비 기타 장비의 공통된 기능을 넣는 스크립트
    //소비 장비 기타 아이템은 상속을 받아 따로 만들어줘야됌

    //주요 기능 상태패턴을 이용해라
}

