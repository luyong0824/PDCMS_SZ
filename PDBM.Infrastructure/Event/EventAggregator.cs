using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    /// <summary>
    /// 事件聚合器
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        private readonly object syncObj = new object();
        private readonly Dictionary<Type, List<object>> eventHandlers = new Dictionary<Type, List<object>>();
        private readonly Func<object, object, bool> eventHandlerEquals = (o1, o2) =>
        {
            var o1Type = o1.GetType();
            var o2Type = o2.GetType();
            if (o1Type.IsGenericType && o1Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>) &&
                o2Type.IsGenericType && o2Type.GetGenericTypeDefinition() == typeof(ActionDelegatedEventHandler<>))
            {
                return o1.Equals(o2);
            }
            return o1Type == o2Type;
        };

        public EventAggregator()
        {
        }

        public EventAggregator(object[] handlers)
        {
            if (handlers == null)
            {
                throw new ArgumentNullException("handlers");
            }

            var registerEventHandlerMethod = (from m in this.GetType().GetMethods()
                                              let parameters = m.GetParameters()
                                              where m.Name == "Subscribe" &&
                                              parameters != null &&
                                              parameters.Length == 1 &&
                                              parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                                              select m).First();

            foreach (var handler in handlers)
            {
                if (handler == null)
                {
                    throw new ArgumentNullException("handler");
                }

                var handlerType = handler.GetType();
                var implementedInterfaces = handlerType.GetInterfaces();
                foreach (var implementedInterface in implementedInterfaces)
                {
                    if (implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventType = implementedInterface.GetGenericArguments().First();
                        registerEventHandlerMethod.MakeGenericMethod(eventType).Invoke(this, new object[] { handler });
                    }
                }
            }
        }

        public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            lock (syncObj)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null)
                    {
                        if (!handlers.Exists(handler => eventHandlerEquals(handler, eventHandler)))
                        {
                            handlers.Add(eventHandler);
                        }
                    }
                    else
                    {
                        handlers = new List<object>();
                        handlers.Add(eventHandler);
                    }
                }
                else
                {
                    eventHandlers.Add(eventType, new List<object> { eventHandler });
                }
            }
        }

        public void Subscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            if (eventHandlers == null)
            {
                throw new ArgumentNullException("eventHandlers");
            }

            foreach (var eventHandler in eventHandlers)
            {
                Subscribe<TEvent>(eventHandler);
            }
        }

        public void Subscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            if (eventHandlers == null)
            {
                throw new ArgumentNullException("eventHandlers");
            }

            foreach (var eventHandler in eventHandlers)
            {
                Subscribe<TEvent>(eventHandler);
            }
        }

        public void Subscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            if (eventHandlerFunc == null)
            {
                throw new ArgumentNullException("eventHandlerFunc");
            }

            Subscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Subscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            if (eventHandlerFuncs == null)
            {
                throw new ArgumentNullException("eventHandlerFuncs");
            }

            foreach (var eventHandlerFunc in eventHandlerFuncs)
            {
                Subscribe<TEvent>(eventHandlerFunc);
            }
        }

        public void Subscribe<TEvent>(params Action<TEvent>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            if (eventHandlerFuncs == null)
            {
                throw new ArgumentNullException("eventHandlerFuncs");
            }

            foreach (var eventHandlerFunc in eventHandlerFuncs)
            {
                Subscribe<TEvent>(eventHandlerFunc);
            }
        }

        public void Unsubscribe<TEvent>(IEventHandler<TEvent> eventHandler)
            where TEvent : class, IEvent
        {
            if (eventHandler == null)
            {
                throw new ArgumentNullException("eventHandler");
            }

            lock (syncObj)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null && handlers.Exists(handler => eventHandlerEquals(handler, eventHandler)))
                    {
                        var handlerToRemove = handlers.First(handler => eventHandlerEquals(handler, eventHandler));
                        handlers.Remove(handlerToRemove);
                    }
                }
            }
        }

        public void Unsubscribe<TEvent>(IEnumerable<IEventHandler<TEvent>> eventHandlers)
            where TEvent : class, IEvent
        {
            if (eventHandlers == null)
            {
                throw new ArgumentNullException("eventHandlers");
            }

            foreach (var eventHandler in eventHandlers)
            {
                Unsubscribe<TEvent>(eventHandler);
            }
        }

        public void Unsubscribe<TEvent>(params IEventHandler<TEvent>[] eventHandlers)
            where TEvent : class, IEvent
        {
            if (eventHandlers == null)
            {
                throw new ArgumentNullException("eventHandlers");
            }

            foreach (var eventHandler in eventHandlers)
            {
                Unsubscribe<TEvent>(eventHandler);
            }
        }

        public void Unsubscribe<TEvent>(Action<TEvent> eventHandlerFunc)
            where TEvent : class, IEvent
        {
            if (eventHandlerFunc == null)
            {
                throw new ArgumentNullException("eventHandlerFunc");
            }

            Unsubscribe<TEvent>(new ActionDelegatedEventHandler<TEvent>(eventHandlerFunc));
        }

        public void Unsubscribe<TEvent>(IEnumerable<Action<TEvent>> eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            if (eventHandlerFuncs == null)
            {
                throw new ArgumentNullException("eventHandlerFuncs");
            }

            foreach (var eventHandlerFunc in eventHandlerFuncs)
            {
                Unsubscribe<TEvent>(eventHandlerFunc);
            }
        }

        public void Unsubscribe<TEvent>(params Action<TEvent>[] eventHandlerFuncs)
            where TEvent : class, IEvent
        {
            if (eventHandlerFuncs == null)
            {
                throw new ArgumentNullException("eventHandlerFuncs");
            }

            foreach (var eventHandlerFunc in eventHandlerFuncs)
            {
                Unsubscribe<TEvent>(eventHandlerFunc);
            }
        }

        public void UnsubscribeAll<TEvent>()
            where TEvent : class, IEvent
        {
            lock (syncObj)
            {
                var eventType = typeof(TEvent);
                if (eventHandlers.ContainsKey(eventType))
                {
                    var handlers = eventHandlers[eventType];
                    if (handlers != null)
                    {
                        handlers.Clear();
                    }
                }
            }
        }

        public void UnsubscribeAll()
        {
            lock (syncObj)
            {
                eventHandlers.Clear();
            }
        }

        public IEnumerable<IEventHandler<TEvent>> GetSubscriptions<TEvent>()
            where TEvent : class, IEvent
        {
            var eventType = typeof(TEvent);
            if (eventHandlers.ContainsKey(eventType))
            {
                var handlers = eventHandlers[eventType];
                if (handlers != null)
                {
                    return handlers.Select(handler => handler as IEventHandler<TEvent>).ToList();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public void Publish<TEvent>(TEvent evt)
            where TEvent : class, IEvent
        {
            if (evt == null)
            {
                throw new ArgumentNullException("evt");
            }

            var eventType = evt.GetType();
            if (eventHandlers.ContainsKey(eventType))
            {
                var handlers = eventHandlers[eventType];
                if (handlers != null && handlers.Count() > 0)
                {
                    foreach (var handler in handlers)
                    {
                        var eventHandler = handler as IEventHandler<TEvent>;
                        if (eventHandler != null)
                        {
                            if (eventHandler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                            {
                                Task.Factory.StartNew(o => eventHandler.Handle((TEvent)o), evt);
                            }
                            else
                            {
                                eventHandler.Handle(evt);
                            }
                        }
                    }
                }
            }
        }

        public void Publish<TEvent>(TEvent evt, Action<TEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TEvent : class, IEvent
        {
            if (evt == null)
            {
                throw new ArgumentNullException("evt");
            }
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            var eventType = evt.GetType();
            if (eventHandlers.ContainsKey(eventType))
            {
                var handlers = eventHandlers[eventType];
                if (handlers != null && handlers.Count() > 0)
                {
                    List<Task> tasks = new List<Task>();
                    try
                    {
                        foreach (var handler in handlers)
                        {
                            var eventHandler = handler as IEventHandler<TEvent>;
                            if (eventHandler != null)
                            {
                                if (eventHandler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                                {
                                    tasks.Add(Task.Factory.StartNew(o => eventHandler.Handle((TEvent)o), evt));
                                }
                                else
                                {
                                    eventHandler.Handle(evt);
                                }
                            }
                        }
                        if (tasks.Count > 0)
                        {
                            if (timeout == null)
                            {
                                Task.WaitAll(tasks.ToArray());
                            }
                            else
                            {
                                Task.WaitAll(tasks.ToArray(), timeout.Value);
                            }
                        }
                        callback(evt, true, null);
                    }
                    catch (Exception ex)
                    {
                        callback(evt, false, ex);
                    }
                }
                else
                {
                    callback(evt, false, null);
                }
            }
            else
            {
                callback(evt, false, null);
            }
        }
    }
}
