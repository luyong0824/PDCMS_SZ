using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 运营商确认实体
    /// </summary>
    public class OperatorsConfirm : AggregateRoot
    {
        protected OperatorsConfirm()
        {
        }

        /// <summary>
        /// 构造运营商确认实体
        /// </summary>
        /// <param name="createUserId">创建人用户Id</param>
        internal OperatorsConfirm(Guid createUserId)
        {
            this.Id = Guid.NewGuid();
            this.OrderCode = "";
            this.OrderState = WFProcessInstanceState.流程通过;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 运营商确认单编码
        /// </summary>
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商确认单状态
        /// </summary>
        public WFProcessInstanceState OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
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
        /// 运营商确认明细实体列表
        /// </summary>
        protected virtual ICollection<OperatorsConfirmDetail> OperatorsConfirmDetails
        {
            get;
            set;
        }
        #endregion
    }
}
