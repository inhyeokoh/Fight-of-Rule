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
        //Action UI
        SKill1Btn,
        SKill2Btn,
        SKill3Btn,
        SKill4Btn,
        AvoidBtn,
        InventoryBtn,
        PlayerInfoBtn,
        SkillWindowBtn,
        ///////////////////////////

        KeyResetBtn,
               
         
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
    Dictionary<InputAction, TMP_Text> keyTexts;   

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_KeySetting);
    }
    protected override void Init()
    {
        base.Init();

        keyInteraction = PlayerController.instance.gameObject.GetComponent<KeyInteraction>();
        keyTexts = new Dictionary<InputAction, TMP_Text>();

        for (int i = 0; i < keyInteraction.InputActions.Length; i++)
        {  
            keyTexts.Add(keyInteraction.InputActions[i].action, _entities[i].GetComponentInChildren<TMP_Text>());
        }

        _entities[(int)Enum_UI_KeySetting.SKill1Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.SKill1, _entities[(int)Enum_UI_KeySetting.SKill1Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill2Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.Skill2, _entities[(int)Enum_UI_KeySetting.SKill2Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill3Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.Skill3, _entities[(int)Enum_UI_KeySetting.SKill3Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.SKill4Btn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.Skill4, _entities[(int)Enum_UI_KeySetting.SKill4Btn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.AvoidBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.Avoid, _entities[(int)Enum_UI_KeySetting.AvoidBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.PlayerInfoBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.UIPlayerInfo, _entities[(int)Enum_UI_KeySetting.PlayerInfoBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };
        _entities[(int)Enum_UI_KeySetting.InventoryBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.UIInven, _entities[(int)Enum_UI_KeySetting.InventoryBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };

        _entities[(int)Enum_UI_KeySetting.SkillWindowBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.StartRebinding(Enum_KeyAction.UISkillWindow, _entities[(int)Enum_UI_KeySetting.SkillWindowBtn].GetComponentInChildren<TMP_Text>(), keyTexts);
        };

        _entities[(int)Enum_UI_KeySetting.KeyResetBtn].ClickAction = (PointerEventData data) =>
        {
            keyInteraction.KeyReset(keyTexts);
        };

        keyInteraction.KeyUISetting(keyTexts);
    }

    private void OnEnable()
    {
        if (keyTexts != null)
        {
            keyInteraction.KeyUISetting(keyTexts);
        }
    }
}
