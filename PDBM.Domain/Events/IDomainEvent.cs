using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Infrastructure.Event;

namespace PDBM.Domain.Events
{
    /// <summary>
    /// 领域事件接口
    /// </summary>
    public interface IDomainEvent : IEvent
    {
        /// <summary>
        /// 获取产生领域事件的事件源对象实例
        /// </summary>
        IEntity Source { get; }
    }
}
