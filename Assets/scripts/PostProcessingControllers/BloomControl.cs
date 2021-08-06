using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

[CreateAssetMenu(menuName = "PostProcessing/Bloom")]
public class BloomControl : PostProcessingController
{
    public override void ControlEffect(string name, float val)
    {
        var objType = ((Bloom)effect).GetType();
        var field = objType.GetField(name);
        var fieldRef = field.GetValue((Bloom)effect);

        var fieldObjType = fieldRef.GetType();
        var prop = fieldObjType.GetProperty("value");

        prop.SetValue(fieldRef, val);

        base.ControlEffect(name, val);
    }


    public override void Init()
    {
        FindObjectOfType<Volume>().profile.TryGet(typeof(Bloom), out effect);
    }
}
