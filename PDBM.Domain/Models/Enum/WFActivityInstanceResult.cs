using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 工作流活动实例操作结果枚举值
    /// </summary>
    public enum WFActivityInstanceResult
    {
        未处理 = 0,
        同意 = 1,
        不同意 = 2,
        已阅 = 3
    }
}
