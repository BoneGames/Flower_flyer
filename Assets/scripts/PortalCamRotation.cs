using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamRotation : MonoBehaviour
{
    Transform playerCamera;
    public Transform basePos;
    public Transform renderScreenPos;
    Camera cam;
    public Vector3 offset;

    private void Awake()
    {
        if (!playerCamera)
            playerCamera = Camera.main.transform;

        cam = GetComponent<Camera>();
        offset = (renderScreenPos.position - basePos.position) / 10;
    }


    // Update is called once per frame
    bool enabledLastFrame;
    void LateUpdate()
    {
        if (cam.enabled != enabledLastFrame)
        {
            print(transform.name + " was just " + (cam.enabled ? "enabled" : "disabled"));
        }
        if (cam.enabled)
        {
            enabledLastFrame = true;
            Vector3 playerOffsetFromPortal = playerCamera.position - renderScreenPos.position;
            transform.position = basePos.position + playerOffsetFromPortal + offset;
            transform.rotation = playerCamera.transform.rotation;
        }
        else
        {
            enabledLastFrame = false;
        }
    }
}




//float angularDiff = Quaternion.Angle(opposite.rotation, otherPortal.rotation);

//Quaternion portalRotDiff = Quaternion.AngleAxis(angularDiff, Vector3.up);

//Vector3 newCamDir = portalRotDiff * playerCamera.forward;
//transform.rotation = Quaternion.LookRotation(newCamDir, Vector3.up);