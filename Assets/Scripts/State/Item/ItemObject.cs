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

    public int itemID; //������ ��ȣ

    public Enum_ItemType itemType;

    protected static bool stateComplete;


    //���� ���� ������
    protected StateMachine stateMachine;
    protected State state;

    //�����̳� ����� ���� ������ �Ѱ��ֱ����� �÷��̾�
    [SerializeField]
    protected CharacterStatus player;   
    private void Awake()
    {       
        stateMachine = new StateMachine();
        item = new Item(null, 0, "����", "����", 0, 0, 5, 0, 0, 0, 0, 0, 0);

      /*  try
        {
            Setting();
        }
        catch
        {
            print("���� �÷��̾ ã���� �����ϴ�");
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

    // ������ �Ǵ� �޼���
    public abstract void Check();


    
    //�������� ������
    //�������� ȿ��
    //�������� �̸�
    //���� �������� ������ ĳ����

    //������ ���̽����� �Һ� ��Ÿ ����� ����� ����� �ִ� ��ũ��Ʈ
    //�Һ� ��� ��Ÿ �������� ����� �޾� ���� �������߉�

    //�ֿ� ��� ���������� �̿��ض�
}

