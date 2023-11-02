using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public abstract class UI_Entity : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, ISelectHandler
{
    public Action<PointerEventData> PointerEnterAction = null;
    public Action<PointerEventData> PointerExitAction = null;
    public Action<PointerEventData> ClickAction = null;
    public Action<PointerEventData> DragAction = null;
    public Action<BaseEventData> SelectAction = null;

    //���� UI_Type
    public Type UIType = null;

    //�θ� UI_Entity
    UI_Entity _mother;

    //��� ���� UI ��ҵ� + ���� UI�κ��� �Ļ��� �˾� UI
    protected List<UI_Entity> _subUIs = new List<UI_Entity>();
    //���� UI�� ���� UI��ҵ�
    protected Dictionary<int, UI_Entity> _entities = new Dictionary<int, UI_Entity>();

    //UI������Ʈ�� ����
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
        if (UIType == null)
            Init();
    }

    //���� ����UI�� �ִٸ� ���� UI���� �������ִ� �뵵
    public void Mount(UI_Entity mother)
    {
        _mother = mother;   //�θ� UI_Entity�� �޾ƿ�
    }

    protected virtual void Init()
    {
        var tnames = GetUINamesAsType();
        //�̸� ��� ��������
        string[] names = tnames == null ? null : Enum.GetNames(tnames);

        //�ڽĵ��� ��� ��ȸ�ϸ� UI_Entity�� Ž���ϰų�, UI_Entity�� ������ �Ǻ�
        for (int i = 0; i < transform.childCount; i++)
        {
            var go = transform.GetChild(i).gameObject;
            var comp = go.GetComponent<UI_Entity>();

            //���� UI_Entity�� �ƴ϶�� ������ ���� �Ǻ� �� ����
            if (comp == null)
                comp = Distinguisher(go, ref names);

            //UI_Entity ����� �ƴ϶�� ����
            if (comp == null) continue;

            comp.Mount(this);
            _subUIs.Add(comp);
        }
    }

    public UI_Entity Distinguisher(GameObject obj, ref string[] names)
    {
        if (obj == null) return null;

        for(int i = 0; i < _components.Count; i++)
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

    //�������� ��������. �޾Ƽ� nullüũ
    public T GetComp<T>() where T : Component
    {
        return GetComponent<T>();
    }

    public void CloseUI()
    {
        if (_mother != null)
            _mother.CloseUI();
        else
            GameManager.Resources.Destroy(gameObject);
    }

    protected abstract Type GetUINamesAsType();

    // �����Ͱ� ������Ʈ�� ������ �� ȣ�� ex) �κ� ������ ���� ����, ��ȣ�ۿ� ������ ��ü�� ��Ÿ���� F���� Ű ǥ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterAction?.Invoke(eventData);
    }

    // �����Ͱ� ������Ʈ ������ ���� �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitAction?.Invoke(eventData);
    }

    // ������Ʈ���� �����͸� ������ ������ ������Ʈ���� �� �� ȣ��
    public void OnPointerClick(PointerEventData eventData)
    {
        ClickAction?.Invoke(eventData);
    }

    // �巡�� ����� �巡�׵Ǵ� ���� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        DragAction?.Invoke(eventData);
    }

    // ������Ʈ�� ���õ� ����, �� ������Ʈ���� ȣ��
    public void OnSelect(BaseEventData eventData)
    {
        SelectAction?.Invoke(eventData);
    }
}
