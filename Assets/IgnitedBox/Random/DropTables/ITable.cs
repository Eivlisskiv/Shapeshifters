using System;
using System.Collections.Generic;

namespace IgnitedBox.Random.DropTables
{
    public abstract class Table
    {
        private static readonly System.Random rng = new System.Random();

        protected System.Random Random => rng;

        protected int RandomInt(int max) => rng.Next(max);
        protected double RandomDouble(int max) => rng.Next(max) + rng.NextDouble();

        private protected abstract int DropIndex();

        private protected abstract object GetObject(int index);

        public TI DropCore<TI>()
        {
            object item = this;
            while (item is Table tab)
            {
                int index = tab.DropIndex();
                if (index == -1) return default;
                item = tab.GetObject(index);
            }

            if (item is TI target) return target;
            throw new InvalidCastException("The Generic type T given does not match the dropped Item's type.");
        }
    }

    /// <summary>
    /// A drop table containing a list of TList items which returns TItem items;
    /// </summary>
    /// <typeparam name="TItem">The type of what is returned by the table;</typeparam>
    /// <typeparam name="TList">The type that the table actually stores;</typeparam>
    public abstract class DropTable<TItem, TList> : Table
    {
        protected readonly List<TList> items;

        public int Count => items.Count;

        public TItem this[int index]
        {
            get => Get(index);
            set => Set(index, value);
        }

        public DropTable()
        {
            items = new List<TList>();
        }

        public DropTable(IEnumerable<TList> items)
        { 
            this.items = new List<TList>(items);
        }

        protected abstract TItem Get(int index);

        protected abstract void Set(int index, TItem value);

        //List manipulation

        protected virtual void OnListChanged() { }

        public virtual bool Contains(TItem item)
            => items.FindIndex(i => i.Equals(item)) >= 0;

        public virtual void Add(TList item)
        {
            items.Add(item);
            OnListChanged();
        }

        public virtual void AddRange(IEnumerable<TList> items)
        {
            this.items.AddRange(items);
            OnListChanged();
        }

        public abstract void Remove(TItem item);

        protected virtual void RemoveList(TList item)
        {
            items.Remove(item);
            OnListChanged();
        }

        public virtual void RemoveAt(int index)
        {
            items.RemoveAt(index);
            OnListChanged();
        }

        public int FindIndex(Predicate<TList> match)
            => items.FindIndex(match);

        public TItem DropOne(out int index)
        {
            index = DropIndex();
            return index < 0 || index > Count ? default : Get(index);
        }

        private protected override object GetObject(int index)
            => Get(index);
    }
}
