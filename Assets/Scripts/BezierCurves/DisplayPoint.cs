using System;
using Unity.VisualScripting;
using UnityEngine;

namespace BezierCurve
{
    public class DisplayPoint : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public bool isBezier { get; private set; } = false;
        public void SetBezier()
        { 
            meshRenderer.material = BezierStaticsSingleton.Instance.BezierMaterial;
            isBezier = true;
        }

        public event Action<DisplayPoint, int> onScroll;
        public event Action onUpdate;
        public event Action<DisplayPoint> onRemove;
        public event Action<DisplayPoint> onDeleteCurve;

        public void RemovePoint()
        {
            if (isBezier)
            {
                onRemove?.Invoke(this);
                Destroy(gameObject);
            }
            else
                onDeleteCurve?.Invoke(this);
        }
        public void OnScroll(int val)
        {
            onScroll?.Invoke(this, val);
        }
        public void OnDirty()
        {
            onUpdate?.Invoke();
        }
    }
}