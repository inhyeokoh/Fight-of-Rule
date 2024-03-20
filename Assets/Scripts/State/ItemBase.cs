using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_PostionType
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
public abstract class ItemBase : MonoBehaviour
{

    public int itemID; //������ ��ȣ

    public Enum_ItemType itemType;

    protected int level; //������ OR ��� ����

    public int Level { get { return level; } }

    protected StateMachine stateMachine;
    protected State state;

    [SerializeField]
    protected CharacterStatus player;

    [SerializeField]
    public int count;

    [SerializeField]
    public string itemName; //������ �̸�
    [SerializeField]
    public string itemDescription;//������ ����
   
    private void Awake()
    {
        stateMachine = new StateMachine();       
        Setting();
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

