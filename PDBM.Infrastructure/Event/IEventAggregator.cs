using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    /// <summary>
    /// 事件聚合器接口
    /// </summary>
    public interface IEventAggregator
    {
        void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;

        void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;

        void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;

        void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent;

        void Subscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerFuncs)
            where TEvent : class, IEvent;

        void Subscribe<TEvent>(params Action<TEvent>[] eventHandlerFuncs)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerFuncs)
            where TEvent : class, IEvent;

        void Unsubscribe<TEvent>(params Action<TEvent>[] eventHandlerFuncs)
            where TEvent : class, IEvent;

        void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent;

        void UnsubscribeAll();

        IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent;

        void Publish<TEvent>(TEvent evt)
            where TEvent : class, IEvent;

        void Publish<TEvent>(TEvent evt, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent;
    }
}
