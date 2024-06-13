using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SubClass<GameManager>
{
    // 전체 퀘스트
    public Dictionary<int, Quest> totalQuestDict = new Dictionary<int, Quest>();
    // 시작 불가 퀘스트 목록
    public List<Quest> unAvailableQuestList = new List<Quest>();
    // 시작 가능 퀘스트 목록
    public List<Quest> availableQuestList = new List<Quest>();
    // 진행중인 퀘스트 목록
    public List<Quest> onGoingQuestList = new List<Quest>();
    // 완료 퀘스트 목록
    public List<Quest> completedQuestList = new List<Quest>();
        
    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
    }

    protected override void _Init()
    {
        foreach (var quest in GameManager.Data.questDict)
        {
            totalQuestDict.Add(quest.Key, new Quest(quest.Key));
        }
    }

    // 레벨업 시에 시작 가능한 퀘스트 목록 갱신
    public void UpdateAvailableQuests()
    {
        foreach (var quest in unAvailableQuestList)
        {
            quest.CheckAvailable();
            if (quest.progress == Enum_QuestProgress.Available)
            {
                unAvailableQuestList.Remove(quest);
                availableQuestList.Add(quest);
                // TODO 퀘스트 팝업 내 배치 순서
            }
        }
    }

    // NPC 대화나 자동 퀘스트 알림UI를 통한 퀘스트 수락
    public void ReceiveQuest(int questId)
    {
        Quest quest = totalQuestDict[questId];

        if (quest.progress != Enum_QuestProgress.Available) return;

        availableQuestList.Remove(quest);
        onGoingQuestList.Add(quest);
        GameManager.Inven.SearchItem(GameManager.Data.DropItems[quest.questData.questObj]); // 이미 인벤에 있는지 체크
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
        Quest quest = totalQuestDict[questId];
        if (quest.CanComplete)
        {
            onGoingQuestList.Remove(quest);
            completedQuestList.Add(quest);
        }
    }
}
