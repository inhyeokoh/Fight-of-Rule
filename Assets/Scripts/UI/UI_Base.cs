using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    // Text,Button,Image 등 타입이 여러가지 필요하므로 Dictionary에 타입별로 저장
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // Enum안의 내용들을 names라는 변수의 string 배열로 받아옴
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; // enum안의 내용들의 수 만큼 objects 배열 생성
        _objects.Add(typeof(T), objects); // Dictionary에 타입과 내용들을 저장

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) // GameObject 타입일때
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            }
            else // 그 외는 T타입으로 컴포넌트를 받아옴 ex) text, image
            {
                objects[i] = Utils.FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Failed to bind{names[i]}");
#endif
            }
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false) // 꺼내오는데 실패
        {
            return null;
        }

        return objects[idx] as T; // 인덱스 번호를 추출한 다음 T로
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // go, action, 그리고 오는 세번째 인자에서 사용하려는 event타입을 받음
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, UI_Define.Enum_UIEvent type = UI_Define.Enum_UIEvent.Click)
    {
        // UI_EventHandler 스크립트가 부착되어 있지 않다면 부착할 수 있도록 함
        UI_EventHandler evt = Utils.GetOrAddComponent<UI_EventHandler>(go);
                
        switch (type)
        {
            case UI_Define.Enum_UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case UI_Define.Enum_UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            default:
                break;
        }

    }
}
