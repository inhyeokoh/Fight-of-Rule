using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


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
public class InGameStateItem
{
    //현재 아이템들의 state패턴을 담을 딕셔너리
    protected static Dictionary<int, StateItem> StateItems = new Dictionary<int, StateItem>();

    //현재 플레이어의 직업확인과 장비를 꼇을때 스텟들을 넘겨주기위한 클래스
    public CharacterStatus playerStatus;

    private StateItemData stateItemData;

    public StateItemData StateItemData { get { return stateItemData; } }

    protected StateMachine stateMachine = new StateMachine();
    protected State state;

    //현재 장비의 정보들을 보내주기 위한 컴포넌트
    [SerializeField]
    private static bool stateComplete;

    // 장비 세팅
    public void Setting(StateItemData stateItemData)
    {
        this.stateItemData = stateItemData;

        playerStatus = PlayerController.instance._playerStat;
    
        if (!stateComplete)        
        {         
            StateSetting();         
            //print("적용 완료" + " " + gameObject.name);          
            stateComplete = true;      
        }

        Debug.Log(stateItemData.name);
        
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
    // 다른 아이템들도 잘 연동되는지 확인
    public void StateSetting()
    {
        StateItems.Add(1000, new StateItem((item) => 
        {
            playerStatus.SumAttack += item.attack;
        },         
        (item) => 
        {
            // Debug.Log($"현재 fixed작동중 공격력 : {item.attack}");
        }, 
        (item) => 
        { 

        }, 
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
        })) ;
        StateItems.Add(1001, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
        }));
        StateItems.Add(1002, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
        }));
        StateItems.Add(1003, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
        }));
        StateItems.Add(1004, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
        }));
        StateItems.Add(1006, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.defense;
            playerStatus.SumMaxHP += item.maxHp;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.defense;
            playerStatus.SumMaxHP -= item.maxHp;
        }));
        StateItems.Add(1008, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.defense;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.defense;
        }));
        StateItems.Add(1012, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.attack;
            playerStatus.SumDefense += item.defense;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.attack;
            playerStatus.SumDefense -= item.defense;
        }));
        StateItems.Add(1016, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.defense;
            playerStatus.SumSpeed += item.speed;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.defense;
            playerStatus.SumSpeed -= item.speed;
        }));
        StateItems.Add(2, new StateItem((item) =>
        {
            Debug.Log($"{item.name} {item.attack}");
        },
        (item) =>
        {
            Debug.Log($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(3, new StateItem((item) =>
        {
            Debug.Log($"{item.name} {item.attack}");
        },
        (item) =>
        {
            Debug.Log($"현재 fixed작동중{item.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
    }
}
