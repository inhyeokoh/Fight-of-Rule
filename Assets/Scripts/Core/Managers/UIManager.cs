using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class UIManager : SubClass<GameManager>
{
    PlayerInput playerAction;
    string playername = "Player";

    // public GameObject Inventory;
    public GameObject Noti;
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
        GameObject player = GameObject.Find($"{playername}");
        if (player != null)
        {
            playerAction = player.GetComponent<PlayerInput>(); // 로그인 씬 말고 인게임 들어갔을때 실행 필요할듯
        }

        // 나중엔 pool로 관리
        popupTr = GameObject.Find("Canvas").transform;
        Noti = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Noti", popupTr);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupTr);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupTr);

        // 리스트 초기화
        _allPopupList = new List<GameObject>()
        {
            Noti, Setting, InputName
        };

        _activePopupList = new LinkedList<GameObject>();

        InitCloseAll();
    }

    // 시작 시 모든 팝업 닫기
    public void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    // 모든 팝업 닫기
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

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
            popup.transform.SetAsLastSibling(); // 하이어라키에서 순서 맨 아래 오도록 변경 (그래야 뷰에서 가장 위에 표시됨)
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

    public void Escape()
    {
        if (_activePopupList.Count > 0)
        {
            // ESC 누를 경우 링크드리스트의 First 닫기
            ClosePopup(_activePopupList.First.Value);
        }
        else
        {
            GameManager.Scene.GetPreviousScene();
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