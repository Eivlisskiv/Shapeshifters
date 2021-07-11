using IgnitedBox.EditorDropdown.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.Attribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : PropertyAttribute
    {
        public bool Valid => loaded &&
            items != null && items.Count > 0;

        public int Count => items?.Count ?? 0;

        public string[] Options { get; private set; }
        public FieldInfo FieldInfo { get; private set; }
        public object TargetObject { get; private set; }

        public int[] Index { get; private set; }

        public bool IsList { get; private set; }

        private readonly Type type;
        private readonly string path;

        private ObservableCollection<object> items;

        private bool loaded = false;

        public object this[int index] => items[index];

        /// <summary>
        /// Creates a dropdown popup in the editor for this field.
        /// </summary>
        /// <param name="type">The type where the static list is located.</param>
        /// <param name="path">The name of the field or property of the list.</param>
        public DropdownAttribute(Type type, string path)
        {
            this.type = type;
            this.path = path;
        }

        /// <summary>
        /// Creates a dropdown popup in the editor for this field.
        /// </summary>
        /// <param name="path">The name of the field or property of the list</param>
        public DropdownAttribute(string path)
        {
            this.path = path;
        }

        public virtual object Parse(object item)
        {
            if (item is Type t) return t.Name.Replace('_', ' ');

            return item;
        }

        protected virtual string GetName(object o)
        {
            if (o is Type t) return t.Name.Replace('_', ' ');

            if (o is UnityEngine.Object unityObj) return unityObj.name;

            return o.ToString();
        }

        public void Load(FieldInfo fieldInfo, SerializedProperty prop)
        {
            if (loaded) return;

            loaded = true;

            FieldInfo = fieldInfo;

            TargetObject = prop.GetSourceObject(fieldInfo);

            if (LoadList())
            {
                SetIndex();
                return;
            }

            Debug.LogError($"Dropdown list {type?.Name ?? TargetObject}.{path} for {TargetObject}.{FieldInfo.Name} could not be loaded");
        }

        private bool LoadList()
        {
            if (type != null && ParseList(ReflectionExtension
                .GetValueFromPath(path, type: type).Item2)) return true;

            if (ParseList(ReflectionExtension
                .GetValueFromPath(path, null, TargetObject).Item2)) return true;

            return false;
        }

        public void SetIndex()
        {
            var value = FieldInfo.GetValue(TargetObject);

            var llist = items.ToList();

            if (value is Array array)
            {
                LoadIndexes(llist, array.Length, array, (arr, i) => arr.GetValue(i));
                return;
            }

            if (value is System.Collections.IList list)
            {
                LoadIndexes(llist, list.Count, list, (arr, i) => arr[i]);
                return;
            }

            Index = new int[] { FindIndex(llist, value) };
            IsList = false;
        }

        private void LoadIndexes<T>(List<object> list, int count, T items, Func<T, int, object> get)
        {
            Index = new int[count];
            IsList = true;
            for (int i = 0; i < count; i++)
                Index[i] = FindIndex(list, get(items, i));
        }

        private int FindIndex(List<object> items, object value)
            => items.FindIndex(o => Parse(o).Equals(value));

        private bool ParseList(object value)
        {
            if (value == null) return false;

            if (value is IEnumerable<object> list)
            {
                SetList(list);
                return true;
            }

            return false;
        }

        private void SetList(IEnumerable<object> list)
        {
            items = new ObservableCollection<object>(list);
            items.CollectionChanged += OnListChanged;
            UpdateOptions();
        }

        internal bool SetValue(int selected, int index = 0)
        {
            if (Index[index] == selected) return false;

            Index[index] = selected;

            object value = Parse(Index[index]);

            if (!IsList)
            {
                FieldInfo.SetValue(TargetObject, value);
                return true;
            }

            var field = FieldInfo.GetValue(TargetObject);

            if (field is Array array)
            {
                array.SetValue(value, index);
                return true;
            }

            if (field is System.Collections.IList list)
            {
                list[index] = value;
                return true;
            }

            return false;
        }

        public object Parse(int index)
            => Parse(items[index]);

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
            => UpdateOptions();

        private void UpdateOptions()
            => Options = items.Select(GetName).ToArray();
    }
}
