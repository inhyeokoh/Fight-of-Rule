// #define TEST
#define INVENTEST
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : SubClass<GameManager>
{
    PlayerInput pi;
    InputAction moveAction;
    InputAction fireAction;
/*    public GameObject SignUp;
    public GameObject Inventory;
    public GameObject PlayerInfo;
    public GameObject Shop;
    public GameObject Setting;
    public GameObject InputName;
    public GameObject ConfirmYN;
    public GameObject ConfirmY;
    public GameObject StatusWindow;
    //public GameObject SkillWindow;
    public GameObject InGameMain;*/

    public UI_SignUp SignUp;
    public UI_InputName InputName;
    public UI_Setting Setting;
    public UI_Blocker Blocker;
    public UI_ConfirmYN ConfirmYN;
    public UI_ConfirmY ConfirmY;

    public UI_Inventory Inventory;
    public UI_PlayerInfo PlayerInfo;
    public UI_Shop Shop;
    public UI_InGameConfirmYN InGameConfirmYN;
    public UI_InGameConfirmY InGameConfirmY;
    public UI_StatusWindow StatusWindow;
    //public GameObject SkillWindow;
    public UI_InGameMain InGameMain;

    GameObject popupCanvas;

    int blockerCount = 0;

    // 실시간 팝업 관리
    public LinkedList<UI_Entity> _activePopupList;

    bool _init;

    protected override void _Clear()
    {
    }

    protected override void _Excute()
    {
        if (!_init)
        {
            _DeactivateAllPopups();
            _init = true;
        }
    }

    protected override void _Init()
    {
        // 커서 화면 밖으로 안 나가도록. 게임 제작중에는 불편해서 주석처리
        // Cursor.lockState = CursorLockMode.Confined;
#if TEST
        _activePopupList = new LinkedList<GameObject>();
#elif INVENTEST
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI 관련된 기능들을 수행할 수 있는 프리팹 생성
        popupCanvas = GameObject.Find("PopupCanvas");
        Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupCanvas.transform).GetComponent<UI_Inventory>();
        PlayerInfo = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/PlayerInfo", popupCanvas.transform).GetComponent<UI_PlayerInfo>();
        Shop = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ShopUI", popupCanvas.transform).GetComponent<UI_Shop>();
        Blocker = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Blocker", popupCanvas.transform).GetComponent<UI_Blocker>();
        InGameConfirmYN = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InGameConfirmYN", popupCanvas.transform).GetComponent<UI_InGameConfirmYN>();
        InGameConfirmY = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InGameConfirmY", popupCanvas.transform).GetComponent<UI_InGameConfirmY>();
        _activePopupList = new LinkedList<UI_Entity>();
#else
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI 관련된 기능들을 수행할 수 있는 프리팹 생성
        Object.DontDestroyOnLoad(uiManage);
        popupCanvas = GameObject.Find("PopupCanvas");
        Object.DontDestroyOnLoad(popupCanvas);
        SetOutGamePopups();

        _activePopupList = new LinkedList<GameObject>();
#endif
    }
/*    public void SetOutGamePopups()
    {
        SignUp = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SignUp", popupCanvas.transform);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupCanvas.transform);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupCanvas.transform);
        ConfirmYN = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ConfirmYN", popupCanvas.transform);
        ConfirmY = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ConfirmY", popupCanvas.transform);
    }*/

    public void SetInGamePopups()
    {
        // Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupCanvas.transform);
        // PlayerInfo = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/PlayerInfo", popupCanvas.transform);
        // Shop = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ShopUI", popupCanvas.transform);
        //StatusWindow = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/StatusWindow", popupCanvas.transform);
        // SkillWindow = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SkillWindow", popupCanvas.transform);
        //StatusWindow.SetActive(false);
    }


    public void ConnectPlayerInput()
    {
        pi = GameObject.Find("PlayerController").GetComponent<PlayerInput>();
        moveAction = pi.currentActionMap.FindAction("Move");
        fireAction = pi.currentActionMap.FindAction("Fire");
    }

    void _DeactivateAllPopups()
    {
        Shop.GetComponent<UI_Shop>().shopSell.gameObject.SetActive(false);
        Shop.GetComponent<UI_Shop>().shopRepurchase.gameObject.SetActive(false);
        // Shop.SetActive(false);
        Shop.gameObject.SetActive(false);
    }

    public void OpenPopup(UI_Entity targetPopup)
    {
        _activePopupList.AddLast(targetPopup);
        SortPopupView();
        targetPopup.gameObject.SetActive(true);
    }

    public void ClosePopup(UI_Entity targetPopup)
    {
        _activePopupList.Remove(targetPopup);
        targetPopup.gameObject.SetActive(false);
    }

    public void CloseLinkedPopup()
    {
    }

    // 특정 팝업과 그 하위 팝업들을 닫는 메소드
    public void ClosePopupAndChildren(UI_Entity targetPopup)
    {
        // targetPopup이 현재 활성화된 팝업 리스트에 있는지 확인
        if (_activePopupList.Contains(targetPopup))
        {
            // targetPopup과 그 하위 팝업들을 닫음
            ClosePopupRecursively(targetPopup);
        }
    }

    // targetPopup과 그 하위 팝업들을 재귀적으로 닫음
    private void ClosePopupRecursively(UI_Entity targetPopup)
    {
        // targetPopup 닫기
        ClosePopup(targetPopup);

        // targetPopup의 하위 UI_Entity들에 대해 반복
        foreach (var child in targetPopup.childPopups)
        {
            ClosePopupRecursively(child);
        }
    }

    // 가장 마지막에 연 팝업이 화면상 가장 위에 오도록
    public void SortPopupView()
    {
        var popup = _activePopupList.Last.Value;
        popup.transform.SetAsLastSibling();
    }

    // 클릭한 팝업이 가장 앞으로 오도록
    public void GetPopupForward(UI_Entity targetPopup)
    {
        _activePopupList.Remove(targetPopup);
        _activePopupList.AddLast(targetPopup);
        SortPopupView();
    }

    public void UseBlocker(bool activate)
    {
        if (activate)
        {
            if (blockerCount == 0)
            {
                Blocker.gameObject.SetActive(true);
            }
            blockerCount++;
        }
        else
        {
            blockerCount--;
            if (blockerCount <= 0)
            {
                Blocker.gameObject.SetActive(false);
                blockerCount = 0; // 음수 방지
            }
        }

        // Blocker의 위치를 팝업 바로 아래로 설정합니다.
        int activatePopupIndex = 0;
        for (int i = 0; i < popupCanvas.transform.childCount; i++)
        {
            if (popupCanvas.transform.GetChild(i).gameObject.activeSelf)
            {
                activatePopupIndex = i;
            }
        }
        int beforeLast = activatePopupIndex - 1;
        Blocker.transform.SetSiblingIndex(beforeLast);
    }

    public void OpenOrClose(UI_Entity targetPopup)
    {
        if (!targetPopup.gameObject.activeSelf)
        {
            OpenPopup(targetPopup);
        }
        else
        {
            ClosePopup(targetPopup);
        }
    }

    public void PointerOnUI(bool On)
    {
        if (On)
        {           
            moveAction.Disable();
            fireAction.Disable();
        }
        else
        {
            moveAction.Enable();
            fireAction.Enable();
        }
    }
}