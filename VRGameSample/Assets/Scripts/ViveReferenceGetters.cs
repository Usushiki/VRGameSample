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
    [SerializeField]
    private SteamVR_TrackedObject rightControllerTracked;






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
        if (rightControllerTracked == null) throw new UnityException("right hand SteamVR_TrackedObject is not assingned");       	

	}
	
	// Update is called once per frame
	void Update ()
    {

        if (leftControllerTracked == null || rightControllerTracked == null)
            throw new UnityException("Left or/and Right SteamVR Tracker is nor assigned");

        //コントローラーIDの更新
        leftControllerIndex = (int)leftControllerTracked.index;
        rightControllerIndex = (int)rightControllerTracked.index;

        //コントローラーの入力状態の更新
        if(leftControllerIndex > -1)leftInput = SteamVR_Controller.Input(leftControllerIndex);
        if(rightControllerIndex > -1)rightInput = SteamVR_Controller.Input(rightControllerIndex);
	}


    /// <summary>
    /// コントローラーのGameObjectを取得
    /// </summary>
    /// <param name="witch_Hands">どっちの手か</param>
    /// <returns></returns>
    public static GameObject GetControllerObject(WITCH_HANDS witch_Hands)
    {
        return witch_Hands == WITCH_HANDS.LEFT ? instance.leftController : instance.rightController;
    }

    /// <summary>
    /// コントローラーのTrackedObjectコンポーネントを取得
    /// </summary>
    /// <param name="witch_Hands">どっちの手か</param>
    /// <returns></returns>
    public static SteamVR_TrackedObject GetControllerTrckedObjectComponent(WITCH_HANDS witch_Hands)
    {
        return (witch_Hands == WITCH_HANDS.LEFT) ? instance.leftControllerTracked : instance.rightControllerTracked;
    }

    /// <summary>
    /// コントローラーのTransformを取得
    /// </summary>
    /// <param name="witch_Hands"><どっちの手か/param>
    /// <returns></returns>
    public static Transform GetLeftControllerTransform(WITCH_HANDS witch_Hands)
    {
        return witch_Hands == WITCH_HANDS.LEFT ? instance.leftController.transform : instance.rightController.transform;
    }

    /// <summary>
    /// コントローラーの入力デバイスを取得
    /// </summary>
    /// <param name="witch_Hands">どっちの手か</param>
    /// <returns></returns>
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
