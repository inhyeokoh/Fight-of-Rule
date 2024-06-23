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
        OptionPanel,
        GoBack,
        Create,
        Settings
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_JobSelect);
    }

    protected override void Init()
    {
        base.Init();
        character = GameManager.Data.CurrentCharacter;

        _entities[(int)Enum_UI_JobSelect.Settings].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.OpenOrClose(GameManager.UI.Settings);
        };

        // 이름 생성 팝업 띄우기
        _entities[(int)Enum_UI_JobSelect.Create].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.InputName);
        };

        _entities[(int)Enum_UI_JobSelect.GoBack].ClickAction = (PointerEventData data) => {
            SceneController.instance.LoadPreviousScene();
        };
    }

/*
    void _SwitchImageAndDescription(Enum_Class className)
    {
        // 설명란 변경
        switch (className)
        {
            case Enum_Class.Warrior:
                josDescript.text = $"전사는 큰 방어력과 체력을 가지고 있습니다.";
                break;
            case Enum_Class.Wizard:
                josDescript.text = $"마법사는 적에게 큰 데미지를 줄 수 있거나 팀을 치유할 수 있습니다.";
                break;
            case Enum_Class.Archer:
                josDescript.text = $"궁수는 장거리에서도 치명적인 데미지를 줄 수 있습니다.";
                break;
            case Enum_Class.Default:
                josDescript.text = $"디폴트";
                break;
            default:
                break;
        }

        // 이미지 변경
        string strClassName = Enum.GetName(typeof(Enum_Class), className);
        jobImage.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{strClassName}");
    }

    void _SetBasedOnSelectedOption(Enum_Class className)
    {
        character.BaseInfo.Job = (int)className;
    }

    /// <summary>
    /// 성별 설정
    /// </summary>
    /// <param name="gender"> true = 남자, false = 여자 </param>
    void _SetBasedOnSelectedOption(bool gender)
    {
        character.BaseInfo.Gender = gender;
    }*/

    public void SendCharacterPacket()
    {
        C_NEW_CHARACTER new_character_pkt = new C_NEW_CHARACTER();
        new_character_pkt.Character = new CHARACTER_BASE();
        new_character_pkt.Character.Gender = character.BaseInfo.Gender;
        new_character_pkt.Character.CharacterClass = character.BaseInfo.CharacterClass;
        new_character_pkt.Character.Nickname = character.BaseInfo.Nickname;
        new_character_pkt.Character.SlotIndex = GameManager.Data.SelectedSlotNum;
        new_character_pkt.Character.CharacterId = character.BaseInfo.CharacterId; // 기본값 0 전송

        GameManager.Network.Send(PacketHandler.Instance.SerializePacket(new_character_pkt));
    }
}
