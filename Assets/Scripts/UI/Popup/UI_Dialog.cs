using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class UI_Dialog : UI_Entity
{
    int _npcID;

    CameraFollow cameraFollow;

    #region 퀘스트 목록
    List<Quest> accessibleQuests;
    GameObject questPanel;
    GameObject questList;
    ToggleGroup toggleGroup;
    GameObject canComplete;
    GameObject available;
    GameObject ongoing;
    Quest selectedQuest;
    #endregion

    #region 대화창
    TMP_Text mainText;
    const string Default_Text = "오늘은 무슨 일이 생기려나...";
    GameObject nextButton;
    GameObject acceptButton;
    int dialogCount;
    #endregion

    Enum_NpcType _npcType;

    enum Enum_UI_Dialog
    {
        Panel,
        QuestPanel,
        MainText,
        Next,
        Accept,
        Cancel
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Dialog);
    }

    void OnEnable()
    {
        if (!GameManager.UI.init) return;

        switch (_npcType)
        {
            case Enum_NpcType.Quest:
                GameManager.UI.PointerOnUI(true);
                nextButton.SetActive(false);
                acceptButton.SetActive(false);
                _ShowQuests();
                cameraFollow = GameObject.FindWithTag("vCam").GetComponent<CameraFollow>();
                cameraFollow.NpcPos = GameManager.Data.npcDict[_npcID].transform.position;
                cameraFollow.ZoomState = CameraFollow.Enum_ZoomTypes.DialogZoom;
                mainText.text = Default_Text;
                dialogCount = 0;
                break;
            case Enum_NpcType.Shop:
                GameManager.UI.OpenPopup(GameManager.UI.Shop);
                questPanel.SetActive(false);
                nextButton.SetActive(false);
                acceptButton.SetActive(false);
                cameraFollow = GameObject.FindWithTag("vCam").GetComponent<CameraFollow>();
                cameraFollow.NpcPos = GameManager.Data.npcDict[_npcID].transform.position;
                cameraFollow.ZoomState = CameraFollow.Enum_ZoomTypes.DialogZoom;
                mainText.text = Default_Text;
                break;
            default:
#if UNITY_EDITOR
                Debug.Assert(false);
#endif
                break;
        }
    }

    void OnDisable()
    {
        switch (_npcType)
        {
            case Enum_NpcType.Quest:
                _DestroyQuestNameToggles();
                canComplete.gameObject.SetActive(false);
                available.gameObject.SetActive(false);
                ongoing.gameObject.SetActive(false);
                GameManager.UI.PointerOnUI(false);
                break;
            case Enum_NpcType.Shop:
                GameManager.UI.PointerOnUI(false);
                break;
            default:
#if UNITY_EDITOR
                Debug.Assert(false);
#endif
                break;
        }
    }

    protected override void Init()
    {
        base.Init();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        questPanel = _entities[(int)Enum_UI_Dialog.QuestPanel].gameObject;
        nextButton = _entities[(int)Enum_UI_Dialog.Next].gameObject;
        acceptButton = _entities[(int)Enum_UI_Dialog.Accept].gameObject;
        questList = _entities[(int)Enum_UI_Dialog.QuestPanel].transform.GetChild(0).gameObject;
        toggleGroup = questList.GetComponent<ToggleGroup>();
        canComplete = questList.transform.GetChild(0).gameObject;
        available = questList.transform.GetChild(1).gameObject;
        ongoing = questList.transform.GetChild(2).gameObject;

        mainText = _entities[(int)Enum_UI_Dialog.MainText].GetComponent<TMP_Text>();

        _entities[(int)Enum_UI_Dialog.Next].ClickAction = (PointerEventData data) => {
            _ContinueDialog();
        };

        _entities[(int)Enum_UI_Dialog.Accept].ClickAction = (PointerEventData data) => {
            switch (selectedQuest.progress)
            {                
                case Enum_QuestProgress.Available:
                    GameManager.Quest.ReceiveQuest(selectedQuest.questData.questID);
                    GameManager.UI.ClosePopup(GameManager.UI.Dialog);
                    break;
                case Enum_QuestProgress.CanComplete:
                    GameManager.Quest.CompleteQuest(selectedQuest.questData.questID);
                    GameManager.UI.ClosePopup(GameManager.UI.Dialog);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Assert(false);
#endif
                    break;
            }
        };

        _entities[(int)Enum_UI_Dialog.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Dialog);
            switch (_npcType)
            {
                case Enum_NpcType.Quest:
                    break;
                case Enum_NpcType.Shop:
                    GameManager.UI.ClosePopup(GameManager.UI.Shop);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Assert(false);
#endif
                    break;
            }
        };

        gameObject.SetActive(false);
    }

    public void HandOverNpcID(int npcID)
    {
        _npcID = npcID;
        _npcType = GameManager.Data.npcDict[_npcID].npcType;
    }

    void _ShowQuests()
    {
        accessibleQuests = new List<Quest>();
        foreach (var quest in GameManager.Quest.questsByNpcID[_npcID])
        {
            // 시작 불가능, 완료된 퀘스트 제외 하고 보여주기
            if (quest.progress != Enum_QuestProgress.UnAvailable && quest.progress != Enum_QuestProgress.Completed)
            {
                accessibleQuests.Add(quest);
            }
        }

        if (accessibleQuests == null)
        {
            questPanel.SetActive(false);
            return;
        }

        questPanel.SetActive(true);
        foreach (var quest in accessibleQuests)
        {
            GameObject toggle;
            switch (quest.progress)
            {
                case Enum_QuestProgress.Available:
                    available.gameObject.SetActive(true);
                    toggle = GameManager.Resources.Instantiate("Prefabs/UI/Scene/QuestNameToggle", available.transform.GetChild(1).transform);
                    toggle.GetComponent<Toggle>().group = toggleGroup;
                    toggle.transform.GetChild(0).GetComponent<TMP_Text>().text = quest.questData.title;
                    toggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => selectedQuest = quest);
                    break;
                case Enum_QuestProgress.Ongoing:
                    ongoing.gameObject.SetActive(true);
                    toggle = GameManager.Resources.Instantiate("Prefabs/UI/Scene/QuestNameToggle", ongoing.transform.GetChild(1).transform);
                    toggle.GetComponent<Toggle>().group = toggleGroup;
                    toggle.transform.GetChild(0).GetComponent<TMP_Text>().text = quest.questData.title;
                    toggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => selectedQuest = quest);
                    break;
                case Enum_QuestProgress.CanComplete:
                    canComplete.gameObject.SetActive(true);
                    toggle = GameManager.Resources.Instantiate("Prefabs/UI/Scene/QuestNameToggle", canComplete.transform.GetChild(1).transform);
                    toggle.GetComponent<Toggle>().group = toggleGroup;
                    toggle.transform.GetChild(0).GetComponent<TMP_Text>().text = quest.questData.title;
                    toggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => selectedQuest = quest);
                    break;
                default:
