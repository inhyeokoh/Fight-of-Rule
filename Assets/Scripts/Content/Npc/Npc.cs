using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Enum_NpcType
{
    Quest,
    Shop
    // TODO : 제작 Npc 등등
}

public class Npc : MonoBehaviour
{
    [SerializeField]
    string npcName;

    [SerializeField]
    int npcID;
    public int NpcID
    {
        get { return npcID; }
        set { npcID = value; }
    }

    Enum_NpcType npcType;

    List<Quest> assignedQuests = new List<Quest>();

    List<Quest> accessibleQuests = new List<Quest>();
    public List<Quest> AccessibleQuests
    {
        get
        {
            accessibleQuests.Clear();
            foreach (var quest in assignedQuests)
            {
                if (quest.progress != Enum_QuestProgress.UnAvailable && quest.progress != Enum_QuestProgress.Completed)
                {
                    accessibleQuests.Add(quest);
                }
            }
            return accessibleQuests;
        }
    }

    public void AssignQuest(Quest quest)
    {
        assignedQuests.Add(quest);
        quest.OnQuestProgressChanged += UpdateQuestIcon;
    }
    
    // 퀘스트 상태에 따라 아이콘 업데이트
    public void UpdateQuestIcon()
    {
        int progressCount = assignedQuests.Select(quest => quest.progress switch
        {
            Enum_QuestProgress.Available => 3,
            Enum_QuestProgress.CanComplete => 2,
            Enum_QuestProgress.Ongoing => 1,
            _ => 0
        }).Max();

        GetComponentInChildren<QuestMarkers>().ProgressCount = progressCount;
    }
}
