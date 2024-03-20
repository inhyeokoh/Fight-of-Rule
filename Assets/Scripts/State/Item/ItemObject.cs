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

    public int itemID; //������ ��ȣ

    public Enum_ItemType itemType;



    //���� ���� ������
    protected StateMachine stateMachine;
    protected State state;

    //�����̳� ����� ���� ������ �Ѱ��ֱ����� �÷��̾�
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

    // ������ �Ǵ� �޼���
    public abstract void Check();


    public abstract void Data(int itemID);
    
    //�������� ������
    //�������� ȿ��
    //�������� �̸�
    //���� �������� ������ ĳ����

    //������ ���̽����� �Һ� ��Ÿ ����� ����� ����� �ִ� ��ũ��Ʈ
    //�Һ� ��� ��Ÿ �������� ����� �޾� ���� �������߉�

    //�ֿ� ��� ���������� �̿��ض�
}

