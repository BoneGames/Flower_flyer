using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RandomPlanter : MonoBehaviour
{
    public int flowerCountSqrt;
    public float randomDisplacementMax;
    public Vector2 randomScaleRange;
    public GameObject[] prefabs;
    Renderer rend;
    Collider col;
    public Transform parent;
    private void Awake()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 offsetBig = col.bounds.extents;
        float increment = col.bounds.size.x / (float)flowerCountSqrt;
        Vector3 offsetSmall = new Vector3(increment / 2f, 0, increment / 2f);
        Vector3 offset = offsetBig - offsetSmall;

        for (float i = 0; i < col.bounds.size.x; i += increment)
        {
            for (float j = 0; j < col.bounds.size.x; j += increment)
            {
                Vector3 randomDist = new Vector3(Random.Range(-randomDisplacementMax, randomDisplacementMax), 0, Random.Range(-randomDisplacementMax, randomDisplacementMax));
                Vector3 position = transform.position + new Vector3(i, 0, j) - offset + randomDist;
                if (position.x < offsetBig.x && position.z < offsetBig.z)
                {
                    GameObject go = prefabs[Random.Range(0, prefabs.Length)];
                    GameObject g = new GameObject();
#if UNITY_EDITOR
                    g = PrefabUtility.InstantiatePrefab(go) as GameObject;
#endif

                    g.transform.position = position;
                    g.transform.parent = parent;

                    float scale = Random.Range(randomScaleRange.x, randomScaleRange.y);
                    g.transform.localScale = new Vector3(scale, scale, scale);

                    g.transform.rotation = Quaternion.identity * Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
                }
            }
        }
    }


}
