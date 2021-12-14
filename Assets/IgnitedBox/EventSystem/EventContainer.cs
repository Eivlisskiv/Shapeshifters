using System;

namespace IgnitedBox.EventSystem
{
    public abstract class EventContainer
    {
        public abstract void CleanInstance(object target);
    }

    public class EventContainer<TSource, TArgument> : EventContainer
    {
        public delegate void EventHandler(TSource source, TArgument args);

        private event EventHandler Event;

        public override void CleanInstance(object target)
        {
            if (Event == null) return;

            Delegate[] evnts = Event.GetInvocationList();
            for (int i = 0; i < evnts.Length; i++)
            {
                Delegate action = evnts[i];
                if (action.Target == target)
                    Event = (EventHandler)Delegate.RemoveAll(Event, action);
            }
        }

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
