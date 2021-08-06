using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class flight_controller_V2 : MonoBehaviour
{
    public bool stationary = false;
    public UnityStandardAssets.Characters.FirstPerson.BoostCheck boostControl;
    //[BoxGroup("Boost Values")] public float lastBoostStart = -10;
    //[BoxGroup("Boost Values")] public float lastBoostEnd = 0;
    //[BoxGroup("Boost Values")] public float boostTime;
    //[BoxGroup("Boost Values")] public float boostCoolOff;
    //[BoxGroup("Boost Values")] public AnimationCurve IncreaseCurve;
    //[BoxGroup("Boost Values")] public AnimationCurve DecreaseCurve;
    //[HideInInspector] public UI ui;
    //[BoxGroup("Boost Values")] public float transitionTime;

    Rigidbody rigid;
    //public PostProcessVolume pPV;
    

    [BoxGroup("Flight Values")] public float currSpeed;
    //[BoxGroup("Flight Values")] public bool isBoosting;
    //[BoxGroup("Flight Values")] public Vector2 speeds;
    [BoxGroup("Flight Values")] public bool canVertMove;
    [BoxGroup("Flight Values")] public float maxPitch;
    [BoxGroup("Flight Values")] public float maxRoll;
    [BoxGroup("Flight Values")] public float angularVelocity;
    [BoxGroup("Flight Values")] public float targetRoll;
    [Range(0f, 0.05f)]
    [BoxGroup("Flight Values")]
    public float pitchRollSpeed;
    [BoxGroup("Flight Values")] public float turnLRSpeed;
    [BoxGroup("Flight Values")] public AnimationCurve rollCurve;

    public Coroutine boost;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
    }
    void Start()
    {
        SetCursor();
        boostControl.OnStart(this, FindObjectOfType<UI>());
    }

    Vector2 GetMouseLook()
    {
        Vector2 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;
        mousePos *= 2;
        return new Vector2(mousePos.x, mousePos.y);
    }

    private void Update()
    {
         if (Input.GetKeyDown(KeyCode.Space))
            stationary = !stationary;
    }
    void FixedUpdate()
    {
        if (stationary)
        {
            rigid.velocity = Vector3.zero;
            return;
        }


        Vector3 forwardDir = canVertMove ? transform.forward : new Vector3(transform.forward.x, 0, transform.forward.z);
        currSpeed = boostControl.lerpSpeed;
        rigid.velocity = forwardDir * Time.deltaTime * currSpeed;

        Vector2 mouseLook = GetMouseLook();

        angularVelocity = (-mouseLook.x / Time.deltaTime) / 50f;
        angularVelocity *= rollCurve.Evaluate(Mathf.Abs(angularVelocity));
        targetRoll = angularVelocity * maxRoll;

        Vector3 localRot = transform.localRotation.eulerAngles;
        Quaternion currRot = Quaternion.Euler(localRot);
        Quaternion targetRot = Quaternion.Euler(new Vector3(-mouseLook.y * maxPitch, localRot.y + mouseLook.x * turnLRSpeed, targetRoll));

         transform.rotation = Quaternion.Lerp(currRot, targetRot, pitchRollSpeed);
    }

    void SetCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("vertStopN"))
        {
            Debug.Log("vstop Top Barrier");
            StartCoroutine(VerticalStop(true));
        }
        else if (other.transform.CompareTag("vertStopS"))
        {
            Debug.Log("vstop Bottom Barrier");
            StartCoroutine(VerticalStop(false));
        }
    }

    IEnumerator VerticalStop(bool top)
    {
        //Debug.LogError("starting y freeze. top: " + top);
        canVertMove = false;
        if (top)
        {
            while (transform.forward.y >= 0)
            {
                yield return null;
            }
        }
        else
        {
            while (transform.forward.y <= 0)
            {
                yield return null;
            }
        }
        //Debug.LogError("end y freeze. top: " + top);
        canVertMove = true;
    }
    //private float BoostCheck()
    //{
    //    bool wasBoosting = isBoosting;

    //    bool canboost = boostControl.CanBoost(wasBoosting);

    //    bool canMAXboost = false;

    //    if (canboost)
    //        canMAXboost = boostControl.CanMAXboost();

    //    isBoosting = Input.GetMouseButton(0) && canboost;

    //    boostControl.SetBoostTimes(!isBoosting, !wasBoosting);

    //    currSpeed = boostControl.SetSpeed(speeds.x, speeds.y, !isBoosting, !wasBoosting, this);
    //}
}