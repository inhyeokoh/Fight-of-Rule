using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Data
{
    public string itemType; //아이템 타입
    public string itemName; //아이템 이름
    public string itemDescription;//아이템 설명
    public string itemNumber; //아이템 수량
    public bool isUsing; //아이템 사용중 여부

    // public string itemID; //아이템 번호
    // private int itemStat; //아이템 능력치
    // private bool duration; //지속 아이템인지 아닌지

    public enum Enum_ItemType
    {
        Consumption,
        Equipment,
        Etc
    }

    private void OnEnable()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // public void Use();

    //아이템의 정보들
    //아이템의 효과
    //아이템의 이름
    //현재 아이템을 적용할 캐릭터

    //아이템 베이스에는 소비 기타 장비의 공통된 기능을 넣는 스크립트
    //소비 장비 기타 아이템은 상속을 받아 따로 만들어줘야됌

    //생각 해보면 기타아이템은 상속할 이유가 전혀 없는데



    //주요 기능 상태패턴을 이용해라
    //지속 시간이 필요하니 불을 이용해서 fixidUpdate로 시간을 나타내라

    public Item(string _itemType, string _itemName, string _itemDescription, string _itemNumber, bool _isUsing)
    {
        itemType = _itemType;
        itemName = _itemName;
        itemDescription = _itemDescription;
        itemNumber = _itemNumber;
        isUsing = _isUsing;
    }

}
