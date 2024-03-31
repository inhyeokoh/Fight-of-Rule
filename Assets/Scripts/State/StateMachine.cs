using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{       
    private State currentState; // 현재 상태    
    private State previousState; // 이전 상태

    //처음에 스테이트머신을 생성 할때 자기자신과 딕셔너리,배열,리스트에 인덱스에 넣어놨던 처음에 실행해야하는 상태를 넣고 호출
    public void Setup(State entryState)
    {    
        currentState = null;
        previousState = null;
        
        ChangeState(entryState);
    }

    //Stay와 FixedStay는 유니티에서 지원하는 Update,FixedUpdate에 연결해야함
    public void FixedStay()
    {
        if (currentState != null)
        {
            currentState.FixedStay();
        }
    }
    public void Stay()
    {
        if (currentState != null)
        {
           currentState.Stay();
        }
           
    }


    //ChangeState 함수는 딕셔너리,배열,리스트에서 객체를 생성하고 저장했던 State<T>클래스를 가져와서 
    //그 인덱스에 할당되있었던 State<T> Enter,Exit를 호출하고 Stay,FixedStay를 프레임마다 호출
    public void ChangeState(State newState)
    {
        if (newState == null)
        {
            return;
        }

        if (currentState != null)
        {
            previousState = currentState;
            currentState.Exit();          
        }

        currentState = newState;     
        currentState.Enter();

    }

    //아이템만 쓸경우
    public void EnterState(State newState)
    {
        if (newState == null)
        {
            return;
        }

        currentState = newState;
        currentState.Enter();
    }
    

    //상태를 체인지 하지말고 Exit만 발동하고 싶을때? 
    public void ExitState()
    {
        currentState.Exit();
        currentState = null;
    }

    //ReversState는 ChangeState에서 previousState변수에 저장됬었던 이전상태를 다시 전환하고 싶을때 호출하면됌
    public void ReversState()
    {
        ChangeState(previousState);
    }
}
