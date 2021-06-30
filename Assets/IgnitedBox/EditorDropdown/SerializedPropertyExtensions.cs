using System;
using UnityEditor;

namespace IgnitedBox.EditorDropdown
{
    public static class SerializedPropertyExtensions
    {
        public static object GetSourceObject(
            this SerializedProperty prop)
        {
            var mono = prop.serializedObject.targetObject;
            string[] path = prop.propertyPath.Split('.');

            return GetInners(mono, path, 0);
        }

        private static object GetInners(object instance, string[] path, int depth)
        {
            if (depth < path.Length - 1)
                return GetInners(GetInner(instance, path[depth]), path, depth + 1);
            return instance;
        }

        private static object GetInner(object instance, string variable)
        {
            Type type = instance.GetType();

            return type.GetField(variable)?
                .GetValue(instance) ??
                type.GetProperty(variable)?
                .GetValue(instance);
        }
    }
}
