using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//클래스 이름을 StateItemObject으로 바꿀 예정


public class StateItem
{
    //어렵게 생각하지 말고 아이템 스테이트와 아이템 스테이트 머신을 따로 분류해서 만들었음
    public Action<StateItemData> enterAction;
    public Action<StateItemData> fixedStayAction;
    public Action<StateItemData> stayAction;
    public Action<StateItemData> exitAction;

    public StateItem(Action<StateItemData> enterAction, Action<StateItemData> fixedStayAction, Action<StateItemData> stayAction, Action<StateItemData> exitAction)
    {
        this.enterAction = enterAction;
        this.fixedStayAction = fixedStayAction;
        this.stayAction = stayAction;
        this.exitAction = exitAction;
    }
}
public class InGameStateItem : MonoBehaviour
{

    //현재 아이템들의 state패턴을 담을 딕셔너리
    protected static Dictionary<int, StateItem> StateItems = new Dictionary<int, StateItem>();

    //현재 플레이어의 직업확인과 장비를 꼇을때 스텟들을 넘겨주기위한 클래스
    public CharacterEquipment playerEquipment;

    public StateItemData stateItemData;

    protected StateMachine stateMachine = new StateMachine();
    protected State state;

    //현재 장비의 정보들을 보내주기 위한 컴포넌트
    public InGameStateItem ap;
    [SerializeField]
    private static bool stateComplete;

    private Enum_Class equipmentClass;
    //장비 부위 확인
    [SerializeField]
    private Enum_EquipmentType equipmentType;

    public Enum_Class EquipmentClass { get { return equipmentClass; } }
    public Enum_EquipmentType EquipmentType { get { return equipmentType; } }


    // 장비 세팅
    public void Setting()
    {
        playerEquipment = PlayerController.instance._playerEquipment;
    
        if (!stateComplete)        
        {         
            StateSetting();         
            print("적용 완료" + " " + gameObject.name);          
            stateComplete = true;      
        }         
        else      
        {        
            print("이미 적용됌" + " " + gameObject.name);               
        }

        //stateItem = StateItems[item.id];      
        state = new State(
            () => 
            {         
                StateItems[stateItemData.id].enterAction?.Invoke(stateItemData);
            },  
            () =>    
            {
                StateItems[stateItemData.id].fixedStayAction?.Invoke(stateItemData);
            },
            () => 
            {
                StateItems[stateItemData.id].stayAction?.Invoke(stateItemData);
            }, 
            () =>
            {
                StateItems[stateItemData.id].exitAction?.Invoke(stateItemData);              
            });
    }


    private void FixedUpdate()
    {
        FixedStay();
    }

    private void Update()
    {
        Stay();
    }

    public void Enter()
    {
        stateMachine.EnterState(state);
    }

    public void FixedStay()
    {
        stateMachine.FixedStay();
    }
    public void Stay()
    {
        stateMachine.Stay();
    }
    public void Exit()
    {
        stateMachine.ExitState();
    }
    // 장비를 낄려 했을때 장비들의 조건이 맞는지 맞으면 장착하게 하는 메서드
    public  void Check()
    {
        Debug.Log(ap);
        playerEquipment = PlayerController.instance._playerEquipment;
        playerEquipment.EquipmentCheck(ap);
    }

    // 다른 아이템들도 잘 연동되는지 확인
    public void StateSetting()
    {
        StateItems.Add(1000, new StateItem((item) => 
        {
            print($"{item.name} 가격 : {item.purchaseprice}");
        },         
        (item) => 
        {
            print($"현재 fixed작동중 가격 : {item.purchaseprice}");
        }, 
        (item) => 
        { 

        }, 
        (item) =>
        { 

        })) ;
        StateItems.Add(1004, new StateItem((item) =>
        {
            print($"{item.name} 공격력 : {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중 공격력 : {item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(2, new StateItem((item) =>
        {
            print($"{item.name} {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(3, new StateItem((item) =>
        {
            print($"{item.name} {item.attack}");
        },
        (item) =>
        {
            print($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
    }
}
