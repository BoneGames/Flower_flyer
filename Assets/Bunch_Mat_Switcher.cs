using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunch_Mat_Switcher : MonoBehaviour
{
    //public struct MatType
    //{
    //    public bool isTransMat;
    //    public Material mat;
    //}

    [System.Serializable]
    public class MatPair
    {
        public Material[] mats = new Material[2];
    }
    public MatPair[] allMats;
    public Dictionary<string, Material> matDuosDict = new Dictionary<string, Material>();

    public bool isTrans;
    Renderer[] allRends;
    private void Start()
    {
        allRends = GetComponentsInChildren<Renderer>();
        foreach (var item in allMats)
        {
            matDuosDict.Add(item.mats[0].name, item.mats[1]);
            matDuosDict.Add(item.mats[1].name, item.mats[0]);
        }
        Debug.Log("Dict Length: " + matDuosDict.Keys.Count);
    }

    public void SwitchMats(bool toTrans)
    {

        if (isTrans == toTrans)
            return;
        Debug.Log("Switching Mats to: " + (toTrans ? "trans" : "opaque"));
        foreach (Renderer r in allRends)
        {
            Material curr = r.material;
            try
            {
            r.material = matDuosDict[curr.name.Replace(" (Instance)", "")];

            }
            catch
            {
                Debug.Log(curr.name + " not in dict");
            }
        }
    }
}
