using UnityEngine;

namespace IgnitedBox.UnityUtilities.Vectors
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

        public static float WorldAngle(this Vector2 from, Vector2 to)
        {
            int y = from.y > to.y ? -1 : 1;
            return Vector2.Angle(Vector2.right, to - from) * y;
        }
    }

    [System.Serializable]
    public struct SerializableVector2
    {
        public static implicit operator Vector2(SerializableVector2 v)
            => new Vector2(v.x, v.y);
        public static implicit operator SerializableVector2(Vector2 v)
            => new SerializableVector2(v.x, v.y);

        public float x;
        public float y;

        public SerializableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [System.Serializable]
    public struct SerializableVector2Int
    {
        public static implicit operator Vector2Int(SerializableVector2Int v)
            => new Vector2Int(v.x, v.y);
        public static implicit operator SerializableVector2Int(Vector2Int v)
            => new SerializableVector2Int(v.x, v.y);

        public int x;
        public int y;

        public SerializableVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
