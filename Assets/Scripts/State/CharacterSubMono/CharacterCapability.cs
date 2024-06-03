using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCapability : SubMono<PlayerController>
{
    InGameStateItem disposableItem;
    
    List<InGameStateItem> items;
   
    
    List<PassivSkill> passivSkills;

    protected override void _Init()
    {
        disposableItem = new InGameStateItem();
        items = new List<InGameStateItem>();
        passivSkills = new List<PassivSkill>();
    }

    protected override void _Excute()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Stay();
        }
    }
    protected override void _Clear()
    {
        
    }

    public void Use(StateItemData data)
    {
        if (!data.durationBool)
        {
            if (disposableItem.StateItemData != null && data.id == disposableItem.StateItemData.id)
            {
                disposableItem.Enter();
            }
            else
            {
                disposableItem.Setting(data);
                disposableItem.Enter();
            }     
        }
        else
        {
            bool check = false;
            int index = -1;
          
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].StateItemData.id == data.id)
                {
                    check = true;
                    index = i;
                    break;
                }
            }

            if (check)
            {
                items[index].duration = data.duration;
            }
            else
            {
                InGameStateItem durationType = new InGameStateItem();
                durationType.Setting(data);
                durationType.Enter();
                items.Add(durationType);
            }
        }
    }

    public void Remove(InGameStateItem remove)
    {
        items.Remove(remove);
    }
}
