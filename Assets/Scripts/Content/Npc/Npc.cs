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
    string npcName;
    [SerializeField]
    int npcID;

    public string NpcName
    {
        get { return npcName; }
        set { npcName = value; }
    }
    public int NpcID
    {
        get { return npcID; }
        set { npcID = value; }
    }
    public Enum_NpcType npcType { get; set; }

    [SerializeField]
    QuestMarkers questMarker;

    public void DetectQuestProgress(Quest quest)
    {
        quest.OnQuestProgressChanged += UpdateQuestIcon;
    }

    /// <summary>
    /// 퀘스트 상태에 따라 아이콘 업데이트
    /// </summary>
    public void UpdateQuestIcon()
    {
        if (GameManager.Quest.questsByNpcID.TryGetValue(npcID, out var quests))
        {
            int progressCount = GameManager.Quest.questsByNpcID[npcID].Select(quest => quest.progress switch
            {
                Enum_QuestProgress.CanComplete => 3,
                Enum_QuestProgress.Available => 2,
                Enum_QuestProgress.Ongoing => 1,
                _ => 0
            }).Max();

            questMarker.ProgressCount = progressCount;
        }
        else
        {
            return; // 키가 존재하지 않음
        }
    }
}
