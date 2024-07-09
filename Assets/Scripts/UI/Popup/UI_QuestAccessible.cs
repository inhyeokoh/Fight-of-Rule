using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class UI_QuestAccessible : UI_Entity
{
    #region 퀘스트 목록
    List<Quest> accessibleQuests;
    GameObject questList;
    ToggleGroup toggleGroup;
    GameObject canComplete;
    GameObject available;
    GameObject ongoing;
    Quest selectedQuest;
    #endregion

    enum Enum_UI_QuestAccessible
    {
        QuestList,
        Next
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_QuestAccessible);
    }

    void OnEnable()
    {
        if (!GameManager.UI.init || accessibleQuests == null) return;

        _ShowQuests();
    }

    void OnDisable()
    {
        if (!GameManager.UI.init || accessibleQuests == null) return;

        _DestroyQuestNameToggles();
    }

    protected override void Init()
    {
        base.Init();

        questList = _entities[(int)Enum_UI_QuestAccessible.QuestList].gameObject;
        toggleGroup = questList.GetComponent<ToggleGroup>();
        canComplete = questList.transform.GetChild(0).gameObject;
        available = questList.transform.GetChild(1).gameObject;
        ongoing = questList.transform.GetChild(2).gameObject;

        _entities[(int)Enum_UI_QuestAccessible.Next].ClickAction = (PointerEventData data) => {
            if (selectedQuest == null) return;

            GameManager.Quest.CurrentSelectedQuest = selectedQuest;
            GameManager.UI.Dialog.StartDialog();
        };


        gameObject.SetActive(false);
    }

    public bool CheckAccessibleQuests(int npcID)
    {
        accessibleQuests = new List<Quest>();
        foreach (var quest in GameManager.Quest.questsByNpcID[npcID])
        {
            // 시작 불가능 퀘스트, 완료된 퀘스트 제외
            if (quest.progress != Enum_QuestProgress.UnAvailable && quest.progress != Enum_QuestProgress.Completed)
            {
                accessibleQuests.Add(quest);
            }
        }

        return accessibleQuests.Count != 0;
    }

    void _ShowQuests()
    {
        bool isFirstToggle = true;

        foreach (var quest in accessibleQuests)
        {
            switch (quest.progress)
            {
                case Enum_QuestProgress.Available:
                    CreateAndSetupToggle(available, quest, ref isFirstToggle);
                    break;
                case Enum_QuestProgress.Ongoing:
                    CreateAndSetupToggle(ongoing, quest, ref isFirstToggle);
                    break;
                case Enum_QuestProgress.CanComplete:
                    CreateAndSetupToggle(canComplete, quest, ref isFirstToggle);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Assert(false);
#endif
                    break;
            }
        }
    }

    void CreateAndSetupToggle(GameObject parent, Quest quest, ref bool isFirstToggle)
    {
        // TODO quest AddListener Clear
        parent.gameObject.SetActive(true);
        GameObject toggle = GameManager.Resources.Instantiate("Prefabs/UI/Scene/QuestNameToggle", parent.transform.GetChild(1).transform);
        Toggle toggleComponent = toggle.GetComponent<Toggle>();
        toggleComponent.group = toggleGroup;
        toggle.GetComponentInChildren<TMP_Text>().text = quest.questData.title;
        toggleComponent.onValueChanged.AddListener((value) => {
            if (value)
            {
                selectedQuest = quest;
            }
            else
            {
                selectedQuest = null;
            }
        });

        if (isFirstToggle)
        {
            toggleComponent.isOn = true;
            selectedQuest = quest;
            isFirstToggle = false;
        }
    }


    void _DestroyQuestNameToggles()
    {
        foreach (Transform questProgressType in questList.transform)
        {
            if (questProgressType.gameObject.activeSelf)
            {
                questProgressType.gameObject.SetActive(false);
                Transform childTransform = questProgressType.GetChild(1);
                foreach (Transform questNameToggle in childTransform)
                {
                    GameManager.Resources.Destroy(questNameToggle.gameObject);
                }
            }
        }
    }
}
