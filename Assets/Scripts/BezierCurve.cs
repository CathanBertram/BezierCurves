using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve
{

    public static Vector3[] GetCurve(int resolution, List<Vector3> points)
    {
        var positions = new List<Vector3>();
        positions.Add(points[0]);

        for (int i = 1; i < resolution; i++)
        {
            positions.Add(GetPointAtT((float)i / resolution, points));
        }

        positions.Add(points[points.Count - 1]);
        return positions.ToArray();
    }
    public static Vector3[] GetCurve(int resolution, List<Transform> points)
    {
        var positions = new List<Vector3>();
        positions.Add(points[0].position);

        for (int i = 1; i < resolution; i++)
        {
            positions.Add(GetPointAtT((float)i / resolution, points));
        }

        positions.Add(points[points.Count - 1].position);
        return positions.ToArray();
    }
    public static Vector3[] GetCurve(int resolution, List<GameObject> points)
    {
        var positions = new List<Vector3>();
        positions.Add(points[0].transform.position);

        for (int i = 1; i < resolution; i++)
        {
            positions.Add(GetPointAtT((float)i / resolution, points));
        }

        positions.Add(points[points.Count - 1].transform.position);
        return positions.ToArray();
    }
    private static Vector3 GetPointAtT(float t, List<Vector3> points)
    {
        Vector3[] pointsAtT = new Vector3[points.Count - 1];
        for (int i = 0; i < points.Count - 1; i++)
        {
            pointsAtT[i] = Easing.Ease(EasingType.Linear, points[i], points[i + 1], t);
        }
        if (pointsAtT.Length == 1)
            return pointsAtT[0];

        Vector3[] tempPoints = pointsAtT;
        while (pointsAtT.Length > 1)
        {
            pointsAtT = new Vector3[tempPoints.Length - 1];
            for (int i = 0; i < tempPoints.Length - 1; i++)
            {
                pointsAtT[i] = Easing.Ease(EasingType.Linear, tempPoints[i], tempPoints[i + 1], t);
            }

            tempPoints = pointsAtT;
        }
        return pointsAtT[0];
    }
    private static Vector3 GetPointAtT(float t, List<Transform> points)
    {
        Vector3[] pointsAtT = new Vector3[points.Count - 1];
        for (int i = 0; i < points.Count - 1; i++)
        {
            pointsAtT[i] = Easing.Ease(EasingType.Linear, points[i].position, points[i + 1].position, t);
        }
        if (pointsAtT.Length == 1)
            return pointsAtT[0];

        Vector3[] tempPoints = pointsAtT;
        while (pointsAtT.Length > 1)
        {
            pointsAtT = new Vector3[tempPoints.Length - 1];
            for (int i = 0; i < tempPoints.Length - 1; i++)
            {
                pointsAtT[i] = Easing.Ease(EasingType.Linear, tempPoints[i], tempPoints[i + 1], t);
            }

            tempPoints = pointsAtT;
        }
        return pointsAtT[0];
    }
    private static Vector3 GetPointAtT(float t, List<GameObject> points)
    {
        Vector3[] pointsAtT = new Vector3[points.Count - 1];
        for (int i = 0; i < points.Count - 1; i++)
        {
            pointsAtT[i] = Easing.Ease(EasingType.Linear, points[i].transform.position, points[i + 1].transform.position, t);
        }
        if (pointsAtT.Length == 1)
            return pointsAtT[0];

        Vector3[] tempPoints = pointsAtT;
        while (pointsAtT.Length > 1)
        {
            pointsAtT = new Vector3[tempPoints.Length - 1];
            for (int i = 0; i < tempPoints.Length - 1; i++)
            {
                pointsAtT[i] = Easing.Ease(EasingType.Linear, tempPoints[i], tempPoints[i + 1], t);
            }

            tempPoints = pointsAtT;
        }
        return pointsAtT[0];
    }


 
}
