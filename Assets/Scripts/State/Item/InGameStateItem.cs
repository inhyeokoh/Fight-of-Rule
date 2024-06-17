using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class StateItem
{
    //어렵게 생각하지 말고 아이템 스테이트와 아이템 스테이트 머신을 따로 분류해서 만들었음
    public Action<InGameStateItem> enterAction;
    public Action<InGameStateItem> fixedStayAction;
    public Action<InGameStateItem> stayAction;
    public Action<InGameStateItem> exitAction;

    public StateItem(Action<InGameStateItem> enterAction, Action<InGameStateItem> fixedStayAction, Action<InGameStateItem> stayAction, Action<InGameStateItem> exitAction)
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

    //protected Dictionary<int, StateItem> StateItems = new Dictionary<int, StateItem>();

    //현재 플레이어의 직업확인과 장비를 꼇을때 스텟들을 넘겨주기위한 클래스
    public static CharacterStatus playerStatus;
    public static CharacterCapability playerCapability;
    public float duration;

    private StateItemData stateItemData;

    public StateItemData StateItemData { get { return stateItemData; } }

    protected StateMachine stateMachine;
    protected State state;

    //현재 장비의 정보들을 보내주기 위한 컴포넌트
    [SerializeField]
    private static bool settingComplete;

    // 장비 세팅
    public void Setting(StateItemData stateItemData)
    {
        this.stateItemData = stateItemData;
        duration = stateItemData.duration;
        
        if (!settingComplete)        
        {
            playerStatus = PlayerController.instance._playerStat;
            playerCapability = PlayerController.instance._playerCapability;
            StateSetting();
            //print("적용 완료" + " " + gameObject.name);          
            settingComplete = true;      
        }

      /*  if (!ok)
        {
            StateSetting();
            ok = true;
        }
*/

        state = new State(
            () => 
            {         
                StateItems[stateItemData.id].enterAction?.Invoke(this);
            },  
            () =>    
            {
                StateItems[stateItemData.id].fixedStayAction?.Invoke(this);
            },
            () => 
            {
                StateItems[stateItemData.id].stayAction?.Invoke(this);
            }, 
            () =>
            {
                StateItems[stateItemData.id].exitAction?.Invoke(this);              
            });
        stateMachine = new StateMachine();
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
        StateItems.Add(500, new StateItem((item) =>
        {
            playerStatus.HP += item.stateItemData.hp;
            Debug.Log("빨간포션 사용");
        },
        (item) =>
        {
        },
        (item) =>
        {
        },
        (item) =>
        {
        }
        ));
        StateItems.Add(504, new StateItem((item) =>
        {
            playerStatus.MP += item.stateItemData.mp;
            Debug.Log("파란포션 사용");
        },
        (item) =>
        {
        },
       (item) =>
       {
       },
       (item) =>
       {
       }
       ));
        StateItems.Add(515, new StateItem((item) =>
        {       
            playerStatus.SumAttack += item.stateItemData.attack;
        },
       (item) =>
       {
       },
       (item) =>
       {
           if (item.duration <= 0)
           {
               item.Exit();
           }
           else
           {
               item.duration -= Time.deltaTime;
               Debug.Log($"공격력 물약 지속 시간 :{item.duration}");
           }
       },
       (item) =>
       {
           playerStatus.SumAttack -= item.stateItemData.attack;
           playerCapability.Remove(item);
           Debug.Log($"캐릭터 공격력 : {playerStatus.SumAttack}");
       }
       ));
        StateItems.Add(518, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.stateItemData.defense;
            Debug.Log($"방어력 물약 지속 시간 :{item.duration}");          
        },
       (item) =>
       {
       },
       (item) =>
       {
           if (item.duration <= 0)
           {
               item.Exit();
           }
           else
           {
               item.duration -= Time.deltaTime;
               Debug.Log($"방어력 물약 지속 시간 :{item.duration}");          
           }
       },
       (item) =>
       {
           playerStatus.SumDefense -= item.stateItemData.defense;
           playerCapability.Remove(item);

           Debug.Log($"캐릭터 방어력 : {playerStatus.SumDefense}");
       }
       ));
        StateItems.Add(1000, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
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
            playerStatus.SumAttack -= item.stateItemData.attack;
        }));
        StateItems.Add(1001, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
           playerStatus.SumAttack -= item.stateItemData.attack;
        }));
        StateItems.Add(1002, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.stateItemData.attack;
        }));
        StateItems.Add(1003, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.stateItemData.attack;
        }));
        StateItems.Add(1004, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.stateItemData.attack;
        }));
        StateItems.Add(1006, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.stateItemData.defense;
            playerStatus.SumMaxHP += item.stateItemData.maxHp;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.stateItemData.defense;
            playerStatus.SumMaxHP -= item.stateItemData.maxHp;
        }));
        StateItems.Add(1008, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.stateItemData.defense;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.stateItemData.defense;
        }));
        StateItems.Add(1012, new StateItem((item) =>
        {
            playerStatus.SumAttack += item.stateItemData.attack;
            playerStatus.SumDefense += item.stateItemData.defense;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumAttack -= item.stateItemData.attack;
            playerStatus.SumDefense -= item.stateItemData.defense;
        }));
        StateItems.Add(1016, new StateItem((item) =>
        {
            playerStatus.SumDefense += item.stateItemData.defense;
            playerStatus.SumSpeed += item.stateItemData.speed;
        },
        (item) =>
        {

        },
        (item) =>
        {

        },
        (item) =>
        {
            playerStatus.SumDefense -= item.stateItemData.defense;
            playerStatus.SumSpeed -= item.stateItemData.speed;
        }));
        StateItems.Add(2, new StateItem((item) =>
        {
            Debug.Log($"{item.stateItemData.name} {item.stateItemData.attack}");
        },
        (item) =>
        {
            Debug.Log($"현재 fixed작동중{item.stateItemData.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
        StateItems.Add(3, new StateItem((item) =>
        {
            Debug.Log($"{item.stateItemData.name} {item.stateItemData.attack}");
        },
        (item) =>
        {
            Debug.Log($"현재 fixed작동중{item.stateItemData.attack}");
        },
        (item) =>
        {

        },
        (item) =>
        {

        }));
    }
}
