using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Button : UI_Popup
{
    enum Enum_Buttons
    {
        PointButton,
    }

    enum Enum_Texts
    {
        PointText,
        ScoreText,
    }

    enum Enum_GameObjects
    {
        TestObject,
    }

    enum Enum_Images
    {
        ItemIcon,
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<Text>(typeof(Enum_Texts));
        Bind<GameObject>(typeof(Enum_GameObjects));
        Bind<Image>(typeof(Enum_Images));

        GameObject go = GetImage((int)Enum_Images.ItemIcon).gameObject;
        AddUIEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, UI_Define.Enum_UIEvent.Drag);
    }


    int score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        score++;
        GetText((int)Enum_Texts.ScoreText).text = $"Score : {score}";    
    }
}
