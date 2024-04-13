using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Enum_EquipmentType
{
    Default,
    Weapon,
    Head,
    Body,
    Hand,
    Foot
}
   
public enum Enum_ItemType  
{    
    Consumption,    
    Equipment,   
    ETC
}
public abstract class ItemObject : MonoBehaviour
{
    public StateItemData item;

    public int itemID; //아이템 번호
    public int Attack;


    protected static bool stateComplete;


    //상태 패턴 생성기
    protected StateItemMachine stateMachine;
    protected StateItem stateItem;

    //포션이나 장비등등 스텟 정보를 넘겨주기위한 플레이어
    [SerializeField]

    protected CharacterStatus player;   

    private void Awake()
    {       
        stateMachine = new StateItemMachine();
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
        item = ItemData.StateItemDataReader(itemID);
        item.attack += Attack;
        Setting();       
    }

    private void FixedUpdate()
    {
        FixedStay();
    }

    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            player = PlayerController.instance._playerStat;
        }     
    }
    public void Enter()
    {
        stateMachine.EnterState(stateItem, item);
    }

    public  void FixedStay()
    {
        stateMachine.FixedStay();
    }

    public  void Stay()
    {
        stateMachine.Stay();
    }


    public  void Exit()
    {
        stateMachine.ExitState();
    }

    public abstract void Setting();

    // 지워도 되는 메서드
    public abstract void Check();


    
    //아이템의 정보들
    //아이템의 효과
    //아이템의 이름
    //현재 아이템을 적용할 캐릭터

    //아이템 베이스에는 소비 기타 장비의 공통된 기능을 넣는 스크립트
    //소비 장비 기타 아이템은 상속을 받아 따로 만들어줘야됌

    //주요 기능 상태패턴을 이용해라
}

