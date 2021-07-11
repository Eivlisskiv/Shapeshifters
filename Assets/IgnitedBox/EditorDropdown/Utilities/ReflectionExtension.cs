using System;
using System.Reflection;

namespace IgnitedBox.EditorDropdown.Utilities
{
    public static class ReflectionExtension
    {
        /// <summary>
        /// Get the member info and value of a member from a path starting at a static member.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        /// <param name="type">The type in which the initial static member is located.</param>
        /// <returns>The MemberInfo and value at the end of the path.</returns>
        public static (MemberInfo, object) GetValueFromPath(string path, Type type)
        {
            string[] paths = path.Split('.');
            TryGetMember(type, paths[0], out MemberInfo info);
            TryGetValue(info, null, out object value);
            return GetInners(info, value, paths, 1);
        }

        /// <summary>
        /// Get the member info and value of a member from a path starting at an instance.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        /// <param name="member">The MemberInfo of the starting instance</param>
        /// <param name="instance">The instance from which to start the path</param>
        /// <returns>The MemberInfo and value at the end of the path.</returns>
        public static (MemberInfo, object) GetValueFromPath(string path, MemberInfo member, object instance)
        {
            string[] paths = path.Split('.');

            return GetInners(member, instance, paths, 0);
        }

        private static (MemberInfo, object) GetInners(MemberInfo member, object instance, string[] path, int depth)
        {
            if (instance == null) return (null, null);

            if (depth < path.Length)
            {
                (MemberInfo info, object value) = GetInner(instance, path[depth]);
                return GetInners(info, value, path, depth + 1); 
            }

            return (member, instance);
        }

        private static (MemberInfo, object) GetInner(object instance, string variable)
        {
            Type type = instance.GetType();

            if (TryGetMember(type, variable, out MemberInfo member)
                && TryGetValue(member, instance, out object value))
                return (member, value);

            return (null, null);
        }

        public static bool TryGetValue(this MemberInfo member, object instance, out object value)
        {
            value = member is FieldInfo field ? field.GetValue(instance)
                : member is PropertyInfo prop ? prop.GetValue(instance)
                : null;
            return value != null;
        }

        public static bool TryGetMember(this Type type, string name, out MemberInfo field)
        {
            field = (MemberInfo)type.GetField(name) ?? type.GetProperty(name);
            return field != null;
        }
    }
}
