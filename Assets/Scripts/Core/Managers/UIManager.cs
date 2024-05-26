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
    public GameObject SignUp;
    public GameObject Inventory;
    public GameObject PlayerInfo;
    public GameObject Shop;
    public GameObject Setting;
    public GameObject InputName;
    public GameObject ConfirmYN;
    public GameObject ConfirmY;
    public GameObject StatusWindow;
    //public GameObject SkillWindow;
    public GameObject InGameMain;

    GameObject popupCanvas;
    bool _leafPopup; // 단말 팝업. ex) 확인 여부 묻는 창

    // 실시간 팝업 관리 링크드 리스트
    public LinkedList<GameObject> _activePopupList;
    public List<GameObject> _linkedPopupList;

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
        _linkedPopupList = new List<GameObject>();
#elif INVENTEST
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI 관련된 기능들을 수행할 수 있는 프리팹 생성
        popupCanvas = GameObject.Find("PopupCanvas");
        Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupCanvas.transform);
        PlayerInfo = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/PlayerInfo", popupCanvas.transform);
        Shop = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ShopUI", popupCanvas.transform);
        _activePopupList = new LinkedList<GameObject>();
        _linkedPopupList = new List<GameObject>();
#else
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI 관련된 기능들을 수행할 수 있는 프리팹 생성
        Object.DontDestroyOnLoad(uiManage);
        popupCanvas = GameObject.Find("PopupCanvas");
        Object.DontDestroyOnLoad(popupCanvas);
        SetOutGamePopups();

        _activePopupList = new LinkedList<GameObject>();
        _linkedPopupList = new List<GameObject>();
#endif
    }
    public void SetOutGamePopups()
    {
        SignUp = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SignUp", popupCanvas.transform);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupCanvas.transform);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupCanvas.transform);
        ConfirmYN = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ConfirmYN", popupCanvas.transform);
        ConfirmY = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ConfirmY", popupCanvas.transform);
    }

    public void SetInGamePopups()
    {
        Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupCanvas.transform);
        PlayerInfo = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/PlayerInfo", popupCanvas.transform);
        // Shop = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/ShopUI", popupCanvas.transform);
        StatusWindow = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/StatusWindow", popupCanvas.transform);
        // SkillWindow = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SkillWindow", popupCanvas.transform);
        StatusWindow.SetActive(false);
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
        Shop.SetActive(false);
    }

    public void OpenPopup(GameObject popup)
    {
        if (!_leafPopup)
        {
            _activePopupList.AddLast(popup);
            popup.SetActive(true);
            SortPopupView();
        }
    }
    
    // 종속된 팝업 열기
    public void OpenChildPopup(GameObject popup, bool leaf = false)
    {        
        if (!_leafPopup)
        {
            if (_linkedPopupList.Count == 0)
            {
                var latest = _activePopupList.Last.Value;
                _linkedPopupList.Add(latest); //Root가 될 팝업을 linkedPopupList에 추가
            }

            _activePopupList.AddLast(popup); // OpenPopup과 동일
            popup.SetActive(true);
            SortPopupView();

            _linkedPopupList.Add(popup);
        }
        _leafPopup = leaf;
    }

    public void ClosePopup(GameObject popup)
    {
        if (_linkedPopupList.Count > 0) // ex) 확인 여부 묻는 창에서 취소 누를 경우
        {
            _linkedPopupList.Remove(popup);
        }
        _activePopupList.Remove(popup);
        popup.SetActive(false);
        _leafPopup = false;
    }

    public void CloseLinkedPopup()
    {
        for (int i = _linkedPopupList.Count - 1; i >= 0; i--) // linkedPopupList.Count가 계속 변함을 주의
        {
            ClosePopup(_linkedPopupList[i]);
        }
        _leafPopup = false;
    }

    // 모든 팝업 닫기
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // 가장 마지막에 연 팝업이 화면상 가장 위에 오도록
    public void SortPopupView()
    {
        var popup = _activePopupList.Last.Value;
        popup.transform.SetAsLastSibling();
    }

    // 클릭한 팝업이 가장 앞으로 오도록
    public void GetPopupForward(GameObject go)
    {
        _activePopupList.Remove(go);
        _activePopupList.AddLast(go);
        SortPopupView();
    }

    public void OpenOrClose(GameObject go)
    {
        if (!go.activeSelf)
        {
            OpenPopup(go);
        }
        else
        {
            ClosePopup(go);
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

    /*    public void Enter()
        {
            if (_activePopupList.Count > 0)
            {
                // Enter 누를 경우 수락 기능 실행 후 팝업 닫기
                ClosePopup(_activePopupList.First.Value);
            }
        }*/
}