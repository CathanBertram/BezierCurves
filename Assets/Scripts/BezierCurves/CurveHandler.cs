using BezierCurve;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CurveHandler : MonoBehaviour
{
    private LinkedList<BezierCurveDisplay> curves = new LinkedList<BezierCurveDisplay>();
    [SerializeField] private int initialCurvePoints = 1;
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
        c.Initialise(curveData);
        c.onDelete += OnCurveDelete;
        curves.AddLast(c);

    }

    public void OnCurveDelete(BezierCurveDisplay curveDisplayToDelete)
    {
        if(curves.Count > 2)
        {
            //Connect curves surrounding deleted curve
        }
       
    }

}
