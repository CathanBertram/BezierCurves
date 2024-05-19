using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SimpleBezierCurve
{
    public class BezierPointObject : CurvePointObject
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, pointRadius);
        }
        #region Interfaces
        public override void OnClick()
        {
        }
        public override void OnRelease()
        {
        }
        public override void OnAltClick()
        {
        }

        public override void OnAltRelease()
        {

        }
        #endregion
    }
}