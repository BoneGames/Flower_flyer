using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalViewDetect : MonoBehaviour
{
    // Start is called before the first frame update
    public float distance;
    public int layer_mask;
    //Vector3 l_FOV, r_FOV, FOV;
    public List<string> visibleTags = new List<string>();
    public List<Vector3> lookDirs = new List<Vector3>();
    Teleport_Manager tM;

    private void Awake()
    {
        if (!tM)
            tM = FindObjectOfType<Teleport_Manager>();

        layer_mask = LayerMask.GetMask("Portals");
        distance = Camera.main.farClipPlane;

        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!tM)
            return;
        if (!tM.isRendered)
            return;

     

        visibleTags.Clear();
        lookDirs.Clear();

        lookDirs = new List<Vector3>() {
            (Quaternion.Euler(0, -Camera.main.fieldOfView, 0) * transform.forward), // left
            Quaternion.Euler(0, Camera.main.fieldOfView, 0) * transform.forward, // right
            transform.forward }; // forward

        //if (!Input.GetKeyDown(KeyCode.Space))
        //    return;

        foreach (Vector3 dir in lookDirs)
        {
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, distance, layer_mask))
            {
                visibleTags.Add(hit.transform.tag);
            }
        }

        if (visibleTags.Count > 0)
        {
            foreach (var item in visibleTags)
            {
                print("visTAG: " + item);
            }
            Debug.Log(visibleTags.ToString());
            tM.ActivatePortalCam(visibleTags);
        }
    }

    private void OnDrawGizmos()
    {
        if(!tM)
            tM = FindObjectOfType<Teleport_Manager>();
        if (!tM.isRendered)
            return;
        if (lookDirs.Count > 2)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + lookDirs[0] * distance);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + lookDirs[1] * distance);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
        }
    }
}
