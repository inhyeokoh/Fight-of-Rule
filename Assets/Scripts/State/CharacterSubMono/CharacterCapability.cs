using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCapability : SubMono<PlayerController>
{
    InGameStateItem disposableItem;
    
    Queue<StateItemData> disposableItemDatas;
    List<InGameStateItem> items;

    [SerializeField]
    List<PassiveSkill> skills;

    protected override void _Init()
    {
        disposableItemDatas = new Queue<StateItemData>();
        disposableItem = new InGameStateItem();
        items = new List<InGameStateItem>();
        skills = SkillManager.Skill.passiveSkills;
    }

    protected override void _Excute()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].Stay();
        }
        for (int i = 0; i < skills.Count; i++)
        {
            if (skills[i].PassiveOn)
            {
                skills[i].Stay();
            }
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
                disposableItemDatas.Enqueue(data);
                while(disposableItemDatas.Count > 0)
                {
                    disposableItem.Setting(disposableItemDatas.Dequeue());
                    disposableItem.Enter();
                }               
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
