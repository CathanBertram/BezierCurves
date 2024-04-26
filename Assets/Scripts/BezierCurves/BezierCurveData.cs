using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BezierCurve
{
    public class BezierCurveData
    {
        public int pointCount => points.Count;
        private LinkedList<DisplayPoint> points;
        private Transform parentTransform;
        public event Action onDirty;
        public event Action onDelete;
        private void OnDirty() { onDirty?.Invoke(); }
        public BezierCurveData(DisplayPoint initialPoint, Transform parentTransform)
        {
            this.parentTransform = parentTransform;
            points = new LinkedList<DisplayPoint>();
            points.AddFirst(initialPoint);
            initialPoint.onRemove += RemovePoint;
            initialPoint.onUpdate += OnDirty;
            initialPoint.transform.parent = parentTransform;
        }
        public BezierCurveData(DisplayPoint initialPoint, DisplayPoint finalPoint, int initialBezierPointCount, Transform parentTransform)
        {
            this.parentTransform = parentTransform;
            points = new LinkedList<DisplayPoint>();
            points.AddFirst(initialPoint);
            points.AddLast(finalPoint);
            AddPointsAfter(0, initialBezierPointCount);
            initialPoint.onRemove += RemovePoint;
            initialPoint.onUpdate += OnDirty;
            initialPoint.onScroll += OnAddPoint;
            finalPoint.onRemove += RemovePoint;
            finalPoint.onUpdate += OnDirty;
            finalPoint.onScroll += OnAddPoint;
            initialPoint.transform.parent = parentTransform;
        }
        public DisplayPoint GetEndPoint()
        {
            return points.Last();
        }

        private void AddPointsAfter(int initialPointIndex, int pointsToAdd)
        {
            var curPoint = points.ElementAt(initialPointIndex);
            var initialPosition = curPoint.transform.position;
            var endPosition = points.ElementAt(initialPointIndex + 1).transform.position;
            var tIncrement = 1f / (pointsToAdd + 1);
            for (int i = 1; i <= pointsToAdd; i++)
            {
                curPoint = AddPoint(Easing.Ease(EasingType.Linear, initialPosition, endPosition, i * tIncrement), curPoint);
            }
        }
        public DisplayPoint AddPoint(Vector3 position, DisplayPoint parentPoint)
        {
            var newPoint = CreatePoint(position);
            newPoint.onRemove += RemovePoint;
            newPoint.onUpdate += OnDirty;
            newPoint.onScroll += OnAddPoint;
            points.AddAfter(points.Find(parentPoint), newPoint);
            newPoint.SetBezier();
            onDirty?.Invoke();
            return newPoint;
        }
        public void AddPointAdjacentLeft(DisplayPoint centrePoint)
        {
            if (centrePoint == points.First()) return;

            var prevPoint = points.Find(centrePoint).Previous;
            var middlePos = Easing.Ease(EasingType.Linear, prevPoint.Value.transform.position, centrePoint.transform.position, 0.5f);
            AddPoint(middlePos, prevPoint.Value);
        }
        public void AddPointAdjacentRight(DisplayPoint centrePoint)
        {
            if (centrePoint == points.Last()) return;

            var nextPoint = points.Find(centrePoint).Next;
            var middlePos = Easing.Ease(EasingType.Linear, centrePoint.transform.position, nextPoint.Value.transform.position, 0.5f);
            AddPoint(middlePos, centrePoint);
        }

        public void AddPointLast(Vector3 position, bool addBezierPoints = false, int bezierPointCount = 1)
        {
            var newPoint = CreatePoint(position);
            newPoint.onRemove += RemovePoint;
            newPoint.onUpdate += OnDirty;
            newPoint.onScroll += OnAddPoint;
            points.AddLast(newPoint);

            if (addBezierPoints)
                AddPointsAfter(0, bezierPointCount);
        }
        public void RemovePoint(DisplayPoint point)
        {
            //If point to delete is either the first or last it is necessary to delete the entire curve
            if (point == points.First() || point == points.Last())
            { 
                DeleteCurve(point);
                return;
            }

            points.Remove(point);
            GameObject.Destroy(point);
            onDirty?.Invoke();

        }
        private void OnAddPoint(DisplayPoint centre, int direction)
        {
            if (direction == -1)
                AddPointAdjacentLeft(centre);
            else if(direction == 1)
                AddPointAdjacentRight(centre);
        }
        private void DeleteCurve(DisplayPoint point)
        {
            //Destroy any unnecessary points
            while(points.Count > 2)
            {
                var temp = points.First.Next;
                GameObject.Destroy(temp.Value);
                points.Remove(temp);
            }
            GameObject.Destroy(point);
            points.Remove(point);
            onDelete?.Invoke();
        }
        public DisplayPoint CreatePoint(Vector3 position)
        {
            var newPoint = UnityEngine.Object.Instantiate(BezierStaticsSingleton.Instance.DisplayPointPrefab) as DisplayPoint;
            newPoint.transform.SetParent(parentTransform);
            newPoint.transform.position = position;
            return newPoint;
        }

        public Vector3[] GetCurve()
        {
            //Get Resolution of Curve
            int resolution = BezierStaticsSingleton.Instance.CurveResolution;
            //Get LinkedList as List of Positions (Vector3)
            var curvePoints = points.Select(x => x.gameObject.transform.position).ToList();

            //Create Array for Positions
            Vector3[] positions = new Vector3[resolution];
            //Add start position for curve
            positions[0] = curvePoints[0];

            //Get the positions for the curve
            for (int i = 1; i < resolution; i++)
            {
                positions[i] = GetPointAtT((float)i / resolution, curvePoints);
            }
            //Add end position for curve
            positions[resolution - 1] = curvePoints[curvePoints.Count - 1];
            return positions.ToArray();
        }

        private Vector3 GetPointAtT(float t, List<Vector3> curvePoints)
        {
            //Create Array for the Number of Points (Cant explain properly)
            Vector3[] pointsAtT = new Vector3[points.Count - 1];

            //Get the Position at t for each Line (Each line is from point N to point N + 1)
            for (int i = 0; i < points.Count - 1; i++)
            {
                pointsAtT[i] = Easing.Ease(EasingType.Linear, curvePoints[i], curvePoints[i + 1], t);
            }
            //If the number of points = 1 we can return that point
            if (pointsAtT.Length == 1)
                return pointsAtT[0];

            //If the number of points > 1 we need to loop over the remaining points recursively until the number of points = 1
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
}