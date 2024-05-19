using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
namespace SimpleBezierCurve
{
    public class BezierCurve
    {
        private LinkedList<CurvePoint> curvePoints;
        private int curID = 0;
        public event Action onDirty;
        public event Action<CurvePoint> onDelete;
        public BezierCurve()
        {
            curvePoints = new LinkedList<CurvePoint>();
        }

        public Vector3[] GetCurve(int resolutionPerCurve)
        {
            var pointCount = resolutionPerCurve * curvePoints.Count - 1;
            List<Vector3> points = new List<Vector3>();
            var curNode = curvePoints.First;
            while(curNode != curvePoints.Last)
            {
                AddCurve(points, curNode, resolutionPerCurve);
                curNode = curNode.Next;
            }
            return points.ToArray();
        }

        private void AddCurve(List<Vector3> points, LinkedListNode<CurvePoint> curveNode, int resolutionPerCurve)
        {
            var curPoint = curveNode.Value;
            var nextPoint = curveNode.Next.Value;

            for(int i = 0; i < resolutionPerCurve; i++)
            {
                points.Add(GetPointAtT((float)i / resolutionPerCurve, curPoint, nextPoint));
            }
        }
        private Vector3 GetPointAtT(float t, CurvePoint curPoint, CurvePoint nextPoint)
        {
            //Create Array for Points in Curve
            Vector3[] pointsAtT = new Vector3[]
            {
                curPoint.CurvePointPosition,
                curPoint.BezierPointPosition(1),
                nextPoint.BezierPointPosition(0),
                nextPoint.CurvePointPosition
            };
            //Create a temp array to manipulate
            Vector3[] tempPoints = pointsAtT;

            //Loop through each line (consecutive points) and get the position between them at t
            //Repeat this until there is only one point
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
        public void AddCurvePoint(Vector3 position)
        {
            CurvePoint curvePoint = new CurvePoint(position, curID);
            curID++;
            curvePoints.AddLast(curvePoint);
            curvePoint.onDirty += OnDirty;
            curvePoint.onDelete += OnDelete;

            if(curvePoints.Count > 1)
            {
                AddPointBackwards(curvePoints.Last);
                AddPointForwards(curvePoints.Last.Previous);
            }
        }
        //Add Bezier Point to Previous Curve Point
        private void AddPointBackwards(LinkedListNode<CurvePoint> curveNode)
        {
            //If Previous Point is null Return
            var prev = curveNode.Previous;
            if (prev == null) return;

            var curVal = curveNode.Value;
            var prevVal = prev.Value;

            var dir = (prevVal.CurvePointPosition - curVal.CurvePointPosition).normalized;
            var dist = Vector3.Distance(curVal.CurvePointPosition, prevVal.CurvePointPosition);

            var addPosition = curVal.CurvePointPosition + (dir * (dist * 0.25f));

            curVal.CreateBezierPoint(0, addPosition);
            curVal.SetBezierPointPosition(0, addPosition);

            //OnDirtyPrevPoint(prevVal);
        }
        private void AddPointForwards(LinkedListNode<CurvePoint> curveNode)
        {
            var next = curveNode.Next;
            if (next == null) return;

            var curVal = curveNode.Value;
            var nextVal = next.Value;

            var dir = (nextVal.CurvePointPosition - curVal.CurvePointPosition).normalized;
            var dist = Vector3.Distance(curVal.CurvePointPosition, nextVal.CurvePointPosition);

            var addPosition = curVal.CurvePointPosition + (dir * (dist * 0.25f));


            curVal.CreateBezierPoint(1, addPosition);
            curVal.SetBezierPointPosition(1, addPosition);

            if(curveNode.Previous != null)
                OnDirtyPrevPoint(curveNode.Value);
        }

        private void OnDirty(CurvePoint curvePoint, byte point) 
        {
            if (point == 0)
                OnDirtyCurvePoint(curvePoint);
            if (point == 1)
                OnDirtyPrevPoint(curvePoint);
            if (point == 2)
                OnDirtyNextPoint(curvePoint);

            onDirty?.Invoke();
        }

        private void OnDirtyCurvePoint(CurvePoint curvePoint)
        {
            var prev = curvePoint.bezierPoints[0];
            var next = curvePoint.bezierPoints[1];

            if (prev == null || next == null) return;

            var midPoint = Easing.Ease(EasingType.Linear, prev.transform.position, next.transform.position, 0.5f);

            var dir = (curvePoint.curvePoint.transform.position - midPoint).normalized;
            var dist = Vector3.Distance(curvePoint.curvePoint.transform.position, midPoint);

            prev.transform.position += dir * dist;
            next.transform.position += dir * dist;
        }
        private void OnDirtyPrevPoint(CurvePoint curvePoint)
        {
            var prev = curvePoint.bezierPoints[0];
            var curvePointPosition = curvePoint.CurvePointPosition;
            var next = curvePoint.bezierPoints[1];

            if (prev == null || next == null) return;

            var dir = (prev.transform.position - curvePointPosition).normalized;
            var dist = Vector3.Distance(prev.transform.position, curvePointPosition);
            var newPosition = curvePointPosition - dir * dist;

            next.transform.position = newPosition;
        }

        private void OnDirtyNextPoint(CurvePoint curvePoint)
        {
            var prev = curvePoint.bezierPoints[0];
            var curvePointPosition = curvePoint.CurvePointPosition;
            var next = curvePoint.bezierPoints[1];

            if (prev == null || next == null) return;

            var dir = (next.transform.position - curvePointPosition).normalized;
            var dist = Vector3.Distance(next.transform.position, curvePointPosition);
            var newPosition = curvePointPosition - dir * dist;

            prev.transform.position = newPosition;
        }

        private void OnDelete(CurvePoint curvePoint) 
        {
            curvePoints.Remove(curvePoint);   

            onDelete?.Invoke(curvePoint); 
        }


    }
}
