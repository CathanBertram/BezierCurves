using System;
using UnityEngine;

public class DebugPoint : MonoBehaviour
{
    public float radius;
    public Color color;
    public event Action<DebugPoint> removeDebugPoint;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);

    }

    public void RemovePoint()
    {
        removeDebugPoint?.Invoke(this);
        Destroy(gameObject);
    }
}
