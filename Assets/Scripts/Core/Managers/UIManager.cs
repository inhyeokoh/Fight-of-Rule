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

    // �ǽð� �˾� ���� ��ũ�� ����Ʈ
    public LinkedList<GameObject> _activePopupList;

    // ��ü �˾� ���
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
            playerAction = player.GetComponent<PlayerInput>(); // �α��� �� ���� �ΰ��� ������ ���� �ʿ��ҵ�
        }

        // ���߿� pool�� ����
/*        _inventoryPopup = GameManager.Resources.Instantiate($"Prefabs/UI/Inventory", GameObject.Find("Canvas").transform);
        _notiPopup = GameManager.Resources.Instantiate($"Prefabs/UI/Notification", GameObject.Find("Canvas").transform);
        _inventoryPopup.SetActive(false);
        _notiPopup.SetActive(false);*/

        _activePopupList = new LinkedList<GameObject>();

        // ����Ʈ �ʱ�ȭ
        _allPopupList = new List<GameObject>()
        {
            _inventoryPopup, _notiPopup
        };
    }

    // ���� �� ��� �˾� �ݱ�
    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    // �˾��� ���� ��ũ�帮��Ʈ�� ��ܿ� �߰�
    public void OpenPopup(GameObject popup)
    {
        _activePopupList.AddFirst(popup);
        popup.SetActive(true);
        RefreshAllPopupDepth();
    }

    // �˾��� �ݰ� ��ũ�帮��Ʈ���� ����
    public void ClosePopup(GameObject popup)
    {
        _activePopupList.Remove(popup);
        popup.SetActive(false);
        RefreshAllPopupDepth();
    }

    // ��ũ�帮��Ʈ �� ��� �˾��� �ڽ� ���� ���ġ
    public void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupList)
        {
            popup.transform.SetAsFirstSibling(); // ���̾��Ű���� ��������
        }
    }

    // Ŭ���� �˾��� ���� ������ ������
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
            // ESC ���� ��� ��ũ�帮��Ʈ�� First �ݱ�
            ClosePopup(_activePopupList.First.Value);
        }
    }

    /*    public void Enter()
        {
            if (_activePopupList.Count > 0)
            {
                // Enter ���� ��� ���� ��� ���� �� �˾� �ݱ�
                ClosePopup(_activePopupList.First.Value);
            }
        }*/
}
