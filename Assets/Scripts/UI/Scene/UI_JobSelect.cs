using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_JobSelect : UI_Entity
{
    enum Enum_UI_JobSelect
    {
        Panel_L = 0,
        Panel_R,
        Select,
        Warrior,
        Wizard,
        Archer,
        Men,
        Women
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
        _entities[(int)Enum_UI_JobSelect.Warrior].ClickAction = (PointerEventData data) => { GameManager.Data.character.job = "Warrior";};
        _entities[(int)Enum_UI_JobSelect.Wizard].ClickAction = (PointerEventData data) => { GameManager.Data.character.job = "Wizard"; };
        _entities[(int)Enum_UI_JobSelect.Archer].ClickAction = (PointerEventData data) => { GameManager.Data.character.job = "Archer"; };
        _entities[(int)Enum_UI_JobSelect.Men].ClickAction = (PointerEventData data) => { GameManager.Data.character.gender = "Men"; };
        _entities[(int)Enum_UI_JobSelect.Women].ClickAction = (PointerEventData data) => { GameManager.Data.character.gender = "Women"; };

        // 이름 생성
        _entities[(int)Enum_UI_JobSelect.Select].ClickAction = (PointerEventData data) => {
            GameObject inputName = GameManager.Resources.Instantiate($"Prefabs/UI/Popup/InputName", transform.parent);
             GameManager.UI._activePopupList.AddFirst(inputName);
        };
    }
}
