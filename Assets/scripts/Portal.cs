using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    public Teleport_Manager tM;
    public Portal opposite;
    public bool xAxis, biggerThan;
    //public bool enterActive, exitActive;
    public bool active;
    public PortalCamRotation portalCam;



    private void Awake()
    {

        // get teleport Manager and register portal
        tM = FindObjectOfType<Teleport_Manager>().RegisterPortal(this);
       
        
        if(tM.isRendered)
        {
            RenderedSetup();
        }

        active = true;
    }

    void RenderedSetup()
    {
        // get opposite
        if (!opposite && Physics.Raycast(transform.position, transform.up, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Portals")))
        {
            opposite = hit.transform.GetComponent<Portal>();
            if(tM.debug)
                Debug.Log(transform.name + " didn't have it's opposite portal. Fetched with raycast");
        }

        xAxis = -transform.up.x != 0;
        if (xAxis)
        {
            biggerThan = -transform.up.x > 0;
        }
        else
        {
            biggerThan = -transform.up.z > 0;
        }
        // enterActive = exitActive = true;

        Cam_Mat_Setup();
    }

    void Cam_Mat_Setup()
    {
        // create new mat
        Material _mat = new Material(Shader.Find("Unlit/ScreenCutoutShader"));
        // apply mat to portal wall
        GetComponent<MeshRenderer>().material = _mat;
        // remove texture from cam if exists
        Camera _cam = portalCam.GetComponent<Camera>();
        if (_cam.targetTexture != null)
            _cam.targetTexture.Release();
        
        // assign new RT to portal cam
        _cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        // assign texture to mat
        _mat.mainTexture = _cam.targetTexture;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.up * 100);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (tM.debug)
            Debug.Log("In " + transform.name + " Portal Trigger");
        if (other.gameObject.CompareTag("Player"))
        {
            if (active)
            {
                if(tM.isRendered)
                    tM.TeleportRendered(other.transform, this, opposite, portalCam.offset);
                else
                    tM.TeleportNoRender(other.transform, this, opposite);
            }
            else
            {
                if (tM.debug)
                    Debug.Log("Portal: " + transform.name + " Not Active");
            }
        }
        else
        {
            if (tM.debug)
                Debug.Log("not player in trigger");
        }
    }

    IEnumerator FrameDelay(Collider other)
    {
        yield return new WaitForEndOfFrame();
        if (other.gameObject.CompareTag("Player") && active)
        {
            string portalName = transform.name;
            // check that player is going out backside of trigger
            Vector3 playerForward = Camera.main.transform.forward;
            if (xAxis)
            {
                if (biggerThan)
                {
                    if (playerForward.x > 0)
                    {
                        if (tM.debug)
                            Debug.LogError("Backside teleport");
                        tM.TeleportRendered(other.transform, this, opposite, portalCam.offset);
                    }
                }
                else
                {
                    if (playerForward.x < 0)
                    {
                        if (tM.debug)
                            Debug.LogError("Backside teleport");
                        tM.TeleportRendered(other.transform, this, opposite, portalCam.offset);
                    }
                }
            }
            else
            {
                if (biggerThan)
                {
                    if (playerForward.z > 0)
                    {
                        if (tM.debug)
                            Debug.LogError("Backside teleport");
                        tM.TeleportRendered(other.transform, this, opposite, portalCam.offset);
                    }
                }
                else
                {
                    if (playerForward.z < 0)
                    {
                        if (tM.debug)
                            Debug.LogError("Backside teleport");
                        tM.TeleportRendered(other.transform, this, opposite, portalCam.offset);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(tM.isRendered)
            StartCoroutine(FrameDelay(other));
    }
}
