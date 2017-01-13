using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer : MonoBehaviour
{

    [System.Serializable]
    public enum WITCH_HANDS
    {
        LEFT= 0,
        RIGHT = 1,
    }

    [System.Serializable]
    public enum POINTER_VISIBILITY
    {
        ON_WHEN_ACTIVE,
        ALWAYS_ON,
        OFF
    }

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

    [Tooltip("ポインターのマテリアル　デフォルトはworldPointer")]
    public Material pointerMaterial;

    [Tooltip("ポインタ方向を正面に合わせるか")]
    public bool pointerCursorMatchTargetNormal = false;

    [Tooltip("ポインターカーソルを距離に合わせてりスケーリングするか")]
    public bool pointerCursorRescaledAlongDistance = false;

    [Tooltip("どっちの手か")]
    public WITCH_HANDS witchHands = WITCH_HANDS.LEFT;

    [Tooltip("ポインターの表示タイミング")]
    public POINTER_VISIBILITY pointerVisibility = POINTER_VISIBILITY.ALWAYS_ON;

    [Tooltip("無視するlayer")]
    public LayerMask layersIgnore = Physics.IgnoreRaycastLayer;

    private GameObject pointerHolder;
    private GameObject pointerBeam;
    private GameObject pointerTip;
    private Vector3 pointerTipScale = new Vector3(0.05f, 0.05f, 0.05f);
    private Vector3 pointerCursorOriginalScale = Vector3.one;
    private Vector3 destinationPosition = Vector3.zero;

    private RaycastHit pointerConstantRaycastHit = new RaycastHit();

    private Transform pointerOriginTransform;
    private Transform pointerConstantTarget = null;

    private float pointerConstantDistance = 0f;
    private bool activeEnable;
    private bool storedBeamState;
    private bool storedTipState;
    #endregion


    public void OnEnable()
    {
        pointerOriginTransform = (pointerOriginTransform == null ? 
                                    (witchHands == WITCH_HANDS.LEFT) ? 
                                    ViveReferenceGetters.instance.GetLeftControllerTransform() 
                                    : ViveReferenceGetters.instance.GetRightControllerTransform() 
                                 : pointerOriginTransform);

        var tmpMterial = Resources.Load("worldPointer") as Material;
        if(pointerMaterial != null)
        {
            tmpMterial = pointerMaterial;
        }

        pointerMaterial = new Material(tmpMterial);
        

    }

    public void OnDisable()
    {
        if(pointerHolder != null)
        {
            Destroy(pointerHolder);
        }
    }


	
	// Update is called once per frame
	void Update ()
    {
		if(pointerBeam && pointerBeam.activeSelf)
        {
            Ray pointerRaycast = new Ray(pointerOriginTransform.position, pointerOriginTransform.forward);
            RaycastHit pointerCollidedWith;

            var rayHit = Physics.Raycast(pointerRaycast, out pointerCollidedWith, pointerLength,~layersIgnore);
            var pointerBeamLength = GetPointerBeamLength(rayHit,pointerCollidedWith);
            SetPointerTransform(pointerBeamLength, pointerThickness);

            if(rayHit)
            {
                if(pointerCursorMatchTargetNormal)
                {
                    pointerTip.transform.forward = -pointerCollidedWith.normal;
                }

                if(pointerCursorRescaledAlongDistance)
                {
                    float collisionDistance = Vector3.Distance(pointerCollidedWith.point, pointerOriginTransform.position);
                    pointerTip.transform.localScale = pointerCursorOriginalScale *collisionDistance;
                }
            }
            else
            {
                if(pointerCursorMatchTargetNormal)
                {
                    pointerTip.transform.forward = pointerOriginTransform.forward;
                }
                if(pointerCursorRescaledAlongDistance)
                {
                    pointerTip.transform.localScale = pointerCursorOriginalScale * pointerBeamLength;    
                }
            }

            if(activeEnable)
            {
                activeEnable = false;
                pointerBeam.GetComponentInChildren<Renderer>().enabled = storedBeamState;
                pointerTip.GetComponentInChildren<Renderer>().enabled = storedTipState;
            }
        }
	}



    private  void InitPointer()
    {
        pointerHolder = GameObject.Find(string.Format("PointerHolder"));
        pointerHolder.transform.localPosition = Vector3.zero;

        pointerBeam = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointerBeam.transform.name = string.Format("PointerHolder",gameObject.name);
        pointerBeam.transform.SetParent(pointerHolder.transform);
        pointerBeam.GetComponent<BoxCollider>().isTrigger = true;
        pointerBeam.GetComponent<Rigidbody>().isKinematic = true;
        pointerBeam.layer = LayerMask.NameToLayer("Ignore Raycast");

        var pointerRenderer = pointerBeam.GetComponent<MeshRenderer>();
        pointerRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        pointerRenderer.receiveShadows = false;
        pointerRenderer.material = pointerMaterial;


        if(customPointerCursor)
        {
            pointerTip = Instantiate(customPointerCursor);
        }
        else
        {
            pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerTip.transform.localScale = pointerTipScale;

            var pointerTipRenderer = pointerTip.GetComponent<MeshRenderer>();
            pointerTipRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            pointerTipRenderer.receiveShadows = false;
            pointerTipRenderer.material = pointerMaterial;
        }

        pointerCursorOriginalScale = pointerTip.transform.localScale;
        pointerTip.transform.name = string.Format("Pointer", gameObject.name);
        pointerTip.transform.SetParent(pointerHolder.transform);
        pointerTip.GetComponent<Collider>().isTrigger = true;
        pointerTip.GetComponent<Rigidbody>().isKinematic = true;
        pointerTip.layer = LayerMask.NameToLayer("Ignore Raycast");


        SetPointerTransform(pointerLength, pointerThickness);
        TogglePointer(false);

    }

    private void TogglePointer(bool state)
    {
        state = (pointerVisibility == POINTER_VISIBILITY.ALWAYS_ON ? true : false);

        if(pointerBeam)
        {
            pointerBeam.SetActive(state);
        }
        var tipState = (isShowPointerTip ? state : false);
        if(pointerTip)
        {
            pointerTip.SetActive(tipState);
        }
    }

    private void SetPointerTransform(float setlength,float setThickness)
    {
        var beamPointer = setlength / (2 + 0.00001f);

        pointerBeam.transform.localScale = new Vector3(setThickness, setThickness, setlength);
        pointerBeam.transform.localPosition = new Vector3(0f, 0f, beamPointer);
        pointerTip.transform.localPosition = new Vector3(0f, 0f, setlength - (pointerTip.transform.localScale.z / 2));


        pointerHolder.transform.localPosition = pointerOriginTransform.localPosition;
        pointerHolder.transform.localRotation = pointerOriginTransform.localRotation;
        pointerHolder.transform.position = transform.position;
        pointerHolder.transform.rotation = transform.rotation;
    }

    private float GetPointerBeamLength(bool hasRayHit,RaycastHit collidedWith)
    {
        var actualLemgth = pointerLength;

        if(!hasRayHit || (pointerConstantRaycastHit.collider && pointerConstantRaycastHit.collider != collidedWith.collider))
        {
            if(pointerConstantRaycastHit.collider != null)
            {
                //PointerOut
            }
            pointerConstantDistance = 0f;
            pointerConstantTarget = null;
            pointerConstantRaycastHit = new RaycastHit();
            destinationPosition = Vector3.zero;


        }


        if(hasRayHit)
        {
            pointerConstantDistance = collidedWith.distance;
            pointerConstantTarget = collidedWith.transform;
            pointerConstantRaycastHit = collidedWith;

            //PointerIn
        }

        if(hasRayHit && pointerConstantDistance  <pointerLength)
        {
            actualLemgth = pointerConstantDistance;
        }

        return actualLemgth;
    }
}
