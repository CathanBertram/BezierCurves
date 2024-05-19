using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SimpleBezierCurve.Display
{
    public class BCDLineRenderer : BezierCurveDisplay
    {
        [SerializeField] private LineRenderer lineRenderer;
        public override void UpdateDisplay(BezierCurve bezierCurve)
        {
            if (bezierCurve == null) return;

            var curve = bezierCurve.GetCurve(resolutionPerPoint);
            lineRenderer.positionCount = curve.Count();
            lineRenderer.SetPositions(curve);
        }
        public override void OnPointAdded()
        {
            
        }
    }
}