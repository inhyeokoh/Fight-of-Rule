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

        // TODO �����ϴ� �̸����� üũ
        // _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => { };

        // �̸� ����
        _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => {
            GameManager.Data.character.charName = _entities[(int)Enum_UI_InputName.InputField].GetComp<TMP_InputField>().text;
        };

        // ����ϴ� �̸��� ���Ϸ� ������ ���� �� ĳ���� ���þ����� �̵�
        _entities[(int)Enum_UI_InputName.Accept].ClickAction = (PointerEventData data) => {
            GameManager.Data.SaveData("slot0", GameManager.Data.character); SceneManager.LoadScene("Select");
        };
    }
}
