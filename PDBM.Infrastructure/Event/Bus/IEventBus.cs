using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Infrastructure.Common;

namespace PDBM.Infrastructure.Event.Bus
{
    /// <summary>
    /// 事件总线接口
    /// </summary>
    public interface IEventBus : IUnitOfWork, IDisposable
    {
        Guid Id { get; }

        void Publish<TMessage>(TMessage message)
            where TMessage : class, IEvent;

        void Publish<TMessage>(IEnumerable<TMessage> messages)
            where TMessage : class, IEvent;

        void Clear();
    }
}
