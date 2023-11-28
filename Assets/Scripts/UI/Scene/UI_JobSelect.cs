using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_JobSelect : UI_Entity
{
    string defaultJob = "Warrior";
    string defaultGender = "Men";

    enum Enum_UI_JobSelect
    {
        Panel_L = 0,
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

        GameManager.Data.CreateChar();
        string jobName = "";
        TMP_Text josDesc = _entities[(int)Enum_UI_JobSelect.JobDescription].GetComponent<TMP_Text>();
        Image jobImage = _entities[(int)Enum_UI_JobSelect.Panel_L].GetComponent<Image>();


        // ����,����,�̹��� �⺻ ����
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{defaultJob}"); // �̹��� �ε�
        josDesc.text = $"{defaultJob}s have high defense and health."; // ����� ����
        GameManager.Data.character.job = $"{defaultJob}"; // ������ ����
        GameManager.Data.character.gender = $"{defaultGender}";

        // ��ư ���ÿ� �°� �̹���, ����� �� ������ ������ ����
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => {
            jobName = "Warrior";
            jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");
            josDesc.text = $"{jobName}s have high defense and health.";
            GameManager.Data.character.job = $"{jobName}";
        };
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => {
            jobName = "Wizard";
            jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");
           josDesc.text = $"{jobName}s deal powerful damage or help their teammates.";
            GameManager.Data.character.job = $"{jobName}";
        };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => {
            jobName = "Archer";
            jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");
            josDesc.text = $"{jobName}s can inflict lethal damage from long range.";
            GameManager.Data.character.job = $"{jobName}";
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
}
