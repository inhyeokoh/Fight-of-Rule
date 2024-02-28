using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : UI_Entity
{
    int slotNum;
    CharData character;

    enum Enum_UI_Slot
    {
        Image,
        Background,
        Label,
        Create
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Slot);
    }

    protected override void Init()
    {
        base.Init();

        slotNum = Convert.ToInt32(gameObject.name);
        character = GameManager.Data.characters[slotNum];

        if (character != null) // ������Ʈ ��� ��ġ�ϴ� ������ ������ �ִٸ�,
        {
            _entities[(int)Enum_UI_Slot.Create].gameObject.SetActive(false);

            // ������ �´� �̹��� �ε�
            Image image = _entities[(int)Enum_UI_Slot.Image].GetComponent<Image>();
            image.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{character.job}");


            // �ش� ���� �ؽ�Ʈ ���ڿ� ������ ����
            _entities[(int)Enum_UI_Slot.Label].GetComponent<TMP_Text>().text =
                $"{character.charName}\n {character.level}\n {character.job}\n {character.gender}\n";

            _entities[(int)Enum_UI_Slot.Background].ClickAction = (PointerEventData data) => {
                GetComponent<Toggle>().isOn = true;
                GameManager.Data.selectedSlotNum = slotNum;
            };
        }
        else
        {
            SetEmpty();
        }

/*        if (GameManager.Data.CheckData(gameObject.name)) // ������Ʈ ��� ��ġ�ϴ� ������ ������ �ִٸ�,
        {
            _entities[(int)Enum_UI_Slot.Create].gameObject.SetActive(false);

            // ������Ʈ��� ������ �̸��� ���� ���� �ε�
            GameManager.Data.character = JsonUtility.FromJson<CharData>(GameManager.Data.LoadData(gameObject.name));

            // ������ �´� �̹��� �ε�
            Image image = _entities[(int)Enum_UI_Slot.Image].GetComponent<Image>();
            image.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{GameManager.Data.character.job}");

            _entities[(int)Enum_UI_Slot.Background].ClickAction = (PointerEventData data) => {
                GetComponent<Toggle>().isOn = true;
                GameManager.Data.character = JsonUtility.FromJson<CharData>(GameManager.Data.LoadData(gameObject.name)); // �ش� ĳ���� ������ ����
            };

            // �ش� ���� �ؽ�Ʈ ���ڿ� ������ ����
            _entities[(int)Enum_UI_Slot.Label].GetComponent<TMP_Text>().text =
                $"{GameManager.Data.character.charName}\n {GameManager.Data.character.level}\n {GameManager.Data.character.job}\n {GameManager.Data.character.gender}\n";
        }
        else // ������Ʈ ��� ��ġ�ϴ� ������ ������ ���ٸ�,
        {
            SetSlotEmpty();
        }*/
    }

    void SetEmpty()
    {
        GetComponent<Toggle>().group = null; // ��� �׷쿡�� ���� (���� �Ұ��� �ϵ���);
        _entities[(int)Enum_UI_Slot.Image].gameObject.SetActive(false);
        _entities[(int)Enum_UI_Slot.Label].gameObject.SetActive(false);
        // "�����̸�+Create" �̸��� ���� ĳ���� ������ư�� ��� �ο�
        _entities[(int)Enum_UI_Slot.Create].ClickAction = (PointerEventData data) => {
            GameManager.Data.selectedSlotNum = slotNum;
            GameManager.Scene.GetPreviousScene();
        };
    }
}
