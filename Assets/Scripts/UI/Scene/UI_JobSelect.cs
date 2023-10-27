using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UI_JobSelect : UI_Scene
{
    enum Enum_Buttons
    {
    }

    enum Enum_Texts
    {
    }

    enum Enum_GameObjects
    {
    }

    enum Enum_Images
    {
        Panel,
        JobImage
    }

    enum Enum_JobTypes
    {
        Warrior,
        Wizard,
        Archer
    }

    int jobTypes = Enum.GetValues(typeof(Enum_JobTypes)).Length;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Enum_Images));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<GameObject>(typeof(Enum_GameObjects));
        Bind<Button>(typeof(Enum_Buttons));

        Image panel = GetImage((int)Enum_Images.Panel);

        // 패널 아래에 직업 종류별로 프리팹 생성

        for (int i = 0; i < jobTypes ; i++)
        {
            GameObject job = GameManager.Resources.Instantiate("Prefabs/UI/Scene/UI_JobName", panel.transform);
            UI_JobName jobName = Utils.GetOrAddComponent<UI_JobName>(job); // 스크립트 부착

            jobName.SetInfo(Enum.GetName(typeof(Enum_JobTypes), i)); // Enum 번호 순서대로 직업이름 등록
        }
    }

    public void OnButtonClicked(PointerEventData data)
    {
    }
}
