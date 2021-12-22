using UnityEngine;

namespace IgnitedBox.UnityUtilities.Vectors
{
    public class Vectors4
    {

    }

    [System.Serializable]
    public class SerializableFloat4
    {
        public static implicit operator Vector4(SerializableFloat4 v)
            => new Vector4(v.x, v.y, v.z, v.w);
        public static implicit operator SerializableFloat4(Vector4 v)
            => new SerializableFloat4(v.x, v.y, v.z, v.w);

        public static implicit operator Color(SerializableFloat4 v)
            => new Color(v.x, v.y, v.z, v.w);
        public static implicit operator SerializableFloat4(Color v)
            => new SerializableFloat4(v.r, v.g, v.b, v.a);

        public float x;
        public float y;
        public float z;
        public float w;

        public SerializableFloat4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }

    [System.Serializable]
    public struct SerializableInt4
    {
        /*
        public static implicit operator Vector4Int(SerializableInt4 v)
            => new Vector3Int(v.x, v.y, v.z);
        public static implicit operator SerializableInt4(Vector4Int v)
            => new SerializableInt3(v.x, v.y, v.z);
        */

        public int x;
        public int y;
        public int z;

        public SerializableInt4(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}
