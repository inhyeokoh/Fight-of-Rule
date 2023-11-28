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
        popupTr = GameObject.Find("Canvas").transform;
        Noti = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Noti", popupTr);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupTr);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupTr);

        // ����Ʈ �ʱ�ȭ
        _allPopupList = new List<GameObject>()
        {
            Noti, Setting, InputName
        };

        _activePopupList = new LinkedList<GameObject>();

        InitCloseAll();
    }

    // ���� �� ��� �˾� �ݱ�
    public void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
    }

    // ��� �˾� �ݱ�
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

    // �˾��� ��Ȱ��ȭ�ϰ� ��ũ�帮��Ʈ���� ����
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
            popup.transform.SetAsLastSibling(); // ���̾��Ű���� ���� �� �Ʒ� ������ ���� (�׷��� �信�� ���� ���� ǥ�õ�)
        }
    }

    // Ŭ���� �˾��� ���� ������ ������
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
            // ESC ���� ��� ��ũ�帮��Ʈ�� First �ݱ�
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
                // Enter ���� ��� ���� ��� ���� �� �˾� �ݱ�
                ClosePopup(_activePopupList.First.Value);
            }
        }*/
}