using UnityEngine;

namespace BezierCurve
{
    public class BezierStaticsSingleton : MonoBehaviour
    {
        private static BezierStaticsSingleton instance;
        public static BezierStaticsSingleton Instance => instance;
        private void Awake()
        {
            if (instance != null)
                Destroy(this);
            else
                instance = this;
        }

        [SerializeField] private DisplayPoint displayPointPrefab;
        public DisplayPoint DisplayPointPrefab => displayPointPrefab;
        [SerializeField] private int curveResolution;
        public int CurveResolution => curveResolution;

        [SerializeField] private Transform curveParentTransform;
        public Transform CurveParentTransform => curveParentTransform;

        [SerializeField] private BezierCurveDisplay curveDisplayPrefab;
        public BezierCurveDisplay CurveDisplayPrefab => curveDisplayPrefab;

        [SerializeField] private Material bezierMaterial;
        public Material BezierMaterial => bezierMaterial;
    }
}