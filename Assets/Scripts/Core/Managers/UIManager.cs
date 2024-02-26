using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UIManager : SubClass<GameManager>
{
    PlayerInput playerAction;
    string playername = "Warrior(Clone)";

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
        GameObject player = GameObject.Find($"{playername}");
        if (player != null)
        {
            playerAction = player.GetComponent<PlayerInput>(); // �α��� �� ���� �ΰ��� ������ ���� �ʿ��ҵ�
        }
        else
        {
            GameObject uiManage = GameManager.Resources.Instantiate($"Prefabs/UI/Base/UI_Manage"); // UI ���õ� ��ɵ��� ������ �� �ִ� ������ ����
            Object.DontDestroyOnLoad(uiManage);
        }

        // ����Ʈ �ʱ�ȭ
        _allPopupList = new List<GameObject>()
        {
            Inventory, Setting, InputName
        };

        _activePopupList = new LinkedList<GameObject>();
    }

    public void SetPopups() // ���Ŀ� ���ڷ� �ΰ��Ӿ����� ���θ� �޾Ƽ� �����ؼ� �˾� �����ص� �ɵ�
    {
        popupTr = GameObject.Find("Canvas").transform;
        Inventory = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Inventory", popupTr);
        Setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", popupTr);
        InputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", popupTr);
        Inventory.SetActive(false);
        Setting.SetActive(false);
        InputName.SetActive(false);
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

    public void Escape()
    {
        if (_activePopupList.Count > 0)
        {
            // ESC ���� ��� ��ũ�帮��Ʈ�� First �ݱ�
            ClosePopup(_activePopupList.First.Value);
        }
        else
        {
            // ������ ��ġ�ߴ� ������
            GameManager.Scene.GetLocatedScene();
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