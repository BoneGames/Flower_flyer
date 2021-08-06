using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MaterialColorController")]
public class MaterialColorController : EffectController
{
    public List<Material> mats;
    public Color a, b;

    public override void ControlEffect(string paramName, float val)
    {
        foreach (var m in mats)
        {
            m.color = Color.Lerp(a, b, val);
        }
        base.ControlEffect(paramName, val);
    }

    public override void Init()
    {
        foreach (var m in mats)
        {
            m.color = a;
        }
    }
}
