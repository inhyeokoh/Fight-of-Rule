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

    // �ǽð� �˾� ���� ��ũ�� ����Ʈ
    public LinkedList<GameObject> _activePopupList;

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

    // ��� �˾� �ݱ�
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // ���̾��Ű���� �� �Ʒ� ������ �����Ͽ� �信�� ���� ���� ǥ�õǵ���
    public void SortPopupView()
    {
        // ��ũ�帮��Ʈ ��ȸ�ϸ鼭 �˾� ���� ���ġ.
        // ���� �������� �� �˾��� ȭ��� ���� ���� ������
        foreach (var popup in _activePopupList)
        {            
            popup.transform.SetAsLastSibling();
        }
    }

    // Ŭ���� �˾��� ���� ������ ������
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
                // Enter ���� ��� ���� ��� ���� �� �˾� �ݱ�
                ClosePopup(_activePopupList.First.Value);
            }
        }*/
}