using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JobSelect : UI_Entity
{
    string jobName;
    TMP_Text josDesc;
    Image jobImage;

    string defaultJob = "Warrior";
    string defaultGender = "Men";

    enum Enum_UI_JobSelect
    {
        Setting = 0,
        Panel_L,
        Panel_R,
        Select,
        Warrior,
        Wizard,
        Archer,
        Men,
        Women,
        GoBack,
        JobDescription,
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_JobSelect);
    }


    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _subUIs.Count; i++)
        {
            Debug.Log(_subUIs[i].gameObject.name);
        }

        _entities[(int)Enum_UI_JobSelect.Setting].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        jobName = "";
        josDesc = _entities[(int)Enum_UI_JobSelect.JobDescription].GetComponentInChildren<TMP_Text>();
        jobImage = _entities[(int)Enum_UI_JobSelect.Panel_L].GetComponent<Image>();

        // ����,����,�̹��� �⺻ ����
        SaveOptions($"{defaultJob}");
        GameManager.Data.character.gender = $"{defaultGender}";

        // ��ư ���ÿ� �°� �̹���, ����� �� ������ ������ ����
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => {
            SaveOptions("Warrior");
        };
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => {
            SaveOptions("Wizard");
        };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => {
            SaveOptions("Archer");
        };
        _entities[(int)Enum_UI_JobSelect.Men].ClickAction = (PointerEventData data) => { GameManager.Data.character.gender = "Men"; };
        _entities[(int)Enum_UI_JobSelect.Women].ClickAction = (PointerEventData data) => { GameManager.Data.character.gender = "Women"; };

        // �̸� ���� �˾� ����
        _entities[(int)Enum_UI_JobSelect.Select].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_JobSelect.GoBack].ClickAction = (PointerEventData data) => {
            GameManager.Scene.GetPreviousScene();
        };
    }

    void SaveOptions(string jobName)
    {
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");
        GameManager.Data.character.job = $"{jobName}";

        switch (jobName)
        {
            case "Warrior":
                josDesc.text = $"{jobName}s have high defense and health."; break;
            case "Wizard":
                josDesc.text = $"{jobName}s deal powerful damage or help their teammates."; break;
            case "Archer":
                josDesc.text = $"{jobName}s can inflict lethal damage from long range."; break;
            default:
                break;
        }
    }
}
