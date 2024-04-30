using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DropConfirm : UI_Entity
{
    TMP_Text _mainText;
    int _slotIndex;

    enum Enum_UI_DropConfirm
    {
        MainText,
        Accept,
        Cancel
    }


    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_DropConfirm);
    }

    protected override void Init()
    {
        base.Init();

        _mainText = _entities[(int)Enum_UI_DropConfirm.MainText].transform.GetChild(0).GetComponent<TMP_Text>();

        // 입력한 수량에 맞게 버리기
        _entities[(int)Enum_UI_DropConfirm.Accept].ClickAction = (PointerEventData data) => {
            GameManager.Inven.DropItem(_slotIndex);
            transform.parent.gameObject.SetActive(false);
        };

        _entities[(int)Enum_UI_DropConfirm.Cancel].ClickAction = (PointerEventData data) => {
            transform.parent.gameObject.SetActive(false);
        };

        transform.parent.gameObject.SetActive(false);
    }

    public void ChangeText(int slotIndex)
    {
        _slotIndex = slotIndex;
        _mainText.text = $"{GameManager.Inven.items[slotIndex].name} 아이템을 버리시겠습니까?";
    }
}
