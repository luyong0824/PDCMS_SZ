using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Common;
using PDBM.Domain.Models.WorkFlow;

namespace PDBM.Domain.Events.WorkFlow
{
    /// <summary>
    /// 工作流活动实例处理事件
    /// </summary>
    public class WFActivityInstanceDoingEvent : DomainEvent
    {
        public IList<WFActivityInstance> forwardWFActivityInstances;

        public WFActivityInstanceDoingEvent(IEntity source, IList<WFActivityInstance> forwardWFActivityInstances)
            : base(source)
        {
            this.forwardWFActivityInstances = forwardWFActivityInstances;
        }
    }
}
