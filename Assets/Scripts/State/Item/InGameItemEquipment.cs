using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemEquipment : ItemObject
{
    protected static List<State> weaponState = new List<State>();
    protected static List<State> headState = new List<State>();
    protected static List<State> bodyState = new List<State>();
    protected static List<State> handState = new List<State>();
    protected static List<State> footState = new List<State>();


    //����� ���� Ȯ��

    //���� �÷��̾��� ����Ȯ�ΰ� ��� ������ ���ݵ��� �Ѱ��ֱ����� Ŭ����
    public CharacterEquipment playerEquipment;

    //���� ����� �������� �����ֱ� ���� ������Ʈ
    public InGameItemEquipment ap;
    [SerializeField]
    protected Enum_Class equipmentClass;

    //��� ���� Ȯ��
    [SerializeField]
    protected Enum_EquipmentType equipmentType;

    public Enum_Class EquipmentClass { get { return equipmentClass; } }
    public Enum_EquipmentType EquipmentType { get { return equipmentType; } }


    // ��� ����
    public override void Setting()
    {
        if (!stateComplete)
        {
            StateSetting();
            print("���� �Ϸ�" + " " + gameObject.name);
            stateComplete = true;
        }
        else
        {
            print("�̹� �����" + " " + gameObject.name);
            print("���� ���� ī����" + weaponState.Count + " " + gameObject.name);
        }
      
        playerEquipment = PlayerController.instance._playerEquipment;

        switch (equipmentType) 
        {
            case Enum_EquipmentType.Weapon:
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
                break;        
        }
    }

    // ��� ���� ������ ������ ������ �´��� ������ �����ϰ� �ϴ� �޼���
    public override void Check()
    {
        Debug.Log(ap);
        playerEquipment = PlayerController.instance._playerEquipment;
        playerEquipment.EquipmentCheck(ap);      
    }

    public void StateSetting()
    {
        weaponState.Add(new State(() => { player.SumAttack += item.Attack; }, () => { }, () => { }, () => { player.SumAttack -= item.Attack; }));
        weaponState.Add(new State(() => { player.SumAttack += item.Attack; }, () => { }, () => { }, () => { player.SumAttack -= item.Attack; }));
        weaponState.Add(new State(() => { player.SumAttack += item.Attack; }, () => { }, () => { }, () => { player.SumAttack -= item.Attack; }));
        weaponState.Add(new State(() => { player.SumAttack += item.Attack;  }, () => { }, () => { }, () => { player.SumAttack -= item.Attack; }));
    }
}
