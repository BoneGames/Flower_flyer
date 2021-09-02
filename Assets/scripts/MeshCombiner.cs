using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    public void CombineMeshes()
    {
        Vector2 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;

        MeshFilter[] filters = GetComponentsInChildren<MeshFilter>();

        Debug.Log(name + " is combing " + filters.Length + " meshes!");

        Mesh finalMesh = new Mesh();

        Matrix4x4 ourMatrix = transform.localToWorldMatrix;

        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            if (filters[i].transform == transform)
                continue;
            if (filters[i].sharedMesh.name != "Cylinder")
                continue;

            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = filters[i].sharedMesh;
            combiners[i].transform = filters[i].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);

        GetComponent<MeshFilter>().sharedMesh = finalMesh;

        transform.rotation = oldRot;
        transform.position = oldPos;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    //public void SaveMesh()
    //{
    //    Mesh m = GetComponent<MeshFilter>().sharedMesh;
    //    if (!m)
    //    {
    //        Debug.Log("no mesh to save");
    //        return;
    //    }

    //    AssetDatabase.CreateAsset(m, "Assets/Saved_Meshes/" + transform.name);

    //    //// Print the path of the created asset
    //    //Debug.Log(AssetDatabase.GetAssetPath(material));
    //}
}
