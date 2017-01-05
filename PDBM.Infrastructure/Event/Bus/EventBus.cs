using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Event.Bus
{
    /// <summary>
    /// 事件总线，不支持分布式事务
    /// </summary>
    public class EventBus : DisposableObject, IEventBus
    {
        private readonly Guid id = Guid.NewGuid();
        private volatile bool committed = true;
        private readonly object syncObj = new object();
        private Queue<object> messageQueue = new Queue<object>();
        private readonly IEventAggregator eventAggregator;
        private readonly MethodInfo publishMethod;

        public EventBus(IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            this.eventAggregator = eventAggregator;
            publishMethod = (from m in eventAggregator.GetType().GetMethods()
                             let parameters = m.GetParameters()
                             let methodName = m.Name
                             where methodName == "Publish" &&
                             parameters != null &&
                             parameters.Length == 1
                             select m).First();
        }

        protected override void Dispose(bool disposing)
        {
        }

        public Guid Id
        {
            get { return id; }
        }

        public void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            lock (syncObj)
            {
                messageQueue.Enqueue(message);
                committed = false;
            }
        }

        public void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent
        {
            if (messages == null)
            {
                throw new ArgumentNullException("messages");
            }

            lock (syncObj)
            {
                messages.ToList().ForEach(m =>
                {
                    if (m == null)
                    {
                        throw new ArgumentNullException("m");
                    }

                    messageQueue.Enqueue(m); committed = false;
                });
            }
        }

        public void Clear()
        {
            lock (syncObj)
            {
                messageQueue.Clear();
                committed = true;
            }
        }

        public bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public bool Committed
        {
            get { return committed; }
        }

        public void Commit()
        {
            lock (syncObj)
            {
                while (messageQueue.Count > 0)
                {
                    var evt = messageQueue.Dequeue();
                    var evtType = evt.GetType();
                    var method = publishMethod.MakeGenericMethod(evtType);
                    method.Invoke(eventAggregator, new object[] { evt });
                }
                committed = true;
            }
        }

        public void RollBack()
        {
            Clear();
        }
    }
}
