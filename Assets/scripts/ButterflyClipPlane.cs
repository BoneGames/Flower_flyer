using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyClipPlane : MonoBehaviour
{
    public float butterflyCullDistance;
    // Start is called before the first frame update
    void Start()
    {
        float[] distances = new float[32];
        distances[14] = butterflyCullDistance;
        GetComponent<Camera>().layerCullDistances = distances;
    }
}
