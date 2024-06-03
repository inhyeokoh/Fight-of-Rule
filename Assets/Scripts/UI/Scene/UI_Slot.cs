using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : UI_Entity
{
    public int index;
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

        character = GameManager.Data.characters[index];

        if (character != null) // 캐릭터 정보 존재시
        {
            string gender = character.gender ? "Men" : "Women";
            string job = Enum.GetName(typeof(CharData.Enum_Job), character.job);

            // 직업에 맞는 이미지 로드
            Image image = _entities[(int)Enum_UI_Slot.Image].GetComponent<Image>();
            image.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{job}");

            // 해당 슬롯 텍스트 상자에 데이터 기입

            _entities[(int)Enum_UI_Slot.Label].GetComponent<TMP_Text>().text =
                $"캐릭터명 : {character.charName}\n 레벨: {character.level}\n 직업: {job}\n 성별: {gender}\n";

            _entities[(int)Enum_UI_Slot.Background].ClickAction = (PointerEventData data) => {
                GetComponent<Toggle>().isOn = true;
                GameManager.Data.selectedSlotNum = index;
            };

            _entities[(int)Enum_UI_Slot.Create].gameObject.SetActive(false); // 캐릭터 생성 버튼 비활성화 
        }
        else
        {
            _SetEmptySlot();
        }
    }

    void _SetEmptySlot()
    {
        GetComponent<Toggle>().group = null; // 선택 불가능 하도록 토글 그룹에서 제외
        _entities[(int)Enum_UI_Slot.Image].gameObject.SetActive(false);
        _entities[(int)Enum_UI_Slot.Label].gameObject.SetActive(false);

        // 캐릭터 생성버튼에 기능 부여
        _entities[(int)Enum_UI_Slot.Create].ClickAction = (PointerEventData data) => {
            GameManager.Data.selectedSlotNum = index;
            GameManager.Scene.LoadScene("Create");
        };
    }
}
