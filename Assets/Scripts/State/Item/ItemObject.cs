using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_PotionType
{
    Heal,
    Mana,
    Exp,
    Defenes,
    Attack
}

public enum Enum_EquipmentType
{
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



    //상태 패턴 생성기
    protected StateMachine stateMachine;
    protected State state;

    //포션이나 장비등등 스텟 정보를 넘겨주기위한 플레이어
    [SerializeField]

    protected CharacterStatus player;   

    private void Awake()
    {
        stateMachine = new StateMachine();       
        Setting();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        Data(itemID);
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

    // Update is called once per frame
  /*  public virtual void FixedUpdate()
    {
        stateMachine.FixedStay();
    }
    public virtual void Update()
    {
        stateMachine.Stay();
    }*/

    public abstract void Enter();

    public abstract void FixedStay();

    public abstract void Stay();

    public abstract void Exit();

    public abstract void Setting();

    // 지워도 되는 메서드
    public abstract void Check();


    public abstract void Data(int itemID);
    
    //아이템의 정보들
    //아이템의 효과
    //아이템의 이름
    //현재 아이템을 적용할 캐릭터

    //아이템 베이스에는 소비 기타 장비의 공통된 기능을 넣는 스크립트
    //소비 장비 기타 아이템은 상속을 받아 따로 만들어줘야됌

    //주요 기능 상태패턴을 이용해라
}

