using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(-transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.LogError(transform.forward);
    }
}
