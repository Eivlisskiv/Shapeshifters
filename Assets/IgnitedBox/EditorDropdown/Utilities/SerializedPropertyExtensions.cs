using System.Reflection;
using UnityEditor;

namespace IgnitedBox.EditorDropdown.Utilities
{
    public static class SerializedPropertyExtensions
    {
        public static int GetIndex(this SerializedProperty prop)
        {
            string path = prop.propertyPath;
            int s = path.IndexOf('[');

            if (s == -1) return 0;

            s++;
            int e = path.IndexOf(']');
            string r = path.Substring(s, e - s);
            return int.Parse(r);
        }

        private static string TrimPath(string path)
        {
            if (path.Contains("Array.data["))
            {
                int s = path.IndexOf('[') + 1;
                path = path.Substring(0, s - 1)
                    .Replace(".Array.data", "");
            }

            int target = path.LastIndexOf('.');

            if (target == -1) return null;

            path = path.Substring(0, target);

            return path;
        }

        public static object GetSourceObject(
            this SerializedProperty prop, FieldInfo field)
        {
            var mono = prop.serializedObject.targetObject;

            string path = TrimPath(prop.propertyPath);

            if (path == null) return mono;

            return ReflectionExtension.GetValueFromPath
                (path, field, mono).Item2;
        }
    }
}
