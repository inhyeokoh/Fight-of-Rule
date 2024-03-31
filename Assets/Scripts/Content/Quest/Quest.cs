using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest : MonoBehaviour
{
    public enum QuestProgress
    {
        Not_Available,
        Available,
        Accepted,
        Complete,
        Done,    
    }

    public string title;
    public int id;
    public QuestProgress progress;
    public string description;
    public string hint;
    public string congratulation;
    public string summary;
    public int nextQuest; // 체인 퀘스트

    public string questObj;
    public int questObjCount;
    public int questObjRequirement;

    public int expReward;
    public int goldReward;
    public string itemReward;
}
