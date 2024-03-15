using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemEquipment : ItemBase
{
    [SerializeField]
    protected Enum_Class equipmentClass;
    [SerializeField]
    protected Enum_EquipmentType equipmentType;
    public CharacterEquipment playerEquipment;
    public InGameItemEquipment ap;

    public Enum_Class EquipmentClass { get { return equipmentClass; } }
    public Enum_EquipmentType EquipmentType { get { return equipmentType; } }

    public override void Setting()
    {
        playerEquipment = PlayerController.instance._playerEquipment;
    }

    public override void Enter()
    {
        stateMachine.EnterState(state);
    }

    public override void FixedStay()
    {
        stateMachine.FixedStay();
    }

    public override void Stay()
    {
        stateMachine.Stay();
    }


    public override void Exit()
    {
        stateMachine.ExitState();
    }

    public override void Check()
    {
        Debug.Log(ap);
        playerEquipment = PlayerController.instance._playerEquipment;
        playerEquipment.EquipmentCheck(ap);
      
        /* switch (equipmentType) 
        {
            case Enum_EquipmentType.Weapon:
                playerEquipment.EquipmentCheck(ap);
                break;
            case Enum_EquipmentType.Head:
                playerEquipment.EquipmentCheck(ap);
                break;
            case Enum_EquipmentType.Body:
                playerEquipment.EquipmentCheck(ap);
                break;
            case Enum_EquipmentType.Hand:
                playerEquipment.EquipmentCheck(ap);
                break;
            case Enum_EquipmentType.Foot:
                playerEquipment.EquipmentCheck(ap);
                break;
            default:
                print("이 아이템은 데이터상에 존재하지 않습니다.");
                return;
        }*/
     
    }

    public override void Data(int itemID)
    {
        
    }
}
