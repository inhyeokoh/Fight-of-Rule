using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

// InputAction 호출 받기, 씬 간 연결, 씬 별로 상이한 팝업 호출 실행 
public class UI_UserInput : MonoBehaviour
{
    public static UI_UserInput instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Escape(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            if (GameManager.UI._activePopupList.Count > 0)
            {
                GameManager.UI.ClosePopup(GameManager.UI._activePopupList.Last.Value);
            }
            else
            {
                SceneController.instance.LoadPreviousScene();                
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
