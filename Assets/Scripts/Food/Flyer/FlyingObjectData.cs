using UnityEngine;

namespace Food.Flyer
{
    public class FlyingObjectData
    {
        public Ingredient Ingredient { get; private set; }
        public BezierCurve Curve { get; private set; }
        public float StartFlyTime { get; private set; }

        public FlyingObjectData(Ingredient ingredient, BezierCurve curve)
        {
            Ingredient = ingredient;
            Curve = curve;
            StartFlyTime = Time.time;
        }
    }
}