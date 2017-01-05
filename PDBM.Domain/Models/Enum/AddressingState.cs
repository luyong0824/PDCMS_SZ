using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 寻址状态枚举值
    /// </summary>
    public enum AddressingState
    {
        未寻址确认 = 1,
        已寻址确认 = 2,
        流转中 = 3,
        流程终止 = 4
    }
}
