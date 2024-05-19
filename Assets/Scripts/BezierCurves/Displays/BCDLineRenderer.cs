using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SimpleBezierCurve.Display
{
    public class BCDLineRenderer : BezierCurveDisplay
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private int resolutionPerCurve;
        public override void UpdateDisplay(BezierCurve bezierCurve)
        {
            var curve = bezierCurve.GetCurve(resolutionPerCurve);
            lineRenderer.positionCount = curve.Count();
            lineRenderer.SetPositions(curve);
        }
        public override void OnPointAdded()
        {
            
        }
    }
}