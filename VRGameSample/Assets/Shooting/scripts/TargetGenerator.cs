using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using System.Linq;

public class TargetGenerator : MonoBehaviour
{

    #region Member

    [SerializeField]
    private GameObject target;

    private ObjectPool pool;

    private List<GameObject> targetpoints = new List<GameObject>();
    #endregion

    void Awake()
    {
        foreach(Transform child in transform)
        {
            targetpoints.Add(child.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
       
        pool = ObjectPool.GetObjectPool(target);
        pool.maxCount = 30;
        pool.Interval = 1;

     
        this.UpdateAsObservable()
             .ThrottleFirst(System.TimeSpan.FromSeconds(2))
             .Subscribe(_ =>
             {
                 if (CheckEmptyPoint() & (EventTimer.instance.startTimer && EventTimer.instance.CurrentTime > 0.0f))
                 {
                     var target = pool.GetInstance();
                     var point = GetSpawnPoint();

                     target.transform.position = point.transform.position;
                     target.transform.parent = point.transform;
                 }
             }
             );



   }
	
    private bool CheckEmptyPoint()
    {

        foreach(GameObject obj in targetpoints)
        {
            if(obj.transform.childCount == 0)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetSpawnPoint()
    {
        GameObject[] emptyPoints = targetpoints.Where(o => o.transform.childCount == 0)
                                               .ToArray();

        if (emptyPoints.Length == 0)
            return null;

        int num = Random.Range(0, emptyPoints.Length - 1);

        return emptyPoints[num];
    }
}
