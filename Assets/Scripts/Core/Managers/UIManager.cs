using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : SubClass<GameManager>
{
    PlayerInput pi;
    InputAction moveAction;
    InputAction fireAction;
    public GameObject Inventory;
    public GameObject Setting;
    public GameObject InputName;

    Transform popupTr;

    // 실시간 팝업 관리 링크드 리스트
    public LinkedList<GameObject> _activePopupList;

    // 전체 팝업 목록
    public List<GameObject> _allPopupList;

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

        // 리스트 초기화
        _allPopupList = new List<GameObject>()
        {
            Inventory, Setting, InputName
        };

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
            Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupTr);
            InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupTr);
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

    // 모든 팝업 닫기
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // 팝업을 활성화하고 링크드리스트에서 등록
    public void OpenPopup(GameObject popup)
    {
        _activePopupList.AddFirst(popup);
        popup.SetActive(true);
        RefreshAllPopupDepth();
    }

    // 팝업을 비활성화하고 링크드리스트에서 제거
    public void ClosePopup(GameObject popup)
    {
        _activePopupList.Remove(popup);
        popup.SetActive(false);
        RefreshAllPopupDepth();
    }

    // 링크드리스트 내 모든 팝업의 자식 순서 재배치
    public void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupList)
        {
            // 하이어라키에서 순서 맨 아래 오도록 변경
            // 뷰에서 가장 위에 표시됨
            popup.transform.SetAsLastSibling();
        }
    }

    // 클릭한 팝업이 가장 앞으로 오도록
    public void GetPopupFoward(GameObject go)
    {
        _activePopupList.Remove(go);
        _activePopupList.AddFirst(go);
        RefreshAllPopupDepth();
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