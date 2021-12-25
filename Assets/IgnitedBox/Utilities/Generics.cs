namespace IgnitedBox.Utilities
{
    public static class Generics
    {
        public static T ParamAs<T>(this object[] items, int index, T defaultValue = default)
            => items != null && index < items.Length && items[index] is T t ? t : defaultValue;
        public static T As<T>(this object value, T defaultValue = default)
            => value is T t ? t : defaultValue;
    }
}
