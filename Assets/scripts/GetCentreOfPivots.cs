using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCentreOfPivots : MonoBehaviour
{
    public List<Transform> petals = new List<Transform>();
    public Vector3 centre;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 massPos = new Vector3();
        int count = 0;
        foreach (Transform t in transform)
        {
            petals.Add(t);
            massPos += t.position;
            count++;
        }
        centre = massPos / count;
        transform.DetachChildren();
        transform.position = centre;
    }
}
