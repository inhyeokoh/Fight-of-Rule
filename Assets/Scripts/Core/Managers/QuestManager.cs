using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager questManager;

    public List<Quest> questList = new List<Quest>();
    // ������ �Ǵ� �Ϸ�� ����Ʈ ���
    public List<Quest> currentQuestList = new List<Quest>();

    void Awake()
    {
        if (questManager == null)
        {
            questManager = this;
        }
        else if (questManager != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AddQuestItem(string questobj, int itemAmount)
    {
        for (int i = 0; i < currentQuestList.Count; i++)
        {
            if (currentQuestList[i].questObj == questobj
                /*&& currentQuestList[i].progress == Quest.QuestProgress.Accepted*/ )
            {
                currentQuestList[i].questObjCount += itemAmount;
            }

            if (currentQuestList[i].questObjCount >= currentQuestList[i].questObjRequirement
                /*&& currentQuestList[i].progress == Quest.QuestProgress.Accepted*/ )            
            {
                currentQuestList[i].progress = Quest.QuestProgress.Complete;
            }
        }    
    }

    // TODO: �κ��丮���� ���Ž��� �ڵ� �ۼ�


    public bool RequestAvailableQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID &&
                questList[i].progress == Quest.QuestProgress.Available)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestAcceptedQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID &&
                questList[i].progress == Quest.QuestProgress.Accepted)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestCompleteQuest(int questID)
    {
        for (int i = 0; i < questList.Count; i++)
        {
            if (questList[i].id == questID &&
                questList[i].progress == Quest.QuestProgress.Complete)
            {
                return true;
            }
        }
        return false;
    }


}
