using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class Stalk_Stacker : MonoBehaviour
{
    public GameObject cylinder, baseObject;
    public int sections;
    public int sectionHeightAdjust = 1;
    [Range(0f,0.2f)]
    public float overlap;

    public string baseName;
    public string sectionName = "stalk_";
    public string pivotName = "pivot_";

    // Start is called before the first frame update
    //[Button]
    public void Build()
    {
        // create base transform object
        GameObject _base = Instantiate(baseObject, transform.position, Quaternion.identity);
        _base.transform.name = baseName;

        // init vars
        Vector3 pos = transform.position;
        float halfHeight = ((cylinder.transform.localScale.y) * sectionHeightAdjust) * (1f-overlap);
        Transform parent = _base.transform;

        // create sections
        for (int i = 0; i < sections; i++)
        {
            // add new cylinder section
            GameObject section = Instantiate(cylinder, pos + new Vector3(0, halfHeight, 0), Quaternion.identity, parent);
            section.transform.name = sectionName + (i + 1).ToString();

            // add new pivot
            GameObject nextPivot = new GameObject();
            nextPivot.transform.position = pos + new Vector3(0, halfHeight * 2, 0);
            nextPivot.transform.rotation = Quaternion.identity;
            nextPivot.transform.parent = parent;
            nextPivot.transform.name = pivotName + (i + 1).ToString();

            // update vals for next iteration
            parent = nextPivot.transform;
            pos = parent.position;
        }

        _base.transform.GetComponent<MechSway>().GetRotateTransforms();
        _base.transform.GetComponent<StemScaleAdjust>().sway = _base.transform.GetComponent<MechSway>();
    }
}
