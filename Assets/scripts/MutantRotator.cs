using UnityEngine;

public class MutantRotator : MonoBehaviour
{
    void Start()
    {
        transform.rotation *= Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
    }
}
