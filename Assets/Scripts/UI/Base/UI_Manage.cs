using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

// InputAction ȣ�� �ޱ�, �� �� ����, �� ���� ������ �˾� ȣ�� ���� 
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
            Debug.Log("Esc����");
            if (GameManager.UI._activePopupList.Count > 0)
            {
                // ESC ���� ��� ��ũ�帮��Ʈ�� First �ݱ�
                GameManager.UI.ClosePopup(GameManager.UI._activePopupList.First.Value);
            }
            else
            {
                // ������ ��ġ�ߴ� ������
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
