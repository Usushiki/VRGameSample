using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveReference;

namespace ViveReference
{
    [System.Serializable]
    public enum WITCH_HANDS
    {
        LEFT = 0,
        RIGHT = 1,
    }

}




public class ViveReferenceGetters : MonoBehaviour
{

    

  
    #region Member

    [Header("Reference parameters PLEASE SET!!!")]

    [Tooltip("Left Controller")]
    [SerializeField]
    private GameObject leftController;
    [SerializeField]
    private SteamVR_TrackedObject leftControllerTracked;



    [Tooltip("Right Controller")]
    [SerializeField]
    private GameObject rightController;



    

    private int leftControllerIndex = -1;
    private int rightControllerIndex = -1;

    SteamVR_Controller.Device leftInput = null;
    SteamVR_Controller.Device rightInput = null;

    public static ViveReferenceGetters instance = null;

    private bool started = false;

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
        if (started)
            return;

        started = true;

        if (leftController == null) throw new UnityException("left Hand Game Object is not assigned.");
        if (leftControllerTracked == null) throw new UnityException("left Hand SteamVR_TrackedObject is not assingned.");

        if (rightController == null) throw new UnityException("right hand game object is not assingned.");
               	
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    //LeftController
    public static GameObject GetControllerObject(WITCH_HANDS witch_Hands)
    {
        return witch_Hands == WITCH_HANDS.LEFT ? instance.leftController : instance.rightController;
    }
    public static Transform GetLeftControllerTransform(WITCH_HANDS witch_Hands)
    {
        return witch_Hands == WITCH_HANDS.LEFT ? instance.leftController.transform : instance.rightController.transform;
    }
    public static SteamVR_Controller.Device GetLeftControllerInputDevice(WITCH_HANDS witch_Hands)
    {
        SteamVR_Controller.Device input = (witch_Hands == WITCH_HANDS.LEFT) ? instance.leftInput : instance.rightInput;

        if (input != null)
            return input;

        string t = (witch_Hands == WITCH_HANDS.LEFT) ? "Left Input" : "Right Input";
        Debug.LogWarning("WARNING:" + t + "dies not exist in ViveReferenceGetters.cs");


        return null;
    }


}
