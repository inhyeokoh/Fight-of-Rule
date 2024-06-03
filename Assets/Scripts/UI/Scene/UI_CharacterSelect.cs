using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterSelect : UI_Entity
{
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

        _entities[(int)Enum_UI_CharSelect.Setting].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        _entities[(int)Enum_UI_CharSelect.Start].ClickAction = (PointerEventData data) => {
            UI_Loading.LoadScene("StatePattern");
        };

        // TODO 캐릭터 삭제
    }
}
