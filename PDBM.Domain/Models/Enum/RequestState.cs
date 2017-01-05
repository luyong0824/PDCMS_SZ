using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.Enum
{
    /// <summary>
    /// 完工请求状态
    /// </summary>
    public enum RequestState
    {
        未请求 = 1,
        请求中 = 2,
        请求完成 = 3,
        请求退回 = 4
    }
}
