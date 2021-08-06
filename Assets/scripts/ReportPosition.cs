using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ReportPosition : MonoBehaviour
{
    public Transform petalParent;
    public Material[] petals;
    // Start is called before the first frame update
    void Start()
    {
        petalParent = RecursiveFindChild(transform, "Petal_Parent");
        petals = petalParent.GetComponentsInChildren<Renderer>().Select(x => x.material).ToArray();
        foreach (var item in petals)
        {
            item.SetVector("Vector2_5B7BCD40", new Vector2(transform.localPosition.x, transform.localPosition.z));
        }
    }

    public Transform RecursiveFindChild(Transform parent, string childName)
    {
        Transform result = null;

        foreach (Transform child in parent)
        {
            if (child.name == childName)
                result = child.transform;
            else
                result = RecursiveFindChild(child, childName);

            if (result != null) break;
        }

        return result;
    }
}
