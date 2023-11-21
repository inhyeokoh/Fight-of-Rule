using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_InputName : UI_Entity
{
    enum Enum_UI_InputName
    {
        Image,
        InputField,
        Create,
        Accept,
        Cancel
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_InputName);
    }


    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < _subUIs.Count; i++)
        {
            Debug.Log(_subUIs[i].gameObject.name);
        }

        // TODO 존재하는 이름인지 체크
        // _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => { };

        // 이름 저장
        _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => {
            GameManager.Data.character.charName = _entities[(int)Enum_UI_InputName.InputField].GetComp<TMP_InputField>().text;
        };

        // 희망하는 이름의 파일로 데이터 저장 후 캐릭터 선택씬으로 이동
        _entities[(int)Enum_UI_InputName.Accept].ClickAction = (PointerEventData data) => {
            GameManager.Data.SaveData("slot0", GameManager.Data.character); SceneManager.LoadScene("Select");
        };
    }
}
