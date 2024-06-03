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
    string gender;
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

        if (character != null) // 오브젝트 명과 일치하는 데이터 파일이 있다면,
        {
            _entities[(int)Enum_UI_Slot.Create].gameObject.SetActive(false);

            // 직업에 맞는 이미지 로드
            Image image = _entities[(int)Enum_UI_Slot.Image].GetComponent<Image>();
            image.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{character.job}");


            // 해당 슬롯 텍스트 상자에 데이터 기입
            string gender = character.gender ? "Men" : "Women";
            string job = Enum.GetName(typeof(CharData.Enum_Job), character.job);

            _entities[(int)Enum_UI_Slot.Label].GetComponent<TMP_Text>().text =
                $"{character.charName}\n {character.level}\n {job}\n {gender}\n";

            _entities[(int)Enum_UI_Slot.Background].ClickAction = (PointerEventData data) => {
                GetComponent<Toggle>().isOn = true;
                GameManager.Data.selectedSlotNum = slotNum;
            };
        }
        else
        {
            SetEmpty();
        }
    }

    void SetEmpty()
    {
        GetComponent<Toggle>().group = null; // 토글 그룹에서 제외 (선택 불가능 하도록);
        _entities[(int)Enum_UI_Slot.Image].gameObject.SetActive(false);
        _entities[(int)Enum_UI_Slot.Label].gameObject.SetActive(false);
        // "슬롯이름+Create" 이름을 가진 캐릭터 생성버튼에 기능 부여
        _entities[(int)Enum_UI_Slot.Create].ClickAction = (PointerEventData data) => {
            GameManager.Data.selectedSlotNum = slotNum;
            GameManager.Scene.GetPreviousScene();
        };
    }
}
