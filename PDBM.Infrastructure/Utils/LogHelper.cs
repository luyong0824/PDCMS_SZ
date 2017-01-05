using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 日志工具
    /// </summary>
    public static class LogHelper
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("PDBM.Logger");

        /// <summary>
        /// 初始化配置
        /// </summary>
        public static void InitConfigure()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 将指定的字符串信息写入日志
        /// </summary>
        /// <param name="message">字符串</param>
        public static void Log(string message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 将指定的异常实例信息写入日志
        /// </summary>
        /// <param name="ex">异常实例</param>
        public static void Log(Exception ex)
        {
            log.Error("Exception", ex);
        }
    }
}
