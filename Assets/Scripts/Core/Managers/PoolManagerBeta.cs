using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManagerBeta : SubClass<GameManager>
{
    // 오브젝트풀이 필요한 오브젝트를 관리하기 위해
    // 그리고 프리팹을 생성할수도있고 리소스로 생성할수도 있음
    // 똑같이 Instantiate를해서 GameObject를 반환시켜 줘여함

    Dictionary<string, Stack<GameObject>> multipleObjectPools = new Dictionary<string, Stack<GameObject>>();


    /*  private void Awake()
      {      
          beta = this;       
      }*/


    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {

    }

    protected override void _Init()
    {

    }

    public GameObject Instantiate(GameObject go, Vector3 position, Quaternion rotation, Transform parant = null)
    {
        string objectName = go.name;

        GameObject clone = null;

        if (multipleObjectPools.ContainsKey(objectName))
        {
            clone = InstantiateObject(multipleObjectPools[objectName], go, position, rotation, parant);
            clone.name = objectName;
        }
        else
        {
            multipleObjectPools.Add(objectName, new Stack<GameObject>());
            clone = InstantiateObject(multipleObjectPools[objectName], go, position, rotation, parant);
            clone.name = objectName;
        }
        return clone;
    }


    private GameObject InstantiateObject(Stack<GameObject> objects, GameObject go, Vector3 position, Quaternion rotation, Transform parant)
    {
        GameObject clone = null;

        while (objects.Count > 0)
        {
            for (int i = 0; i < objects.Count; i++)
            {              
                if (objects.Peek().activeSelf == true)
                {
                    objects.Pop();
                    continue;
                }
                else
                {
                    clone = objects.Pop();
                    clone.transform.parent = parant;
                    clone.transform.position = position;
                    clone.transform.rotation = rotation;
                    clone.SetActive(true);

                    return clone;
                }
            }
        }
        clone = Object.Instantiate(go, position, rotation, parant);      
        return clone;
    }

    public void ObjectFalse(GameObject go)
    {
        string objectName = go.name;

        if (multipleObjectPools.ContainsKey(objectName))
        {
            go.SetActive(false);
            Push(multipleObjectPools[objectName], go);
        }
        else
        {
            Object.Destroy(go);
        }
    }

    private void Push(Stack<GameObject> objects, GameObject go)
    {
        objects.Push(go);
        Debug.Log($"오브젝트를 넣고 현재 오브젝트풀 : {objects.Count}");
    }

    /*public void Destroy(GameObject go)
    {
        if (go == null)
        {
            Debug.Log("현재 게임오브젝트가 Hierarchy View에 없습니다");
        }

        string name = go.name;
        if (multipleObjectPools.ContainsKey("name") && DestroyObject(multipleObjectPools[name], go))
        {
            return;
        }
        else
        {
            Object.Destroy(go);
        }
    }*/


    private bool DestroyObject(List<GameObject> objects, GameObject go)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == go)
            {
                objects.Remove(objects[i]);
                Object.Destroy(go);
                return true;
            }
        }
        return false;
    }


    public void PoolDestroy(string objectString)
    {
        GameObject go = GameManager.Resources.Load<GameObject>(objectString);

        string name = go.name;

        if (multipleObjectPools.ContainsKey(name))
        {
            PoolRemove(multipleObjectPools[name]);
            multipleObjectPools.Remove(name);
        }
        else
        {
            Debug.Log($"{name} 오브젝트풀이 존재하지 않습니다");
        }
    }

    public void PoolDestroy(GameObject go)
    {
        string name = go.name;

        if (multipleObjectPools.ContainsKey(name))
        {
            PoolRemove(multipleObjectPools[name]);
            multipleObjectPools.Remove(name);
        }
        else
        {
            Debug.Log($"{name} 오브젝트풀이 존재하지 않습니다");
        }
    }


    private void PoolRemove(Stack<GameObject> objects)
    {
        while (objects.Count > 0)
        {
            if (objects.Peek().activeSelf == true)
            {
                objects.Peek().SetActive(false);
            }

            Object.Destroy(objects.Pop());
        }
        objects = null;
    }
}