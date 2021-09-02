using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class Reduce_Sound_Zone_Reach : MonoBehaviour
{
    public float division;
    //[Button]
    public void DivideReach()
    {
        AudioSource[] allSources = Resources.FindObjectsOfTypeAll(typeof(AudioSource)) as AudioSource[];
        Debug.Log(allSources.Length);
        foreach (var item in allSources)
        {
            if (item.maxDistance == 0)
                continue;
            item.maxDistance /= division;
        }
    }
}
