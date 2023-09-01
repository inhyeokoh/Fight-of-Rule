using UnityEngine;

public class Utils
{
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform ttr = FindChild<Transform>(go, name, recursive);
        if (ttr == null) return null;

        return ttr.gameObject; // ttr�� null �϶� ttr.gameObject ȣ��� �����P��
    }
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive)
        {
            T[] tmp = go.GetComponentsInChildren<T>();
            for (int i = 0; i < tmp.Length; i++)
            {
                if (string.IsNullOrEmpty(name) || tmp[i].name == name) //�̸��� �Էµ��� �ʾ����� ����ó�� TŸ�Թ�ȯ,�ԷµǾ����� ��ġ�� ��ȯ
                {
                    return tmp[i];
                }
            }
        }
        else
        {
            Transform tmp;
            T ctmp;
            for (int i = 0; i < go.transform.childCount; i++)
            {
                tmp = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || tmp.name == name)
                {
                    ctmp = tmp.GetComponent<T>();
                    if (ctmp != null)
                    {
                        Debug.Log(ctmp.name);
                        return ctmp;
                    }
                }
            }
        }
        return null;
    }

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T Comp = go.GetComponent<T>();
        if (Comp == null)
            Comp = go.AddComponent<T>();
        return Comp;
    }

    public static Component GetOrAddComponent<T>(GameObject go, T type) where T : System.Type
    {
        Component Comp = go.GetComponent(type);
        if (Comp == null)
            Comp = go.AddComponent(type);
        return Comp;
    }

    public static bool ContainsInvaild(string s)
    {
        if (s.Length == 0)
            return true;

        for (int i = 0; i < s.Length; i++)
        {
            if (char.GetUnicodeCategory(s[i]) == System.Globalization.UnicodeCategory.OtherLetter)
                return true;

            if (s[i] == ' ')
                return true;
        }
        return false;
    }
}
