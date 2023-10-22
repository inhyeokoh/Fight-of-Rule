using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class UI_JobName : UI_Scene
{
    enum Enum_Buttons
    {
    }

    enum Enum_Texts
    {
        JobNameText
    }

    enum Enum_GameObjects
    {

    }

    enum Enum_Images
    {
    }

    string _name;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<TMP_Text>(typeof(Enum_Texts));

        GetText((int)Enum_Texts.JobNameText).text = _name;
    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
