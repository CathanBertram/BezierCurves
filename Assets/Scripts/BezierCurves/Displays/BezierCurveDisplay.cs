using UnityEngine;

namespace SimpleBezierCurve.Display
{
    public class BezierCurveDisplay : MonoBehaviour
    {
        [SerializeField] private BezierCurve bezierCurve;
        [SerializeField] protected int resolutionPerPoint;

        private void Awake()
        {
            bezierCurve = new BezierCurve();
            bezierCurve.onDirty += OnDirty;
            bezierCurve.onDelete += OnDelete;
        }
        public void AddPoint(Vector3 position)
        {
            bezierCurve.AddCurvePoint(position);
            OnPointAdded();
            UpdateDisplay(bezierCurve);
        }

        private void OnDirty()
        {
            UpdateDisplay(bezierCurve);
        }

        private void OnDelete(CurvePoint curvePoint)
        {
            OnDirty();
        }
        public virtual void UpdateDisplay(BezierCurve bezierCurve)
        {

        }

        public virtual void OnPointAdded()
        {

        }

        private void OnValidate()
        {
            if (UnityEngine.Application.isPlaying)
            {
                bezierCurve.UpdateConnection();
                UpdateDisplay(bezierCurve);

            }
        }
    }
}