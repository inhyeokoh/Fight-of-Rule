using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterSelect : UI_Entity
{
    GameObject panel;
    int _totalSlot = 4;

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

        _DrawCharacterSlot();

        _entities[(int)Enum_UI_CharSelect.Setting].ClickAction = (PointerEventData data) => {
            GameManager.UI.OpenOrClose(GameManager.UI.Setting);
        };

        _entities[(int)Enum_UI_CharSelect.Start].ClickAction = (PointerEventData data) => {
            UI_Loading.LoadScene("StatePattern");
        };

        // 캐릭터 삭제
        _entities[(int)Enum_UI_CharSelect.Delete].ClickAction = (PointerEventData data) => {
            // 현재 선택 캐릭터 받기
            int selected = 0;
            Toggle[] toggles = panel.GetComponentsInChildren<Toggle>();
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn == true)
                {
                    selected = i;
                }    
            }
            // 서버에 삭제한 캐릭터 id(번호) 전송
            // GameManager.Data.characters[selected].characterId

            // 캐릭터 데이터 삭제
            GameManager.Data.characters[selected] = null;
        };
    }

    void _DrawCharacterSlot()
    {
        for (int i = 0; i < _totalSlot; i++)
        {
            GameObject slot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/CharacterSlot", panel.transform);
            slot.GetComponent<UI_Slot>().index = i;
            if (i == 0) // 기본으로 첫번째 캐릭터 선택 되어 있도록
            {
                slot.GetComponent<Toggle>().isOn = true;
            }
        }        
    }
}
