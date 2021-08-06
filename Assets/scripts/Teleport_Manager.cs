using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport_Manager : MonoBehaviour
{
    public bool debug;
    public List<Portal> portals = new List<Portal>();
    public float shutdown_delay;
    public bool invertRe_entry;
    public Transform camsParent, textParent;
    float scale;
    public bool isRendered;
    public Transform centreTile;
    UI ui;
    public float tileTeleportAmount;

    private void Awake()
    {
        ui = FindObjectOfType<UI>();
        scale = centreTile.localScale.x * tileTeleportAmount * 10;

        camsParent.gameObject.SetActive(isRendered);
    }

    public Teleport_Manager RegisterPortal(Portal _portal)
    {
        if (!portals.Contains(_portal))
            portals.Add(_portal);

        _portal.portalCam = camsParent.Find(_portal.transform.name).GetComponent<PortalCamRotation>();
        return this;
    }

    public void TeleportRendered(Transform player, Portal entry, Portal re_entry, Vector3 landingOffset)
    {
        entry.active = false;
        re_entry.active = false;
        StartCoroutine(ShutdownDelay(entry, re_entry));
        Vector3 relativeOffset = player.transform.position - entry.transform.position;
        if (debug)
        {
            Debug.Log("Teleporting from: " + entry.name + " to " + re_entry.name);
            Debug.Log(relativeOffset);
        }
        //player.position = re_entry.transform.position + (invertRe_entry ? InverseAxis(re_entry_point, entry.xAxis) : re_entry_point) + landingOffset;
        player.position = re_entry.transform.position + relativeOffset;

    }

    public void TeleportNoRender(Transform player, Portal entry, Portal re_entry)
    {
        if(debug)
            Debug.Log("Teleporting from: " + entry.name + " to " + re_entry.name);
        // Debug.LogWarning("posChck: " + player.position.ToString());
        //Debug.LogWarning("Pre teleport relative position: " + (player.transform.position - GetTilePos(player)).ToString());
        //float col // - player.GetComponent<Collider>().bounds.extents.mag;

        Vector3 dir = (re_entry.transform.position - entry.transform.position).normalized;
        Vector3 translation = dir * scale;


        //Debug.Log("translation: " + translation);
        //Debug.Log("pre-pos: " + player.position);
        player.position += translation;
        //Debug.LogWarning("posChck: " + player.position.ToString());
        //Debug.LogWarning("Post teleport relative position: " + (player.transform.position - GetTilePos(player)).ToString());
        //Debug.Log("post-pos: " + player.position);
    }

    //Vector3 GetTilePos(Transform player)
    //{
    //    if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("tile")))
    //    {
    //        return hit.transform.position;
    //    }
    //    Debug.LogError("raycast for tile failed");
    //    return Vector3.zero;
    //}

    //Vector3 InverseAxis(Vector3 eD, bool xAxis)
    //{
    //    return xAxis ? new Vector3(eD.x, eD.y, eD.z * -1) : new Vector3(eD.x * -1, eD.y, eD.z);
    //}

    IEnumerator ShutdownDelay(Portal entry, Portal exit)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        entry.active = true;
        exit.active = true;
    }

    public void ActivatePortalCam(List<string> tagsToRender)
    {
        foreach (Portal p in portals)
        {
            bool activate = tagsToRender.Contains(p.portalCam.tag);
            if (activate)
            {
                p.portalCam.enabled = true;
                //Debug.Log(p.portalCam.transform.name + " enabled");
            }
            else
            {
                p.portalCam.enabled = false;
            }

            if (ui != null)
                ui.SetActiveCamDisplay(p.transform.name, p.portalCam.enabled);
        }
    }

}
