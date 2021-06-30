using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.ByAttribute
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : PropertyAttribute
    {
        public bool Valid => loaded && items != null && items.Count > 0;

        public int Count => items?.Count ?? 0;

        public string[] Options { get; private set; }
        public FieldInfo FieldInfo { get; private set; }
        public object TargetObject { get; private set; }

        public int Index { get; private set; }

        private readonly Type type;
        private readonly string path;

        private ObservableCollection<object> items;

        private bool loaded = false;

        public object this[int index] => items[index];

        public DropdownAttribute(Type type, string path)
        {
            this.type = type;
            this.path = path;
        }

        public DropdownAttribute(string path)
        {
            this.path = path;
        }

        public void Load(FieldInfo fieldInfo, SerializedProperty prop)
        {
            if (loaded) return;

            loaded = true;

            FieldInfo = fieldInfo;
            TargetObject = prop.GetSourceObject();


            if (type != null && GetList(type, null)) return;

            if (GetList(TargetObject.GetType(), FieldInfo.GetValue(TargetObject))) return;

            Debug.LogError($"Dropdown list {type?.Name} {path} for {FieldInfo.Name} could not be loaded");
        }

        private bool GetList(Type type, object instance)
        {
            var value = type.GetProperty(path)?.GetValue(instance)
                ?? type.GetField(path)?.GetValue(instance);

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

        internal bool SetValue(int index)
        {
            if (Index == index) return false;

            Index = index;
            FieldInfo.SetValue(TargetObject, Parse(Index));

            return true;
        }

        public virtual object Parse(int index)
        {
            object item = items[index];

            if (item is Type t) return t.Name;

            return item;
        }

        protected virtual string GetName(object o)
        {
            if (o is Type t) return t.Name;

            return o.ToString();
        }

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
            => UpdateOptions();

        private void UpdateOptions()
            => Options = items.Select(GetName).ToArray();


    }
}
