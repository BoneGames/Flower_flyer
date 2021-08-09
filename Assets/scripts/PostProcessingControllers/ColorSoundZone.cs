using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using NaughtyAttributes;
using UnityEngine.Rendering;

[RequireComponent(typeof(AudioSource), typeof(SphereCollider))]
public class ColorSoundZone : MonoBehaviour
{
    string audioTrack;
    public Color zoneCol;
    AudioSource aS;
    //[SerializeField] ColorAdjustments colorChange;
    Material mat;
    SphereCollider col;
    UI ui;

    void Awake()
    {
        GetComponents();
        SetComponents();
    }

    [Button]
    public void GetComponents()
    {
        aS = GetComponent<AudioSource>();
        audioTrack = aS.clip.name;
        //mat = Application.isPlaying ? GetComponent<Renderer>().material : GetComponent<Renderer>().sharedMaterial;
        //col = GetComponent<SphereCollider>();

        //FindObjectOfType<Volume>().profile.TryGet(typeof(ColorAdjustments), out colorChange);
        ui = FindObjectOfType<UI>();
    }

    [Button]
    public void SetComponents()
    {
        float maxDist = aS.maxDistance;
        //mat.color = zoneCol;

        transform.localScale = Vector3.one;
        transform.localScale = new Vector3((maxDist*2) / transform.lossyScale.x, (maxDist * 2) / transform.lossyScale.y, (maxDist * 2) / transform.lossyScale.z);

        //transform.localScale = new Vector3(maxDist, maxDist, maxDist) * 2;
    }

    //void SetColorGrading(bool enter)
    //{
    //    colorChange.active = enter;
    //    colorChange.colorFilter.hdr = enter;
    //    colorChange.colorFilter.value = enter ? zoneCol : new Color(0, 0, 0, 0);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Entering Zone");
            //CheckZone(other.transform.position);
            ui.ShowSoundZones(audioTrack, true);
        }
    }

    //void CheckZone(Vector3 playerPos)
    //{
    //    //ColorSoundZone[] inZones = FindObjectsOfType<ColorSoundZone>().Where(o => o.col.ClosestPoint(playerPos) == playerPos).ToArray();
    //    //foreach (var iZ in inZones)
    //    //{
    //    //    if (sZ.col.ClosestPoint(playerPos) == playerPos)
    //    //        inZones.Add(sZ);
    //    //}

    //    ui.ShowSoundZones(inZones.Select(o => o.audioTrack).ToArray());
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Exiting Zone");
            //CheckZone(other.transform.position);
            ui.ShowSoundZones(audioTrack, false);
        }
    }
}
