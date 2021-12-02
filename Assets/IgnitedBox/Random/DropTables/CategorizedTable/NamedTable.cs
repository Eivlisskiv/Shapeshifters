namespace IgnitedBox.Random.DropTables.CategorizedTable
{
    public abstract class NamedTable<TItem, TList> : DropTable<TItem, TList>
    {
        public string Name => name;

        protected readonly string name;

        public NamedTable(string name, params TList[] items)
            : base(items) { this.name = name; }
    }
}
