using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trans_Mat_Switch : MonoBehaviour
{
    Transform player;
    public Material trans, opaque;
    public bool isTrans;


    private void Start()
    {
        player = FindObjectOfType<flight_controller_V2>().transform;
        isTrans = trans.color.a < 1;
    }
    void SetMaterial()
    {

    }

    void CheckDistance()
    {
        if(isTrans)
        {
            if (trans.color.a >= 1)
                isTrans = !isTrans;
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }
}
