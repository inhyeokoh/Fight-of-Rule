using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SubClass<GameManager>
{
    // 전체 퀘스트
    public Dictionary<int, Quest> totalQuestDict = new Dictionary<int, Quest>();
    // 시작 불가 퀘스트 목록
    List<Quest> unAvailableQuestList = new List<Quest>();
    // 시작 가능 퀘스트 목록
    List<Quest> availableQuestList = new List<Quest>();
    // 진행중인 퀘스트 목록
    List<Quest> onGoingQuestList = new List<Quest>();
    // 완료 퀘스트 목록
    List<Quest> completedQuestList = new List<Quest>();
        
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

    void _SetQuestList()
    {
        foreach (var quest in GameManager.Data.questDict)
        {
            totalQuestDict.Add(quest.Key, new Quest(quest.Key));
        }

        // 서버에서 가져오지 않을까..?

        // TEST
        unAvailableQuestList.Add(totalQuestDict[0]);
        unAvailableQuestList.Add(totalQuestDict[1]);
    }

    // 레벨업 시에 시작 가능한 퀘스트 목록 갱신
    public void UpdateAvailableQuests()
    {
        for (int i = unAvailableQuestList.Count - 1; i >= 0; i--)
        {
            var quest = unAvailableQuestList[i];
            quest.CheckAvailable();
            if (quest.progress == Enum_QuestProgress.Available)
            {
                unAvailableQuestList.RemoveAt(i);
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

        quest.SetProgress(Enum_QuestProgress.Ongoing);
        availableQuestList.Remove(quest);
        onGoingQuestList.Add(quest);
        quest.ReceiveEventWhenQuestStarts();
        if (!String.IsNullOrEmpty(quest.questData.questObj))
        {
            // 퀘스트 아이템이 이미 인벤에 있는지 체크
            GameManager.Inven.SearchItem(GameManager.Data.DropItems[quest.questData.questObj]);
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
        Quest quest = totalQuestDict[questId];
        quest.SetProgress(Enum_QuestProgress.Completed);
        if (quest.CanComplete)
        {
            onGoingQuestList.Remove(quest);
            completedQuestList.Add(quest);
        }
    }
}
