using UnityEngine;

namespace Scripts.UI.InGame.Objectives
{
    public struct ObjectiveTracking
    {
        public static implicit operator ObjectiveTracking(Transform t)
        {
            return new ObjectiveTracking()
            {
                mobileTarget = t
            };
        }

        public static implicit operator ObjectiveTracking(Vector3 t)
        {
            return new ObjectiveTracking()
            {
                staticTarget = t
            };
        }

        public static implicit operator ObjectiveTracking(Vector2 t)
        {
            return new ObjectiveTracking()
            {
                staticTarget = t
            };
        }

        public static implicit operator Vector3(ObjectiveTracking ot)
            => ot.mobileTarget ? ot.mobileTarget.position : ot.staticTarget;

        public static implicit operator Vector2(ObjectiveTracking ot)
            => ot.mobileTarget ? ot.mobileTarget.position : ot.staticTarget;

        Transform mobileTarget;
        Vector3 staticTarget;

        public override bool Equals(object obj)
            => base.Equals(obj) || (obj is Transform tr && mobileTarget == tr);

        public override int GetHashCode() => base.GetHashCode();
    }
}
