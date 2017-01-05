using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event.Bus
{
    /// <summary>
    /// MSMQ选项
    /// </summary>
    public class MSMQOptions
    {
        public bool SharedModeDenyReceive
        {
            get;
            set;
        }

        public bool EnableCache
        {
            get;
            set;
        }

        public QueueAccessMode QueueAccessMode
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public bool UseInternalTransaction
        {
            get;
            set;
        }

        public IMessageFormatter MessageFormatter
        {
            get;
            set;
        }

        public MSMQOptions(string path, bool sharedModeDenyReceive, bool enableCache, QueueAccessMode queueAccessMode, bool useInternalTransaction, IMessageFormatter messageFormatter)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("path");
            }
            if (messageFormatter == null)
            {
                throw new ArgumentNullException("messageFormatter");
            }

            Path = path;
            SharedModeDenyReceive = sharedModeDenyReceive;
            EnableCache = enableCache;
            QueueAccessMode = queueAccessMode;
            UseInternalTransaction = useInternalTransaction;
            MessageFormatter = messageFormatter;
        }

        public MSMQOptions(string path)
            : this(path, false, false, QueueAccessMode.SendAndReceive, false, new XmlMessageFormatter())
        {
        }

        public MSMQOptions(string path, bool useInternalTransaction)
            : this(path, false, false, QueueAccessMode.SendAndReceive, useInternalTransaction, new XmlMessageFormatter())
        {
        }
    }
}
