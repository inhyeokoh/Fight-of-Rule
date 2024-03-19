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
    public GameObject Confirm;
    bool _leafPopup;

    GameObject popupCanvas;

    // �ǽð� �˾� ���� ��ũ�� ����Ʈ
    public LinkedList<GameObject> _activePopupList;
    public List<GameObject> _linkedPopupList;

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
        popupCanvas = GameObject.Find("PopupCanvas");
        Object.DontDestroyOnLoad(popupCanvas);
        SetOutGamePopups();

        _activePopupList = new LinkedList<GameObject>();
        _linkedPopupList = new List<GameObject>();

    }

    public void SetOutGamePopups()
    {
        SignUp = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/SignUp", popupCanvas.transform);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupCanvas.transform);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupCanvas.transform);
        Confirm = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Confirm", popupCanvas.transform);
    }

    public void SetInGamePopups()
    {
        Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupCanvas.transform);
    }


    public void ConnectPlayerInput()
    {
        pi = GameObject.Find("PlayerController").GetComponent<PlayerInput>();
        moveAction = pi.currentActionMap.FindAction("Move");
        fireAction = pi.currentActionMap.FindAction("Fire");
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
    
    // ���ӵ� �˾� ����
    public void OpenChildPopup(GameObject popup, bool leaf = false)
    {        
        if (!_leafPopup)
        {
            if (_linkedPopupList.Count == 0)
            {
                var latest = _activePopupList.Last.Value;
                _linkedPopupList.Add(latest); //Root�� �� �˾��� linkedPopupList�� �߰�
            }

            OpenPopup(popup);
            _linkedPopupList.Add(popup);
        }
        _leafPopup = leaf;
    }

    public void ClosePopup(GameObject popup)
    {
        if (_linkedPopupList.Count > 0)
        {
            _linkedPopupList.Remove(popup);
        }
        _activePopupList.Remove(popup);
        popup.SetActive(false);
        _leafPopup = false;
    }

    public void CloseLinkedPopup()
    {
        for (int i = _linkedPopupList.Count - 1; i >= 0; i--) // linkedPopupList.Count�� ��� ������ ����
        {
            ClosePopup(_linkedPopupList[i]);
        }
        _leafPopup = false;
    }

    // ��� �˾� �ݱ�
    public void CloseAll()
    {
        foreach (var popup in _activePopupList)
        {
            ClosePopup(popup);
        }
    }

    // ���� �������� �� �˾��� ȭ��� ���� ���� ������
    public void SortPopupView()
    {
        var popup = _activePopupList.Last.Value;
        popup.transform.SetAsLastSibling();
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