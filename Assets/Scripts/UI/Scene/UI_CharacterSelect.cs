using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_CharacterSelect : UI_Entity
{
    GameObject panel;
    int _totalSlot = 4;

    enum Enum_UI_CharSelect
    {
        Panel,
        Start,
        Delete,
        Settings,
        GoBack
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_CharSelect);
    }

    protected override void Init()
    {
        base.Init();

        panel = _entities[(int)Enum_UI_CharSelect.Panel].gameObject;
        _DrawCharacterSlot();

        _entities[(int)Enum_UI_CharSelect.Settings].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Settings);
        };

        _entities[(int)Enum_UI_CharSelect.Start].ClickAction = (PointerEventData data) => {
            if (GameManager.Data.CurrentCharacter == null) return;

            C_INGAME load_ingame_pkt = new C_INGAME();
            load_ingame_pkt.CharacterId = GameManager.Data.CurrentCharacter.BaseInfo.CharacterId;
            GameManager.Network.Send(PacketHandler.Instance.SerializePacket(load_ingame_pkt));
            Debug.Log(load_ingame_pkt.CharacterId);

            UI_Loading.LoadScene("StatePattern");
        };

        // 캐릭터 삭제
        _entities[(int)Enum_UI_CharSelect.Delete].ClickAction = (PointerEventData data) => {
            GameManager.UI.ConfirmYN.ChangeText(UI_ConfirmYN.Enum_ConfirmTypes.AskDeleteCharacter);
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmYN);
        };

        _entities[(int)Enum_UI_CharSelect.GoBack].ClickAction = (PointerEventData data) => {
            GameManager.Scene.LoadPreviousScene();
        };
    }

    void _DrawCharacterSlot()
    {
        for (int i = 0; i < _totalSlot; i++)
        {
            GameObject slot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/CharacterSlot", panel.transform);
            slot.GetComponent<UI_CharacterSlot>().Index = i;
        }
    }
}
