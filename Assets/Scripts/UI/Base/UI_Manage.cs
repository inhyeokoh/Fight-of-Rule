using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

// InputAction 호출 받기, 씬 간 연결, 씬 별로 상이한 팝업 호출 실행 
public class UI_Manage : MonoBehaviour
{
    public int curSceneNum;
    public Stack<int> scenes;

    private void Awake()
    {
        scenes = new Stack<int>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        curSceneNum = SceneManager.GetActiveScene().buildIndex;
        scenes.Push(curSceneNum);
                
        if (scene.name == "StatePattern")
        {
            GameManager.UI.ConnectPlayerInput();
            GameManager.UI.SetInGamePopups();
            GameManager.Inven.ConnectInvenUI();
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            if (GameManager.UI._activePopupList.Count > 0)
            {
                // ESC 누를 경우 링크드리스트의 First 닫기
                GameManager.UI.ClosePopup(GameManager.UI._activePopupList.Last.Value);
            }
            else
            {
                // 이전에 위치했던 씬으로
                GameManager.Scene.GetLocatedScene();
            }           
        }
    }

    public void Inven(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Inventory);
        }
    }
}
