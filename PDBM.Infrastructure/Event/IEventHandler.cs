using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Event
{
    /// <summary>
    /// 事件处理器接口
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    public interface IEventHandler<in TEvent>
        where TEvent : class, IEvent
    {
        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="evt">需要处理的事件</param>
        void Handle(TEvent evt);
    }
}
