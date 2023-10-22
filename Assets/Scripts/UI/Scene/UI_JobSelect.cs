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
        Panel
    }

    enum Enum_JobTypes
    {
        Warrior,
        Wizard,
        Archer,
    }

    int jobTypes = Enum.GetValues(typeof(Enum_JobTypes)).Length;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Enum_Buttons));
        Bind<TMP_Text>(typeof(Enum_Texts));
        Bind<Image>(typeof(Enum_Images));
        Bind<GameObject>(typeof(Enum_GameObjects));

        Image panel = GetImage((int)Enum_Images.Panel);

        for (int i = 0; i < jobTypes ; i++)
        {
            GameObject job = GameManager.Resources.Instantiate("Prefabs/UI/Scene/UI_JobName");
            job.transform.SetParent(panel.transform);

            UI_JobName jobName = Utils.GetOrAddComponent<UI_JobName>(job);
            jobName.SetInfo(Enum.GetName(typeof(Enum_JobTypes), i));
        }
    }

    public void OnButtonClicked(PointerEventData data)
    {
    }
}
