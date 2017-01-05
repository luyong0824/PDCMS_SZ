using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 会计主体实体
    /// </summary>
    public class AccountingEntity : AggregateRoot
    {
        protected AccountingEntity()
        {
        }

        /// <summary>
        /// 构造会计主体实体
        /// </summary>
        /// <param name="accountingEntityCode">会计主体编码</param>
        /// <param name="accountingEntityName">会计主体名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">会计主体状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal AccountingEntity(string accountingEntityCode, string accountingEntityName, string remarks, State state, Guid createUserId)
        {
            accountingEntityCode.IsNullOrEmptyOrTooLong("会计主体编码", true, 50);
            accountingEntityName.IsNullOrEmptyOrTooLong("会计主体名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("会计主体状态");

            this.Id = Guid.NewGuid();
            this.AccountingEntityCode = accountingEntityCode;
            this.AccountingEntityName = accountingEntityName;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 会计主体编码
        /// </summary>
        public string AccountingEntityCode
        {
            get;
            set;
        }

        /// <summary>
        /// 会计主体名称
        /// </summary>
        public string AccountingEntityName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 会计主体状态
        /// </summary>
        public State State
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
        /// 修改人用户Id
        /// </summary>
        public Guid ModifyUserId
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

        /// <summary>
        /// 修改日期
        /// </summary>
        public DateTime ModifyDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 项目实体列表
        /// </summary>
        protected virtual ICollection<Project> Projects
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 修改会计主体实体
        /// </summary>
        /// <param name="accountingEntityCode">会计主体编码</param>
        /// <param name="accountingEntityName">会计主体名称</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">会计主体状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string accountingEntityCode, string accountingEntityName, string remarks, State state, Guid modifyUserId)
        {
            accountingEntityCode.IsNullOrEmptyOrTooLong("会计主体编码", true, 50);
            accountingEntityName.IsNullOrEmptyOrTooLong("会计主体名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("会计主体状态");

            this.AccountingEntityCode = accountingEntityCode;
            this.AccountingEntityName = accountingEntityName;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
