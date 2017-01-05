using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 工作流过程实例状态枚举值
    /// </summary>
    public enum WFProcessInstanceState
    {
        未发送 = 1,
        流转中 = 2,
        流程通过 = 3,
        流程终止 = 4
    }
}
