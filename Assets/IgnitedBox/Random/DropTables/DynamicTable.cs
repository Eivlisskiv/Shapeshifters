using System.Collections.Generic;

namespace IgnitedBox.Random.DropTables
{
    public class DynamicTable<T> : DropTable<T, (int, T)>
    {
        public static implicit operator DynamicTable<T>((int, T)[] items)
            => new DynamicTable<T>(items);

        private int[] rates;
        private int maximum;

        public DynamicTable()
            : base() { }

        public DynamicTable(IEnumerable<(int, T)> items) : base(items)
        {
            OnListChanged();
        }

        public DynamicTable(params (int, T)[] items) : base(items)
        {
            OnListChanged();
        }

        public override void Remove(T item)
        {
            int index = items.FindIndex(it => it.Item2.Equals(item));
            RemoveAt(index);
        }

        protected override T Get(int index)
            => items[index].Item2;

        protected override void Set(int index, T value)
            => items[index] = (items[index].Item1, value);

        private protected override int DropIndex()
        {
            int roll = RandomInt(maximum);
            for (int i = 0; i < rates.Length; i++)
            {
                if (roll <= rates[i]) return i;
            }

            //This shouldn't hit, but if basic check fails, return a random element;
            return RandomInt(rates.Length);
        }

        protected override void OnListChanged()
        {
            rates = new int[items.Count];
            maximum = 0;
            for (int i = 0; i < items.Count; i++)
            {
                (int rate, _) = items[i];
                rates[i] = maximum += rate;
            }
        }
    }
}
