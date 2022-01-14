#if UNITY_EDITOR
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
    //To support types and allow Class Types to be added, a Class attribute must be added

    [AttributeUsage(AttributeTargets.Field)]
    public class DropdownAttribute : PropertyAttribute
    {
        public bool Valid => loaded &&
            items != null && items.Count > 0;

        public int Count => items?.Count ?? 0;

        public string[] Options { get; private set; }
        public FieldInfo FieldInfo { get; private set; }

        public Type FieldType { get; private set; }
        public string FieldName { get; private set; }

        private Type converterType;
        private MethodInfo convertOperator;

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

        /// <summary>
        /// Parses the items from the options list to the field
        /// </summary>
        /// <param name="item">The new selected option to be parsed</param>
        /// <returns></returns>
        public virtual object Parse(object item)
        {
            Type iType = item.GetType();
            if (iType == FieldType) return item;

            if (item is Type t)
            {
                if (t.IsSubclassOf(FieldType))
                    return Activator.CreateInstance(t);
                if(FieldType == typeof(string))
                    return t.Name.Replace('_', ' ');
            }

            if (FieldType == typeof(string)) return item.ToString();

            if(iType.IsAssignableFrom(FieldType))
                return Convert.ChangeType(item, FieldType);

            if (converterType == null || converterType != iType) UpdateConvertingOperator(iType);

            if (convertOperator != null) return convertOperator.Invoke(null, new object[] { item });

            try
            { return Convert.ChangeType(item, FieldType); } 
            catch (Exception) { }

            Debug.LogError($"Dropdown Conversion Error: Dropdown {FieldInfo} cannot convert {iType} to {FieldType}. " +
                "You must either change the types, create a casting operator or..." +
                " \n create your own Attribute by extending DropdownAttribute and overriding the Parse method");

            return null;
        }

        private void UpdateConvertingOperator(Type type)
        {
            converterType = type;
            Type[] args = new Type[] { type };
            convertOperator = FieldType.GetMethod("op_Explicit", args) ?? FieldType.GetMethod("op_Implicit", args);
        }

        /// <summary>
        /// Get the names of the option for said option in the list
        /// </summary>
        /// <param name="o">The option whose name to get</param>
        /// <returns></returns>
        protected virtual string GetName(object o)
        {
            if (o is string str) return str.Replace('_', ' ');

            if (o is Type t) return t.Name.Replace('_', ' ');

            if (o is UnityEngine.Object unityObj) return unityObj.name;

            Type otype = o.GetType();
            if (otype.IsClass && !otype.IsPrimitive) return otype.Name.Replace('_', ' ');

            return o.ToString();
        }

        public void Load(FieldInfo fieldInfo, SerializedProperty prop)
        {
            if (loaded) return;

            loaded = true;

            FieldInfo = fieldInfo;
            //If it is an array, this type will be overriten to the element type in SetIndex
            FieldType = fieldInfo.FieldType; 

            TargetObject = prop.GetSourceObject(fieldInfo);

            if (!LoadList())
            {
                Debug.LogError($"Dropdown list {type?.Name ?? TargetObject}.{path} for {TargetObject}.{FieldInfo.Name} could not be loaded");
                return;
            }

            SetIndex();

            FieldName = FieldType.Name;
        }

        private bool LoadList()
        {
            if (type != null && ParseList(ReflectionExtension
                .GetValueFromPath(path, type: type).target)) return true;

            if (ParseList(ReflectionExtension
                .GetValueFromPath(path, null, TargetObject).target)) return true;

            return false;
        }

        /// <summary>
        /// Set the currently selected index.
        /// </summary>
        public void SetIndex()
        {
            var value = FieldInfo.GetValue(TargetObject);

            var llist = items.ToList();

            if (value is Array array)
            {
                FieldType = array.GetType().GetElementType();
                LoadIndexes(llist, array.Length, array, (arr, i) => arr.GetValue(i));
                return;
            }

            if (value is System.Collections.IList list)
            {
                FieldType = list.GetType().GetElementType();
                LoadIndexes(llist, list.Count, list, (arr, i) => arr[i]);
                return;
            }

            Index = new int[] { FindIndex(llist, value) };
            IsList = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The option list</param>
        /// <param name="count"></param>
        /// <param name="items"></param>
        /// <param name="get"></param>
        private void LoadIndexes<T>(List<object> list, int count, T items, Func<T, int, object> get)
        {
            Index = new int[count];
            IsList = true;
            for (int i = 0; i < count; i++)
                Index[i] = FindIndex(list, get(items, i));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int FindIndex(List<object> items, object value)
            => items.FindIndex(o => (value != null && value.Equals(o)) || GetName(o).Equals(GetName(value)) );

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

        /// <summary>
        /// Set the value of "index" as the option at index "selected".
        /// </summary>
        /// <param name="selected">The option index</param>
        /// <param name="index">The field list index or 0 if just the field</param>
        /// <returns></returns>
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
#endif