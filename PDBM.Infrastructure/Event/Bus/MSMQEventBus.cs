using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Event.Bus
{
    /// <summary>
    /// 基于MSMQ的事件总线，支持分布式事务
    /// </summary>
    public class MSMQEventBus : DisposableObject, IEventBus
    {
        private readonly Guid id = Guid.NewGuid();
        private volatile bool committed = true;
        private readonly bool useInternalTransaction;
        private readonly object syncObj = new object();
        private readonly MessageQueue messageQueue;
        private readonly MSMQOptions options;
        private readonly Queue<object> mockQueue = new Queue<object>();

        public MSMQEventBus(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            options = new MSMQOptions(path);
            messageQueue = new MessageQueue(path, options.SharedModeDenyReceive, options.EnableCache, options.QueueAccessMode);
            messageQueue.Formatter = options.MessageFormatter;
            useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        public MSMQEventBus(string path, bool useInternalTransaction)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }

            options = new MSMQOptions(path, useInternalTransaction);
            messageQueue = new MessageQueue(path, options.SharedModeDenyReceive, options.EnableCache, options.QueueAccessMode);
            messageQueue.Formatter = options.MessageFormatter;
            this.useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        public MSMQEventBus(MSMQOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = options;
            messageQueue = new MessageQueue(options.Path, options.SharedModeDenyReceive, options.EnableCache, options.QueueAccessMode);
            messageQueue.Formatter = options.MessageFormatter;
            useInternalTransaction = options.UseInternalTransaction && messageQueue.Transactional;
        }

        private void SendMessage<TMessage>(TMessage message, MessageQueueTransaction transaction = null)
            where TMessage : class, IEvent
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            Message msmqMessage = new Message(message);
            if (useInternalTransaction)
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException("transaction");
                }
                messageQueue.Send(msmqMessage, transaction);
            }
            else
            {
                messageQueue.Send(msmqMessage, MessageQueueTransactionType.Automatic);
            }
        }

        private void SendMessage(object message, MessageQueueTransaction transaction = null)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            var sendMessageMethod = (from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
                                     where m.IsGenericMethod && m.Name == "SendMessage"
                                     select m).First();
            var evtType = message.GetType();
            sendMessageMethod.MakeGenericMethod(evtType).Invoke(this, new object[] { message, transaction });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (messageQueue != null)
                {
                    messageQueue.Close();
                    messageQueue.Dispose();
                }
            }
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
                mockQueue.Enqueue(message);
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
                    mockQueue.Enqueue(m);
                    committed = false;
                });
            }
        }

        public void Clear()
        {
            lock (syncObj)
            {
                mockQueue.Clear();
                committed = true;
            }
        }

        public bool DistributedTransactionSupported
        {
            get { return true; }
        }

        public bool Committed
        {
            get { return committed; }
        }

        public void Commit()
        {
            lock (syncObj)
            {
                if (useInternalTransaction)
                {
                    using (MessageQueueTransaction transaction = new MessageQueueTransaction())
                    {
                        try
                        {
                            transaction.Begin();
                            while (mockQueue.Count() > 0)
                            {
                                object msg = mockQueue.Dequeue();
                                this.SendMessage(msg, transaction);
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Abort();
                            throw;
                        }
                    }
                }
                else
                {
                    while (mockQueue.Count() > 0)
                    {
                        object msg = mockQueue.Dequeue();
                        this.SendMessage(msg);
                    }
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
