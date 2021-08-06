using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PostProcessing/Chromatic Aberration")]
public class ChromaticAberrationControl : PostProcessingController
{
    public string showVal;

    public override void ControlEffect(string paramName, float val)
    {
        var objType = ((ChromaticAberration)effect).GetType();
        var field = objType.GetField(paramName);
        var fieldRef = field.GetValue((ChromaticAberration)effect);

        var fieldObjType = fieldRef.GetType();
        var prop = fieldObjType.GetProperty("value");

        prop.SetValue(fieldRef, val);

        base.ControlEffect(paramName, val);
    }


    public override void Init()
    {
        FindObjectOfType<Volume>().profile.TryGet(typeof(ChromaticAberration), out effect);
    }
}
