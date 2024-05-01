using BezierCurve;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class CurveHandler : MonoBehaviour
{
    private LinkedList<BezierCurveDisplay> curves = new LinkedList<BezierCurveDisplay>();
    [SerializeField] private int initialCurvePoints = 1;

    public void AddCurve(DisplayPoint point)
    {
        var curve = Instantiate(BezierStaticsSingleton.Instance.CurveDisplayPrefab);
        curve.Initialise(point);
        curve.onDelete += OnCurveDelete;
        curves.AddLast(curve);
        return;
    }
    public void AddCurve(Vector3 position)
    {
        //If there are not curves create an initial curve
        if(curves.Count == 0)
        {
            var displayPoint = Instantiate(BezierStaticsSingleton.Instance.DisplayPointPrefab);
            displayPoint.transform.position = position;
            var curve = Instantiate(BezierStaticsSingleton.Instance.CurveDisplayPrefab);
            curve.Initialise(displayPoint);
            curve.onDelete += OnCurveDelete;
            curves.AddLast(curve);
            return;
        }
        //If there is only one curve check that the curve has a start and end point
        if (curves.Count == 1)
        {
            if(curves.First.Value.curveData.pointCount == 1)
            {
                curves.First.Value.curveData.AddPointLast(position, true, initialCurvePoints);
                return;
            }
        }
        //Add Additional Curves
        var finalPoint = UnityEngine.Object.Instantiate(BezierStaticsSingleton.Instance.DisplayPointPrefab);
        finalPoint.transform.position = position;
        var c = UnityEngine.Object.Instantiate(BezierStaticsSingleton.Instance.CurveDisplayPrefab);
        BezierCurveData curveData = new BezierCurveData(curves.Last.Value.GetEndPoint(), finalPoint, initialCurvePoints, BezierStaticsSingleton.Instance.CurveParentTransform);
        c.Initialise(curveData, true);
        c.onDelete += OnCurveDelete;
        curves.AddLast(c);

    }

    public void OnCurveDelete(BezierCurveDisplay curveDisplayToDelete, DisplayPoint deletionPoint)
    {
        if (curveDisplayToDelete.GetFirst() == deletionPoint && curveDisplayToDelete != curves.First.Value)
            return;
        
        //If More Than Two Curves and It's Not the Last Curve, Conjoin the Surrounding Curves
        if (curves.Count > 2)
        {
            if (!(curves.Find(curveDisplayToDelete) == curves.Last))
            {
                var node = curves.Find(curveDisplayToDelete);
                var prevCurve = node.Previous.Value;
                var nextCurve = node.Next.Value;

                nextCurve.SetFirst(prevCurve.GetLast());
                curveDisplayToDelete.DestroyCurve();
                curves.Remove(curveDisplayToDelete);

                return;
            }        
        }    
        
        if(curves.Count == 1)
        {
            bool first = curveDisplayToDelete.GetFirst() == deletionPoint;
            DisplayPoint cachedPoint = null;
            if(first)
            {
                cachedPoint = curveDisplayToDelete.GetLast();
                curveDisplayToDelete.DestroyCurve(false, true);
            }
            else
            {
                cachedPoint = curveDisplayToDelete.GetFirst();
                curveDisplayToDelete.DestroyCurve(true);
            }

            AddCurve(cachedPoint);

            return;
        }

        //If We're Deleting The First Curve, Make Sure To Destroy The First Point Too
        var curve = curves.Find(curveDisplayToDelete);
        if (curve.Previous == null) // Is First
        {
            //var lastPoint = curve.Value.GetLast();
            //curve.Next.Value.SetFirst(lastPoint);
            curveDisplayToDelete.DestroyCurve(false, true);
            curves.Remove(curveDisplayToDelete);
            return;
        }

        //var curNode = curves.Find(curveDisplayToDelete);
        //var prev = curNode.Previous.Value;

        //prev.SetLast(curNode.Value.GetFirst());

        curveDisplayToDelete.DestroyCurve(true);
        curves.Remove(curveDisplayToDelete);

    }

}
