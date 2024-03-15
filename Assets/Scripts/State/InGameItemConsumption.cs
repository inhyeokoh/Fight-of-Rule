using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemConsumption : ItemBase
{   
    public Enum_PostionType postionType;

    public int stat; // �� ������ �ɷ�ġ

    public override void Setting()
    {     
        state = new State(() => 
        {
            switch(postionType)
            {
                case Enum_PostionType.Heal:
                    player.HP += stat;
                    break;

                case Enum_PostionType.Mana:
                    player.MP += stat;
                    break;

                case Enum_PostionType.Exp:
                    player.EXP += stat;
                    break;

                case Enum_PostionType.Defenes:
                    player.Defense += stat;
                    break;

                case Enum_PostionType.Attack:
                    player.Attack += stat;
                    break;
            }
            if (count == 1)
            {
                stateMachine.ExitState();
            }
            else
            {
                count -= 1;
            }

            Debug.Log("�����");
        }, () => { }, () => { }, () => { gameObject.SetActive(false); });
    }
    public override void Enter()
    {
        stateMachine.EnterState(state);   
    }

    public override void Exit()
    {
        stateMachine.ExitState();
    }

    public override void FixedStay()
    {
        stateMachine.FixedStay();
    }

    public override void Stay()
    {
        stateMachine.Stay();
    }

    public override void Check()
    {
      
    }

    public override void Data(int itemID)
    {
        
    }
}
