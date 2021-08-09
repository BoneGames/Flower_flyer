using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track_Leveler : MonoBehaviour
{
    Transform player;
    bool playerMatSwitchActive;
    void Start()
    {
        Distance_Check_Test p = FindObjectOfType<Distance_Check_Test>();
        player = p.transform;
        playerMatSwitchActive = p.isEnabled;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerMatSwitchActive)
            transform.position = transform.parent.position + new Vector3(0, player.position.y, 0);
    }
}
