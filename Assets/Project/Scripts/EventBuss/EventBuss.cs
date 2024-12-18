using System;
using System.Collections.Generic;

namespace Project.Scripts.EventBuss
{
    public class EventBuss : IEventBuss
    {
        private readonly Dictionary<Type, object> _listenersRegistry = new Dictionary<Type, object>();

        public void AddListener<T>(IListener<T> listener, int priority = 0) where T : new()
        {
            GetOrCreateRegistry<T>().AddListener(listener, priority);
        }

        public void RemoveListener<T>(IListener<T> listener) where T : new()
        {
            if (_listenersRegistry.TryGetValue(typeof(T), out var registry))
            {
                ((ListenerRegistry<T>)registry).RemoveListener(listener);
            }
        }

        public void Execute<T>(T value) where T : new()
        {
            if (_listenersRegistry.TryGetValue(typeof(T), out var registry))
            {
                ((ListenerRegistry<T>)registry).Execute(value);
            }
        }

        private ListenerRegistry<T> GetOrCreateRegistry<T>() where T : new()
        {
            if (!_listenersRegistry.TryGetValue(typeof(T), out var registry))
            {
                registry = new ListenerRegistry<T>();
                _listenersRegistry.Add(typeof(T), registry);
            }

            return (ListenerRegistry<T>)registry;
        }

        private class ListenerRegistry<T> where T : new()
        {
            private readonly List<(int Priority, IListener<T> Listener)> _listeners =
                new List<(int Priority, IListener<T> Listener)>();

            public void AddListener(IListener<T> listener, int priority)
            {
                _listeners.Add((priority, listener));
                _listeners.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            }

            public void RemoveListener(IListener<T> listener)
            {
                _listeners.RemoveAll(entry => entry.Listener == listener);
            }

            public void Execute(T command)
            {
                foreach (var entry in _listeners)
                {
                    entry.Listener.Execute(command);
                }
            }
        }
    }
}