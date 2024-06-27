using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class UI_Dialog : UI_Entity
{
    List<Quest> accessibleQuests;
    int _npcID;

    CameraFollow cameraFollow;

    #region 퀘스트 목록
    GameObject questPanel;
    ToggleGroup toggleGroup;
    GameObject canComplete;
    GameObject available;
    GameObject ongoing;
    #endregion

    #region 대화창
    TMP_Text mainText;
    const string defaultText = "오늘은 무슨 일이 생기려나...";
    GameObject nextButton;
    GameObject acceptButton;
    int dialogCount;
    #endregion

    Quest selectedQuest;
        
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

    private void OnEnable()
    {
        if (GameManager.UI.init)
        {
            GameManager.UI.PointerOnUI(true);
            nextButton.SetActive(false);
            acceptButton.SetActive(false);
            _ShowQuests();
            cameraFollow = GameObject.FindWithTag("vCam").GetComponent<CameraFollow>();
            cameraFollow.NpcPos = GameManager.Data.npcDict[_npcID].transform.position;
            cameraFollow.ZoomState = CameraFollow.Enum_ZoomTypes.DialogZoom;
            mainText.text = defaultText;
            dialogCount = 0;
        }
    }

    private void OnDisable()
    {
        canComplete.gameObject.SetActive(false);
        available.gameObject.SetActive(false);
        ongoing.gameObject.SetActive(false);
        GameManager.UI.PointerOnUI(false);
    }

    protected override void Init()
    {
        base.Init();

        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        questPanel = _entities[(int)Enum_UI_Dialog.QuestPanel].gameObject;
        nextButton = _entities[(int)Enum_UI_Dialog.Next].gameObject;
        acceptButton = _entities[(int)Enum_UI_Dialog.Accept].gameObject;
        GameObject questList = _entities[(int)Enum_UI_Dialog.QuestPanel].transform.GetChild(0).gameObject;
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
                    break;
            }
        };

        _entities[(int)Enum_UI_Dialog.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Dialog);
        };

        gameObject.SetActive(false);
    }

    public void HandOverNpcID(int npcID)
    {
        _npcID = npcID;
        accessibleQuests = GameManager.Data.npcDict[_npcID].AccessibleQuests ?? null;
    }

    void _ShowQuests()
    {
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
                    break;
            }
        }
        toggleGroup.ActiveToggles().FirstOrDefault();
    }
    
    // 퀘스트 선택 후 다음 버튼 눌렀을 때 호출하는 메서드
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
                break;
        }
    }
}