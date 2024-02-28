using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_InputName : UI_Entity
{
    // TODO : bool������ �ƴ� ��������,�ߺ��г���,�����Ұ��� ���̽��� ���������
    bool canCreate = true;
    string nickName = "";
    TMP_Text msg;

    enum Enum_UI_InputName
    {
        Image,
        InputField,
        Create,
        Accept,
        Cancel,
        CheckName
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

        msg = _entities[(int)Enum_UI_InputName.CheckName].GetComponentInChildren<TMP_Text>();

        // ���� ������ �̸����� üũ ��, ���� �� �� �̵�
        _entities[(int)Enum_UI_InputName.Create].ClickAction = (PointerEventData data) => {            
            if (canCreate)
            {
                msg.text = "This name can be created.";
                GameManager.Data.characters[GameManager.Data.selectedSlotNum].charName = nickName;
                _entities[(int)Enum_UI_InputName.Accept].ClickAction = (PointerEventData data) => {
                    GameManager.Data.login.slotCount++; // ���� �� ����
                    GameManager.Data.SaveData("LoginData", GameManager.Data.login);
                    GetDefaultStats();
                    // GameManager.Data.SaveData(GameManager.Data.fileName, GameManager.Data.character);
                    GameManager.Scene.GetNextScene();
                };
            }
            else
            {
                msg.text = "This name cannot be created.";
            }
        };

        _entities[(int)Enum_UI_InputName.Cancel].ClickAction = (PointerEventData data) => {
            GameManager.UI.ClosePopup(GameManager.UI.InputName);
        };
    }

    private void Update()
    {
        nickName = _entities[(int)Enum_UI_InputName.InputField].GetComp<TMP_InputField>().text;
        if (String.IsNullOrEmpty(nickName))
        {
            msg.text = "Please enter your name.";
        }
        else if (nickName.Length < 4 || nickName.Length > 12)
        {
            msg.text = "Please enter at least 4 characters and no more than 12 characters.";
        }
    }

    void GetDefaultStats() // �⺻ ���� �ο�
    {
        GameManager.Data.character.level = 1;
        GameManager.Data.character.maxHP = 50;
        GameManager.Data.character.maxMP = 50;
        GameManager.Data.character.maxEXP = 100;

        GameManager.Data.character.hp = 50;
        GameManager.Data.character.mp = 50;
        GameManager.Data.character.exp = 0;
        GameManager.Data.character.attack = 5;
        GameManager.Data.character.attackSpeed = 1;
        GameManager.Data.character.defense = 3;
        GameManager.Data.character.speed = 10;
        GameManager.Data.character.skillDamage = 1;
    }
}
