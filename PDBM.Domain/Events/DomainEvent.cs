using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Domain.EventHandlers;
using PDBM.Infrastructure.Event;
using PDBM.Infrastructure.IoC;

namespace PDBM.Domain.Events
{
    /// <summary>
    /// 领域事件抽象类
    /// </summary>
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        private readonly IEntity source;
        private Guid id = Guid.NewGuid();
        private DateTime timestamp = DateTime.Now;

        public DomainEvent(IEntity source)
        {
            this.source = source;
        }

        /// <summary>
        /// 事件源
        /// </summary>
        public IEntity Source
        {
            get
            {
                return source;
            }
        }

        /// <summary>
        /// 事件唯一标识
        /// </summary>
        public Guid Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 事件发生的时间戳
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件接口类型</typeparam>
        /// <param name="domainEvent">领域事件实例</param>
        public static void Publish<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IDomainEvent
        {
            IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = ServiceLocator.Instance.GetServices<IDomainEventHandler<TDomainEvent>>();
            if (handlers != null && handlers.Count() > 0)
            {
                foreach (var handler in handlers)
                {
                    if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                    {
                        Task.Factory.StartNew(() => handler.Handle(domainEvent));
                    }
                    else
                    {
                        handler.Handle(domainEvent);
                    }
                }
            }
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TDomainEvent">领域事件接口类型</typeparam>
        /// <param name="domainEvent">领域事件实例</param>
        /// <param name="callback">回调函数</param>
        /// <param name="timeout">超时时间戳</param>
        public static void Publish<TDomainEvent>(TDomainEvent domainEvent, Action<TDomainEvent, bool, Exception> callback, TimeSpan? timeout = null)
            where TDomainEvent : class, IDomainEvent
        {
            IEnumerable<IDomainEventHandler<TDomainEvent>> handlers = ServiceLocator.Instance.GetServices<IDomainEventHandler<TDomainEvent>>();
            if (handlers != null && handlers.Count() > 0)
            {
                List<Task> tasks = new List<Task>();
                try
                {
                    foreach (var handler in handlers)
                    {
                        if (handler.GetType().IsDefined(typeof(ParallelExecutionAttribute), false))
                        {
                            tasks.Add(Task.Factory.StartNew(() => handler.Handle(domainEvent)));
                        }
                        else
                        {
                            handler.Handle(domainEvent);
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
                    callback(domainEvent, true, null);
                }
                catch (Exception ex)
                {
                    callback(domainEvent, false, ex);
                }
            }
            else
            {
                callback(domainEvent, false, null);
            }
        }
    }
}
