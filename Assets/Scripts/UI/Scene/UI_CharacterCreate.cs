using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Google.Protobuf;

public class UI_CharacterCreate : UI_Entity
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
        GameManager.Data.characters[GameManager.Data.selectedSlotNum] = new CharData();
        character = GameManager.Data.characters[GameManager.Data.selectedSlotNum];

        josDescript = _entities[(int)Enum_UI_JobSelect.JobDescription].GetComponentInChildren<TMP_Text>();
        jobImage = _entities[(int)Enum_UI_JobSelect.Panel_L].GetComponent<Image>();
        _SetDefalut();

        // 팝업 열고 닫기
        _entities[(int)Enum_UI_JobSelect.Setting].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        // 버튼 선택에 맞게 이미지, 설명란 및 저장할 데이터 변경
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => {
            _SaveOptions(0);
        };
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => {
            _SaveOptions(1);
        };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => {
            _SaveOptions(2);
        };
        _entities[(int)Enum_UI_JobSelect.Men].ClickAction = (PointerEventData data) => { character.gender = true; };
        _entities[(int)Enum_UI_JobSelect.Women].ClickAction = (PointerEventData data) => { character.gender = false; };

        // 이름 생성 팝업 띄우기
        _entities[(int)Enum_UI_JobSelect.Select].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_JobSelect.GoBack].ClickAction = (PointerEventData data) => {
            GameManager.Scene.GetPreviousScene();
        };
    }

    void _SetDefalut()
    {
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/Warrior");
    }


    void _SaveOptions(int job)
    {
        character.job = job;
        string jobName = "Warrior";

        // 설명란 변경
        switch (job)
        {
            case 0:
                jobName = "Warrior";
                josDescript.text = $"Warriorss have high defense and health."; break;
            case 1:
                jobName = "Wizard";
                josDescript.text = $"Wizards deal powerful damage or help their teammates."; break;
            case 2:
                jobName = "Archer";
                josDescript.text = $"Archers can inflict lethal damage from long range."; break;
            default:
                break;
        }

        // 이미지 변경
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{jobName}");
    }

    public void SendCharacterPacket()
    {
        C_NEW_CHARACTER new_character_pkt = new C_NEW_CHARACTER();
        new_character_pkt.Character = new CHARACTER_BASE();
        new_character_pkt.Character.Gender = character.gender;
        new_character_pkt.Character.Job = character.job;
        new_character_pkt.Character.Nickname = ByteString.CopyFrom(character.charName, System.Text.Encoding.Unicode);
        new_character_pkt.Character.SlotNum = GameManager.Data.selectedSlotNum;
        new_character_pkt.Character.CharacterId = character.characterId; // 기본값 0 전송

        GameManager.Network.mainSession.Send(PacketHandler.Instance.SerializePacket(new_character_pkt));

        GameManager.UI.OpenPopup(GameManager.UI.BlockAll);
    }
}
