using SimpleBezierCurve.Display;
using UnityEngine;

namespace SimpleBezierCurve
{
    public class BezierCurveHandler : MonoBehaviour
    {
        private BezierCurve bezierCurve;
        [SerializeField] private BezierCurveDisplay bezierCurveDisplay;
        private void Awake()
        {
            bezierCurve = new BezierCurve();
            bezierCurve.onDirty += OnDirty;
            bezierCurve.onDelete += OnDelete;
        }
        public void AddPoint(Vector3 position)
        {
            bezierCurve.AddCurvePoint(position);
            bezierCurveDisplay.OnPointAdded();
            bezierCurveDisplay.UpdateDisplay(bezierCurve);
        }

        private void OnDirty()
        {
            bezierCurveDisplay.UpdateDisplay(bezierCurve);
        }

        private void OnDelete(CurvePointObject curvePointObject)
        {

        }
    }
}