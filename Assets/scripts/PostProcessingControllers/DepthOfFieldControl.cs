using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PostProcessing/DepthOfField")]
public class DepthOfFieldControl : PostProcessingController
{
    public override void ControlEffect(string name, float val)
    {
        var objType = ((DepthOfField)effect).GetType();
        var field = objType.GetField(name);
        var fieldRef = field.GetValue((DepthOfField)effect);

        var fieldObjType = fieldRef.GetType();
        var prop = fieldObjType.GetProperty("value");
        
        prop.SetValue(fieldRef, val);

        base.ControlEffect(name, val);
    }

    public override void Init()
    {
        FindObjectOfType<Volume>().profile.TryGet(typeof(DepthOfField), out effect);
    }
}
