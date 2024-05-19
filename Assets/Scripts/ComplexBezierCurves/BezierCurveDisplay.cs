using System;
using System.Linq;
using UnityEngine;

namespace ComplexBezierCurve
{

    public class BezierCurveDisplay : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        public int id;
        public BezierCurveData curveData { get; private set; }

        public event Action<BezierCurveDisplay, DisplayPoint> onDelete;

        public void Initialise(BezierCurveData curveData, int id, bool shouldUpdate = false)
        {
            this.curveData = curveData;
            Initialise(id, shouldUpdate);
        }
        public void Initialise(DisplayPoint initialPoint, int id, bool shouldUpdate = false)
        {
            curveData = new BezierCurveData(initialPoint, BezierStaticsSingleton.Instance.CurveParentTransform);
            Initialise(id, shouldUpdate);
        }

        public void Initialise(int id, bool shouldUpdate = false)
        {
            curveData.onDirty += UpdateCurve;
            curveData.onDelete += DeleteCurve;
            this.id = id;

            if (shouldUpdate)
                UpdateCurve();
        }
        public void OnDestroy()
        {
            curveData.onDirty -= UpdateCurve;
            curveData.onDelete -= DeleteCurve;
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
}