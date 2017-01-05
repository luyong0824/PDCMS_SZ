using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Events;
using PDBM.Infrastructure.Event;

namespace PDBM.Domain.EventHandlers
{
    /// <summary>
    /// 领域事件处理器接口
    /// </summary>
    /// <typeparam name="TDomainEvent">领域事件类型</typeparam>
    public interface IDomainEventHandler<TDomainEvent> : IEventHandler<TDomainEvent>
       where TDomainEvent : class, IDomainEvent
    {
    }
}
