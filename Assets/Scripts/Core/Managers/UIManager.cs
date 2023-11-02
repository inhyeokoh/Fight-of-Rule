using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager
{
/*    // sortingOrder 등을 관리하는 변수, 앞에 뜰 수 있는 0~9를 남겨둠
    int _order = 10;

    // gameobject가 아닌 UI_Popup 컴포넌트를 가지고 있는 것으로 구분
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    public GameObject Root // 해당 이름 오브젝트 아래 생성하여 정리하고자 함. 없다면 생성
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

    // popupUI가 켜질때 sortingOrder를 정해줌
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true; // 캔버스 안에 캔버스가 있을때 부모와 상관없이 sortingOrder를 갖도록 함

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else // popup이랑 연관 없는 UI
        {
            canvas.sortingOrder = 0;
        }    
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene // ui scene 상속 받는 애 없으면 제너릭 타입
    {
        // 이름을 따로 입력하지 않으면 T타입의 이름으로 사용
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = GameManager.Resources.Instantiate($"Prefabs/UI/Scene/{name}");
        T sceneUI = go.GetOrAddComponent<T>(); // 프리팹 안에 스크립트 추가
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform); // 부모 오브젝트 지정

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
        _popupStack.Push(popup); // 차이점

        go.transform.SetParent(Root.transform);

        return popup; // 차이점
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
        {
            return;
        }

        UI_Popup popup = _popupStack.Pop();
        GameManager.Resources.Destroy(popup.gameObject);
        popup = null; // 삭제 후에 혹시라도 접근이 불가하도록
        _order--;
    }

    // 이름을 명시해서 안전하게 확인하는 version
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
