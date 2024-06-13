using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField]
    int npcID;
    public int NpcID
    {
        get { return npcID; }
        set { npcID = value; }
    }
    [SerializeField]
    string npcName;
    GameObject questMarkers;  // NPC 위의 퀘스트 아이콘 오브젝트
    List<Quest> assignedQuests = new List<Quest>();

    public void AssignQuest(Quest quest)
    {
        assignedQuests.Add(quest);
        quest.OnQuestProgressChanged += UpdateQuestIcon;
    }
    
    // 퀘스트 상태에 따라 아이콘 업데이트
    // TODO 1번 퀘스트 진행중이어도 2번 퀘스트 시작 가능이면 시작 가능 아이콘
    public void UpdateQuestIcon()
    {
        int progressCount = GetComponentInChildren<QuestMarkers>().ProgressCount;

        progressCount = assignedQuests.Select(quest => quest.progress switch
        {
            Enum_QuestProgress.Available => 3,
            Enum_QuestProgress.CanComplete => 2,
            Enum_QuestProgress.Ongoing => 1,
            _ => 0
        }).Max();
    }
}
