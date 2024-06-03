using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_PlayerInfo : UI_Entity
{
    bool _init;
    public GameObject dragImg;
    public GameObject descrPanel;
    GameObject _infoBoard;
    GameObject _statusBoard;
    GameObject equipSlots;

    int _leftSlotCount = 5;

    public Rect panelRect;
    Vector2 _descrUISize;

    // 드래그 Field
    Vector2 _playerInfoUIPos;
    Vector2 _dragBeginPos;
    Vector2 _offset;

    string levelText;
    string expText;
    string hpText;
    string mpText;
    string attackText;
    string atkspeedText;
    string defenseText;
    string moveSpeedText;

    enum Enum_UI_PlayerInfo
    {
        Interact,
        Panel,
        Panel_U,
        Equipments,
        InfoPanel,
        Close,
        DragImg,
        DescrPanel,
        DropConfirm,
        DropCountConfirm
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_PlayerInfo);
    }

    private void OnEnable()
    {
        if (!_init) return;

        for (int i = 0; i < GameManager.Inven.equips.Count; i++)
        {
            UpdateEquipUI(i);

        }
        UpdateStatus();
    }

    private void OnDisable()
    {
        GameManager.UI.PointerOnUI(false);
    }

    private void Update()
    {
        UpdateStatus();
    }

    protected override void Init()
    {
        base.Init();
        equipSlots = _entities[(int)Enum_UI_PlayerInfo.Equipments].gameObject;
        dragImg = _entities[(int)Enum_UI_PlayerInfo.DragImg].gameObject;
        descrPanel = _entities[(int)Enum_UI_PlayerInfo.DescrPanel].gameObject;
        _infoBoard = _entities[(int)Enum_UI_PlayerInfo.InfoPanel].transform.GetChild(1).gameObject;
        _statusBoard = _entities[(int)Enum_UI_PlayerInfo.InfoPanel].transform.GetChild(3).gameObject;
        panelRect = _entities[(int)Enum_UI_PlayerInfo.Panel].GetComponent<RectTransform>().rect;
        _descrUISize = _GetUISize(descrPanel);
        _DrawSlots();
        _DrawCharacterInfo();

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.PlayerInfo);
            };

            // UI위에 커서 있을 시 캐릭터 행동 제약
            _subUI.PointerEnterAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(true);
            };

            _subUI.PointerExitAction = (PointerEventData data) =>
            {
                GameManager.UI.PointerOnUI(false);
            };
        }

        // 유저 정보 창 드래그 시작
        _entities[(int)Enum_UI_PlayerInfo.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _playerInfoUIPos = transform.position;
            _dragBeginPos = data.position;
        };

        // 유저 정보 창 드래그
        _entities[(int)Enum_UI_PlayerInfo.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _playerInfoUIPos + _offset;
        };

        // 유저 정보 창 닫기
        _entities[(int)Enum_UI_PlayerInfo.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopupAndChildren(GameManager.UI.Inventory); // 테스트
            // GameManager.UI.ClosePopup(GameManager.UI.PlayerInfo);            
        };

        _init = true;
    }

    // 유저 정보 창 내 초기 장비 슬롯 생성
    void _DrawSlots()
    {
        for (int i = 0; i < _leftSlotCount; i++)
        {
            GameObject _equipSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/EquipSlot", equipSlots.transform.GetChild(1));
            _equipSlot.name = "EquipSlot_" + i;
            _equipSlot.GetComponent<UI_EquipSlot>().Index = i;
        }

        for (int i = _leftSlotCount; i < GameManager.Inven.EquipSlotCount; i++)
        {
            GameObject _equipSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/EquipSlot", equipSlots.transform.GetChild(2));
            _equipSlot.name = "EquipSlot_" + i;
            _equipSlot.GetComponent<UI_EquipSlot>().Index = i;
        }
    }

    // 유저 정보 창 내 정보 및 스탯 표기
    void _DrawCharacterInfo()
    {        
        CharData character = GameManager.Data.characters[GameManager.Data.selectedSlotNum];
        for (int i = 0; i < 5; i++)
        {
            GameManager.Resources.Instantiate("Prefabs/UI/Scene/Status", _infoBoard.transform);
        }

        GameObject name = _infoBoard.transform.GetChild(0).gameObject;
        name.transform.GetChild(0).GetComponent<TMP_Text>().text = "캐릭터명";
        name.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{character.charName}";

        GameObject job = _infoBoard.transform.GetChild(1).gameObject;
        job.transform.GetChild(0).GetComponent<TMP_Text>().text = "직업";
        string strJob = Enum.GetName(typeof(CharData.Enum_Job), character.job);
        job.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{strJob}";

        GameObject gender = _infoBoard.transform.GetChild(2).gameObject;
        gender.transform.GetChild(0).GetComponent<TMP_Text>().text = "성별";
        string strGender = character.gender ? "Men" : "Women";
        gender.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{strGender}";

        GameObject level = _infoBoard.transform.GetChild(3).gameObject;
        level.transform.GetChild(0).GetComponent<TMP_Text>().text = "레벨";
        levelText = level.transform.GetChild(1).GetComponent<TMP_Text>().text;
        levelText = $"{character.level}";

        GameObject exp = _infoBoard.transform.GetChild(4).gameObject;
        exp.transform.GetChild(0).GetComponent<TMP_Text>().text = "경험치/최대 경험치";
        expText = exp.transform.GetChild(1).GetComponent<TMP_Text>().text;
        expText = $"{character.exp}/{character.maxEXP}";

        for (int i = 0; i < 6; i++)
        {
            GameManager.Resources.Instantiate("Prefabs/UI/Scene/Status", _statusBoard.transform);
        }

        GameObject hp = _statusBoard.transform.GetChild(0).gameObject;
        hp.transform.GetChild(0).GetComponent<TMP_Text>().text = "HP";
        hpText = hp.transform.GetChild(1).GetComponent<TMP_Text>().text;
        hpText = $"{character.hp}/{character.maxHP}";

        GameObject mp = _statusBoard.transform.GetChild(1).gameObject;
        mp.transform.GetChild(0).GetComponent<TMP_Text>().text = "MP";
        mpText = mp.transform.GetChild(1).GetComponent<TMP_Text>().text;
        mpText = $"{character.mp}/{character.maxMP}";

        GameObject attack = _statusBoard.transform.GetChild(2).gameObject;
        attack.transform.GetChild(0).GetComponent<TMP_Text>().text = "공격력";
        attackText = attack.transform.GetChild(1).GetComponent<TMP_Text>().text;
        attackText = $"{character.attack}";

        GameObject atkSpeed = _statusBoard.transform.GetChild(3).gameObject;
        atkSpeed.transform.GetChild(0).GetComponent<TMP_Text>().text = "공격 속도";
        atkspeedText = atkSpeed.transform.GetChild(1).GetComponent<TMP_Text>().text;
        atkspeedText = $"{character.attackSpeed}";

        GameObject defense = _statusBoard.transform.GetChild(4).gameObject;
        defense.transform.GetChild(0).GetComponent<TMP_Text>().text = "방어력";
        defenseText = defense.transform.GetChild(1).GetComponent<TMP_Text>().text;
        defenseText = $"{character.defense}";

        GameObject moveSpeed = _statusBoard.transform.GetChild(5).gameObject;
        moveSpeed.transform.GetChild(0).GetComponent<TMP_Text>().text = "이동 속도";
        moveSpeedText = moveSpeed.transform.GetChild(1).GetComponent<TMP_Text>().text;
        moveSpeedText = $"{character.speed}";
    }

    public void UpdateStatus()
    {
        CharacterStatus status = PlayerController.instance._playerStat;

        GameObject level = _infoBoard.transform.GetChild(3).gameObject;
        level.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.level}";

        GameObject exp = _infoBoard.transform.GetChild(4).gameObject;
        exp.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.exp}/{status.MaxEXP}";

        GameObject hp = _statusBoard.transform.GetChild(0).gameObject;
        hp.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.hp}/{status.sumMaxHP}";

        GameObject mp = _statusBoard.transform.GetChild(1).gameObject;
        mp.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.mp}/{status.sumMaxMP}";

        GameObject attack = _statusBoard.transform.GetChild(2).gameObject;
        attack.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.sumAttack}";

        GameObject atkSpeed = _statusBoard.transform.GetChild(3).gameObject;
        atkSpeed.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.sumAttackSpeed}";

        GameObject defense = _statusBoard.transform.GetChild(4).gameObject;
        defense.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.sumDefense}";

        GameObject moveSpeed = _statusBoard.transform.GetChild(5).gameObject;
        moveSpeed.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{status.sumSpeed}";

        /*        levelText = $"{status.level}";
                expText = $"{status.exp}/{status.MaxEXP}";
                hpText = $"{status.hp}/{status.sumMaxHP}";
                mpText = $"{status.mp}/{status.sumMaxMP}";
                attackText = $"{status.sumAttack}";
                atkspeedText = $"{status.sumAttackSpeed}";
                defenseText = $"{status.sumDefense}";
                moveSpeedText = $"{status.sumSpeed}";*/
    }

    public void RestrictItemDescrPos()
    {
        Vector2 option = new Vector2(300f, -165f);
        StartCoroutine(RestrictUIPos(descrPanel, _descrUISize, option));
    }

    public void StopRestrictItemDescrPos(PointerEventData data)
    {
        StopCoroutine(RestrictUIPos(descrPanel, _descrUISize));
    }

    // UI 사각형 좌표의 좌측하단과 우측상단 좌표를 전역 좌표로 바꿔서 사이즈 계산
    Vector2 _GetUISize(GameObject UI)
    {
        Vector2 leftBottom = UI.transform.TransformPoint(UI.GetComponent<RectTransform>().rect.min);
        Vector2 rightTop = UI.transform.TransformPoint(UI.GetComponent<RectTransform>().rect.max);
        Vector2 UISize = rightTop - leftBottom;
        return UISize;
    }

    // UI가 화면 밖으로 넘어가지 않도록 위치 제한
    IEnumerator RestrictUIPos(GameObject UI, Vector2 UISize, Vector2? option = null)
    {
        while (true)
        {
            Vector3 mousePos = Input.mousePosition;
            float x = Math.Clamp(mousePos.x + option.Value.x, UISize.x / 2, Screen.width - (UISize.x / 2));
            float y = Math.Clamp(mousePos.y + option.Value.y, UISize.y / 2, Screen.height - (UISize.y / 2));
            UI.transform.position = new Vector2(x, y);
            yield return null;
        }
    }

    // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    public void UpdateEquipUI(int slotIndex)
    {
        if (slotIndex < _leftSlotCount)
        {
            UI_EquipSlot equipSlot = equipSlots.transform.GetChild(1).GetChild(slotIndex).GetComponent<UI_EquipSlot>();
            equipSlot.ItemRender();
        }
        else
        {
            UI_EquipSlot equipSlot = equipSlots.transform.GetChild(2).GetChild(slotIndex - _leftSlotCount).GetComponent<UI_EquipSlot>();
            equipSlot.ItemRender();
        }
    }

    public bool CheckUIOutDrop()
    {
        if (dragImg.transform.localPosition.x < panelRect.xMin || dragImg.transform.localPosition.y < panelRect.yMin ||
            dragImg.transform.localPosition.x > panelRect.xMax || dragImg.transform.localPosition.y > panelRect.yMax)
        {
            return true;
        }

        return false;
    }
}
