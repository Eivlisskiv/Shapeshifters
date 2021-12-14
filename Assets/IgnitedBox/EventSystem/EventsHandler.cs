using System.Collections.Generic;
using UnityEngine;

namespace IgnitedBox.EventSystem
{
    public class EventsHandler<TKey>
    {
        readonly Dictionary<TKey, EventContainer> eventsContainers
            = new Dictionary<TKey, EventContainer>();

        public void Invoke<TSource, TArgument>(TKey key, TSource source, TArgument arg)
        {
            EventContainer<TSource, TArgument> container = Get<TSource, TArgument>(key);
            container?.InvokeEvent(source, arg);
        }

        public void Subscribe<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventHandler func)
        {
            EventContainer<TSource, TArgument> container = Get<TSource, TArgument>(key);
            container?.Add(func);
        }

        public void UnSubscribe<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventHandler func)
        {
            EventContainer<TSource, TArgument> container = Get<TSource, TArgument>(key);
            container?.Remove(func);
        }

        public void CleanInstace(object target)
        {
            foreach (KeyValuePair<TKey, EventContainer> keypair in eventsContainers)
            {
                keypair.Value.CleanInstance(target);
            }
        }

        private EventContainer<TSource, TArgument> Get<TSource, TArgument>(TKey key)
        {
            if (!eventsContainers.TryGetValue(key, out EventContainer container))
            {
                container = new EventContainer<TSource, TArgument>();
                eventsContainers.Add(key, container);
            }

            if (container is EventContainer<TSource, TArgument> eventContainer)
                return eventContainer;

            Debug.LogError($"Event Container {container.GetType()} @ {key} does not match types " +
                $"{typeof(TSource)} &| {typeof(TArgument)}");
            return null;
        }
    }
}
