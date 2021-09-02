using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;

public class Leaf_Delete : MonoBehaviour
{
    //[Button]
    public void LeafGo()
    {
        MoveUp[] rs = GetComponentsInChildren<MoveUp>();
        List<GameObject> n_leaves = new List<GameObject>();

        foreach (var item in rs)
        {
            if (item.transform.localScale.x < 1)
            {
                if(item.transform.GetChild(0).GetComponent<Renderer>().sharedMaterial.name.StartsWith("Normal_Leaf"))
                    n_leaves.Add(item.gameObject);
            }

        }
        Debug.Log(n_leaves.Count);
        for (int i = n_leaves.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(n_leaves[i]);
        }
    }
}
