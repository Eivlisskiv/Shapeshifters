using UnityEngine;

namespace Scripts.OOP.Utils
{
    public static class Vectors2
    {
        public static Vector2 FromDegAngle(float angle, float magnitude = 1)
            => FromRadAngle(angle * Mathf.Deg2Rad, magnitude);
        public static Vector2 FromRadAngle(float angle, float magnitude = 1)
            => new Vector2(Mathf.Cos(angle) * magnitude, Mathf.Sin(angle) * magnitude);

        public static Vector2 Rotate(this Vector2 vect, float angle)
            => Quaternion.Euler(0, 0, angle) * vect;

        public static float TrueAngle(this Vector2 from)
            => -TrueAngle(from, Vector2.right);

        public static float TrueAngle(Vector2 from, Vector2 to)
            => Vector2.Angle(from, to) * (from.y > to.y ? -1 : 1);
    }
}
