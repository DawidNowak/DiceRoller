using System;
using System.Collections.Generic;

namespace DiceRoller.Helpers
{
    public class PubSubEvent
    {
        
    }

    public class GameChangedEvent : PubSubEvent { }

    public interface IEventAgregator
    {
        void Subscribe<T>(Action action) where T : PubSubEvent;
        void Publish<T>() where T : PubSubEvent;
    }

    public class EventAggregator : IEventAgregator
    {
        private readonly IDictionary<Type, List<Action>> _aggregator = new Dictionary<Type, List<Action>>();

        public void Subscribe<T>(Action action) where T : PubSubEvent
        {
            if (_aggregator.TryGetValue(typeof(T), out var ret))
            {
                ret.Add(action);
            }
            else
            {
                _aggregator[typeof(T)] = new List<Action> { action };
            }
        }

        public void Publish<T>() where T : PubSubEvent
        {
            if (_aggregator.TryGetValue(typeof(T), out var ret))
            {
                ret.ForEach(a => a.Invoke());
            }
        }
    }
}
