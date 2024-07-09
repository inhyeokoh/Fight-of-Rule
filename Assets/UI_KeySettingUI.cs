using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class UI_KeySettingUI : UI_Entity
{
    enum Enum_UI_KeySetting
    {
        SKill1Btn,
        SKill2Btn,
        SKill3Btn,
        SKill4Btn,
        AvoidBtn,
        InventoryBtn,
        PlayerInfoBtn,
        SkillWindowBtn,

        Skill1Txt,
        Skill2Txt,
        Skill3Txt,
        Skill4Txt,
        AvoidTxt,
        InventoryTxt,
        PlayerInfoTxt,
        SkillWindowTxt,
    }


    KeyInteraction keyInteraction;
    Dictionary<Enum_KeyAction, InputActionReference> keyActions;
    Dictionary<InputAction, TMP_Text> keyTexts;
    [SerializeField]InputActionReference[] actions;
    [SerializeField]PlayerInput playerInput;



    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_KeySetting);
    }
    protected override void Init()
    {
        base.Init();

        keyInteraction = PlayerController.instance.gameObject.GetComponent<KeyInteraction>();
        keyActions = new Dictionary<Enum_KeyAction, InputActionReference>();
        keyTexts = new Dictionary<InputAction, TMP_Text>();

        for (int i = 0; i < actions.Length; i++)
        {
            keyActions.Add((Enum_KeyAction)i, actions[i]);
            keyTexts.Add(actions[i].action, _entities[i].GetComponentInChildren<TMP_Text>());
            keyInteraction.ActionCallback((Enum_KeyAction)i, actions[i]);
        }

        _entities[(int)Enum_UI_KeySetting.SKill1Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.SKill1], _entities[(int)Enum_UI_KeySetting.SKill1Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill2Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.Skill2], _entities[(int)Enum_UI_KeySetting.SKill2Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill3Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.Skill3], _entities[(int)Enum_UI_KeySetting.SKill3Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill4Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.Skill4], _entities[(int)Enum_UI_KeySetting.SKill4Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.PlayerInfoBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.UIPlayerInfo], _entities[(int)Enum_UI_KeySetting.PlayerInfoBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.AvoidBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.Avoid], _entities[(int)Enum_UI_KeySetting.AvoidBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.InventoryBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(keyActions[Enum_KeyAction.UIInven], _entities[(int)Enum_UI_KeySetting.InventoryBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };

/*        _entities[(int)Enum_UI_KeySetting.AvoidBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebingding(keyActions[Enum_KeyAction.Avoid], _entities[(int)Enum_UI_KeySetting.AvoidBtn].GetComponentInChildren<TMP_Text>());
        };*/


        playerInput = keyInteraction.PlayerInput;
    }
}
