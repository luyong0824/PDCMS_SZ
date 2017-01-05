using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 项目进度枚举值
    /// </summary>
    public enum ProjectProgress
    {
        未开工 = 1,
        进行中 = 2,
        完工 = 3,
        开通 = 4,
        暂缓 = 5,
        撤销 = 6
    }
}
