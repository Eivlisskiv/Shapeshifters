using System;
using System.Reflection;

namespace IgnitedBox.EditorDropdown.Utilities
{
    public static class ReflectionExtension
    {
        internal struct SearchResult
        {
            internal MemberInfo member;
            internal object target;
        }

        /// <summary>
        /// Get the member info and value of a member from a path starting at a static member.
        /// </summary>
        /// <param name="path">The path to follow.</param>
        /// <param name="type">The type in which the initial static member is located.</param>
        /// <returns>The MemberInfo and value at the end of the path.</returns>
        internal static SearchResult GetValueFromPath(string path, Type type)
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
        internal static SearchResult GetValueFromPath(string path, MemberInfo member, object instance)
        {
            string[] paths = path.Split('.');

            return GetInners(member, instance, paths, 0);
        }

        private static SearchResult GetInners(MemberInfo member, object instance, string[] path, int depth)
        {
            if (depth < path.Length)
            {
                SearchResult result = GetInner(instance, path[depth]);
                return GetInners(result.member, result.target, path, depth + 1); 
            }

            return new SearchResult { member = member, target = instance };
        }

        private static SearchResult GetInner(object instance, string variable)
        {
            Type type = instance.GetType();

            if (TryGetMember(type, variable, out MemberInfo member)
                && TryGetValue(member, instance, out object value))
                return new SearchResult { member = member, target = value };

            return new SearchResult { };
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
