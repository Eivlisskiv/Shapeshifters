using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace IgnitedBox.EditorDropdown.ByClass
{
    public class Dropdown<TValue, TList> : BaseDropdown
    {
        public override bool Valid => items != null && items.Count > 0;

        public int Count => items?.Count ?? 0;

        public override string[] Options { get; protected set; }

        public TValue Value { get; private set; }

        public override int Index { get; set; }

        [SerializeField]
        public override string Selected { get; protected set; }

        private readonly ObservableCollection<TList> items;

        private readonly Func<TList, string> toString;
        private readonly Func<TList, TValue> parseValue;

        public Dropdown(IEnumerable<TList> items,
            Func<TList, TValue> parseValue = null,
            Func<TList, string> toString = null)
        {
            this.items = new ObservableCollection<TList>(items);
            this.items.CollectionChanged += OnListChanged;
            this.toString = toString ?? (o => o.ToString());
            this.parseValue = parseValue ?? ParseValueDefault;
            UpdateOptions();
        }

        private TValue ParseValueDefault(TList o)
        {
            if (o is TValue v) return v;

            Debug.LogError($"{o.GetType()} from list option dropdown could not be prased by default to {typeof(TValue)}");

            return default;
        }

        public override void UpdateValue()
            => parseValue(items[Index]);

        private void OnListChanged(object sender, NotifyCollectionChangedEventArgs e)
            => UpdateOptions();

        private void UpdateOptions()
            => Options = items.Select(o => toString(o)).ToArray();
    }
}
