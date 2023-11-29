using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Setting : UI_Entity
{
    SettingsData setting;

    enum Enum_UI_Settings
    {
        Panel,
        Interact,
        Panel_L,
        Panel_R,
        Scrollbar_L,
        Scrollbar_R,
        Close,
        Reset,
        Accept,
        Cancel
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Settings);
    }

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _subUIs.Count; i++)
        {
            Debug.Log(_subUIs[i].gameObject.name);
        }

        // 드래그 수정 필요
        _entities[(int)Enum_UI_Settings.Interact].DragAction = (PointerEventData data) => {
            transform.position = data.position;
        };

        // 버튼 할당
        _entities[(int)Enum_UI_Settings.Close].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };
        _entities[(int)Enum_UI_Settings.Reset].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };
        _entities[(int)Enum_UI_Settings.Accept].ClickAction = (PointerEventData data) => {

            GameManager.UI.ClosePopup(GameManager.UI.Setting); // 저장 후 창 닫기
        };
        _entities[(int)Enum_UI_Settings.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.Setting);
        };
    }

    void ResetOptions()
    {
    
    
    
    }
}
