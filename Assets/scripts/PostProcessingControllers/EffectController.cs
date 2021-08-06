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

    public virtual void MinMax(bool min)
    {
        foreach (var item in effectSettings)
        {
            ControlEffect(item.paramName, min ? item.minMax.x : item.minMax.y);
        }
    }

    public abstract void Init();
}
