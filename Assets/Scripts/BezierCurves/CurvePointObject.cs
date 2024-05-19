using System;
using UnityEngine;
namespace SimpleBezierCurve
{
    public class CurvePointObject : MonoBehaviour, IClickable, IDraggable, IAltClickable
    {
        public event Action<CurvePointObject> onDirty;
        public event Action<CurvePointObject> onDelete;

        [SerializeField] protected float pointRadius;
        [SerializeField] private SphereCollider sphereCollider;

        private void Start()
        {
            sphereCollider.radius = pointRadius;
        }

        #region Interfaces
        public virtual void OnClick()
        {
        }
        public virtual void OnRelease()
        {
        }
        public virtual void OnAltClick()
        {
        }

        public virtual void OnAltRelease()
        {
            //Delete Point
            onDelete?.Invoke(this);
        }
        public virtual void OnStartDrag(Vector3 position)
        {

        }
        public virtual void OnDrag(Vector3 position)
        {
            transform.position = position;
            onDirty?.Invoke(this);
        }

        public virtual void OnDragEndDrag(Vector3 position)
        {
            transform.position = position;
            onDirty?.Invoke(this);
        }
        #endregion;

        public virtual void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, pointRadius);
        }
    }
}