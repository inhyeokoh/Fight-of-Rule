using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class UI_Entity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, ISelectHandler
{
    public Action<PointerEventData> PointerEnterAction = null;
    public Action<PointerEventData> PointerExitAction = null;
    public Action<PointerEventData> ClickAction = null;
    public Action<PointerEventData> DragAction = null;
    public Action<BaseEventData> SelectAction = null;

    PlayerInput playerInput;
    string playername = "Player";
    string uiKeyInput = "KeyInput";

    //나의 UI_Type
    public Type UIType = null;

    //부모 UI_Entity
    UI_Entity _mother;

    //모든 하위 UI 요소들 + 현재 UI로부터 파생된 팝업 UI
    protected List<UI_Entity> _subUIs = new List<UI_Entity>();
    //현재 UI의 하위 UI요소들
    protected Dictionary<int, UI_Entity> _entities = new Dictionary<int, UI_Entity>();

    //UI컴포넌트들 모음
    static List<Type> _components = new List<Type>()
        {
            typeof(Button),
            typeof(Slider),
            typeof(Image),
            typeof(RawImage),
            typeof(Toggle),
            typeof(TMP_InputField),
            typeof(TMP_Dropdown),
            typeof(TMP_Text),
        };

    protected void Start()
    {
        GameObject keyInput = GameObject.Find($"{uiKeyInput}");
        if (keyInput != null)
        {
            playerInput = keyInput.GetComponent<PlayerInput>();
        }
        /*        GameObject player = GameObject.Find($"{playername}");
                if (player != null)
                {
                    playerInput = player.GetComponent<PlayerInput>(); // 로그인 씬 말고 인게임 들어갔을때 Init 말고 재실행 필요할듯
                }*/
        if (UIType == null)
            Init();
    }

    //만약 상위UI가 있다면 하위 UI에게 전달해주는 용도
    protected void Mount(UI_Entity mother)
    {
        _mother = mother;   //부모 UI_Entity를 받아옴
    }

    protected virtual void Init()
    {
        var tnames = GetUINamesAsType();
        //이름 목록 가져오기
        string[] names = tnames == null ? null : Enum.GetNames(tnames);

        //자식들을 모두 순회하며 UI_Entity를 탐색하거나, UI_Entity를 붙일지 판별
        for (int i = 0; i < transform.childCount; i++)
        {
            var go = transform.GetChild(i).gameObject;
            var comp = go.GetComponent<UI_Entity>();

            //만약 UI_Entity가 아니라면 붙일지 말지 판별 후 부착
            if (comp == null)
                comp = Distinguisher(go, ref names);

            //UI_Entity 대상이 아니라면 생략
            if (comp == null) continue;

            // UI요소라면 다음 코드가 실행됨
            comp.Mount(this);
            _subUIs.Add(comp);
        }
    }

    public UI_Entity Distinguisher(GameObject obj, ref string[] names)
    {
        if (obj == null) return null;

        for (int i = 0; i < _components.Count; i++)
        {
            var component = obj.GetComponent(_components[i]);

            if (component == null)
                continue;

            var uientity = component.gameObject.GetOrAddComponent<UI_SubEntity>();
            uientity.UIType = _components[i];

            if (names == null)
                return uientity;

            for (int str = 0; str < names.Length; str++)
            {
                // 하이어라키 상의 이름과 enum 안의 이름이 일치하면,
                if (component.gameObject.name == names[str])
                {
                    _entities.Add(str, uientity);
                    break;
                }
            }

            return uientity;
        }

        return null;
    }

    //오류검증 하지않음. 받아서 null체크
    public T GetComp<T>() where T : Component
    {
        return GetComponent<T>();
    }

    /*    public void CloseAllUI()
        {
            if (_mother != null)
                _mother.CloseAllUI();
            else
                GameManager.Resources.Destroy(gameObject);
        }*/

    public void CloseUI()
    {
        GameManager.Resources.Destroy(gameObject);
    }


    protected abstract Type GetUINamesAsType();

    // 포인터가 오브젝트에 들어왔을 때 호출 ex) 인벤 아이템 정보 보기
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterAction?.Invoke(eventData);
    }

    // 포인터가 오브젝트 밖으로 나갈 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitAction?.Invoke(eventData);
    }

    // 오브젝트에서 포인터를 누르고 동일한 오브젝트에서 뗄 때 호출
    public void OnPointerClick(PointerEventData eventData)
    {
        ClickAction?.Invoke(eventData);
    }

    // 드래그 대상이 드래그되는 동안 호출
    public void OnDrag(PointerEventData eventData)
    {
        DragAction?.Invoke(eventData);
    }

    // 오브젝트가 선택된 순간, 그 오브젝트에서 호출
    public void OnSelect(BaseEventData eventData)
    {
        SelectAction?.Invoke(eventData);
    }

    public void PointerOnUI(bool On)
    {
        if (On)
        {
            playerInput.currentActionMap.FindAction("Move").Disable();
        }
        else
        {
            playerInput.currentActionMap.FindAction("Move").Enable();
        }
    }
}