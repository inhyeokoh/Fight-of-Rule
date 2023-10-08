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
    }

    enum Enum_Images
    {
        ItemIcon,
    }

    private void Start()
    {
        Init();
    }

    // ���۽ÿ� enum ���� UI_Base ��ũ��Ʈ�� Bind �Լ��� �̿��Ͽ� ������ �޾ƿ�
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        // Text ���� ��� ����
        GetText((int)Enum_Texts.ScoreText).text = "Test";

        // Drag ��� ����
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
