using UnityEngine;
namespace SimpleBezierCurve
{
    public class BezierCurveStatics : MonoBehaviour
    {
        #region Singleton
        private static BezierCurveStatics instance;
        public static BezierCurveStatics Instance => instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }
        #endregion

        public CurvePointObject curvePointObjectPrefab;
        public BezierPointObject bezierPointObjectPrefab;
    }
}