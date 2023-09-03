using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Utils.GetOrAddComponent<T>(go);
    }

    public static Component GetOrAddComponent<T>(this GameObject go, T type) where T : System.Type
    {
        return Utils.GetOrAddComponent(go, type);
    }
}
