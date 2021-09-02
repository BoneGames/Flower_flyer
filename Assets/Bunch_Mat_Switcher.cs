using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class Bunch_Mat_Switcher : MonoBehaviour
{
    public Transform tracker;

    [System.Serializable]
    public class MatPair
    {
        public Material[] mats = new Material[2];
    }
    public MatPair[] allMats;
    public Dictionary<string, Material> matDuosDict;

    public bool isTrans;
    Renderer[] allRends;
    private void Start()
    {
        //isTrans = true;
    }
    //[Button]
    public void PopulateDict()
    {
        matDuosDict = new Dictionary<string, Material>();
        allRends = GetComponentsInChildren<Renderer>();
        foreach (var item in allMats)
        {
            string key1 = item.mats[0].name;// + " (Instance)";
            string key2 = item.mats[1].name;// + " (Instance)";
            matDuosDict.Add(key1, item.mats[1]);
            matDuosDict.Add(key2, item.mats[0]);
            //Debug.Log(key1);
            //Debug.Log(key2);
        }
        isTrans = allRends[0].sharedMaterial.name.Contains("trans");
    }

    public void SwitchMats(bool toTrans)
    {

        if (isTrans == toTrans)
            return;
        Debug.LogWarning("Switching Mats to: " + (toTrans ? "trans" : "opaque"));
        foreach (Renderer r in allRends)
        {
            Material curr = r.sharedMaterial;
            try
            {
                r.sharedMaterial = matDuosDict[curr.name];
                Debug.Log("Mat Switched");
            }
            catch
            {
                Debug.Log(curr.name + " not in dict");
            }
        }
        isTrans = toTrans;
    }
}
