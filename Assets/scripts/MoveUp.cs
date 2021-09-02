using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class MoveUp : MonoBehaviour
{
    //[Button]
    public void Up()
    {
        Transform newParent = transform.parent.parent.GetChild(1).GetChild(0);
        if (newParent.name.Contains("stalk"))
        {
            Vector3 localPosition = transform.localPosition;

            transform.parent = newParent;
            transform.localPosition = localPosition;
        }

    }
    //[Button]
    public void Down()
    {
        Transform newParent = transform.parent.parent.parent.GetChild(0);
        if (newParent.name.Contains("stalk"))
        {
            Vector3 localPosition = transform.localPosition;

            transform.parent = newParent;

            transform.localPosition = localPosition;
        }
    }

    //[Button]
    public void NewAngle()
    {
        // get random section
        GameObject section = transform.parent.gameObject;

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

        // reposition
        transform.position = edgePos;

        // make face outward
        transform.forward = edgePos - section.transform.position;
    }
}
