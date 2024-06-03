using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class UI_BlockAll : UI_Entity
{
    enum Enum_UI_BlockAll
    {
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_BlockAll);
    }

    protected override void Init()
    {
        base.Init();

        gameObject.SetActive(false);
    }
}
