using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class Distance_Check_Test : MonoBehaviour
{
    public bool isEnabled;
    Bunch_Mat_Switcher[] allBunches;
    public int distance;
    public float currDist;

    public List<Bunch_Mat_Switcher> inRangeBunches = new List<Bunch_Mat_Switcher>();

    private void Start()
    {
        GetAllBunches();
    }
    [Button]
    public void GetAllBunches()
    {
        allBunches = GameObject.FindGameObjectsWithTag("bunch").Select(o => o.GetComponent<Bunch_Mat_Switcher>()).ToArray();
        Debug.Log(allBunches.Length);
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        List<Bunch_Mat_Switcher> newInRangeBunches = new List<Bunch_Mat_Switcher>();

        foreach (var item in allBunches)
        {
            currDist = Vector3.Distance(transform.position, item.transform.position);

            if (currDist < distance)
            {
                newInRangeBunches.Add(item);
            }

        }

        foreach (var item in newInRangeBunches)
        {
            item.SwitchMats(false);
        }

        var itemsOnlyInFirst = inRangeBunches.Except(newInRangeBunches).ToArray();
        foreach (var item in itemsOnlyInFirst)
        {
            item.SwitchMats(true);
        }

        inRangeBunches = newInRangeBunches;
    }
}
