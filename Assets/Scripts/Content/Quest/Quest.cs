using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enum_QuestType
{
    NoneRepeat,
    Repeat, // 바로 반복 수행 가능
    DailyRepeat,
    WeeklyRepeat
}

// 특정 레벨 도달 및 특정 퀘스트 완료 시 NotAvailable -> Available
public enum Enum_QuestProgress
{
    UnAvailable,
    Available,
    Ongoing,
    CanComplete,
    Completed,
}

public class Quest
{
    public Enum_QuestProgress progress { get; set; } = Enum_QuestProgress.UnAvailable;
    public QuestData questData;

    // 완료 조건
    public int questObjCount;
    public int questMonsterCount;
    private bool canComplete;
    public bool CanComplete
    {
        get
        {
            // CheckCanCompleteQuest 메서드 호출하여 상태 갱신 및 반환
            return _CheckCanCompleteQuest();
        }
        private set
        {
            canComplete = value;
        }
    }

    // 퀘스트 상태 변화 이벤트
    public delegate void QuestProgressChanged();
    public event QuestProgressChanged OnQuestProgressChanged;

    void _SetProgress(Enum_QuestProgress newProgress)
    {
        if (progress != newProgress)
        {
            progress = newProgress;
            OnQuestProgressChanged?.Invoke();
        }
    }



    /// <summary>
    /// 퀘스트 시작 레벨 충족 여부와 사전 퀘스트 완료 여부 검사
    /// </summary>
    public void CheckAvailable()
    {
        if (PlayerController.instance._playerStat.Level < questData.requiredLevel) return;

        if (questData.previousQuestID.HasValue && progress != Enum_QuestProgress.Completed) return;

        _SetProgress(Enum_QuestProgress.Available);
    }

    bool _CheckCanCompleteQuest()
    {
        if (progress != Enum_QuestProgress.Ongoing) return false;

        if (questData.questMonsterRequiredCount <= questMonsterCount &&
            questData.questObjRequiredCount <= questObjCount)
        {
            _SetProgress(Enum_QuestProgress.CanComplete);
            CanComplete = true;
            return true;
        }

        CanComplete = false;
        return false;
    }

    public Quest(int questID)
    {
        questData = GameManager.Data.questDict[questID];
        // NPC에 퀘스트 분배
        foreach (var npcID in questData.npcID)
        {
            if (GameManager.Data.npcDict.ContainsKey(npcID))
            {
                GameManager.Data.npcDict[npcID].AssignQuest(this);
                Debug.Log($"{npcID} {questData.title}");
            }
        }
        if (!string.IsNullOrEmpty(questData.questMonster))
        {
            Debug.Log($"{questData.title} {questData.questMonster}");
            MonsterState.OnMonsterKilled += _OnMonsterKilled;
        }
        if (!string.IsNullOrEmpty(questData.questObj))
        {
            Debug.Log($"{questData.title} {questData.questObj}");
            GameManager.Inven.OnItemGet += OnItemGet;
        }
    }

    ~Quest() // 이벤트 구독 취소 시켜야 GC가 수집함
    {
        if (!string.IsNullOrEmpty(questData.questMonster))
        {
            MonsterState.OnMonsterKilled -= _OnMonsterKilled;
        }
        if (!string.IsNullOrEmpty(questData.questObj))
        {
            GameManager.Inven.OnItemGet -= OnItemGet;
        }
    }

    void _OnMonsterKilled(int monsterID)
    {
        if (monsterID == GameManager.Data.monsterNameToID[questData.questMonster])
        {
            questMonsterCount++;
            Debug.Log($"{questData.questMonster} 처치 수: {questMonsterCount}");
        }
    }

    public void OnItemGet(int itemID, int itemCount)
    {
        if (itemID == GameManager.Data.DropItems[questData.questObj])
        {
            questObjCount += itemCount;
            Debug.Log($"{questData.questObj} 아이템 수: {itemCount}");
        }
    }

    // TODO 보상 지급
}
