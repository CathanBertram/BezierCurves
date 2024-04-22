using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiBezierCurve : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution;
    public int pointCount;
    public int pointsPerCurve;
    public DebugPoint pointPrefab;
    public List<DebugPoint> points;


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

    private void OnValidate()
    {
        if (resolution < 1)
            resolution = 1;


    }
}
