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
    public GameObject Setting;
    public GameObject InputName;

    Transform popupTr;

    // 실시간 팝업 관리 링크드 리스트
    public LinkedList<GameObject> _activePopupList;

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
    }

    protected override void _Init()
    {
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI 관련된 기능들을 수행할 수 있는 프리팹 생성
        moveAction = uiManage.GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
        Object.DontDestroyOnLoad(uiManage); 

        _activePopupList = new LinkedList<GameObject>();
    }

    public void SetPopups(bool ingame)
    {
        popupTr = GameObject.Find("Canvas").transform;
        if (ingame)
        {            
            Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupTr);
            Inventory.SetActive(false);
        }
        else
        {
            SignUp = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SignUp", popupTr);
            Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupTr);
            InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupTr);
            SignUp.SetActive(false);
            Setting.SetActive(false);
            InputName.SetActive(false);
        }

    }

    public void ConnectPlayerInput()
    {
        pi = GameObject.Find("PlayerController").GetComponent<PlayerInput>();
        moveAction = pi.currentActionMap.FindAction("Move");
        fireAction = pi.currentActionMap.FindAction("Fire");
    }

    public void OpenPopup(GameObject popup)
    {
        _activePopupList.AddLast(popup);
        popup.SetActive(true);
        SortPopupView();
    }

    public void ClosePopup(GameObject popup)
    {
        _activePopupList.Remove(popup);
        popup.SetActive(false);
        SortPopupView();
    }

    // 모든 팝업 닫기
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // 하이어라키에서 맨 아래 오도록 변경하여 뷰에서 가장 위에 표시되도록
    public void SortPopupView()
    {
        // 링크드리스트 순회하면서 팝업 순서 재배치.
        // 가장 마지막에 연 팝업이 화면상 가장 위에 오도록
        foreach (var popup in _activePopupList)
        {            
            popup.transform.SetAsLastSibling();
        }
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