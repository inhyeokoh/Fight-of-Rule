using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameItemDuraction : ItemBase
{

    public int stat;
    public int duractionTime;
    public bool drinkOn;
    public float coolTime;
    public Enum_PostionType duractionPostionType;

    public override void Setting()
    {
        state = new State(() => 
        {
            if (!drinkOn)
            {
                switch (duractionPostionType)
                {
                    case Enum_PostionType.Defenes:
                        player.SumDefense += stat;
                        break;
                    case Enum_PostionType.Attack:
                        player.SumAttack += stat;
                        break;
                }

                drinkOn = true;
            }            
            coolTime = duractionTime;

            if (count == 1)
            {
                gameObject.SetActive(false);
            }
            else
            {
                count -= 1;
            }
        }, () => { }, 
        () => 
        {
            if (coolTime > 0)
            {
                coolTime -= Time.deltaTime;
                print(coolTime);
            }
            else
            {
                stateMachine.ExitState();
            }
        }, 
        () => 
        {
            print("³¡³µ¾î");
            
            switch (duractionPostionType)
            {
                case Enum_PostionType.Defenes:
                    player.SumDefense -= stat;
                    break;
                case Enum_PostionType.Attack:
                    player.SumAttack -= stat;
                    break;
            }
            coolTime = 0;
            drinkOn = false;
        });
    }

    public override void Enter()
    {
        stateMachine.EnterState(state);
    }
    public override void Exit()
    {
        
    }

    public override void Check()
    {
      
    }

    public override void FixedStay()
    {
       
    }

    public override void Stay()
    {
        
    }

    public override void Data(int itemID)
    {
        
    }
}
