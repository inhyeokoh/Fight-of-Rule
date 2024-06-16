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
    UI_Slot[] characterSlots;

    enum Enum_UI_CharSelect
    {
        Setting,
        Panel,
        Start,
        Delete
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_CharSelect);
    }

    protected override void Init()
    {
        base.Init();

        panel = _entities[(int)Enum_UI_CharSelect.Panel].gameObject;

        characterSlots = new UI_Slot[_totalSlot];
        _DrawCharacterSlot();

        _entities[(int)Enum_UI_CharSelect.Setting].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        _entities[(int)Enum_UI_CharSelect.Start].ClickAction = (PointerEventData data) => {
            UI_Loading.LoadScene("StatePattern");
        };

        // 캐릭터 삭제
        _entities[(int)Enum_UI_CharSelect.Delete].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenPopup(GameManager.UI.ConfirmYN);
            GameManager.UI.ConfirmYN.ChangeText(UI_ConfirmYN.Enum_ConfirmTypes.AskDeleteCharacter);
        };
    }

    void _DrawCharacterSlot()
    {
        for (int i = 0; i < _totalSlot; i++)
        {
            GameObject slot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/CharacterSlot2", panel.transform);
            characterSlots[i] = slot.GetComponent<UI_Slot>();
            characterSlots[i].Index = i;
        }
    }
}
