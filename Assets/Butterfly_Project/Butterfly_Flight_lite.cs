﻿using System.Collections;
using UnityEngine;

public class Butterfly_Flight_lite : MonoBehaviour
{
    public float area;
    public Vector2 timeRange;
    Vector3 basePos;
    public float turnSpeed;
    public float maxStartDelay, newFlightDelayMax;

    void Start()
    {
        basePos = transform.parent.position;
        Invoke("AnimStart", Random.Range(0, maxStartDelay));
    }

    void AnimStart()
    {
        // start anim
        GetComponent<Animator>().SetTrigger("Start");
        SetPath();
    }

    public void SetPath()
    {
        Vector3 newPosition = basePos + (Random.insideUnitSphere * area);
        Vector3 via = ((transform.position + newPosition) / 2) + Random.insideUnitSphere * newPosition.magnitude;
        float timeDiv = Random.Range(timeRange.x, timeRange.y);
        StartCoroutine(Fly(newPosition, via, timeDiv));
    }

    IEnumerator Fly(Vector3 dest, Vector3 via, float timeDiv)
    {

        float timer = 0;
        Vector3 start = transform.position;
        Vector3 forward = (dest - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(forward);
        Quaternion startRot = transform.rotation;
        while (timer <= 1)
        {
            transform.rotation = Quaternion.Lerp(startRot, lookRot, timer * turnSpeed);
            Vector3 sm = Vector3.Lerp(start, via, timer);
            Vector3 me = Vector3.Lerp(via, dest, timer);
            transform.position = Vector3.Lerp(sm, me, timer);
            timer += Time.deltaTime / timeDiv;
            yield return null;
        }
        yield return new WaitForSeconds(Random.Range(0, newFlightDelayMax));
        SetPath();
    }
}