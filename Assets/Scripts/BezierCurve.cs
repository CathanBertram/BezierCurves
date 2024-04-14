using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution;

    public Transform pointA;
    public Transform pointB;
    public Transform bezierPoint;

    public EasingType easingType;
    List<Vector3> positions = new List<Vector3>();

    private void Update()
    {
        UpdateBezier();
    }

    private void UpdateBezier()
    {
        positions.Clear();
        positions.Add(pointA.position);
        for (int i = 1; i < resolution; i++)
        {
            var aPoint = pointA.position + Easing.Ease(easingType, 0, 1, (float)i / resolution) * (bezierPoint.position - pointA.position);
            var bPoint = bezierPoint.position + Easing.Ease(easingType, 0, 1, (float)i / resolution) * (pointB.position - bezierPoint.position);
            var point = aPoint + Easing.Ease(easingType, 0, 1, (float)i / resolution) * (bPoint - aPoint);
            positions.Add(point);
        }
        positions.Add(pointB.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void OnValidate()
    {
        if (resolution < 1)
            resolution = 1;
    }
}
