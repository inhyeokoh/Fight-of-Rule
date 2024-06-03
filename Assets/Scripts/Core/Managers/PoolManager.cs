using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SubClass<GameManager>
{
    protected override void _Clear()
    {
        if (_root == null)
            return;

        //상황에 따라서 남겨야 할 풀이 분명히 존재할 수 있다.
        for (int i = 0; i < _root.transform.childCount; i++)
        {
            GameObject.Destroy(_root.transform.GetChild(i).gameObject);
        }

        Dic_Pool.Clear();
    }

    protected override void _Excute()
    {
        
    }

    protected override void _Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@@Pool Root");
            Object.DontDestroyOnLoad(_root);
        }
    }

    static readonly Vector3 _poolPos = new Vector3(100000f, 100000f, 100000f);
    public static Vector3 PoolPos { get { return _poolPos; } }

    #region  Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public GameObject Root { get; set; }

        Stack<Poolable> PoolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject();
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        //Init의 count만큼 풀링 오브젝트를 만들어줄 함수
        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable plab)
        {
            if (plab == null) return;

            plab.transform.parent = Root.transform;
            plab.gameObject.SetActive(false);
            plab.IsUsing = false;

            PoolStack.Push(plab);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable plab;

            if (PoolStack.Count > 0)
                plab = PoolStack.Pop();
            else
                plab = Create();

            plab.gameObject.SetActive(true);

            if (parent == null)
                plab.transform.parent = Camera.main.transform;

            plab.transform.parent = null;
            plab.IsUsing = true;

            return plab;
        }
    }
    #endregion
    Dictionary<string, Pool> Dic_Pool = new Dictionary<string, Pool>();

    GameObject _root;


    public void CreatPool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.transform.parent = _root.transform;

        Dic_Pool.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        poolable.gameObject.transform.position = PoolPos;
        string name = poolable.gameObject.name;

        if (!Dic_Pool.ContainsKey(name))
            CreatPool(poolable.gameObject, 0);

        Dic_Pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (!Dic_Pool.ContainsKey(original.name))
            CreatPool(original);
        return Dic_Pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (!Dic_Pool.ContainsKey(name))
            return null;
        else
            return Dic_Pool[name].Original;
    }
}
