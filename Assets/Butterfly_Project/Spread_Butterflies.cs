using UnityEngine;

public class Spread_Butterflies : MonoBehaviour
{
    public Transform player;
    public Vector3 extents;
    // Start is called before the first frame update
    void Start()
    {
        Spread();
    }

    void Spread()
    {
        foreach(Transform t in transform)
        {
            float x = Random.Range(-extents.x, extents.x);
            float y = Random.Range(-extents.y, extents.y);
            float z = Random.Range(-extents.z, extents.z);
            t.position = new Vector3(x, y, z);
        }
    }
}
