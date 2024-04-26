using BezierCurve;
using System;
using UnityEngine;

public class BezierCurveDisplay : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public BezierCurveData curveData { get; private set; }
    
    public event Action<BezierCurveDisplay> onDelete;

    public void Initialise(BezierCurveData curveData)
    {
        this.curveData = curveData;
        Initialise();
    }
    public void Initialise(DisplayPoint initialPoint)
    {
        curveData = new BezierCurveData(initialPoint, BezierStaticsSingleton.Instance.CurveParentTransform);
        Initialise();
    }

    public void Initialise()
    {
        curveData.onDirty += UpdateCurve;
        curveData.onDelete += DeleteCurve;
    }
    private void UpdateCurve()
    {
        var positions = curveData.GetCurve();
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    private void DeleteCurve()
    {
        onDelete?.Invoke(this);
        Destroy(gameObject);
    }
    public DisplayPoint GetEndPoint()
    {
        return curveData.GetEndPoint();
    }
}
