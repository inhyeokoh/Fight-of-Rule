using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Google.Protobuf;

public class UI_CharacterCreate : UI_Entity
{
    CHARACTER_INFO character;
    TMP_Text josDescript;
    Image jobImage;

    enum Enum_UI_JobSelect
    {
        Setting,
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
        GameManager.Data.CurrentCharacter = new CHARACTER_INFO();
        character = GameManager.Data.CurrentCharacter;

        josDescript = _entities[(int)Enum_UI_JobSelect.JobDescription].GetComponentInChildren<TMP_Text>();
        jobImage = _entities[(int)Enum_UI_JobSelect.Panel_L].GetComponent<Image>();
        _SetDefalutInfo();

        _entities[(int)Enum_UI_JobSelect.Setting].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Settings);
        };

        // 버튼 선택에 맞게 이미지, 설명란 및 저장할 데이터 변경
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => {
            _SaveOptions(Enum_Class.Warrior);
        };
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => {
            _SaveOptions(Enum_Class.Wizard);
        };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => {
            _SaveOptions(Enum_Class.Archer);
        };
        _entities[(int)Enum_UI_JobSelect.Men].ClickAction = (PointerEventData data) => {
            _SaveOptions(true);
        };
        _entities[(int)Enum_UI_JobSelect.Women].ClickAction = (PointerEventData data) => {
            _SaveOptions(false);
        };

        // 이름 생성 팝업 띄우기
        _entities[(int)Enum_UI_JobSelect.Select].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_JobSelect.GoBack].ClickAction = (PointerEventData data) => {
            GameManager.Scene.GetPreviousScene();
        };
    }

    void _SetDefalutInfo()
    {
        character.BaseInfo = new CHARACTER_BASE();
        character.BaseInfo.CharacterId = 0;
        character.BaseInfo.Nickname = ByteString.CopyFrom("기본 이름", System.Text.Encoding.Unicode);
        character.BaseInfo.Job = 0;
        character.BaseInfo.Gender = true;

        character.Stat = new CHARACTER_STATUS();
        character.Xyz = new CHARACTER_POS();

        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/Warrior");
    }


    string GetJobImageName(Enum_Class className)
    {
        switch (className)
        {
            case Enum_Class.Warrior:
                return "Warrior";
            case Enum_Class.Wizard:
                return "Wizard";
            case Enum_Class.Archer:
                return "Archer";
            default:
                return "Warrior";
        }
    }

    void _SaveOptions(Enum_Class className)
    {
        character.BaseInfo.Job = (int)className;

        // 설명란 변경
        switch (className)
        {
            case Enum_Class.Warrior:
                josDescript.text = $"Warriors have high defense and health.";
                break;
            case Enum_Class.Wizard:
                josDescript.text = $"Wizards deal powerful damage or help their teammates.";
                break;
            case Enum_Class.Archer:
                josDescript.text = $"Archers can inflict lethal damage from long range.";
                break;
            case Enum_Class.Default:
                josDescript.text = $"Warriors have high defense and health.";
                break;
            default:
                break;
        }

        // 이미지 변경
        string imageName = GetJobImageName(className);
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{imageName}");
    }

    void _SaveOptions(bool gender)
    {
        character.BaseInfo.Gender = gender;
    }

    public void SendCharacterPacket()
    {
        C_NEW_CHARACTER new_character_pkt = new C_NEW_CHARACTER();
        new_character_pkt.Character = new CHARACTER_BASE();
        new_character_pkt.Character.Gender = character.BaseInfo.Gender;
        new_character_pkt.Character.Job = character.BaseInfo.Job;
        new_character_pkt.Character.Nickname = character.BaseInfo.Nickname;
        new_character_pkt.Character.SlotNum = GameManager.Data.SelectedSlotNum;
        new_character_pkt.Character.CharacterId = character.BaseInfo.CharacterId; // 기본값 0 전송

        GameManager.Network.Send(PacketHandler.Instance.SerializePacket(new_character_pkt));
    }
}
