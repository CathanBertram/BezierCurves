using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MultiPointBezierCurve : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int resolution;
    public int bezierPointsPerCurve = 1;
    public DebugPoint pointPrefab;
    public List<DebugPoint> points;

    private void Awake()
    {
        points = new List<DebugPoint>();
    }

    private void Update()
    {
        //ValidatePoints();
        if (points.Count < 3) return;
        UpdateBezier();
    }

    public void AddPoint(Vector3 position)
    {
        if(points.Count < 1)
        {
            CreatePoint(position, false);
            return;
        }
        var lastPoint = points[points.Count - 1];
        var distanceBetweenPoints = (position - lastPoint.transform.position) / (bezierPointsPerCurve + 1);
        for (int i = 1; i <= bezierPointsPerCurve; i++)
        {
            CreatePoint(lastPoint.transform.position + distanceBetweenPoints * i ,true);
        }
        CreatePoint(position, false);
        
    }

    private void CreatePoint(Vector3 position, bool bezierPoint)
    {
        var obj = Object.Instantiate(pointPrefab) as DebugPoint;
        obj.transform.SetParent(this.transform);
        obj.transform.position = position;
        points.Add(obj);

        if(!bezierPoint)
        {
            obj.removeDebugPoint += RemovePoint;
        }
        else
        {
            obj.ChangeColour();
        }
    }
    private void RemovePoint(DebugPoint pointToRemove)
    {
        var index = points.IndexOf(pointToRemove);

        Debug.Log(index);
        if(index != 0)
            for (int i = index - bezierPointsPerCurve; i < index; i++)
            {
                Debug.Log(i);
                Destroy(points[i].gameObject);
                points.RemoveAt(i);
            }

        points.Remove(pointToRemove);
        
    }

    private void UpdateBezier()
    {
        List<Vector3> positions = new List<Vector3>();
        int pointsToUse = bezierPointsPerCurve + 2;
        for (int i = 0; i < points.Count; i += pointsToUse)
        {
            positions.AddRange(BezierCurve.GetCurve(resolution, points.Skip(i).Take(pointsToUse).Select(x => x.gameObject.transform.position).ToList()));
        }


        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    private void OnValidate()
    {
        if (resolution < 1)
            resolution = 1;

        
    }
}
