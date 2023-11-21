using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class UIManager : SubClass<GameManager>
{
    PlayerInput playerAction;
    string playername = "Player";
    public GameObject _inventoryPopup;
    public GameObject _notiPopup;

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
/*        _inventoryPopup = GameManager.Resources.Instantiate($"Prefabs/UI/Inventory", GameObject.Find("Canvas").transform);
        _notiPopup = GameManager.Resources.Instantiate($"Prefabs/UI/Notification", GameObject.Find("Canvas").transform);
        _inventoryPopup.SetActive(false);
        _notiPopup.SetActive(false);*/

        _activePopupList = new LinkedList<GameObject>();

        // 리스트 초기화
        _allPopupList = new List<GameObject>()
        {
            _inventoryPopup, _notiPopup
        };
    }

    // 시작 시 모든 팝업 닫기
    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    // 팝업을 열고 링크드리스트의 상단에 추가
    public void OpenPopup(GameObject popup)
    {
        _activePopupList.AddFirst(popup);
        popup.SetActive(true);
        RefreshAllPopupDepth();
    }

    // 팝업을 닫고 링크드리스트에서 제거
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
            popup.transform.SetAsFirstSibling(); // 하이어라키에서 순서변경
        }
    }

    // 클릭한 팝업이 가장 앞으로 오도록
    public void GetPopupFoward(GameObject go)
    {
        _activePopupList.Remove(go);
        _activePopupList.AddFirst(go);
        RefreshAllPopupDepth();
    }

    public void Inven()
    {
        if (!_inventoryPopup.activeSelf)
        {
            OpenPopup(_inventoryPopup);
        }
        else
        {
            ClosePopup(_inventoryPopup);
        }
    }

    public void Escape()
    {
        if (_activePopupList.Count > 0)
        {
            // ESC 누를 경우 링크드리스트의 First 닫기
            ClosePopup(_activePopupList.First.Value);
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
