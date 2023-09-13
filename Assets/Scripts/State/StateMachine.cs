using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : class
{
    private T ownerEntity; // 현재 클래스
    private State<T> currentState; // 현재 상태
    private State<T> previousState; // 이전 상태

    public void Setup(T ownerEntity, State<T> entryState)
    {
        this.ownerEntity = ownerEntity;
        currentState = null;
        previousState = null;
       
        ChangeState(entryState);
    }

    public void Stay()
    {
        if (currentState != null)
            currentState.Stay(ownerEntity);
    }
    
    public void ChangeState(State<T> newState)
    {
        if (newState == null)
        {
            return;
        }   
            
        if (currentState.GetType() == typeof(WarriorOwnedState.Avoid))
        {
            return;
        }

        if (currentState != null)
        {
            previousState = currentState;
            currentState.Exit(ownerEntity);
        }

        currentState = newState;        
        currentState.Enter(ownerEntity);      
    }


    public void ReversState()
    {
        ChangeState(previousState);
    }
}
