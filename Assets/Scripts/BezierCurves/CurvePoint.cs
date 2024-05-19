using System;
using UnityEngine;

namespace SimpleBezierCurve
{
    public class CurvePoint
    {
        public event Action<CurvePoint, byte> onDirty;
        public event Action<CurvePoint> onDelete;
        private int id;

        public CurvePointObject curvePoint { get; private set; }
        public BezierPointObject[] bezierPoints { get; private set; }
        public Vector3 CurvePointPosition => curvePoint.transform.position;
        public Vector3 BezierPointPosition(byte index)
        {
            if (index > 1) return Vector3.zero;

            return bezierPoints[index].transform.position;
        }
        public CurvePoint(Vector3 position, int id)
        {
            onDirty = null;
            onDelete = null;
            this.id = id;
            curvePoint = GameObject.Instantiate(BezierCurveStatics.Instance.curvePointObjectPrefab);
            curvePoint.transform.position = position;
            curvePoint.onDirty += OnDirty;
            curvePoint.onDelete += OnDelete;
            curvePoint.name = $"Curve Point {id}";
            bezierPoints = new BezierPointObject[2];
        }

        public void SetBezierPointPosition(byte pointToSet, Vector3 position)
        {
            if (pointToSet > 1) return;
            bezierPoints[pointToSet].UpdatePosition(position);
        }

        public BezierPointObject CreateBezierPoint(byte pointToSet, Vector3 position)
        {
            var bezierPoint = GameObject.Instantiate(BezierCurveStatics.Instance.bezierPointObjectPrefab);
            bezierPoint.transform.position = position;
            bezierPoint.onDirty += OnDirty;
            bezierPoint.onDelete += OnDelete;
            bezierPoint.name = $"Bezier Point {id} {pointToSet}";
            bezierPoints[pointToSet] = bezierPoint;
            return bezierPoint;
        }

        private void OnDirty(CurvePointObject curvePointObject) 
        { 
            if (curvePointObject == curvePoint)
                onDirty?.Invoke(this, 0);
            else if (curvePointObject == bezierPoints[0]) // Prev
                onDirty?.Invoke(this, 1);
            else if (curvePointObject == bezierPoints[1]) // Next
                onDirty?.Invoke(this, 2);


        }
        private void OnDelete(CurvePointObject curvePointObject) 
        {
            GameObject.DestroyImmediate(curvePoint);
            if (bezierPoints[0] != null)
                GameObject.DestroyImmediate(bezierPoints[0]);
            if (bezierPoints[1] != null)
                GameObject.DestroyImmediate(bezierPoints[1]);
            onDelete?.Invoke(this);
        }

    }
}