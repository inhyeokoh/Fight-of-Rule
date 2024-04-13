using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateItemMachine : MonoBehaviour
{
    //분류한 아이템 스테이트 머신

    private StateItem currentState; // 현재 상태    
    private StateItem previousState; // 이전 상태

    private StateItemData currentItem;

    //처음에 스테이트머신을 생성 할때 자기자신과 딕셔너리,배열,리스트에 인덱스에 넣어놨던 처음에 실행해야하는 상태를 넣고 호출
    public void Setup(StateItem entryState, StateItemData p)
    {
        currentState = null;
        previousState = null;

        EnterState(entryState, p);
    }

    //Stay와 FixedStay는 유니티에서 지원하는 Update,FixedUpdate에 연결해야함
    public void FixedStay()
    {
        if (currentState != null)
        {
            currentState.FixedStay(currentItem);
        }
    }
    public void Stay()
    {
        if (currentState != null)
        {
            currentState.Stay(currentItem);
        }

    }

    //아이템만 쓸경우
    public void EnterState(StateItem newState, StateItemData p)
    {
        if (newState == null)
        {
            return;
        }

        currentState = newState;
        currentItem = p;
        currentState.Enter(currentItem);
    }


    //상태를 체인지 하지말고 Exit만 발동하고 싶을때? 
    public void ExitState()
    {
        currentState.Exit(currentItem);
        currentItem = null; 
        currentState = null;
    }

    //ReversState는 ChangeState에서 previousState변수에 저장됬었던 이전상태를 다시 전환하고 싶을때 호출하면됌
    /*public void ReversState()
    {
        ChangeState(previousState);
    }*/
}
