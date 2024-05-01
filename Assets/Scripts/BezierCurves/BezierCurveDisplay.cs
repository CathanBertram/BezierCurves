using BezierCurve;
using System;
using System.Linq;
using UnityEngine;

public class BezierCurveDisplay : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public BezierCurveData curveData { get; private set; }
    
    public event Action<BezierCurveDisplay, DisplayPoint> onDelete;

    public void Initialise(BezierCurveData curveData, bool shouldUpdate = false)
    {
        this.curveData = curveData;
        Initialise(shouldUpdate);
    }
    public void Initialise(DisplayPoint initialPoint, bool shouldUpdate = false)
    {
        curveData = new BezierCurveData(initialPoint, BezierStaticsSingleton.Instance.CurveParentTransform);
        Initialise(shouldUpdate);
    }

    public void Initialise(bool shouldUpdate = false)
    {
        curveData.onDirty += UpdateCurve;
        curveData.onDelete += DeleteCurve;

        if (shouldUpdate)
            UpdateCurve();
    }
    private void UpdateCurve()
    {
        var positions = curveData.GetCurve();
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void DeleteCurve(DisplayPoint deletionPoint)
    {
        onDelete?.Invoke(this, deletionPoint);
        Destroy(gameObject);
    }

    public void DestroyCurve(bool destroyLast = false, bool destroyFirst = false)
    {
        curveData.DestroyCurve(destroyLast, destroyFirst);
    }
    public DisplayPoint GetEndPoint()
    {
        return curveData.GetEndPoint();
    }

    public void SetFirst(DisplayPoint point)
    {
        curveData.SetFirst(point);
    }

    public void SetLast(DisplayPoint point)
    {
        curveData.SetLast(point);
    }

    public DisplayPoint GetFirst()
    {
        return curveData.GetFirstPoint();
    }

    public DisplayPoint GetLast()
    {
        return curveData.GetLastPoint();
    }
}
