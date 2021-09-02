using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class SetPos : MonoBehaviour
{

    public Transform newpos;
    //[Button]
    public void SetPosition()
    {
        transform.position = newpos.position;
    }
}
