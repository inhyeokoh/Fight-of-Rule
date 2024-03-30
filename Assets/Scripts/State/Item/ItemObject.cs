using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_PotionType
{
    Defult,
    Heal,
    Mana,
    Exp,
    Defenes,
    Attack
}

public enum Enum_EquipmentType
{
    Defult,
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
    Etc  
}
public abstract class ItemObject : MonoBehaviour
{

    public Item item;

    public int itemID; //아이템 번호

    public Enum_ItemType itemType;

    protected static bool stateComplete;


    //상태 패턴 생성기
    protected StateMachine stateMachine;
    protected State state;

    //포션이나 장비등등 스텟 정보를 넘겨주기위한 플레이어
    [SerializeField]
    protected CharacterStatus player;   
    private void Awake()
    {       
        stateMachine = new StateMachine();
        item = new Item(null, 0, "무기", "설명", 0, 0, 5, 0, 0, 0, 0, 0, 0);

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
           
        Setting();       
    }

    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            player = PlayerController.instance._playerStat;



            if (itemType == Enum_ItemType.Consumption)
            {
                Enter();
            }           
            else if (itemType == Enum_ItemType.Equipment)
            {
                Check();
            }
        }     
    }
    public void Enter()
    {
        stateMachine.EnterState(state);
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
    public void Data(int itemID)
    {

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

