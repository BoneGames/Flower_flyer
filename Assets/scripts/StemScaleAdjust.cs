using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class StemScaleAdjust : MonoBehaviour
{
    public MechSway sway;
    public Vector3 scale;
    public GameObject[] leaves;
    [Range(1, 5)]
    public int sectionChange;
    public List<Transform> stalks = new List<Transform>();

    public void SpineCompress(bool compress)
    {
        print("SpineCompress");
        GetRotateTransforms();
        float multi = compress ? 0.95f : 1;

        foreach (var item in stalks)
        {
            item.localPosition = new Vector3(item.localPosition.x, item.localScale.y * multi, item.localPosition.z);
        }

    }

    public void GetRotateTransforms()
    {
        stalks.Clear();
        Transform t = transform;
        while (true)
        {
            try
            {
                if (!t.name.ToLower().Contains("seed"))
                    stalks.Add(t.GetChild(0));
                t = t.GetChild(1);
            }
            catch
            {
                break;
            }
        }
    }

    //[Button]
    public void Resize()
    {
        foreach (var item in sway.pivots)
        {
            Transform section = item.GetChild(0);
            if (section.name.Contains("stalk"))
            {
                section.localScale = scale;
                section.localPosition = new Vector3(0, scale.y, 0);
            }
            item.localPosition = new Vector3(0, scale.y * 2 * (0.95f), 0);
        }
    }
    public int sectionOperateIndex;
    //[Button]
    public void AddSection()
    {
        for (int i = 0; i < sectionChange; i++)
        {


            if (sectionOperateIndex < 1 || sectionOperateIndex >= sway.pivots.Count)
            {
                Debug.LogError("cant add section at index that is larger than total number opf sections or smaller than 1");
                return;
            }
            // separate insertion point
            Transform detached = sway.pivots[sectionOperateIndex];
            Vector3 displacement = detached.localPosition;
            Transform _base = detached.parent;
            detached.parent = null;

            // Get new section from top
            GameObject newSection = Instantiate(sway.pivots[sway.pivots.Count - 2].gameObject);
            // remove flower stuff beyond it
            DestroyImmediate(newSection.transform.GetChild(1).gameObject);

            // attach new section to base
            newSection.transform.parent = _base.transform;
            newSection.transform.localScale = new Vector3(1, 1, 1);
            newSection.transform.localPosition = displacement;
            // re-attach top section to new section
            detached.parent = newSection.transform;

            // shift up stalk to accomodate new section
            detached.localPosition = displacement;

            // reset scale
            detached.transform.localScale = new Vector3(1, 1, 1);

            // insert new pivot into sway List
            sway.pivots.Insert(sectionOperateIndex, newSection.transform);
        }
        RenameSections();
    }
    //[Button]
    public void RemoveSection()
    {
        for (int i = 0; i < sectionChange; i++)
        {


            if (sectionOperateIndex < 1 || sectionOperateIndex >= sway.pivots.Count)
            {
                Debug.LogError("cant add section at index that is larger than total number opf sections or smaller than 1");
                return;
            }
            // get sections
            Transform t1 = sway.pivots[sectionOperateIndex - 1];
            Transform t2 = sway.pivots[sectionOperateIndex];
            Transform t3 = sway.pivots[sectionOperateIndex + 1];
            // get distance
            Vector3 displacement = t2.localPosition;
            // disconnect
            t2.parent = null;
            t3.parent = null;
            // remove center
            DestroyImmediate(t2.gameObject);
            // re-attach 
            t3.parent = t1;
            t3.localPosition = displacement;
            // update sway list
            sway.pivots.RemoveAt(sectionOperateIndex);
        }
        RenameSections();
    }

    //[Button]
    public void AddLeaf()
    {
        // get random section
        GameObject section = sway.pivots[Random.Range(3, sway.pivots.Count - 6)].GetChild(0).gameObject;

        // get position on edge
        Collider col = section.GetComponent<Collider>();
        // random angle
        Vector3 dir = new Vector3(Random.Range(-col.bounds.size.x * 2, col.bounds.size.x * 2), 0, Random.Range(-col.bounds.size.x * 2, col.bounds.size.x * 2));
        // ensure its big enough to get beyond edge
        while (dir.magnitude < (col.bounds.size.x * 2))
        {
            dir = new Vector3(Random.Range(-col.bounds.size.x * 2, col.bounds.size.x * 2), 0, Random.Range(-col.bounds.size.x * 2, col.bounds.size.x * 2));
        }
        // get touching point on stalk
        Vector3 edgePos = col.ClosestPoint(section.transform.position + dir);

        // add leaf
        GameObject leaf = Instantiate(leaves[Random.Range(0, leaves.Length)], edgePos, Quaternion.identity);

        // make face outward
        leaf.transform.forward = edgePos - section.transform.position;
        leaf.transform.parent = section.transform;
    }
    //[Button]
    public void RenameSections()
    {
        for (int i = 0; i < sway.pivots.Count; i++)
        {
            sway.pivots[i].name = "pivot_" + (i).ToString();

            sway.pivots[i].GetChild(0).name = "stalk_" + (i + 1).ToString();
        }
    }

}
