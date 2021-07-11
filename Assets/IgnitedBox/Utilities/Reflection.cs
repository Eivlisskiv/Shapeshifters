using System;
using System.Collections.Generic;
using System.Linq;

namespace IgnitedBox.Utilities
{
    public static class Reflection
    {
        public static IEnumerable<Type> GetImplements(this Type type)
        {
            var assembly = type.Assembly;
            return assembly.GetTypes().Where(t =>
                !t.IsInterface && !t.IsAbstract
                && t.IsSubclassOf(type));
        }
    }
}
