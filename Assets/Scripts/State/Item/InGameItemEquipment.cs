using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemEquipment : ItemObject
{
    //����� ���� Ȯ��
    [SerializeField]
    protected Enum_Class equipmentClass;

    //��� ���� Ȯ��
    [SerializeField]
    protected Enum_EquipmentType equipmentType;

    //���� �÷��̾��� ����Ȯ�ΰ� ��� ������ ���ݵ��� �Ѱ��ֱ����� Ŭ����
    public CharacterEquipment playerEquipment;

    //���� ����� �������� �����ֱ� ���� ������Ʈ
    public InGameItemEquipment ap;

    public Enum_Class EquipmentClass { get { return equipmentClass; } }
    public Enum_EquipmentType EquipmentType { get { return equipmentType; } }


    // ��� ����
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

    // ��� ���� ������ ������ ������ �´��� ������ �����ϰ� �ϴ� �޼���
    public override void Check()
    {
        Debug.Log(ap);
        playerEquipment = PlayerController.instance._playerEquipment;
        playerEquipment.EquipmentCheck(ap);     
    }

    public override void Data(int itemID)
    {
        
    }
}
