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
        GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI ���õ� ��ɵ��� ������ �� �ִ� ������ ����
        moveAction = uiManage.GetComponent<PlayerInput>().currentActionMap.FindAction("Move");
        Object.DontDestroyOnLoad(uiManage); 

        // ����Ʈ �ʱ�ȭ
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

    // ��� �˾� �ݱ�
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // �˾��� Ȱ��ȭ�ϰ� ��ũ�帮��Ʈ���� ���
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
            // ���̾��Ű���� ���� �� �Ʒ� ������ ����
            // �信�� ���� ���� ǥ�õ�
            popup.transform.SetAsLastSibling();
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
                // Enter ���� ��� ���� ��� ���� �� �˾� �ݱ�
                ClosePopup(_activePopupList.First.Value);
            }
        }*/
}