namespace IgnitedBox.Random.DropTables.CategorizedTable
{
    public class PathTable : NamedTable<string, string>
    {
        public PathTable(string prefix, params string[] items) 
            : base(prefix, items) { }

        public override void Remove(string item)
            => RemoveList(item);

        protected override string Get(int index)
            => name + items[index];

        protected override void Set(int index, string value)
            => items[index] = value;

        public override bool Contains(string item)
            => items.Contains(item);

        private protected override int DropIndex()
            => RandomInt(Count);
    }
}
