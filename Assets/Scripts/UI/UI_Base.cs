using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UI_Base : MonoBehaviour
{
    // Text,Button,Image �� Ÿ���� �������� �ʿ��ϹǷ� Dictionary�� Ÿ�Ժ��� ����
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    public abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type); // Enum���� ������� names��� ������ string �迭�� �޾ƿ�
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length]; // enum���� ������� �� ��ŭ objects �迭 ����
        _objects.Add(typeof(T), objects); // Dictionary�� Ÿ�԰� ������� ����

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject)) // GameObject Ÿ���϶�
            {
                objects[i] = Utils.FindChild(gameObject, names[i], true);
            }
            else // �� �ܴ� TŸ������ ������Ʈ�� �޾ƿ� ex) text, image
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
        if (_objects.TryGetValue(typeof(T), out objects) == false) // �������µ� ����
        {
            return null;
        }

        return objects[idx] as T; // �ε��� ��ȣ�� ������ ���� T��
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // go, action, �׸��� ���� ����° ���ڿ��� ����Ϸ��� eventŸ���� ����
    public static void AddUIEvent(GameObject go, Action<PointerEventData> action, UI_Define.Enum_UIEvent type = UI_Define.Enum_UIEvent.Click)
    {
        // UI_EventHandler ��ũ��Ʈ�� �����Ǿ� ���� �ʴٸ� ������ �� �ֵ��� ��
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
