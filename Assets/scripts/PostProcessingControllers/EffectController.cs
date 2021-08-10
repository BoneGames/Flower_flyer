using UnityEngine;

public abstract class EffectController : ScriptableObject
{
    public bool on;
    [System.Serializable]
    public struct FieldSetCurve
    {
        public string paramName;
        public Vector2 minMax;
        public AnimationCurve curve;
    }

    public FieldSetCurve[] effectSettings;
    public string debugVal;

    public virtual void ControlEffect(string paramName, float val)
    {
        debugVal = val.ToString();
    }

    public virtual void MinMax()
    {
        foreach (var item in effectSettings)
        {
            //float smaller = item.minMax.x > item.minMax.y ? item.minMax.y : item.minMax.x;
            //float larger = item.minMax.x < item.minMax.y ? item.minMax.y : item.minMax.x;

            //ControlEffect(item.paramName, min ? smaller : larger);
            ControlEffect(item.paramName, item.minMax.y);
        }
    }

    public abstract void Init();
}
