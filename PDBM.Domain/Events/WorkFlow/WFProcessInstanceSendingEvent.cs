using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;

namespace PDBM.Domain.Events.WorkFlow
{
    /// <summary>
    /// 工作流过程实例发送事件
    /// </summary>
    public class WFProcessInstanceSendingEvent : DomainEvent
    {
        public WFProcessInstanceSendingEvent(IEntity source)
            : base(source)
        {
        }
    }
}
