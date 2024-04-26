using System;
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

        public event Action onUpdate;
        public event Action<DisplayPoint> onRemove;

        public void RemovePoint()
        {
            onRemove?.Invoke(this);
            Destroy(gameObject);
        }

        public void OnDirty()
        {
            onUpdate?.Invoke();
        }
    }
}