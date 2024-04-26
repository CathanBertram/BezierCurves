using BezierCurve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public CurveHandler bezierCurve;
    public Camera mainCamera;
    private Vector2 mouseDragStart;
    private bool dragging;
    private DisplayPoint dragTarget = null;

    Plane plane = new Plane(Vector3.forward, Vector3.zero);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftMouseDown();            
        }      
        else if(Input.GetMouseButton(0))
        {
            HandleDrag();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            HandleLeftMouseUp();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            HandleRightMouse();          
        }
        HandleScroll();
    }

    private void HandleLeftMouseDown()
    {
        mouseDragStart = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<DisplayPoint>(out var debugPoint))
            {
                dragTarget = debugPoint;
                dragging = true;
            }
        }
    }
    private void HandleDrag()
    {
        if (dragTarget == null) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
        {
            var position = ray.GetPoint(distance);
            dragTarget.transform.position = position;
            dragTarget.OnDirty();
        }
    }
    private void HandleLeftMouseUp()
    {
        if (dragging)
        {
            dragging = false;
            dragTarget = null;
            return;
        }
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out float distance))
        {
            var position = ray.GetPoint(distance);
            bezierCurve.AddCurve(position);
        }
    }

    private void HandleRightMouse()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<DisplayPoint>(out var debugPoint))
                debugPoint.RemovePoint();
        }
    }

    private void HandleScroll()
    {
        if (Input.mouseScrollDelta == Vector2.zero)
            return;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit))
            return;

        if (!hit.transform.gameObject.TryGetComponent<DisplayPoint>(out var displayPoint))
            return;

        //if (!displayPoint.isBezier)
        //    return;


        if (Input.mouseScrollDelta.y > 0)
            displayPoint.OnScroll(1);
        else if (Input.mouseScrollDelta.y < 0)
            displayPoint.OnScroll(-1);
    }
}
