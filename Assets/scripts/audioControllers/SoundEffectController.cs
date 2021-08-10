using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundController")]
public class SoundEffectController : EffectController
{
    public AudioMixer aM;

    public override void ControlEffect(string paramName, float val)
    {
        aM.SetFloat(paramName, val);
        base.ControlEffect(paramName, val);
    }

    public override void Init()
    {
        //throw new System.NotImplementedException();
    }

    //public override void MinMax(bool min)
    //{
    //    base.MinMax(min);
    //}
}
