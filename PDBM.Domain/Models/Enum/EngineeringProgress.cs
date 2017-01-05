using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 建设进度
    /// </summary>
    public enum EngineeringProgress
    {
        未开工 = 1,
        进行中 = 2,
        已完工 = 3,
        暂缓 = 4,
        取消 = 5
    }
}
