using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Entity
{
    GameObject _content;
    public GameObject goldPanel;
    public GameObject dragImg;
    public GameObject descrPanel;
    public GameObject closeBtn;

    public Rect panelRect;
    Vector2 _descrUISize;

    UI_ItemSlot[] _cachedItemSlots;
    Toggle[] itemTypeToggles;
    int itemTypesCount;

    List<ItemData> _items;

    // 드래그 Field
    Vector2 _invenPos;
    Vector2 _dragBeginPos;
    Vector2 _offset;

    enum Enum_UI_Inventory
    {
        Interact,
        Panel,
        Panel_U,
        Panel_D,
        Sort,
        Expansion,
        ScrollView,
        TempAdd,
        Gold,
        Close,
        DragImg,
        DescrPanel,
    }

    enum Enum_ItemTypeInKorean
    {
        전체,
        장비,
        소비,
        기타
    }

    protected override Type GetUINamesAsType()
    {
        return typeof(Enum_UI_Inventory);
    }

    private void OnDisable()
    {
        GameManager.UI.PointerOnUI(false);
    }

    protected override void Init()
    {
        base.Init();
        _content = _entities[(int)Enum_UI_Inventory.ScrollView].transform.GetChild(0).GetChild(0).gameObject; // Content 담기는 오브젝트
        panelRect = _entities[(int)Enum_UI_Inventory.Panel].GetComponent<RectTransform>().rect;
        goldPanel = _entities[(int)Enum_UI_Inventory.Gold].gameObject;
        dragImg = _entities[(int)Enum_UI_Inventory.DragImg].gameObject;
        descrPanel = _entities[(int)Enum_UI_Inventory.DescrPanel].gameObject;
        closeBtn = _entities[(int)Enum_UI_Inventory.Close].gameObject;
        _descrUISize = _GetUISize(descrPanel);

        itemTypesCount = Enum.GetValues(typeof(Enum_ItemTypeInKorean)).Length;
        _items = GameManager.Inven.items;

        _DrawSlots();
        _SetPanel_U();
        UpdateGoldPanel(GameManager.Inven.Gold);

        foreach (var _subUI in _subUIs)
        {
            _subUI.ClickAction = (PointerEventData data) =>
            {
                GameManager.UI.GetPopupForward(GameManager.UI.Inventory);
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

        // 인벤토리 창 드래그 시작
        _entities[(int)Enum_UI_Inventory.Interact].BeginDragAction = (PointerEventData data) =>
        {
            _invenPos = transform.position;
            _dragBeginPos = data.position;
        };

        // 인벤토리 창 드래그
        _entities[(int)Enum_UI_Inventory.Interact].DragAction = (PointerEventData data) =>
        {
            _offset = data.position - _dragBeginPos;
            transform.position = _invenPos + _offset;
        };

        // 인벤토리 정렬
        _entities[(int)Enum_UI_Inventory.Sort].ClickAction = (PointerEventData data) =>
        {
            GameManager.Inven.SortItems();
        };

        // 인벤토리 확장
        _entities[(int)Enum_UI_Inventory.Expansion].ClickAction = (PointerEventData data) =>
        {
            _ExpandSlot();
        };

        // 아이템 획득 - 임시
        _entities[(int)Enum_UI_Inventory.TempAdd].ClickAction = (PointerEventData data) =>
        {
            _PressGetItem();
            GameManager.UI.OpenPopup(GameManager.UI.PlayerInfo);
        };

        // 인벤토리 닫기
        _entities[(int)Enum_UI_Inventory.Close].ClickAction = (PointerEventData data) =>
        {
            GameManager.UI.ClosePopup(GameManager.UI.Inventory);
        };

        gameObject.SetActive(false);
    }

    // 인벤토리 내 초기 슬롯 생성
    void _DrawSlots()
    {
        _cachedItemSlots = new UI_ItemSlot[_items.Count];
        for (int i = 0; i < GameManager.Inven.TotalSlotCount; i++)
        {
            _cachedItemSlots[i] = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _content.transform).GetComponent<UI_ItemSlot>();
            _cachedItemSlots[i].Index = i;
        }
    }

    void _SetPanel_U()
    {
        itemTypeToggles = new Toggle[itemTypesCount];
        for (int i = 0; i < itemTypesCount; i++)
        {
            itemTypeToggles[i] = GameManager.Resources.Instantiate("Prefabs/UI/Scene/InvenItemTypeToggle", _entities[(int)Enum_UI_Inventory.Panel_U].transform).GetComponent<Toggle>();

            TMP_Text itemTypeToggleName = itemTypeToggles[i].GetComponentInChildren<TMP_Text>();
            itemTypeToggleName.text = Enum.GetName(typeof(Enum_ItemTypeInKorean), i);

            itemTypeToggles[i].isOn = false;
            itemTypeToggles[i].group = _entities[(int)Enum_UI_Inventory.Panel_U].transform.GetComponent<ToggleGroup>();
        }
    }

    public void AddListenerToItemTypeToggle()
    {
        itemTypeToggles[0].onValueChanged.AddListener((value) => _ToggleValueChanged(value, (int)Enum_ItemTypeInKorean.전체));
        for (int i = 1; i < itemTypesCount; i++) // 전체보기를 제외한 분류
        {
            int index = i;
            itemTypeToggles[index].onValueChanged.AddListener((value) => _ToggleValueChanged(value, index));
        }
    }

    void _ToggleValueChanged(bool value, int typeNum)
    {
        if (value)
        {
            if (typeNum == (int)Enum_ItemTypeInKorean.전체)
            {
                _RenderAllItemsBright();
            }
            else
            {
                _RenderByType(typeNum);
            }
        }
    }

    // 전체 아이템을 밝게 렌더링
    void _RenderAllItemsBright()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null) continue;

            _cachedItemSlots[i].RenderBright();
        }
    }

    // 선택한 타입에 해당 아이템은 색 밝게, 나머지는 약간 어둡게
    void _RenderByType(int typeNum)
    {
        Enum_ItemType targetType = (Enum_ItemType)(typeNum - 1);

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == null) continue;

            if (_items[i].itemType != targetType)
            {
                _cachedItemSlots[i].RenderDark();
            }
            else
            {
                _cachedItemSlots[i].RenderBright();
            }
        }
    }     

    // 아이템 배열 정보에 맞게 UI 갱신 시키는 메서드
    public void UpdateInvenSlot(int slotIndex)
    {
        UI_ItemSlot slot = _content.transform.GetChild(slotIndex).GetComponent<UI_ItemSlot>();
        slot.ItemRender();
    }

    // 인벤 확장
    void _ExpandSlot(int newSlot = 6)
    {
        for (int i = GameManager.Inven.TotalSlotCount; i < GameManager.Inven.TotalSlotCount + newSlot; i++)
        {
            GameObject _itemSlot = GameManager.Resources.Instantiate("Prefabs/UI/Scene/ItemSlot", _content.transform);
            _itemSlot.name = "ItemSlot_" + i;
            _itemSlot.GetComponent<UI_ItemSlot>().Index = i;
        }
        GameManager.Inven.TotalSlotCount += newSlot;
        GameManager.Inven.ExtendItemList();
    }

    [Obsolete("Just For Test. Not use anymore.")]
    void _PressGetItem()
    {
        var item = GameManager.Data.StateItemDataReader(500);
        item.count = 70;

        GameManager.Inven.GetItem(item);
        // TODO 장비아이템은 고유번호
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

    public void RestrictItemDescrPos()
    {
        Vector2 option = new Vector2(170f, -135f);
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

    public void UpdateGoldPanel(long gold)
    {
        goldPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = gold.ToString();
    }
}