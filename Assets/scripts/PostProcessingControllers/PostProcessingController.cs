using UnityEngine.Rendering;


public abstract class PostProcessingController : EffectController
{
    public VolumeComponent effect;

    public override void ControlEffect(string paramName, float val)
    {
        base.ControlEffect(paramName, val);
    }
}
