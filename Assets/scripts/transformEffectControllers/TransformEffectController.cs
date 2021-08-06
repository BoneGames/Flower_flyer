using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "TransformEffect")]
public class TransformEffectController : EffectController
{
    public Transform _transform;
    public override void ControlEffect(string paramName, float val)
    {
        _transform.localPosition = Vector3.zero +  (Random.insideUnitSphere * val);

        base.ControlEffect(paramName, val);
    }

    public override void Init()
    {
        _transform = Camera.main.transform;
    }

    public override void MinMax(bool min)
    {
        base.MinMax(true);
    }
}
