using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Slot : UI_Entity
{
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

        if (GameManager.Data.CheckData(gameObject.name)) // 오브젝트 명과 일치하는 데이터 파일이 있다면,
        {
            _entities[(int)Enum_UI_Slot.Create].gameObject.SetActive(false);

            // 오브젝트명과 동일한 이름을 가진 파일 로드
            GameManager.Data.character = JsonUtility.FromJson<CharData>(GameManager.Data.LoadData(gameObject.name));

            // 직업에 맞는 이미지 로드
            Image image = _entities[(int)Enum_UI_Slot.Image].GetComponent<Image>();
            image.sprite = GameManager.Resources.Load<Sprite>($"Materials/JobImage/{GameManager.Data.character.job}");

            // 해당 슬롯 텍스트 상자에 데이터 기입
            _entities[(int)Enum_UI_Slot.Label].GetComponent<TMP_Text>().text =
                $"{GameManager.Data.character.charName}\n {GameManager.Data.character.level}\n {GameManager.Data.character.job}\n {GameManager.Data.character.gender}\n";
        }
        else // 오브젝트 명과 일치하는 데이터 파일이 없다면,
        {
            GetComponent<Toggle>().group = null; // 토글 그룹에서 제외 (선택 불가능 하도록);
            _entities[(int)Enum_UI_Slot.Image].gameObject.SetActive(false);
            _entities[(int)Enum_UI_Slot.Label].gameObject.SetActive(false);
            // "슬롯이름+Create" 이름을 가진 캐릭터 생성버튼에 기능 부여
            _entities[(int)Enum_UI_Slot.Create].ClickAction = (PointerEventData data) => {
                GameManager.Data.fileName = gameObject.name;
                GameManager.Scene.GetPreviousScene();
            };
        }
    }
}
