using SimpleBezierCurve;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public BezierCurveHandler curveHandler;
    public Camera mainCamera;
    private Vector2 mouseDragStart;
    private bool dragging;
    private IDraggable dragTarget = null;

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
    }

    private void HandleLeftMouseDown()
    {
        mouseDragStart = Input.mousePosition;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<IDraggable>(out var debugPoint))
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
            dragTarget.OnDrag(position);
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
            //bezierCurve.AddCurve(position);
            curveHandler.AddPoint(position);
            
        }
    }

    private void HandleRightMouse()
    {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit))
        {
            if (hit.transform.gameObject.TryGetComponent<IAltClickable>(out var debugPoint))
                debugPoint.OnAltClick();
        }
    }
}
