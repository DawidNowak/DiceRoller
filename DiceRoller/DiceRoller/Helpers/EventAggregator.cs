using System;
using System.Collections.Generic;

namespace DiceRoller.Helpers
{
    public class PubSubEvent { }
	public class PubSubEvent<T> : PubSubEvent { }

    public class GameChangedEvent : PubSubEvent { }
	public class AnimateRollChanged : PubSubEvent { }
	public class SaveStateChanged : PubSubEvent { }

    public interface IEventAgregator
    {
		void Subscribe<T>(Action<object> action) where T : PubSubEvent;
        void Publish<T>(object payload) where T : PubSubEvent;
	}

	public class EventAggregator : IEventAgregator
    {
		private readonly IDictionary<Type, List<Action<object>>> _aggregator = new Dictionary<Type, List<Action<object>>>();

		public void Subscribe<T>(Action<object> action) where T : PubSubEvent
		{
			if (_aggregator.TryGetValue(typeof(T), out var ret)) ret.Add(action);
			else _aggregator[typeof(T)] = new List<Action<object>> { action };
		}

		public void Publish<T>(object payload) where T : PubSubEvent
		{
			if (_aggregator.TryGetValue(typeof(T), out var ret)) ret.ForEach(a => a.Invoke(payload));
		}
	}
}
