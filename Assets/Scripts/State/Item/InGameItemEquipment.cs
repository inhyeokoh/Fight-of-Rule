using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemEquipment : ItemObject
{
    protected static Dictionary<int, StateItem> StateItems = new Dictionary<int, StateItem>();


    //장비의 직업 확인

    //현재 플레이어의 직업확인과 장비를 꼇을때 스텟들을 넘겨주기위한 클래스
    public CharacterEquipment playerEquipment;

    //현재 장비의 정보들을 보내주기 위한 컴포넌트
    public InGameItemEquipment ap;
    [SerializeField]
    protected Enum_Class equipmentClass;


    //장비 부위 확인
    [SerializeField]
    protected Enum_EquipmentType equipmentType;

    public Enum_Class EquipmentClass { get { return equipmentClass; } }
    public Enum_EquipmentType EquipmentType { get { return equipmentType; } }


    // 장비 세팅
    public override void Setting()
    {       
        if (!stateComplete)        
        {         
            StateSetting();         
            print("적용 완료" + " " + gameObject.name);          
            stateComplete = true;      
        }         
        else      
        {        
            print("이미 적용됌" + " " + gameObject.name);               
        }

        stateItem = StateItems[item.id];
        Enter();

        playerEquipment = PlayerController.instance._playerEquipment;
  
    }

    // 장비를 낄려 했을때 장비들의 조건이 맞는지 맞으면 장착하게 하는 메서드
    public override void Check()
    {
        Debug.Log(ap);
        playerEquipment = PlayerController.instance._playerEquipment;
        playerEquipment.EquipmentCheck(ap);
    }

    public void StateSetting()
    {
        StateItems.Add(1000, new StateItem((item) => 
        {
            print($"{item.name} 가격 : {item.purchaseprice}");
        },         
        (item) => 
        {
            print($"현재 fixed작동중 가격 : {item.purchaseprice}");
        }, 
        (item) => 
        { 

        }, 
        (item) =>
        { 

        })) ;
        StateItems.Add(1004, new StateItem((item) =>
        {
            print($"{item.name} 공격력 : {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중 공격력 : {item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(2, new StateItem((item) =>
        {
            print($"{item.name} {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(3, new StateItem((item) =>
        {
            print($"{item.name} {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
    }
}