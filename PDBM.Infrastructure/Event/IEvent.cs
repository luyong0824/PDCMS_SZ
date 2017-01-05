using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// 事件唯一标识
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// 事件发生的时间戳
        /// </summary>
        DateTime Timestamp { get; }
    }
}
