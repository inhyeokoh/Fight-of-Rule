using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JobSelect : UI_Entity
{
    CharData character;
    TMP_Text josDescript;
    Image jobImage;

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
        character = GameManager.Data.characters[GameManager.Data.selectedSlotNum];

        josDescript = _entities[(int)Enum_UI_JobSelect.JobDescription].GetComponentInChildren<TMP_Text>();
        jobImage = _entities[(int)Enum_UI_JobSelect.Panel_L].GetComponent<Image>();

        // 팝업 열고 닫기
        _entities[(int)Enum_UI_JobSelect.Setting].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        // 버튼 선택에 맞게 이미지, 설명란 및 저장할 데이터 변경
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => {
            SaveOptions("Warrior");
        };
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => {
            SaveOptions("Wizard");
        };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => {
            SaveOptions("Archer");
        };
        _entities[(int)Enum_UI_JobSelect.Men].ClickAction = (PointerEventData data) => { character.gender = "Men"; };
        _entities[(int)Enum_UI_JobSelect.Women].ClickAction = (PointerEventData data) => { character.gender = "Women"; };

        // 이름 생성 팝업 띄우기
        _entities[(int)Enum_UI_JobSelect.Select].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_JobSelect.GoBack].ClickAction = (PointerEventData data) => {
            GameManager.Scene.GetPreviousScene();
        };
    }

    void SaveOptions(string jobName)
    {
        character.job = jobName;

        // 이미지 변경
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");

        // 설명란 변경
        switch (jobName)
        {
            case "Warrior":
                josDescript.text = $"{jobName}s have high defense and health."; break;
            case "Wizard":
                josDescript.text = $"{jobName}s deal powerful damage or help their teammates."; break;
            case "Archer":
                josDescript.text = $"{jobName}s can inflict lethal damage from long range."; break;
            default:
                break;
        }
    }
}
