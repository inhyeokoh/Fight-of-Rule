using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : SubMono<PlayerController>
{
    /// <summary>
    /// ������ ���� üũ
    /// </summary>
    [SerializeField]
    InGameItemEquipment currentWeapon;
    InGameItemEquipment currentHead;
    InGameItemEquipment currentBody;
    InGameItemEquipment currentHand;
    InGameItemEquipment currentFoot;

    /// <summary>
    /// ��� �����ִ��� üũ
    /// </summary>
    private bool weaponCheck;
    private bool headCheck;
    private bool bodyCheck;
    private bool handCheck;
    private bool footCheck;

    public bool WeaponCheck { get { return weaponCheck; } }
    public bool HeadCheck { get { return headCheck; } }

    public bool BodyCheck { get { return bodyCheck; } }
    public bool HandCheck { get { return handCheck; } }

    public bool FootCheck { get { return footCheck; } }


    [SerializeField]
    private List<InGameItemEquipment> equipments;

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
       
    }

    protected override void _Init()
    {
       // ĳ���Ͱ� �����ִ� ��� �ҷ�����
    }

    /// <summary>
    /// ��� ���� �����ִ��� üũ�ϴ� �޼���
    /// </summary>
    /// <param name="equipment"></param>
    public void EquipmentCheck(InGameItemEquipment equipment)
    {
        Change(equipment);
    }


   /* public void WeaponCheck(InGameItemEquipment weapon)
    {
        if (_board._class != weapon.EquipmentClass || _board._playerStat.Level < weapon.Level)
        {
            Debug.Log("�� ���� ���� �����ϴ�.");
            return;
        }


        if (weaponCheck)
        {
             print("���Ƴ�");
             equipments.Remove(currentWeapon);
             currentWeapon.Exit();
             currentWeapon = weapon;
             currentWeapon.Enter();
             equipments.Add(currentWeapon);           
        }
        else
        {
            print("����");
            currentWeapon = weapon;
            weaponCheck = true;
            currentWeapon.Enter();
            equipments.Add(currentWeapon);
        }
    }  

    public void HeadCheck(InGameItemEquipment head)
    {
        if (_board._class != head.EquipmentClass || _board._playerStat.Level < head.Level)
        {
            Debug.Log("�� ���� ���� �����ϴ�.");
            return;
        }


        if (headCheck)
        {
            print("���Ƴ�");
            equipments.Remove(currentHead);
            currentHead.Exit();
            currentHead = head;         
            currentHead.Enter();
            equipments.Add(currentHead);
        }
        else
        {
            print("����");
            currentHead = head;
            headCheck = true;
            currentHead.Enter();
            equipments.Add(currentHead);
        }

    }

    public void BodyCheck(InGameItemEquipment body)
    {
        if (_board._class != body.EquipmentClass || _board._playerStat.Level < body.Level)
        {
            Debug.Log("�� ���� ���� �����ϴ�.");
            return;
        }


        if (bodyCheck)
        {
            print("���Ƴ�");

            equipments.Remove(currentBody);
            currentBody.Exit();

            currentBody = body;
            
            currentBody.Enter();
        }
        else
        {
            print("����");
            currentBody = body;
            bodyCheck = true;
            currentBody.Enter();
        }
    }

    public void HandCheck(InGameItemEquipment hand)
    {
        if (_board._class != hand.EquipmentClass || _board._playerStat.Level < hand.Level)
        {
            Debug.Log("�� ���� ���� �����ϴ�.");
            return;
        }


        if (handCheck)
        {
            print("���Ƴ�");
            currentHand.Exit();

            currentHand = hand;

            currentHand.Enter();
        }
        else
        {
            print("����");
            currentHand = hand;
            handCheck = true;
            currentHand.Enter();
        }
    }

    public void FootCheck(InGameItemEquipment foot)
    {
        if (_board._class != foot.EquipmentClass || _board._playerStat.Level < foot.Level)
        {
            Debug.Log("�� ���� ���� �����ϴ�.");
            return;
        }


        if (footCheck)
        {
            print("���Ƴ�");
            currentFoot.Exit();

            currentFoot = foot;

            currentFoot.Enter();
        }
        else
        {
            print("����");
            currentFoot = foot;
            footCheck = true;
            currentFoot.Enter();
        }
    }*/


    /// <summary>
    /// ��� �����۵��� ����ȿ������ �����Ӹ��� �ҷ����� �޼���
    /// </summary>
    public void EquipmentFixedStay()
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            equipments[i].FixedStay();
        }       
    }

    public void EquipmentStay()
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            equipments[i].Stay();
        }    
    }



    /// <summary>
    /// ������ ���ǰ� �׸��� �̹� ���������� ���Ƴ��� �޼���
    /// </summary>
    /// <param name="change"></param>
    public void Change(InGameItemEquipment change)
    {
        switch (change.EquipmentType)
        {
            case Enum_EquipmentType.Weapon:
                if (_board._class != change.EquipmentClass || _board._playerStat.Level < change.item.Level)
                {
                    Debug.Log("�� ���� ���� �����ϴ�.");
                    return;
                }


                if (weaponCheck)
                {
                    print("���Ƴ�");
                    equipments.Remove(currentWeapon);
                    currentWeapon.Exit();
                    currentWeapon = change;
                    currentWeapon.Enter();
                    equipments.Add(currentWeapon);
                }
                else
                {
                    print("����");
                    currentWeapon = change;
                    weaponCheck = true;
                    currentWeapon.Enter();
                    equipments.Add(currentWeapon);
                }
                break;
            
            
            case Enum_EquipmentType.Head:
                if (_board._class != change.EquipmentClass || _board._playerStat.Level < change.item.Level)
                {
                    Debug.Log("�� ���� ���� �����ϴ�.");
                    return;
                }


                if (headCheck)
                {
                    print("���Ƴ�");
                    equipments.Remove(currentHead);
                    currentHead.Exit();
                    currentHead = change;
                    currentHead.Enter();
                    equipments.Add(currentHead);
                }
                else
                {
                    print("����");
                    currentHead = change;
                    headCheck = true;
                    currentHead.Enter();
                    equipments.Add(currentHead);
                }
                break;

            case Enum_EquipmentType.Body:
                if (_board._class != change.EquipmentClass || _board._playerStat.Level < change.item.Level)
                {
                    Debug.Log("�� ���� ���� �����ϴ�.");
                    return;
                }


                if (bodyCheck)
                {
                    print("���Ƴ�");

                    equipments.Remove(currentBody);
                    currentBody.Exit();
                    currentBody = change;
                    currentBody.Enter();
                    equipments.Add(currentBody);
                }
                else
                {
                    print("����");
                    currentBody = change;
                    bodyCheck = true;
                    currentBody.Enter();
                    equipments.Add(currentBody);
                }
                break;
           
            case Enum_EquipmentType.Hand:
                if (_board._class != change.EquipmentClass || _board._playerStat.Level < change.item.Level)
                {
                    Debug.Log("�� ���� ���� �����ϴ�.");
                    return;
                }


                if (handCheck)
                {
                    print("���Ƴ�");
                    equipments.Remove(currentHand);
                    currentHand.Exit();

                    currentHand = change;

                    currentHand.Enter();
                    equipments.Add(currentHand);
                }
                else
                {
                    print("����");
                    currentHand = change;
                    handCheck = true;
                    currentHand.Enter();
                    equipments.Add(currentHand);
                }
                break;

            case Enum_EquipmentType.Foot:             
                if (_board._class != change.EquipmentClass || _board._playerStat.Level < change.item.Level)
                {
                    Debug.Log("�� ���� ���� �����ϴ�.");
                    return;
                }


                if (footCheck)
                {
                    print("���Ƴ�");
                    equipments.Remove(currentFoot);
                    currentFoot.Exit();

                    currentFoot = change;

                    currentFoot.Enter();
                    equipments.Add(currentFoot);
                }
                else
                {
                    print("����");
                    currentFoot = change;
                    footCheck = true;
                    currentFoot.Enter();
                    equipments.Add(currentFoot);
                }
                break;
            default:
                print("�� �������� �����ͻ� �������� �ʽ��ϴ�.");
                return;
        }
    }

}
