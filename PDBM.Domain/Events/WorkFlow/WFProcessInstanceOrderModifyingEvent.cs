using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;

namespace PDBM.Domain.Events.WorkFlow
{
    /// <summary>
    /// 工作流实例中的单据修改事件
    /// </summary>
    public class WFProcessInstanceOrderModifyingEvent : DomainEvent
    {
        public WFProcessInstanceOrderModifyingEvent(IEntity source)
            : base(source)
        {
        }
    }
}
