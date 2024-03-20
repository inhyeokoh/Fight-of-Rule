using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{       
    private State currentState; // ���� ����    
    private State previousState; // ���� ����

    //ó���� ������Ʈ�ӽ��� ���� �Ҷ� �ڱ��ڽŰ� ��ųʸ�,�迭,����Ʈ�� �ε����� �־���� ó���� �����ؾ��ϴ� ���¸� �ְ� ȣ��
    public void Setup(State entryState)
    {    
        currentState = null;
        previousState = null;
        
        ChangeState(entryState);
    }

    //Stay�� FixedStay�� ����Ƽ���� �����ϴ� Update,FixedUpdate�� �����ؾ���
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


    //ChangeState �Լ��� ��ųʸ�,�迭,����Ʈ���� ��ü�� �����ϰ� �����ߴ� State<T>Ŭ������ �����ͼ� 
    //�� �ε����� �Ҵ���־��� State<T> Enter,Exit�� ȣ���ϰ� Stay,FixedStay�� �����Ӹ��� ȣ��
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

    //�����۸� �����
    public void EnterState(State newState)
    {
        if (newState == null)
        {
            return;
        }

        currentState = newState;
        currentState.Enter();
    }
    

    //���¸� ü���� �������� Exit�� �ߵ��ϰ� ������? 
    public void ExitState()
    {
        currentState.Exit();
        currentState = null;
    }

    //ReversState�� ChangeState���� previousState������ �������� �������¸� �ٽ� ��ȯ�ϰ� ������ ȣ���ϸ��
    public void ReversState()
    {
        ChangeState(previousState);
    }
}
