using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemConsumption : ItemObject
{   
    //���� ���� Ÿ��
    public Enum_PotionType postionType;

    //Ÿ�Ը��� �ٸ� ���Ǽ���
    public override void Setting()
    {     
        state = new State(() => 
        {
            switch(postionType)
            {
                case Enum_PotionType.Heal:
                    player.HP += item.PortionStat;
                    break;

                case Enum_PotionType.Mana:
                    player.MP += item.PortionStat;
                    break;

                case Enum_PotionType.Exp:
                    player.EXP += item.PortionStat;
                    break;

                case Enum_PotionType.Defenes:
                    player.Defense += item.PortionStat;
                    break;

                case Enum_PotionType.Attack:
                    player.Attack += item.PortionStat;
                    break;
            }      
        }, () => { }, () => { }, () => 
        { 
            gameObject.SetActive(false);       
        });
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
