namespace IgnitedBox.EventSystem
{
    public class EventContainer<TSource, TArgument>
    {
        public delegate void EventHandler(TSource source, TArgument args);

        private event EventHandler Event;

        public virtual void InvokeEvent(TSource source, TArgument args)
        {
            if (Event == null) return;

            Event.Invoke(source, args);
        }

        public void Add(EventHandler func)
            => Event += func;

        public void Remove(EventHandler func)
            => Event -= func;
    }
}
