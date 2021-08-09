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

    public List<Bunch_Mat_Switcher> inRangeBunches = new List<Bunch_Mat_Switcher>();

    private void Start()
    {
        GetAllBunches();
    }
    [Button]
    public void RefreshAllBunches()
    {
        foreach (var item in allBunches)
        {
            item.PopulateDict();
        }
    }

    [Button]
    public void CheckDicts()
    {
        foreach (var item in allBunches)
        {
            Debug.Log(item.matDuosDict.Keys.Count);
        }
    }
    [Button]
    public void AllTrans()
    {
        foreach (var item in allBunches)
        {
            item.SwitchMats(true);
        }
    }
    [Button]
    public void AllOpaque()
    {
        foreach (var item in allBunches)
        {
            item.SwitchMats(false);
        }
    }

    [Button]
    public void GetAllBunches()
    {
        allBunches = GameObject.FindGameObjectsWithTag("bunch").Where(o => o.GetComponent<Bunch_Mat_Switcher>() != null).Select(o => o.GetComponent<Bunch_Mat_Switcher>()).ToArray();
        Debug.Log("Bunch Swicther Count: " + allBunches.Length);
    }

    private void Update()
    {
        if (!isEnabled)
            return;

        foreach (var item in allBunches)
        {
            float currDist = Vector3.Distance(transform.position, item.tracker.position);
            item.SwitchMats(currDist > distance);
        }
    }
}
