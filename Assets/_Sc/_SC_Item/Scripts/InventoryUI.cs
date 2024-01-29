using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("ItemSlotCreate")]
    [Range(0, 1920)]
    [SerializeField] private int _horizontalLength = 400;
    [SerializeField] private int _verticalLength = 400;
    [Range(40, 100)]
    [SerializeField] private float _slotSize = 40f;

    [SerializeField] private RectTransform _slotAreaRT;
    [SerializeField] private int _slotCount;
    [SerializeField] private List<ItemSlotUI> _slotUIList;

    [Space]
    [SerializeField] private bool _showTooltip = true;
    [SerializeField] private bool _showHighlight = true;
    [SerializeField] private bool _showRemovingPopup = true;

    [Header("Connected Objects")]
    [SerializeField] private RectTransform _contentAreaRT; // 슬롯들이 위치할 영역
    [SerializeField] private GameObject _slotUiPrefab;     // 슬롯의 원본 프리팹
    [SerializeField] private ItemTooltipUI _itemTooltip;   // 아이템 정보를 보여줄 툴팁 UI
    [SerializeField] private InventoryPopupUI _popup;

    private Inventory _inventory;

    private ItemSlotUI _pointerOverSlot; // 현재 포인터가 위치한 곳의 슬롯
    public int slotCount;

    //아이템 드래그 앤 드롭
    private GraphicRaycaster _gr;
    private PointerEventData _ped;
    private List<RaycastResult> _rrList;

    private ItemSlotUI _beginDragSlot; // 현재 드래그를 시작한 슬롯
    private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

    private int _leftClick = 0;
    private int _rightClick = 1;

    private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
    private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
    private int _beginDragSlotSiblingIndex;


    private void Awake()
    {
        _slotCount = (_horizontalLength / (int)_slotSize) * (_verticalLength / (int)_slotSize);
        slotCount = _slotCount;
        Init();
        InitSlots();
    }

    private void Init()
    {
        TryGetComponent(out _gr);
        if (_gr == null)
            _gr = gameObject.AddComponent<GraphicRaycaster>();

        // Graphic Raycaster
        _ped = new PointerEventData(EventSystem.current);
        _rrList = new List<RaycastResult>(10);

    }
    private void InitSlots()
    {
        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
        {
            _slotUiPrefab.AddComponent<ItemSlotUI>();
        }

        _slotUiPrefab.SetActive(false);

        _slotUIList = new List<ItemSlotUI>(_slotCount);

        for (int i = 0; i < _slotCount; i++)
        {
            int slotIndex = i;

            var slotRT = CloneSlot();
            slotRT.gameObject.SetActive(true);
            slotRT.gameObject.name = $"Item Slot [{slotIndex}]";

            // rectTransform인데 ItemSlotUI 컴포넌트를 가져올 수 있나?
            var slotUI = slotRT.GetComponent<ItemSlotUI>();
            slotUI.SetSlotIndex(slotIndex);
            _slotUIList.Add(slotUI);

        }

        RectTransform CloneSlot()
        {
            GameObject slotInit = Instantiate(_slotUiPrefab);
            RectTransform rt = slotInit.GetComponent<RectTransform>();
            rt.SetParent(_slotAreaRT);

            return rt;
        }
    }

    /// <summary> 인벤토리 참조 등록 (인벤토리에서 직접 호출) </summary>
    public void SetInventoryReference(Inventory inventory)
    {
        _inventory = inventory;
    }

    /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
    public void SetItemIcon(int index, Sprite icon)
    {
        _slotUIList[index].SetItem(icon);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void SetItemAmountText(int index, int amount)
    {
        EditorLog($"Set Item Amount Text : Slot [{index}], Amount [{amount}]");

        // NOTE : amount가 1 이하일 경우 텍스트 미표시
        _slotUIList[index].SetItemAmount(amount);
    }

    /// <summary> 해당 슬롯의 아이템 개수 텍스트 지정 </summary>
    public void HideItemAmountText(int index)
    {
        EditorLog($"Hide Item Amount Text : Slot [{index}]");

        _slotUIList[index].SetItemAmount(1);
    }

    /// <summary> 슬롯에서 아이템 아이콘 제거, 개수 텍스트 숨기기 </summary>
    public void RemoveItem(int index)
    {
        EditorLog($"Remove Item : Slot [{index}]");

        _slotUIList[index].RemoveItem();
    }

    /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
    public void SetAccessibleSlotRange(int accessibleSlotCount)
    {
        for (int i = 0; i < _slotUIList.Count; i++)
        {
            _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
        }
    }


    [Header("Editor Options")]
    [SerializeField] private bool _showDebug = true;
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    private void EditorLog(object message)
    {
        if (!_showDebug) return;
        UnityEngine.Debug.Log($"[InventoryUI] {message}");
    }

    private void EndDrag()
    {
        ItemSlotUI endDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        // 아이템 슬롯끼리 아이콘 교환 또는 이동
        if (endDragSlot != null && endDragSlot.IsAccessible)
        {
            // 수량 나누기 조건
            // 1) 마우스 클릭 떼는 순간 좌측 Ctrl 또는 Shift 키 유지
            // 2) begin : 셀 수 있는 아이템 / end : 비어있는 슬롯
            // 3) begin 아이템의 수량 > 1
            bool isSeparatable =
                (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) &&
                (_inventory.IsCountableItem(_beginDragSlot.Index) && !_inventory.HasItem(endDragSlot.Index));

            // true : 수량 나누기, false : 교환 또는 이동
            bool isSeparation = false;
            int currentAmount = 0;

            // 현재 개수 확인
            if (isSeparatable)
            {
                currentAmount = _inventory.GetCurrentAmount(_beginDragSlot.Index);
                if (currentAmount > 1)
                {
                    isSeparation = true;
                }
            }

            // 1. 개수 나누기
            if (isSeparation)
                TrySeparateAmount(_beginDragSlot.Index, endDragSlot.Index, currentAmount);
            // 2. 교환 또는 이동
            else
                TrySwapItems(_beginDragSlot, endDragSlot);

            // 툴팁 갱신
            UpdateTooltipUI(endDragSlot);
            return;
        }

        // 버리기(커서가 UI 레이캐스트 타겟 위에 있지 않은 경우)
        if (!IsOverUI())
        {
            int index = _beginDragSlot.Index;
            TryRemoveItem(index);
        }
    }

    /// <summary> 두 슬롯의 아이템 교환 </summary>
    private void TrySwapItems(ItemSlotUI from, ItemSlotUI to)
    {
        if (from == to)
        {
            return;
        }

        from.SwapOrMoveIcon(to);
        _inventory.Swap(from.Index, to.Index);
    }

    ///drag & drop
    private void Update()
    {
        _ped.position = Input.mousePosition;

        OnPointerEnterAndExit();
        if (_showTooltip) ShowOrHideItemTooltip();
        OnPointerDown();
        OnPointerDrag();
        OnPointerUp();
    }
    /// <summary> 슬롯에 포인터가 올라가는 경우, 슬롯에서 포인터가 빠져나가는 경우 </summary>
    private void OnPointerEnterAndExit()
    {
        // 이전 프레임의 슬롯
        var prevSlot = _pointerOverSlot;

        // 현재 프레임의 슬롯
        var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

        if (prevSlot == null)
        {
            // Enter
            if (curSlot != null)
            {
                OnCurrentEnter();
            }
        }
        else
        {
            // Exit
            if (curSlot == null)
            {
                OnPrevExit();
            }

            // Change
            else if (prevSlot != curSlot)
            {
                OnPrevExit();
                OnCurrentEnter();
            }
        }

        // ===================== Local Methods ===============================
        void OnCurrentEnter()
        {
            if (_showHighlight)
                curSlot.Highlight(true);
        }
        void OnPrevExit()
        {
            prevSlot.Highlight(false);
        }
    }
    private T RaycastAndGetFirstComponent<T>() where T : Component
    {
        _rrList.Clear();

        _gr.Raycast(_ped, _rrList);

        if (_rrList.Count == 0)
            return null;

        return _rrList[0].gameObject.GetComponent<T>();
    }

    private void OnPointerDown()
    {
        // Left Click : Begin Drag
        if (Input.GetMouseButtonDown(_leftClick))
        {
            _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotUI>();

            // 아이템을 갖고 있는 슬롯만 해당
            if (_beginDragSlot != null && _beginDragSlot.HasItem)
            {
                EditorLog($"Drag Begin : Slot [{_beginDragSlot.Index}]");
                // 위치 기억, 참조 등록
                _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                _beginDragIconPoint = _beginDragIconTransform.position;
                _beginDragCursorPoint = Input.mousePosition;

                // 맨 위에 보이기
                _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                _beginDragSlot.transform.SetAsLastSibling();

                // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
                _beginDragSlot.SetHighlightOnTop(false);
            }
            else
            {
                _beginDragSlot = null;
            }
        }

        // Right Click : Use Item
        else if (Input.GetMouseButtonDown(_rightClick))
        {
            ItemSlotUI slot = RaycastAndGetFirstComponent<ItemSlotUI>();

            if (slot != null && slot.HasItem && slot.IsAccessible)
            {
                TryUseItem(slot.Index);
            }
        }
    }
    /// <summary> 드래그하는 도중 </summary>
    private void OnPointerDrag()
    {
        if (_beginDragSlot == null) return;

        if (Input.GetMouseButton(0))
        {
            // 위치 이동
            _beginDragIconTransform.position =
                _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
        }
    }
    /// <summary> 클릭을 뗄 경우 </summary>
    private void OnPointerUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // End Drag
            if (_beginDragSlot != null)
            {
                // 위치 복원
                _beginDragIconTransform.position = _beginDragIconPoint;

                // UI 순서 복원
                _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                // 드래그 완료 처리
                EndDrag();

                // 참조 제거
                _beginDragSlot = null;
                _beginDragIconTransform = null;
            }
        }
    }


    /// <summary> UI 및 인벤토리에서 아이템 제거 </summary>
    private void TryRemoveItem(int index)
    {
        _inventory.Remove(index);
    }

    /// <summary> 아이템 사용 </summary>
    private void TryUseItem(int index)
    {
        EditorLog($"UI - Try Use Item : Slot [{index}]");

        _inventory.Use(index);
    }

    /// <summary> 셀 수 있는 아이템 개수 나누기 </summary>
    private void TrySeparateAmount(int indexA, int indexB, int amount)
    {
        if (indexA == indexB)
        {
            EditorLog($"UI - Try Separate Amount: Same Slot [{indexA}]");
            return;
        }

        EditorLog($"UI - Try Separate Amount: Slot [{indexA} -> {indexB}]");

        string itemName = $"{_inventory.GetItemName(indexA)} x{amount}";

        _popup.OpenAmountInputPopup(
            amt => _inventory.SeparateAmount(indexA, indexB, amt),
            amount, itemName
        );
    }

    /// <summary> 툴팁 UI의 슬롯 데이터 갱신 </summary>
    private void UpdateTooltipUI(ItemSlotUI slot)
    {
        if (!slot.IsAccessible || !slot.HasItem)
            return;

        // 툴팁 정보 갱신
        _itemTooltip.SetItemInfo(_inventory.GetItemData(slot.Index));

        // 툴팁 위치 조정
        _itemTooltip.SetRectPosition(slot.SlotRect);
    }

    private void ShowOrHideItemTooltip()
    {
        // 마우스가 유효한 아이템 아이콘 위에 올라와 있다면 툴팁 보여주기
        bool isValid =
            _pointerOverSlot != null && _pointerOverSlot.HasItem && _pointerOverSlot.IsAccessible
            && (_pointerOverSlot != _beginDragSlot); // 드래그 시작한 슬롯이면 보여주지 않기

        if (isValid)
        {
            UpdateTooltipUI(_pointerOverSlot);
            _itemTooltip.Show();
        }
        else
            _itemTooltip.Hide();
    }
    private bool IsOverUI()
        => EventSystem.current.IsPointerOverGameObject();
}
