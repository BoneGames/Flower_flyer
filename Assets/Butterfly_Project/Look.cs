using UnityEngine;

public class Look : MonoBehaviour
{
    //[BoxGroup("Flight Values")] 
    public float maxPitch;
    //[BoxGroup("Flight Values")] 
    public float maxRoll;
    //[BoxGroup("Flight Values")] 
    public float angularVelocity;
    //[BoxGroup("Flight Values")] 
    public float targetRoll;
    [Range(0f, 0.2f)]
    //[BoxGroup("Flight Values")]
    public float pitchRollSpeed;
    //[BoxGroup("Flight Values")] 
    public float turnLRSpeed;
    //[BoxGroup("Flight Values")] 
    public AnimationCurve rollCurve;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
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


    void FixedUpdate()
    {
        Vector2 mouseLook = GetMouseLook();

        angularVelocity = (-mouseLook.x / Time.deltaTime) / 50f;
        angularVelocity *= rollCurve.Evaluate(Mathf.Abs(angularVelocity));
        targetRoll = angularVelocity * maxRoll;

        Vector3 localRot = transform.localRotation.eulerAngles;
        Quaternion currRot = Quaternion.Euler(localRot);
        Quaternion targetRot = Quaternion.Euler(new Vector3(-mouseLook.y * maxPitch, localRot.y + mouseLook.x * turnLRSpeed, targetRoll));

        transform.rotation = Quaternion.Lerp(currRot, targetRot, pitchRollSpeed);
    }
}