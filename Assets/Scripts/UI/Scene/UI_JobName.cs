using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_JobName : UI_Scene
{
    enum Enum_Buttons
    {
        JobName
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
        Bind<Button>(typeof(Enum_Buttons));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        GetText((int)Enum_Texts.JobNameText).text = _name; // 직업 이름 설정

        GetButton((int)Enum_Buttons.JobName).name = _name; // 버튼 오브젝트명 변경

        Image jobimage = GameObject.Find("JobImage").GetComponent<Image>();

        // 버튼 클릭시 직업 별로 이미지 로드
        GameObject jobName = GetButton((int)Enum_Buttons.JobName).gameObject;
        AddUIEvent(jobName, (PointerEventData data) => { jobimage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{_name}"); }, UI_Define.Enum_UIEvent.Click);
    }

    // 직업 이름 등록 (UI_JobSelect에서 호출)
    public void SetInfo(string name)
    {
        _name = name;
    }
}
