using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;


public class EventTimer : MonoBehaviour
{
    #region Member

    public bool startTimer = true;

    [SerializeField]
    private float time = 60.0f;


    private float calcTimer;


    public float CurrentTime
    {
        get
        {
            return calcTimer;
        }
       
    }


    public static EventTimer instance;
    #endregion



    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        calcTimer = time; 

        this.UpdateAsObservable()
            .Where(_=> calcTimer > 0.0f)
            .Subscribe(_ =>
            {
                calcTimer -= Time.deltaTime;
                calcTimer = Mathf.Clamp(calcTimer, 0.0f, time);
            }
            );

	}
	
}
