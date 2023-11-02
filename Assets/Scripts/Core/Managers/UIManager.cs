using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager
{
/*    // sortingOrder ���� �����ϴ� ����, �տ� �� �� �ִ� 0~9�� ���ܵ�
    int _order = 10;

    // gameobject�� �ƴ� UI_Popup ������Ʈ�� ������ �ִ� ������ ����
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root // �ش� �̸� ������Ʈ �Ʒ� �����Ͽ� �����ϰ��� ��. ���ٸ� ����
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }
            return root;
        }    
    }

    // popupUI�� ������ sortingOrder�� ������
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // ĵ���� �ȿ� ĵ������ ������ �θ�� ������� sortingOrder�� ������ ��

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // popup�̶� ���� ���� UI
        {
            canvas.sortingOrder = 0;
        }    
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene // ui scene ��� �޴� �� ������ ���ʸ� Ÿ��
    {
        // �̸��� ���� �Է����� ������ TŸ���� �̸����� ���
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = GameManager.Resources.Instantiate($"Prefabs/UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>(); // ������ �ȿ� ��ũ��Ʈ �߰�
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); // �θ� ������Ʈ ����

        return sceneUI;
    }


    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/{name}");
        T popup = Utils.GetOrAddComponent<T>(go);
        _popupStack.Push(popup); // ������

        go.transform.SetParent(Root.transform);

        return popup; // ������
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        UI_Popup popup = _popupStack.Pop();
        GameManager.Resources.Destroy(popup.gameObject);
        popup = null; // ���� �Ŀ� Ȥ�ö� ������ �Ұ��ϵ���
        _order--;
    }

    // �̸��� ����ؼ� �����ϰ� Ȯ���ϴ� version
    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        if (_popupStack.Peek() != popup)
        {
#if UNITY_EDITOR
            Debug.Log("Close Popup Failed");
#endif
            return;
        }

        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
        {
            ClosePopupUI();
        }
    }*/
}
