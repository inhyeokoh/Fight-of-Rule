using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CharSelect : UI_Entity
{
    enum Enum_UI_CharSelect
    {
        Image,
        Slot0,
        Slot1,
        Slot2,
        Slot3
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_CharSelect);
    }


    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _subUIs.Count; i++)
        {
            Debug.Log(_subUIs[i].gameObject.name);
        }

        // 슬롯 데이터 로드 ( 현재는 좀 하드코딩 상태...)
        CharData slot0 = JsonUtility.FromJson<CharData>(GameManager.Data.LoadData("slot0"));
        _entities[(int)Enum_UI_CharSelect.Slot0].GetComponentInChildren<TMP_Text>().text = $"{slot0.charName}\n {slot0.level}\n {slot0.job}\n {slot0.gender}\n";
    }
}
