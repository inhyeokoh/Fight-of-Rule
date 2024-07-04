#define CLIENT_TEST
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SubClass<GameManager>
{
    // 전체 퀘스트
    public Dictionary<int, Quest> totalQuestDict = new Dictionary<int, Quest>();

    Dictionary<int, List<Quest>> questsByLevel = new Dictionary<int, List<Quest>>();
    public Dictionary<int, List<Quest>> questsByNpcID = new Dictionary<int, List<Quest>>();

    // 시작 가능 퀘스트 목록
    List<Quest> availableQuestList = new List<Quest>();
    // 진행중인 퀘스트 목록
    List<Quest> onGoingQuestList = new List<Quest>();
    // 완료 퀘스트 목록
    List<Quest> completedQuestList = new List<Quest>();

    // NPC와 대화시 선택한 퀘스트
    public Quest CurrentSelectedQuest { get; set; }

    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
    }

    protected override void _Init()
    {
        _SetQuestList();
    }

    /// <summary>
    /// questsByLevel Dict에 퀘스트 추가
    /// </summary>
    void AddQuestsByLevel(Quest quest)
    {
        if (!questsByLevel.ContainsKey(quest.questData.requiredLevel))
        {
            questsByLevel[quest.questData.requiredLevel] = new List<Quest>();
        }
        questsByLevel[quest.questData.requiredLevel].Add(quest);
    }

    /// <summary>
    /// questsByNpcId Dict에 퀘스트 추가
    /// </summary>
    void AddQuestsByNpcID(Quest quest)
    {
        if (!questsByNpcID.ContainsKey(quest.questData.npcID))
        {
            questsByNpcID[quest.questData.npcID] = new List<Quest>();
        }
        questsByNpcID[quest.questData.npcID].Add(quest);
    }



    public void QuestAvailableByLevel(int level)
    {
        if (questsByLevel.ContainsKey(level))
        {
            foreach (var quest in questsByLevel[level])
            {
                if (quest.progress == Enum_QuestProgress.UnAvailable)
                {
                    quest.progress = Enum_QuestProgress.Available;
                    availableQuestList.Add(quest);
                }
            }
        }
    }

    void _SetQuestList()
    {
        foreach (var questKeyValue in GameManager.Data.questDict)
        {
            Quest quest = new Quest(questKeyValue.Key);
            totalQuestDict.Add(questKeyValue.Key, quest);
            AddQuestsByLevel(quest);
            AddQuestsByNpcID(quest);            
        }

        QuestAvailableByLevel(1);
        foreach (var npc in GameManager.Data.npcDict)
        {
            npc.Value.UpdateQuestIcon();
        }
        // 퀘스트 진행 상황을 서버랑 어떻게 통신할건가...?
    }

    /// <summary>
    /// NPC 대화나 자동 퀘스트 알림UI를 통한 퀘스트 수락
    /// </summary>
    public void ReceiveQuest(int questId)
    {
        Quest quest = totalQuestDict[questId];

        quest.SetProgress(Enum_QuestProgress.Ongoing);
        availableQuestList.Remove(quest);
        onGoingQuestList.Add(quest);
        quest.ReceiveEventWhenQuestStarts();

        foreach (var goal in quest.questData.goals)
        {
            if (goal is ObjectGoal objGoal)
            {
                // 퀘스트 아이템이 이미 인벤에 있는지 체크
                GameManager.Inven.SearchItem(GameManager.Data.DropItems[objGoal.ObjectName]);
            }
        }
    }

    // 퀘스트 포기하기
    public void GiveUpQuest(int questId)
    {
        Quest quest = totalQuestDict[questId];
        if (quest.progress == Enum_QuestProgress.Ongoing)
        {
            onGoingQuestList.Remove(quest);
            availableQuestList.Add(quest);
        }
    }

    // 퀘스트 완료
    public void CompleteQuest(int questId)
    {
        // TODO : 보상 지급
        Quest quest = totalQuestDict[questId];
        quest.SetProgress(Enum_QuestProgress.Completed);
        if (quest.questData.itemRewards.Count != 0)
        {
            foreach (var itemReward in quest.questData.itemRewards)
            {
                GameManager.Inven.GetItem(itemReward);
            }
        }
        onGoingQuestList.Remove(quest);
        completedQuestList.Add(quest);
    }
}
