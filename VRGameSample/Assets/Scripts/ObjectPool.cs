using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPool : MonoBehaviour {

    #region Member

    [SerializeField]
    private GameObject prefab;

   
    public int maxCount = 50;  
    public int prepareCount = 0; 

    [SerializeField]
    private int interval = 1;

    private List<GameObject> pooledObjectList = new List<GameObject>();
    private static GameObject poolAttachedObject = null;



    public int Interval
    {
        get
        {
            return interval;
        }

        set
        {
            if(interval != value)
            {
                interval = value;

                StopAllCoroutines();
                if(interval > 0)
                {
                    StartCoroutine(RemoveObjectCheck());
                }
                       
            }
        }
    }
    #endregion


    void OnEnable()
    {
        if (interval > 0)
            StartCoroutine(RemoveObjectCheck());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }


    void OnDestory()
    {
        if (poolAttachedObject == null)
            return;

        if (poolAttachedObject.GetComponents<ObjectPool>().Length == 1)
        {
            poolAttachedObject = null;
        }

        foreach(var obj in pooledObjectList)
        {
            Destroy(obj);
        }

        pooledObjectList.Clear();
    }


    IEnumerator RemoveObjectCheck()
    {
        while (true)
        {
            RemoveObject(prepareCount);
            yield return new WaitForSeconds(interval);
        }
    }



    public GameObject GetInstance()
    {
        return GetInstance(transform);
    }

    public GameObject GetInstance(Transform parent)
    {
        pooledObjectList.RemoveAll(obj => obj == null);

        foreach (GameObject obj in pooledObjectList)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        if (pooledObjectList.Count < maxCount)
        {
            GameObject obj = GameObject.Instantiate(prefab);
            obj.SetActive(true);
            obj.transform.parent = parent;
            pooledObjectList.Add(obj);
            return obj;
        }

        return null;

    }

    public void RemoveObject(int max)
    {
        if (pooledObjectList.Count > max)
        {
            int needRemoveCount = pooledObjectList.Count - max;

            foreach (GameObject obj in pooledObjectList)
            {
                if (needRemoveCount == 0)
                    break;

                if (!obj.activeSelf)
                {
                    pooledObjectList.Remove(obj);
                    Destroy(obj);
                    needRemoveCount--;
                }
            }
        }
    }

   
    

    public static ObjectPool GetObjectPool(GameObject obj)
    {
        if(poolAttachedObject ==null)
        {
            poolAttachedObject = GameObject.Find("ObjectPool");
            if (poolAttachedObject == null)
            {
                poolAttachedObject = new GameObject("ObjectPool");
            }
        }

        foreach(var pool in poolAttachedObject.GetComponents<ObjectPool>())
        {
            if(pool.prefab == obj)
            {
                return pool;
            }
        }


        foreach(var pool in FindObjectsOfType<ObjectPool>())
        {
            if(pool.prefab  == obj)
                    return pool;
        }


        var newPool = poolAttachedObject.AddComponent<ObjectPool>();
        newPool.prefab = obj;
        return newPool;
    }
}
