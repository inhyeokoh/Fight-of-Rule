using UnityEngine;

public class ResourceManager : SubClass<GameManager>
{
    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
        
    }

    protected override void _Init()
    {
        
    }

    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path); //일단 기존 방식이랑 mapping
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject Original = Load<GameObject>(path);

        if (Original == null)
        {
            Debug.Log(path);
            return null;
        }

        return Instantiate(Original, parent);
    }

    public GameObject Instantiate(GameObject preLoadedobj, Transform parent = null)
    {
        if (preLoadedobj.GetComponent<Poolable>() != null)
        {
            return GameManager.Pool.Pop(preLoadedobj, parent).gameObject; //팝할게 없어도 자동으로 주가됨.
        }
            

        //풀링 대상이 아닌 녀석들
        GameObject go = Object.Instantiate(preLoadedobj, parent);
        go.name = preLoadedobj.name;

        return go;
    }

    public void Destroy(GameObject go, float DestoryOffset = 0)
    {
        if (go == null) return;

        Poolable poolable = go.GetComponent<Poolable>();

        //풀링 대상인 녀석인가?
        if (poolable != null)
        {
            GameManager.Pool.Push(poolable);
            return;
        }

        //풀링 대상이 아닌 녀석들
        Object.Destroy(go, DestoryOffset);
    }
}
