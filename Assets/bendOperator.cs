using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class bendOperator : MonoBehaviour
{
    bool editFlowersChanged;
    public bool editFlowers, windOn, normaliseDir;
    List<MechSway> allStalks = new List<MechSway>();
    List<MechSway> movingStalks = new List<MechSway>();
    public MechSway tester;

    public Transform centreTile;
    public Transform[] edgeTiles;
    public Vector2 aoe;
    public float rate;
    public int movingQuantity;
    public int newStalkGroupRefreshRate;


    public int width = 256;
    public int height = 256;
    public float x, y;
    public float scale;
    public float offset_NS, offset_EW;

    private void Awake()
    {
        if (!windOn)
            return;
        // Get All the stalks
        allStalks = GetAllStalks(false);


        x = y = 0;
        offset_NS = UnityEngine.Random.Range(0, 9999);
        offset_EW = UnityEngine.Random.Range(0, 9999);
    }

    List<MechSway> GetAllStalks(bool all)
    {
        // Get All the stalks
        if(all)
        {
            return ((FindObjectsOfTypeAll(typeof(Transform)) as Transform[]).Where(o => o.GetComponent<MechSway>()).Select(o => o.GetComponent<MechSway>()).ToList());
        }
        
        List<MechSway> toReturn = new List<MechSway>();
        toReturn = centreTile.GetComponentsInChildren<MechSway>().ToList();
        toReturn.AddRange(edgeTiles.SelectMany(o => o.GetComponentsInChildren<MechSway>()));
        Debug.Log("STALKS: " + toReturn.Count);
        return toReturn;
    }

    private void OnValidate()
    {
        if (editFlowers != editFlowersChanged)
        {
            foreach (var item in GetAllStalks(true))
            {
                item.editing = editFlowers;
            }
            editFlowersChanged = editFlowers;
        }
    }

    private void Start()
    {
        if (!tester)
            InvokeRepeating("GetNewStalks", 2f, newStalkGroupRefreshRate);
    }

    void GetNewStalks()
    {
        print("getting new stalks");
        movingStalks.Clear();
        movingStalks = allStalks.OrderBy(x => Guid.NewGuid()).Take(movingQuantity).ToList();
    }

    List<Transform> close = new List<Transform>();
    Vector2 adjustment;
    public float forwardMulti;
    void Update()
    {
        if (!windOn)
            return;
        //float height = transform.parent.position.y;
        //transform.localPosition = new Vector3(transform.localPosition.x, -height, transform.localPosition.z);
        //Vector3 pos = transform.parent.position;
        //transform.position = new Vector3(pos.x, 0, pos.z);
        //transform.rotation = Quaternion.identity;
        adjustment = GetAdjustment();

        // story player vals
        float pos_x = transform.position.x + (transform.forward.x * forwardMulti);
        float pos_z = transform.position.z + (transform.forward.z * forwardMulti);

        //close.Clear();

        if (tester)
        {
            tester.AdjustBend(adjustment, normaliseDir);
            return;
        }
        // loops through stalks
        foreach (var item in movingStalks)
        {
            if (InRange(item.transform.position, pos_x, pos_z))
            {
                item.AdjustBend(adjustment, normaliseDir);
                Debug.DrawLine(item.transform.position, item.transform.position + Vector3.forward * gizmoSize, Color.red, 1f);
                //close.Add(item.transform);
                //Debug.LogError("ajust");
            }
        }
    }

    bool InRange(Vector3 pos, float pos_x, float pos_z)
    {
        float item_pos_x = pos.x;
        if ((item_pos_x > pos_x && item_pos_x < (pos_x + aoe.x / 2)) || (item_pos_x < pos_x && item_pos_x > (pos_x - aoe.x / 2)))
        {
            float item_pos_z = pos.z;
            //Debug.LogError("X");
            if ((item_pos_z > pos_z && item_pos_z < (pos_z + aoe.x / 2)) || (item_pos_z < pos_z && item_pos_z > (pos_z - aoe.x / 2)))
            {
                return true;
            }
        }
        return false;
    }



    Vector2 GetAdjustment()
    {
        float delta = Time.time * rate;
        float sheetSample = width * scale;
        float result = delta / sheetSample;
        float perlinA = ((float)Mathf.PerlinNoise(result + offset_NS, 0f) - 0.5f) * bendScale;
        float perlinB = ((float)Mathf.PerlinNoise(result + offset_EW, 0f) - 0.5f) * bendScale;
        return new Vector2(perlinA, perlinB);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 100, 200, 100), "X: " + adjustment.x.ToString());
        GUI.Label(new Rect(0, 120, 200, 100), "Y: " + adjustment.y.ToString());
    }
    public float bendScale;
    //Vector2 PerlinSample(float x, float y)
    //{
    //    return new Vector2(Mathf.PerlinNoise((float)x / width * scale + offset_NS.x, (float)y / height * scale + offset_NS.y) - 0.5f,
    //                        Mathf.PerlinNoise((float)x / width * scale + offset_EW.x, (float)y / height * scale + offset_EW.y) - 0.5f);
    //}
    public bool gizmo;
    private void OnDrawGizmos()
    {
        if (gizmo)
            Gizmos.DrawCube(transform.position + transform.forward * forwardMulti, new Vector3(aoe.x, aoe.x, aoe.x));
        //foreach (var item in close)
        //{
        //    Gizmos.DrawSphere(item.position, gizmoSize, );
        //    Gizmos.

        //}
    }
    public float gizmoSize;
}
