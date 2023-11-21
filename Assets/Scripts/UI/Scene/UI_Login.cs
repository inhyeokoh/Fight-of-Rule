using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Login : UI_Entity
{
    enum Enum_UI_Logins
    {
        MainText,
        TestLogin,
        TestExit,
        Image
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

        Debug.Log(_entities[(int)Enum_UI_Logins.Image].gameObject.name);
    }




}
