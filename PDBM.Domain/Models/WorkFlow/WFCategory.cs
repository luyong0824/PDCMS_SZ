using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流类型实体
    /// </summary>
    public class WFCategory : AggregateRoot
    {
        protected WFCategory()
        {
        }

        /// <summary>
        /// 工作流类型编码
        /// </summary>
        public string WFCategoryCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流类型名称
        /// </summary>
        public string WFCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 实体名称
        /// </summary>
        public string EntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 编码前缀
        /// </summary>
        public string CodePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 打印页地址
        /// </summary>
        public string PrintUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 该类型包含的工作流活动操作类型列表
        /// </summary>
        public string WFActivityOperateList
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流类型状态
        /// </summary>
        public State State
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        public Profession Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 工作流过程实体列表
        /// </summary>
        protected virtual ICollection<WFProcess> WFProcesses
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器实体列表
        /// </summary>
        protected virtual ICollection<WFActivityEditor> WFActivityEditors
        {
            get;
            set;
        }
        #endregion
    }
}
