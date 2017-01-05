using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 机房类型枚举值
    /// </summary>
    public enum MachineRoomType
    {
        自建砖混机房 = 1,
        租用砖混机房 = 2,
        自建彩钢板机房 = 3,
        一体化机柜 = 4,
        其他 = 5
    }
}