#if UNITY_EDITOR
                    Debug.Assert(false);
#endif
                    break;
            }
        }
        toggleGroup.ActiveToggles().FirstOrDefault();
    }
    
    // 퀘스트 패널 하위 버튼에 OnClick 연결 
    public void ContinueWithSelectedQuest()
    {
        questPanel.SetActive(false);
        nextButton.SetActive(true);

        _ContinueDialog();
    }

    void _ContinueDialog()
    {
        switch (selectedQuest.progress)
        {
            case Enum_QuestProgress.Available:
                mainText.text = selectedQuest.questData.desc[dialogCount++];
                if (dialogCount == selectedQuest.questData.desc.Length)
                {
                    nextButton.SetActive(false);
                    acceptButton.SetActive(true);
                }
                break;
            case Enum_QuestProgress.Ongoing:
                mainText.text = "아직인가요?";
                nextButton.SetActive(false);
                break;
            case Enum_QuestProgress.CanComplete:
                mainText.text = selectedQuest.questData.congratulation[dialogCount++];
                if (dialogCount == selectedQuest.questData.congratulation.Length)
                {
                    nextButton.SetActive(false);
                    acceptButton.SetActive(true);
                }
                break;
            default:
#if UNITY_EDITOR
                Debug.Assert(false);
#endif
                break;
        }
    }

    void _DestroyQuestNameToggles()
    {
        foreach (Transform quest in questList.transform)
        {
            if (quest.gameObject.activeSelf)
            {
                Transform childTransform = quest.GetChild(1);
                foreach (Transform questNameToggle in childTransform)
                {
                    GameManager.Resources.Destroy(questNameToggle.gameObject);
                }
            }
        }
    }
}