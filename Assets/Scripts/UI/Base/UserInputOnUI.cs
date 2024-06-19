using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Esc, Enter, Tab 기능
/// </summary>
public class UserInputOnUI : MonoBehaviour
{
    public static UserInputOnUI instance = null;

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

    public void Enter(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {

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
    
    public void Tab(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            
        }
    }
}
