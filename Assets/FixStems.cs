using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixStems : MonoBehaviour
{
    public bool compress;
    // Start is called before the first frame update
    void Awake()
    {
        StemScaleAdjust[] sSA = FindObjectsOfType<StemScaleAdjust>();

        foreach (var item in sSA)
        {
            item.SpineCompress(compress);
        }
    }
}
