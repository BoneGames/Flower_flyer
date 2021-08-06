using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(menuName = "AudioSource_Controller")]
public class AudioSourceController : EffectController
{
    public AudioSource aS;
    public string audioSourceTag;

    public override void ControlEffect(string paramName, float val)
    {
        // this check allows the volume to only be set when it needs to be (on climb or fall, but not when it is already higher up)
        if (paramName == "up")
        {
            if (aS.volume < val)
                aS.volume = val;
        }
        else
        {
            if (aS.volume > val)
                aS.volume = val;
        }
        base.ControlEffect(paramName,val);
    }

    public override void Init()
    {
        aS = GameObject.FindWithTag(audioSourceTag).GetComponent<AudioSource>();
    }

    public override void MinMax(bool min)
    {
        base.MinMax(min);
    }
}
