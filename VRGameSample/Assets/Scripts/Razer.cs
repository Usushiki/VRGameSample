using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer : MonoBehaviour
{

    #region Member

    [Header("Pointer Setting", order = 3)]
    [Tooltip("ビームの厚さ")]
    public float pointerThickness = 0.002f;

    [Tooltip("ビームの長さ")]
    public float pointerLength = 100f;

    [Tooltip("ビーム表示フラグ")]
    public bool isShowPointerTip = true;


    [Header("Custom pointer Setting", order = 4)]
    [Tooltip("ポインターオブジェクトのカスタム　デフォルトはSphere")]
    public GameObject customPointerCursor;

    [Tooltip("ポインタ方向を正面に合わせるか")]
    public bool poiterCursorMatchTargetNormal = false;

    [Tooltip("ポインターカーソルを距離に合わせてりうけーリングするか")]
    public bool pointerCursorRescaledAlongDistance = false;


    private GameObject pointerHolder;
    private GameObject pointerrBeam;
    private GameObject pointerTip;
    private Vector3 pointerTipScale = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 pointerCursorOriginalScale = Vector3.one;

    #endregion


    public void OnEnable()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
