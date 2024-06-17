using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : SubMono<PlayerController>
{
    /// <summary>
    /// 각각의 장비들 체크
    /// </summary>
    [SerializeField]
    InGameStateItem currentWeapon = new InGameStateItem();
    InGameStateItem currentHead = new InGameStateItem();
    InGameStateItem currentBody = new InGameStateItem();
    InGameStateItem currentHand = new InGameStateItem();
    InGameStateItem currentFoot = new InGameStateItem();

    /// <summary>
    /// 장비가 껴져있는지 체크
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
    private List<InGameStateItem> equipments;

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {
        // 캐릭터가 껴져있는 장비를 불러오기
        equipments = new List<InGameStateItem>();
    }

    /// <summary>
    /// 장비를 끼면 껴져있는지 체크하는 메서드
    /// </summary>
    /// <param name="equipment"></param>
    public bool EquipmentCheck(StateItemData equipment, out int equipType)
    {
        bool pass = false;

        pass = Change(equipment,out equipType);

        return pass;
    }


    /* public void WeaponCheck(InGameItemEquipment weapon)
     {
         if (_board._class != weapon.EquipmentClass || _board._playerStat.Level < weapon.Level)
         {
             Debug.Log("이 장비는 낄수 없습니다.");
             return;
         }


         if (weaponCheck)
         {
              print("갈아낌");
              equipments.Remove(currentWeapon);
              currentWeapon.Exit();
              currentWeapon = weapon;
              currentWeapon.Enter();
              equipments.Add(currentWeapon);           
         }
         else
         {
             print("꼈음");
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
             Debug.Log("이 장비는 낄수 없습니다.");
             return;
         }


         if (headCheck)
         {
             print("갈아낌");
             equipments.Remove(currentHead);
             currentHead.Exit();
             currentHead = head;         
             currentHead.Enter();
             equipments.Add(currentHead);
         }
         else
         {
             print("꼈음");
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
             Debug.Log("이 장비는 낄수 없습니다.");
             return;
         }


         if (bodyCheck)
         {
             print("갈아낌");

             equipments.Remove(currentBody);
             currentBody.Exit();

             currentBody = body;

             currentBody.Enter();
         }
         else
         {
             print("꼈음");
             currentBody = body;
             bodyCheck = true;
             currentBody.Enter();
         }
     }

     public void HandCheck(InGameItemEquipment hand)
     {
         if (_board._class != hand.EquipmentClass || _board._playerStat.Level < hand.Level)
         {
             Debug.Log("이 장비는 낄수 없습니다.");
             return;
         }


         if (handCheck)
         {
             print("갈아낌");
             currentHand.Exit();

             currentHand = hand;

             currentHand.Enter();
         }
         else
         {
             print("꼈음");
             currentHand = hand;
             handCheck = true;
             currentHand.Enter();
         }
     }

     public void FootCheck(InGameItemEquipment foot)
     {
         if (_board._class != foot.EquipmentClass || _board._playerStat.Level < foot.Level)
         {
             Debug.Log("이 장비는 낄수 없습니다.");
             return;
         }


         if (footCheck)
         {
             print("갈아낌");
             currentFoot.Exit();

             currentFoot = foot;

             currentFoot.Enter();
         }
         else
         {
             print("꼈음");
             currentFoot = foot;
             footCheck = true;
             currentFoot.Enter();
         }
     }*/


    /// <summary>
    /// 장비 아이템들의 지속효과들을 프레임마다 불러오는 메서드
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

    public void TakeOff(int equipType)
    {
        switch (equipType)
        {
            case 0:
                equipments.Remove(currentHead);
                currentHead.Exit();
                headCheck = false;
                break;
            case 1:
                equipments.Remove(currentBody);
                currentBody.Exit();
                bodyCheck = false;
                break;
            case 2:
                equipments.Remove(currentHand);
                currentHand.Exit();
                handCheck = false;
                break;
            case 3:
                equipments.Remove(currentFoot);
                currentFoot.Exit();
                footCheck = false;
                break;
            case 4:
                equipments.Remove(currentWeapon);
                currentWeapon.Exit();
                weaponCheck = false;
                break;
            default:
                return;
        }
    }


    /// <summary>
    /// 장비들의 조건과 그리고 이미 껴져있으면 갈아끼는 메서드
    /// </summary>
    /// <param name="change"></param>

    public bool Change(StateItemData change,out int equipType)

    {
        switch (change.detailType)
        {
            case Enum_DetailType.Weapon:
                if (_board._class != change.itemClass /*|| _board._playerStat.Level < change.StateItemData.level*/)
                {
                    Debug.Log("이 장비는 낄수 없습니다.");
                    equipType = -1;
                    return false;
                }


                if (weaponCheck)
                {
                    print("갈아낌");
                    equipments.Remove(currentWeapon);
                    currentWeapon.Exit();
                    currentWeapon.Setting(change);
                    currentWeapon.Enter();
                    equipments.Add(currentWeapon);
                }
                else
                {
                    print("꼈음");
                    currentWeapon.Setting(change);
                    weaponCheck = true;
                    currentWeapon.Enter();
                    equipments.Add(currentWeapon);
                }
                equipType = 4;
                break;


            case Enum_DetailType.Head:
                if (_board._class != change.itemClass || _board._playerStat.Level < change.level)
                {
                    Debug.Log("이 장비는 낄수 없습니다.");
                    equipType = -1;
                    return false;
                }


                if (headCheck)
                {
                    print("갈아낌");
                    equipments.Remove(currentHead);
                    currentHead.Exit();
                    currentHead.Setting(change);
                    currentHead.Enter();
                    equipments.Add(currentHead);
                }
                else
                {
                    print("꼈음");
                    currentHead.Setting(change);
                    headCheck = true;
                    currentHead.Enter();
                    equipments.Add(currentHead);
                }
                equipType = 0;
                break;

            case Enum_DetailType.Body:
                if (_board._class != change.itemClass || _board._playerStat.Level < change.level)
                {
                    Debug.Log("이 장비는 낄수 없습니다.");
                    equipType = -1;
                    return false;
                }


                if (bodyCheck)
                {
                    print("갈아낌");

                    equipments.Remove(currentBody);
                    currentBody.Exit();
                    currentBody.Setting(change);
                    currentBody.Enter();
                    equipments.Add(currentBody);
                }
                else
                {
                    print("꼈음");
                    currentBody.Setting(change);
                    bodyCheck = true;
                    currentBody.Enter();
                    equipments.Add(currentBody);
                }
                equipType = 1;
                break;

            case Enum_DetailType.Hand:
                if (_board._class != change.itemClass || _board._playerStat.Level < change.level)
                {
                    Debug.Log("이 장비는 낄수 없습니다.");
                    equipType = -1;
                    return false;
                }


                if (handCheck)
                {
                    print("갈아낌");
                    equipments.Remove(currentHand);
                    currentHand.Exit();

                    currentHand.Setting(change);

                    currentHand.Enter();
                    equipments.Add(currentHand);
                }
                else
                {
                    print("꼈음");
                    currentHand.Setting(change);
                    handCheck = true;
                    currentHand.Enter();
                    equipments.Add(currentHand);
                }
                equipType = 2;
                break;

            case Enum_DetailType.Foot:
                if (_board._class != change.itemClass || _board._playerStat.Level < change.level)
                {
                    Debug.Log("이 장비는 낄수 없습니다.");
                    equipType = -1;
                    return false;
                }


                if (footCheck)
                {
                    print("갈아낌");
                    equipments.Remove(currentFoot);
                    currentFoot.Exit();

                    currentFoot.Setting(change);

                    currentFoot.Enter();
                    equipments.Add(currentFoot);
                }
                else
                {
                    print("꼈음");
                    currentFoot.Setting(change);
                    footCheck = true;
                    currentFoot.Enter();
                    equipments.Add(currentFoot);
                }
                equipType = 3;
                break;
            default:
                //print("이 아이템은 데이터상에 존재하지 않습니다.");
                equipType = -1;
                return false;
        }

        return true;
    }

}