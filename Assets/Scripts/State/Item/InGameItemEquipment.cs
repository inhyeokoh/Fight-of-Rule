using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemEquipment : ItemObject
{
    protected static Dictionary<int, State> ItemStates = new Dictionary<int, State>();


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

        playerEquipment = PlayerController.instance._playerEquipment;

        switch (equipmentType)
        {
          /*  case Enum_EquipmentType.Weapon:
                state = weaponState[item.StateIndex];
                break;
            case Enum_EquipmentType.Head:
                state = headState[item.StateIndex];
                break;
            case Enum_EquipmentType.Body:
                state = bodyState[item.StateIndex];
                break;
            case Enum_EquipmentType.Hand:
                state = handState[item.StateIndex];
                break;
            case Enum_EquipmentType.Foot:
                state = footState[item.StateIndex];
                break;*/
        }
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
       
    }
}