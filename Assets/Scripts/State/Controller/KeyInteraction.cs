using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public enum Enum_KeyAction
{
    //PlayerAction

    SKill1,
    Skill2,
    Skill3,
    Skill4,

    Avoid,

    //UI
    UIInven,
    UIPlayerInfo,
    UISkillWindow,
}

public class KeyInteraction : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    public PlayerController player;

    [SerializeField]
    private InputActionReference[] inputActions;
    public InputActionReference[] InputActions { get { return inputActions; } }


    public InputActionReference currentAction;
    public InputAction changeAction;

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public string path;


    private void Awake()
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            ActionCallbackSetting((Enum_KeyAction)i, inputActions[i]);
        }
    }

    void Start()
    {
        player = PlayerController.instance;
    }
    public void OnMove(InputAction.CallbackContext context)
    {       
        player.Move(context);
    }

    public void OnAvoid(InputAction.CallbackContext context)
    {
       
        player.Avoid(context);

    } 
    public void OnClick(InputAction.CallbackContext context)
    {
        player.Click(context);
    }

    public void OnSkillQ(InputAction.CallbackContext context)
    {
        player.Skill1(context);
    }

    public void OnSkillW(InputAction.CallbackContext context)
    {
        player.Skill2(context);
    }
    public void OnSkillE(InputAction.CallbackContext context)
    {
        player.Skill3(context);
    }
    public void OnSkillR(InputAction.CallbackContext context)
    {
        player.Skill4(context);
    }

    public void OnDeadCheck(InputAction.CallbackContext context)
    {

    }

    public void OnAliveCheck(InputAction.CallbackContext context)
    {

    }

    public void OnInven(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Inventory);
        }
    }

    public void OnPlayerInfo(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            GameManager.UI.OpenOrClose(GameManager.UI.PlayerInfo);
        }
    }

    public void OnSkillWindow(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed)
        {
            GameManager.UI.OpenOrClose(GameManager.UI.SkillWindow);
        }
    }


    public void StartRebinding(Enum_KeyAction getAction, TMP_Text bindingDisplayNameText, Dictionary<InputAction, TMP_Text> keyTexts)
    {
        currentAction = GetAction(getAction);

        if (currentAction == null)
        {
            print("해당키가 없습니다");
            return;
        }

        currentAction.action.Disable();    
       
        if (currentAction.action.bindings[0].hasOverrides)
        {           
            path = currentAction.action.bindings[0].overridePath;
        }
        else
        {          
            path = currentAction.action.bindings[0].path;

        }
      
        bindingDisplayNameText.text = "Waiting For Input...";

        rebindingOperation = currentAction.action.PerformInteractiveRebinding().WithControlsExcluding("<Mouse>/leftButton")
        .WithControlsExcluding("<Keyboard>/enter").WithCancelingThrough("<Keyboard>/escape").WithCancelingThrough("<Mouse>/rightButton")
        .OnCancel(operation => RebindCancel(currentAction, bindingDisplayNameText))
        .OnComplete(operation => RebindComplete(currentAction,bindingDisplayNameText, keyTexts))
        .Start();
    }


    private void RebindComplete(InputActionReference currentAction, TMP_Text bindingDisplayNameText, Dictionary<InputAction, TMP_Text> keyTexts)
    {
        rebindingOperation.Dispose();
        currentAction.action.Enable();

        CheckDuplicateBindings(currentAction.action, keyTexts);      
        ShowBindText(currentAction.action, bindingDisplayNameText);

        currentAction.Set(currentAction.action);
    }

    private void ShowBindText(InputAction copyAction, TMP_Text bindingDisplayNameText)
    {
        int bindingIndex = copyAction.GetBindingIndexForControl(copyAction.controls[0]);
        
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(copyAction.bindings[bindingIndex]
            .effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void RebindCancel(InputAction currentAction, TMP_Text bindingDisplayNameText)
    {
        rebindingOperation.Dispose();
        currentAction.Enable();

        ShowBindText(currentAction, bindingDisplayNameText);
    }

    private void ChanageRebindComplete(InputAction changeAction, TMP_Text bindingDisplayNameText)
    {

        changeAction.Enable();
        changeAction.ApplyBindingOverride(path);

        ShowBindText(changeAction, bindingDisplayNameText);
    }

    private void CheckDuplicateBindings(InputAction action, Dictionary<InputAction, TMP_Text> keyTexts)
    {
        InputBinding newBinding = action.bindings[0];
        foreach (InputBinding binding in action.actionMap.bindings)
        {
            if (binding.action == newBinding.action)
            {
                continue;
            }
            if (binding.effectivePath == newBinding.effectivePath)
            {            
                InputAction changeAction = action.actionMap.FindAction(binding.action);
                ChanageRebindComplete(changeAction, keyTexts[changeAction]);         
                return;
            }
        }
    }

    public void GetServerKey()
    {

    }

    public void KeyUISetting(Dictionary<InputAction, TMP_Text> keyTexts)
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            InputAction action = GetAction((Enum_KeyAction)i).action;
            ShowBindText(action, keyTexts[action]);
        }
    }


    public InputActionReference GetAction(Enum_KeyAction key)
    {
        switch (key)
        {
            case Enum_KeyAction.SKill1:
                return inputActions[(int)Enum_KeyAction.SKill1];
            case Enum_KeyAction.Skill2:
                return inputActions[(int)Enum_KeyAction.Skill2];
            case Enum_KeyAction.Skill3:
                return inputActions[(int)Enum_KeyAction.Skill3];
            case Enum_KeyAction.Skill4:
                return inputActions[(int)Enum_KeyAction.Skill4];
            case Enum_KeyAction.Avoid:
                return inputActions[(int)Enum_KeyAction.Avoid];
            case Enum_KeyAction.UIInven:
                return inputActions[(int)Enum_KeyAction.UIInven];
            case Enum_KeyAction.UIPlayerInfo:
                return inputActions[(int)Enum_KeyAction.UIPlayerInfo];
            case Enum_KeyAction.UISkillWindow:
                return inputActions[(int)Enum_KeyAction.UISkillWindow];
        }

        return null;
    }


    public void ActionCallbackSetting(Enum_KeyAction key, InputActionReference currentAction)
    {
        switch (key)
        {
            case Enum_KeyAction.SKill1:            
                currentAction.action.started += OnSkillQ;
                currentAction.action.performed += OnSkillQ;
                currentAction.action.canceled += OnSkillQ;
                break;
            case Enum_KeyAction.Skill2:               
                currentAction.action.started += OnSkillW;
                currentAction.action.performed += OnSkillW;
                currentAction.action.canceled += OnSkillW;
                break;
            case Enum_KeyAction.Skill3:           
                currentAction.action.started += OnSkillE;
                currentAction.action.performed += OnSkillE;
                currentAction.action.canceled += OnSkillE;
                break;
            case Enum_KeyAction.Skill4:               
                currentAction.action.started += OnSkillR;
                currentAction.action.performed += OnSkillR;
                currentAction.action.canceled += OnSkillR;
                break;
            case Enum_KeyAction.Avoid:              
                currentAction.action.started += OnAvoid;
                currentAction.action.performed += OnAvoid;
                currentAction.action.canceled += OnAvoid;
                break;
            case Enum_KeyAction.UIInven:             
                currentAction.action.started += OnInven;
                currentAction.action.performed += OnInven;
                currentAction.action.canceled += OnInven;
                break;
            case Enum_KeyAction.UIPlayerInfo:            
                currentAction.action.started += OnPlayerInfo;
                currentAction.action.performed += OnPlayerInfo;
                currentAction.action.canceled += OnPlayerInfo;
                break;
            case Enum_KeyAction.UISkillWindow:
                currentAction.action.started += OnSkillWindow;
                currentAction.action.performed += OnSkillWindow;
                currentAction.action.canceled += OnSkillWindow;
                break;

        }
    }

    public void KeyReset(Dictionary<InputAction, TMP_Text> keyTexts)
    {
        for (int i = 0; i < inputActions.Length; i++)
        {
            inputActions[i].action.RemoveAllBindingOverrides();
            ShowBindText(inputActions[i].action, keyTexts[inputActions[i].action]);
        }
    }
}