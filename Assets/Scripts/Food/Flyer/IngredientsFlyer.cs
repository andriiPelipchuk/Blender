using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Food.Flyer
{
    public class IngredientsFlyer : MonoBehaviour
    {
        [SerializeField] private BezierCurve _curveTemplate;
        [SerializeField] private float _flyDuration;
        [SerializeField] private float _curveForce;

        private List<FlyingObjectData> _flyingObjects;

        private ObjectPool<BezierCurve> _curvesPool;

        public void Initialize()
        {
            _curvesPool = new ObjectPool<BezierCurve>(CreateBezierCurve);

            _flyingObjects = new List<FlyingObjectData>();
        }

        public void LaunchIngredientToTarget(Ingredient ingredient)
        {
            var curve = _curvesPool.Get();
            curve[0].position = ingredient.transform.position;
            curve[1].position = _curveTemplate[1].position;

            var data = new FlyingObjectData(ingredient, curve);

            _flyingObjects.Add(data);
        }

        private void FixedUpdate()
        {
            for (var i = _flyingObjects.Count - 1; i >= 0; i--)
            {
                var data = _flyingObjects[i];

                if (data.Ingredient == null)
                {
                    RemoveData(data);
                    continue;
                }

                var progress = Mathf.Clamp01((Time.time - data.StartFlyTime) / _flyDuration);

                var pos = data.Curve.GetPointAt(progress);
                var rb = data.Ingredient.Rigidbody;

                rb.velocity = (pos - rb.position) * _curveForce;
                
                if (progress >= 1f)
                {
                    RemoveData(data);
                }
            }
        }

        private void RemoveData(FlyingObjectData data)
        {
            _curvesPool.Release(data.Curve);
            _flyingObjects.Remove(data);
        }

        private BezierCurve CreateBezierCurve()
        {
            return Instantiate(_curveTemplate, _curveTemplate.transform.parent);
        }
    }
}