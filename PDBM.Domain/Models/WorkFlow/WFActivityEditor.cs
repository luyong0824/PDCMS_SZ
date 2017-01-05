using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.WorkFlow
{
    /// <summary>
    /// 工作流活动编辑器实体
    /// </summary>
    public class WFActivityEditor : AggregateRoot
    {
        protected WFActivityEditor()
        {
        }

        /// <summary>
        /// 工作流类型Id
        /// </summary>
        public Guid WFCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器编码
        /// </summary>
        public string WFActivityEditorCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器名称
        /// </summary>
        public string WFActivityEditorName
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑器地址
        /// </summary>
        public string EditorUrl
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必须编辑
        /// </summary>
        public Bool IsMustEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 编辑器状态
        /// </summary>
        public State State
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
        /// 工作流类型实体
        /// </summary>
        protected virtual WFCategory WFCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实体列表
        /// </summary>
        protected virtual ICollection<WFActivity> WFActivitys
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例实体列表
        /// </summary>
        protected virtual ICollection<WFActivityInstance> WFActivityInstances
        {
            get;
            set;
        }
        #endregion
    }
}
