using UnityEngine;
using TMPro;

public class ResourceManager : SubClass<GameManager>
{
    string uiString = "Prefabs/UI";
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

    public GameObject Instantiate(string path, Transform parent = null, bool objectPool = true)
    {
        GameObject Original = Load<GameObject>(path);

        if (path.Contains(uiString))
        {
            objectPool = false;
        }

        if (Original == null)
        {
            Debug.Log(path);
            return null;
        }

        return Instantiate(Original, parent, objectPool);
    }
    public GameObject Instantiate(string path, Vector3 position, Quaternion rotation, Transform parent = null, bool objectPool = true)
    {
        GameObject Original = Load<GameObject>(path);

        if (path.Contains(uiString))
        {
            objectPool = false;
        }

        if (Original == null)
        {
            Debug.Log(path);
            return null;
        }

        return Instantiate(Original, position, rotation, parent, objectPool);


    }
    public GameObject Instantiate(GameObject preLoadedobj, Vector3 position, Quaternion rotation, Transform parent = null, bool objectPool = true)
    {
        
        if (objectPool && !preLoadedobj.name.Contains("UI"))
        {
            return GameManager.PoolBeta.Instantiate(preLoadedobj, position, rotation, parent);
        }   
       
        GameObject go = Object.Instantiate(preLoadedobj, position, rotation, parent);
        go.name = preLoadedobj.name;

        return go;
    }
    public GameObject Instantiate(GameObject preLoadedobj, Transform parent = null, bool objectPool = true)
    {
        if (objectPool && !preLoadedobj.name.Contains("UI"))
        {           
            return GameManager.PoolBeta.Instantiate(preLoadedobj, Vector3.zero, Quaternion.identity, parent);
        }
        //풀링 대상이 아닌 녀석들
        GameObject go = Object.Instantiate(preLoadedobj, parent);
        go.name = preLoadedobj.name;

        return go;
    }
    public void Destroy(GameObject go, float DestoryOffset = 0)
    {
        if (go == null) return;


        GameManager.PoolBeta.ObjectFalse(go);

        /*Poolable poolable = go.GetComponent<Poolable>();

        //풀링 대상인 녀석인가?
        if (poolable != null)
        {
            GameManager.Pool.Push(poolable);
            return;
        }

        //풀링 대상이 아닌 녀석들
        Object.Destroy(go, DestoryOffset);*/
    }
}
