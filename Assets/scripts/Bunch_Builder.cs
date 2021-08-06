using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Bunch_Builder : MonoBehaviour
{
    [Header("Bunch")]


    public GameObject[] flowers;

    public Vector2Int quantityRange;
    public Vector2Int sectionHeightRange;
    public Vector2Int baseRotationRange;
    public Vector2Int leafQuantityRange;

    public Vector2 stalkRotationRange;
    public Vector2 basePositionRange;
    public Vector2 assymBendScaleRange;

    public float minDistance;
    public float bunchParentScale;


    [Header("Tile")]
    public GameObject tilePrefab;

    public Vector2Int bunchQuantityRange;
    public float minBunchDistance;

    public float size, scale;

    void GetSizeScale()
    {
        GameObject getScale = Instantiate(tilePrefab);
        size = getScale.GetComponent<Collider>().bounds.size.x;
        scale = getScale.transform.localScale.x;
        DestroyImmediate(getScale);
    }

    [Header("World")]
    public GameObject buildTilePrefab;
    [Button]
    public void BuildWorld()
    {
        GameObject World = new GameObject();
        World.transform.position = Vector3.zero;
        World.transform.name = "World";

        GetSizeScale();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3 pos = new Vector3(i * size, 0, j * size);
                Instantiate(buildTilePrefab, pos, Quaternion.identity, World.transform); 
            }
        }
    }
    [Button]
    public void BuildTile()
    {
        GetSizeScale();
        BuildTile(Vector3.zero);
    }

    GameObject BuildTile(Vector3 pos)
    {
        GameObject newTile = Instantiate(tilePrefab, pos, Quaternion.identity);
        // get bounding box
        float edgeLength = size/scale;
        int bunchCount = GetRandom(bunchQuantityRange);
        List<Vector3> bunchPositions = new List<Vector3>();
        float minDistance = (edgeLength / bunchCount) * 2;

        //float halfEdgeLessBunchMax = (edgeLength / 2f) - ((Mathf.Abs(basePositionRange.x - basePositionRange.y)/2));
        float halfEdge = (edgeLength / 2f) * 0.8f;
        for (int i = 0; i < bunchCount; i++)
        {
            // get offset
            Vector3 offset = new Vector3(Random.Range(-halfEdge, halfEdge), 0, Random.Range(-halfEdge, halfEdge));
            while (true)
            {
                bool tooClose = false;
                foreach (var p in bunchPositions)
                {
                    float dist = Vector3.Distance(p, offset);
                    if (dist < minDistance)
                    {
                        offset = new Vector3(Random.Range(-halfEdge, halfEdge), 0, Random.Range(-halfEdge, halfEdge));
                        tooClose = true;
                        //Debug.DrawLine(p * scale, offset * scale, Color.red, 100);
                        // Debug.Log("Bunch Too CLose");
                        break;
                    }
                }
                if(tooClose)
                {
                    continue;
                }
                else
                {
                    bunchPositions.Add(offset);
                    break;
                }
            }
            BuildBunch(newTile.transform, offset);
        }
        return newTile;
    }

    public void BuildBunch(Transform _bunchParent, Vector3 offset)
    {
        // how many flowers in bunch
        int quantity = GetRandom(quantityRange);

        GameObject _base = new GameObject();
        _base.transform.parent = _bunchParent;
        _base.transform.localScale = new Vector3(bunchParentScale, bunchParentScale, bunchParentScale);
        _base.transform.localPosition = offset;
        _base.name = "BUNCH";

        List<GameObject> bunchFlowers = new List<GameObject>();

        for (int i = 0; i < quantity; i++)
        {
            GameObject f = CreateFlower(_base.transform);
            // check if new flower is too close to other flowers
            foreach (var item in bunchFlowers)
            {
                Vector3 cup1 = item.GetComponent<MechSway>().pivots[item.GetComponent<MechSway>().pivots.Count - 1].GetChild(0).position;
                Vector3 cup2 = f.GetComponent<MechSway>().pivots[f.GetComponent<MechSway>().pivots.Count - 1].GetChild(0).position;
                if (Vector3.Distance(cup1, cup2) < minDistance)
                {
                    DestroyImmediate(f);
                    Debug.LogError("Destroyed");
                    i--;
                    break;
                }
            }

            if (f != null)
            {
                bunchFlowers.Add(f);
            }
        }

    }

    GameObject CreateFlower(Transform _base)
    {
        // create new flower
        GameObject newFlower = Instantiate(flowers[Random.Range(0, flowers.Length)], _base);
        // set base displacement
        newFlower.transform.localPosition = new Vector3(GetRandom(basePositionRange), 0, GetRandom(basePositionRange));
        // set base rotation
        newFlower.transform.eulerAngles = new Vector3(GetRandom(baseRotationRange), Random.Range(0, 360), GetRandom(baseRotationRange));


        // set Section Height
        int sectionHeight = GetRandom(sectionHeightRange);
        StemScaleAdjust sSA = newFlower.GetComponent<StemScaleAdjust>();
        while (sSA.sway.pivots.Count != sectionHeight)
        {
            if (sSA.sway.pivots.Count > sectionHeight)
            {
                sSA.RemoveSection();
            }
            else
            {
                sSA.AddSection();
            }
        }

        // add leaves
        int leafCount = GetRandom(leafQuantityRange);
        for (int k = 0; k < leafCount; k++)
        {
            sSA.AddLeaf();
        }

        MechSway ms = newFlower.GetComponent<MechSway>();
        ms.asymmBendScale = GetRandom(assymBendScaleRange);

        ms.bend = new Vector2(GetRandom(stalkRotationRange), GetRandom(stalkRotationRange));
        // set bend bias to none, top or bottom
        switch (Random.Range(0, 3))
        {
            case 0:
                ms.topBendMore = true;
                break;
            case 1:
                ms.topBendMore = true;
                ms.bottomBendMore = true;
                break;
            default:
                break;
        }
        ms.StalkRotate();
        return newFlower;
    }

    int GetRandom(Vector2Int input)
    {
        return Random.Range(input.x, input.y);
    }

    float GetRandom(Vector2 input)
    {
        return Random.Range(input.x, input.y);
    }


}
