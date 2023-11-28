using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Login : UI_Entity
{
    enum Enum_UI_Logins
    {
        Panel,
        IDField,
        PWField,
        Text,
        Login,
        Logout,
        Setting
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Logins);
    }

    protected override void Init()
    {
        base.Init();

        for(int i = 0; i < _subUIs.Count; i++)
        {
            Debug.Log(_subUIs[i].gameObject.name);
        }

        _entities[(int)Enum_UI_Logins.Setting].ClickAction = (PointerEventData data) => {
            GameObject setting = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/Setting", transform.parent);
            GameManager.UI._activePopupList.AddFirst(setting);
        };





    }
}
