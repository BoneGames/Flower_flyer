using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NaughtyAttributes;
using System;

public class MechSway : MonoBehaviour
{
    List<string> groupTags = new List<string>() { "circle", "centre", "perimeter" };
    string ownTag;
    public List<Transform> pivots;
    public Vector2 bend;
    public Vector2 oldBend;
    public Vector2 startBend;
    public float asymmBendScale;
    public bool topBendMore;
    public bool bottomBendMore;
    int pivotCount;
    public bool editing;

    public MechSway innerDouble;
    //MechSwitch switcher;
    public enum BendState
    {
        still,
        bend,
        countdown,
        revert
    }
    public BendState bendState;

    MechSway GetInnerDouble()
    {
        // get own tag
        ownTag = null;
        Transform tagCheck = transform;
        while (ownTag == null)
        {
            ownTag = groupTags.Contains(tagCheck.parent.tag) ? tagCheck.parent.tag : null;
            tagCheck = tagCheck.parent;
        }
        // not required if in centre group
        if (ownTag == "centre")
            return null;

        // get path to self
        List<int> getChildPath = new List<int>();
        Transform checkPoint = transform;
        getChildPath.Insert(0, transform.GetSiblingIndex());
        while (!checkPoint.parent.CompareTag(ownTag))
        {
            checkPoint = checkPoint.parent;
            getChildPath.Insert(0, checkPoint.GetSiblingIndex());
        }

        // use path to get self equivalent in centre
        Transform mirror = FindObjectOfType<centre>().transform;
        for (int i = 0; i < getChildPath.Count; i++)
        {
            try
            {
                int cP = i == 0 ? 0 : getChildPath[i];
                mirror = mirror.GetChild(cP);
            }
            catch (Exception e)
            {
                print(ownTag);
                print(e.Message);
            }

        }

        return mirror.GetComponent<MechSway>();
    }

    void SetBendState(BendState newState)
    {
        // print("Set: " + newState.ToString());
        bendState = newState;
        if (newState == BendState.bend)
        {
            StopAllCoroutines();
            coroutine = false;
        }
    }

    private void Awake()
    {
        if(!FindObjectOfType<bendOperator>().windOn)
        {
            this.enabled = false;
        }
        SetBendState(BendState.still);
        editing = false;
        oldBend = bend;
        pivotCount = pivots.Count;
        startBend = bend;
        revertRate = 0.1f;
        //switcher = FindObjectOfType<MechSwitch>();
        if (ownTag == "perimeter")
            this.enabled = false;
    }

    void MutantRotateMirror()
    {
        MutantRotator mR = GetComponentInChildren<MutantRotator>();
        if (mR)
        {
            if (ownTag != "centre")
            {
                mR.transform.rotation = innerDouble.GetComponentInChildren<MutantRotator>().transform.rotation;
            }
        }
        return;
    }
    private void Start()
    {
        //if (!switcher.mechSwayOn)
        //    this.enabled = false;

        innerDouble = GetInnerDouble();
        MutantRotateMirror();
    }

    //[Button]
    public void GetRotateTransforms()
    {
        pivots.Clear();
        Transform t = transform;
        while (true)
        {
            try
            {
                if (t.name.Contains("pivot"))
                    pivots.Add(t);
                t = t.GetChild(1);
            }
            catch
            {
                break;
            }
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        StalkRotate();
    }
    private void OnValidate()
    {
        if (editing)
        {
            print(gameObject.name);
            StalkRotate();
        }
    }

    public void StalkRotate()
    {
        if (!editing)
        {
            switch (bendState)
            {
                case BendState.still:
                    // start event chain leading back to righted gary if required
                    if (bend != startBend)
                        if (!coroutine)
                            StartCoroutine(Countdown());
                        else
                            Debug.LogError("OVERLAP COROUTINE");
                    return;
                case BendState.bend:
                    // do nothing - continue to apply bend logic
                    break;
                case BendState.countdown:
                    // exit function
                    return;
                case BendState.revert:
                    // do nothing - continue to apply bend logic
                    break;
            }
        }

        print("after bend: State: " + bendState.ToString());
        foreach (var item in pivots)
        {
            // skip base
            if (item.name == transform.name)
                continue;

            int i = bottomBendMore ? pivots.Count - pivots.IndexOf(item) : pivots.IndexOf(item);
            Vector3 r = topBendMore ? new Vector3(bend.x * (i * asymmBendScale), 0, bend.y * (i * asymmBendScale)) : new Vector3(bend.x, 0, bend.y);
            item.localRotation = Quaternion.Euler(r);
            if (innerDouble)
            {
                innerDouble.MirrorRotate(r);
            }
        }
        if (oldBend == bend)
        {
            if (bendState != BendState.revert)
                SetBendState(BendState.still);
        }
        else
        {
            oldBend = bend;
        }
    }
    public float countdownDelay;
    public bool coroutine;
    IEnumerator Countdown()
    {
        coroutine = true;
        SetBendState(BendState.countdown);
        yield return new WaitForSeconds(countdownDelay);
        StartCoroutine(Revert());
    }
    public float revertRate;

    public void MirrorRotate(Vector3 rotation)
    {
        foreach (var item in pivots)
        {
            // skip base
            if (item.name == transform.name)
                continue;

            item.localRotation = Quaternion.Euler(rotation);
        }
    }

    IEnumerator Revert()
    {
        SetBendState(BendState.revert);
        float timer = 0;
        Vector2 start = bend;

        while (timer <= 1)
        {
            bend = Vector2.Lerp(start, startBend, timer);
            timer += Time.deltaTime * revertRate;
            yield return null;
        }
        bend = startBend;
        SetBendState(BendState.still);
        coroutine = false;
    }



    Vector2 StandardizeAdjustmentDir(Vector2 v)
    {
        float degrees = transform.eulerAngles.y;
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public float newBendMax;
    public void AdjustBend(Vector2 windDir, bool normaliseDir)
    {
        Vector2 _windDir = normaliseDir ? StandardizeAdjustmentDir(windDir) : windDir;
        SetBendState(BendState.bend);
        //bend = Vector2.ClampMagnitude((bend + _Adjustment / pivotCount), 1f);
        float diff = Vector2.Distance(startBend, _windDir);
        Vector2 newBend = Vector2.Lerp(startBend, _windDir, 1f / (Mathf.Pow(diff, 2.5f)));
        int x = 0;
        while (Vector2.Distance(bend, newBend) > newBendMax)
        {
            newBend = Vector2.Lerp(bend, newBend, 0.5f);
            x++;
        }
        if (x > 0)
            print("reduced bend amount " + x + " times");
        bend = newBend;

        //if (windDir.x > 0)
        //{
        //    print("positiveX");
        //}
        //else
        //{
        //    print("negativeX");
        //}
    }
}
