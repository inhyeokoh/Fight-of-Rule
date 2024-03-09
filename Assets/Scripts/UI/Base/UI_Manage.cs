using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

// InputAction 호출 받기, 씬 간 연결, 씬 별로 상이한 팝업 호출 실행 
public class UI_Manage : MonoBehaviour
{
    bool inGame;
    public int curSceneNum;
    public Queue<int> scenes;

    private void Awake()
    {
        scenes = new Queue<int>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        curSceneNum = SceneManager.GetActiveScene().buildIndex;
        scenes.Enqueue(curSceneNum);

        if (SceneManager.GetActiveScene().name == "StatePattern")
        {
            inGame = true;
            GameManager.UI.ConnectPlayerInput();
        }

        GameManager.UI.SetPopups(inGame);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            Debug.Log("Esc감지");
            if (GameManager.UI._activePopupList.Count > 0)
            {
                // ESC 누를 경우 링크드리스트의 First 닫기
                GameManager.UI.ClosePopup(GameManager.UI._activePopupList.First.Value);
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
