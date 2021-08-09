using System.Collections;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor (typeof(MeshCombiner))]
public class MeshCombinerEditor : Editor
{
    private void OnSceneGUI()
    {
        MeshCombiner mc = target as MeshCombiner;
        if (Handles.Button(mc.transform.position + Vector3.up * 5, Quaternion.LookRotation(Vector3.up), 1, 1, Handles.CylinderHandleCap))
        {
            mc.CombineMeshes();
        }
    }
}
#endif
