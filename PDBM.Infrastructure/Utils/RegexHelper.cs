using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 正则表达式工具
    /// </summary>
    public static class RegexHelper
    {
        /// <summary>
        /// 是否为日期格式,例如2014-11-23
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDate(string source)
        {
            return Regex.IsMatch(source, @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$");
        }

        /// <summary>
        /// 是否为时间格式,例如17:10:30
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsTime(string source)
        {
            return Regex.IsMatch(source, @"^([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
        }

        /// <summary>
        /// 是否为日期+时间格式,例如2014-11-23 17:10:30
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string source)
        {
            return Regex.IsMatch(source, @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)\s+([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
        }

        /// <summary>
        /// 是否为ISO8601日期+时间格式,例如2014-11-23T17:10:30
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsISO8601DateTime(string source)
        {
            return Regex.IsMatch(source, @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)T+([01][0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
        }
    }
}
