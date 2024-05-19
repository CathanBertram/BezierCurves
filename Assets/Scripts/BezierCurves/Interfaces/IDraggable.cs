using UnityEngine;

public interface IDraggable
{
    public void OnStartDrag(Vector3 position);
    public void OnDrag(Vector3 position);
    public void OnDragEndDrag(Vector3 position);
}
