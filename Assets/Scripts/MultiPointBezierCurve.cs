using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiPointBezierCurve : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution;
    public int pointCount;
    public DebugPoint pointPrefab;
    public List<DebugPoint> points;
    public EasingType easingType;
    List<Vector3> positions = new List<Vector3>();

    private void Awake()
    {
        points = new List<DebugPoint>();
        for (int i = 0; i < pointCount; i++)
        {
            var obj = Object.Instantiate(pointPrefab) as DebugPoint;
            obj.transform.SetParent(this.transform);
            points.Add(obj);
        }

    }

    private void Update()
    {
        //ValidatePoints();
        if (points.Count < 3) return;
        UpdateBezier();
    }

    public void AddPoint(Vector3 position)
    {
        var obj = Object.Instantiate(pointPrefab) as DebugPoint;
        obj.transform.SetParent(this.transform);
        obj.transform.position = position;
        points.Add(obj);
        obj.removeDebugPoint += RemovePoint;
        pointCount++;
    }
    private void RemovePoint(DebugPoint pointToRemove)
    {
        points.Remove(pointToRemove);
        pointCount--;
    }

    private void UpdateBezier()
    {
        positions.Clear();
        positions.Add(points[0].transform.position);

        for (int i = 1; i < resolution; i++)
        {
            positions.Add(GetPointAtT((float)i / resolution, points));
        }

        positions.Add(points[pointCount - 1].transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
    private Vector3 GetPointAtT(float t, List<DebugPoint> points)
    {
        Vector3[] pointsAtT = new Vector3[points.Count - 1];
        for (int i = 0; i < points.Count - 1; i++)
        {
            pointsAtT[i] = Easing.Ease(easingType, points[i].transform.position, points[i + 1].transform.position, t);
        }
        if (pointsAtT.Length == 1)
            return pointsAtT[0];

        Vector3[] tempPoints = pointsAtT;
        while (pointsAtT.Length > 1)
        {
            pointsAtT = new Vector3[tempPoints.Length - 1];
            for (int i = 0; i < tempPoints.Length - 1; i++)
            {
                pointsAtT[i] = Easing.Ease(easingType, tempPoints[i], tempPoints[i + 1], t);
            }

            tempPoints = pointsAtT;
        }
        return pointsAtT[0];
    }
    //private Vector3 GetPointAtT(float t, List<Vector3> points)
    //{
    //    Vector3[] pointsAtT = new Vector3[points.Count - 1];
    //    for (int i = 0; i < points.Count - 1; i++)
    //    {
    //        pointsAtT[i] = Easing.Ease(easingType, points[i + 1], points[i], (float)i / resolution);
    //    }
    //    if (pointsAtT.Length == 1)
    //        return pointsAtT[0];
    //    else
    //        return GetPointAtT(t, points);
    //}
    //private void ValidatePoints()
    //{
    //    if (pointCount < 3)
    //        pointCount = 3;

    //    if (points.Count() < pointCount)
    //    {

    //        for (int i = 0; i < pointCount - points.Count(); i++)
    //        {
    //            var obj = Object.Instantiate(pointPrefab).transform;
    //            obj.SetParent(this.transform);
    //            points.Add(obj);

    //        }
    //    }
    //    else if (points.Count() > pointCount)
    //    {
    //        while (points.Count != pointCount)
    //        {
    //            Destroy(points[0]);
    //            points.RemoveAt(0);
    //        }
    //    }
    //}

    private void OnValidate()
    {
        if (resolution < 1)
            resolution = 1;

        
    }
}
