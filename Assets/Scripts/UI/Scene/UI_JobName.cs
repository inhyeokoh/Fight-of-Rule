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
        JobNameText,
    }

    enum Enum_GameObjects
    {
    }

    enum Enum_Images
    {
    }

    string _name;
    string _jobDescription;

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

        GetText((int)Enum_Texts.JobNameText).text = _name; // ���� �̸� ����

        GetButton((int)Enum_Buttons.JobName).name = _name; // ��ư ������Ʈ�� ����

        Image jobimage = GameObject.Find("JobImage").GetComponent<Image>();
        TMP_Text jobDescript = GameObject.Find("JobDescription").GetComponent<TMP_Text>();

        // ��ư Ŭ���� ���� ���� �̹����� ������ �ε��� �� �ֵ��� ���
        GameObject jobName = GetButton((int)Enum_Buttons.JobName).gameObject;
        AddUIEvent(jobName, (PointerEventData data) => { 
            jobimage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{_name}"); // ���� �̹��� �ε�
            jobDescript.text = _jobDescription; // ���� ���� ����
        }, UI_Define.Enum_UIEvent.Click);
    }

    // ���� �̸� ��� (UI_JobSelect���� ȣ��)
    public void SetInfo(string name)
    {
        _name = name;

        switch (name)
        {
            case "Warrior":
                _jobDescription = " Warriors have high defense and are strong in close combat. "; break;
            case "Wizard":
                _jobDescription = " Wizards have low health, but can deal powerful damage or help their teammates.  "; break;
            case "Archer":
                _jobDescription = " Archers can inflict powerful damage from long distances. "; break;
            default:
                break;
        }
    }
}
