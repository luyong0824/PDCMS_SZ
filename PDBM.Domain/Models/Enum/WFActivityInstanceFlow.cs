using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 工作流活动实例流转类型枚举值
    /// </summary>
    public enum WFActivityInstanceFlow
    {
        未处理 = 0,
        正常流转 = 1,
        转发他人 = 2,
        退回并转至自己 = 3,
        终止流转 = 4
    }
}
