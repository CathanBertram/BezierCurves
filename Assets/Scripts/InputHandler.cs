using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public MultiPointBezierCurve bezierCurve;
    public Camera mainCamera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftMouseDown();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleRightMouse();          
        }
    }
    void HandleLeftMouseDown()
    {
        Plane p = new Plane(Vector3.forward, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(p.Raycast(ray, out float distance))
        {
            var position = ray.GetPoint(distance);
            bezierCurve.AddPoint(position);
        }
    }

    void HandleRightMouse()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<DebugPoint>(out var debugPoint))
                debugPoint.RemovePoint();
        }
    }
}
