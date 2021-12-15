using UnityEngine;

namespace IgnitedBox.UnityUtilities.Vectors
{
    public static class Vectors3
    {
        public static Vector3 Div(this Vector3 a, Vector3 b)
            => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
    }

    [System.Serializable]
    public struct SerializableFloat3
    {
        public static implicit operator Vector3(SerializableFloat3 v)
            => new Vector3(v.x, v.y, v.z);
        public static implicit operator SerializableFloat3(Vector3 v)
            => new SerializableFloat3(v.x, v.y, v.z);

        public float x;
        public float y;
        public float z;

        public SerializableFloat3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [System.Serializable]
    public struct SerializableInt3
    {
        public static implicit operator Vector3Int(SerializableInt3 v)
            => new Vector3Int(v.x, v.y, v.z);
        public static implicit operator SerializableInt3(Vector3Int v)
            => new SerializableInt3(v.x, v.y, v.z);

        public int x;
        public int y;
        public int z;

        public SerializableInt3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
