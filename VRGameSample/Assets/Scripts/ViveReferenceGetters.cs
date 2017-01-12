using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveReferenceGetters : MonoBehaviour
{

    #region Member

    [Header("Reference parameters PLEASE SET!!!")]

    [Tooltip("Left Controller")]
    [SerializeField]
    private GameObject leftController;

    [Tooltip("Right Controller")]
    [SerializeField]
    private GameObject rightController;



    private int leftControllerIndex = -1;
    private int rightControllerIndex = -1;

    public static ViveReferenceGetters instance = null;

    #endregion 


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    //LeftController
    public GameObject GetLeftControllerObject()
    {
        return leftController;
    }
    public Transform GetLeftControllerTransform()
    {
        return leftController.transform;
    }
    public SteamVR_Controller.Device GetLeftControllerInputDevice()
    {

        SteamVR_TrackedObject trackedObject = leftController.GetComponent<SteamVR_TrackedObject>();
        return SteamVR_Controller.Input((int)trackedObject.index);
    }

    //Right Controller
    public GameObject GetRightControllerObject()
    {
        return rightController;
    }
    public Transform GetRightControllerTransform()
    {
        return rightController.transform;
    
    }
    public SteamVR_Controller.Device GetRightInputDevice()
    {
        SteamVR_TrackedObject trackedObject = rightController.GetComponent<SteamVR_TrackedObject>();
        return SteamVR_Controller.Input((int)trackedObject.index);
    }

}
