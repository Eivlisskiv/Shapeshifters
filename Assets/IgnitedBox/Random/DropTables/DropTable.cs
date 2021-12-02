using System.Collections.Generic;

namespace IgnitedBox.Random.DropTables
{
    public class DropTable<T> : DropTable<T, T>
    {
        public static implicit operator DropTable<T>(T[] items)
            => new DropTable<T>(items);

        public DropTable() 
            : base() { }

        public DropTable(IEnumerable<T> items)
            : base(items) { }

        protected override T Get(int index)
            => items[index];

        protected override void Set(int index, T item)
            => items[index] = item;

        public override void Remove(T item)
            => RemoveList(item);

        private protected override int DropIndex()
            => RandomInt(Count);
    }
}
