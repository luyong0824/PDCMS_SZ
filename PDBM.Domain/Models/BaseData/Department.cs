using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 部门实体
    /// </summary>
    public class Department : AggregateRoot
    {
        protected Department()
        {
        }

        /// <summary>
        /// 构造部门实体
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentCode">部门编码</param>
        /// <param name="departmentName">部门名称</param>
        /// <param name="managerUserId">部门经理用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">部门状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Department(Guid companyId, string departmentCode, string departmentName, Guid managerUserId, string remarks, State state, Guid createUserId)
        {
            companyId.IsEmpty("公司Id");
            departmentCode.IsNullOrEmptyOrTooLong("部门编码", true, 50);
            departmentName.IsNullOrEmptyOrTooLong("部门名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("部门状态");

            this.Id = Guid.NewGuid();
            this.CompanyId = companyId;
            this.DepartmentCode = departmentCode;
            this.DepartmentName = departmentName;
            if (managerUserId == Guid.Empty)
            {
                this.ManagerUserId = null;
            }
            else
            {
                this.ManagerUserId = managerUserId;
            }
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门编码
        /// </summary>
        public string DepartmentCode
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 部门经理用户Id
        /// </summary>
        public Guid? ManagerUserId
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
        /// 部门状态
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
        /// 公司实体
        /// </summary>
        protected virtual Company Company
        {
            get;
            set;
        }

        /// <summary>
        /// 部门经理用户实体
        /// </summary>
        public virtual User ManagerUser
        {
            get;
            set;
        }

        /// <summary>
        /// 用户实体列表
        /// </summary>
        protected virtual ICollection<User> Users
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
        #endregion

        /// <summary>
        /// 修改部门实体
        /// </summary>
        /// <param name="departmentCode">部门编码</param>
        /// <param name="departmentName">部门名称</param>
        /// <param name="managerUserId">部门经理用户Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">部门状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string departmentCode, string departmentName, Guid managerUserId, string remarks, State state, Guid modifyUserId)
        {
            departmentCode.IsNullOrEmptyOrTooLong("部门编码", true, 50);
            departmentName.IsNullOrEmptyOrTooLong("部门名称", true, 100);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("部门状态");

            this.DepartmentCode = departmentCode;
            this.DepartmentName = departmentName;
            if (managerUserId == Guid.Empty)
            {
                this.ManagerUserId = null;
            }
            else
            {
                this.ManagerUserId = managerUserId;
            }
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 用户登录检查
        /// </summary>
        public void CheckByLogin()
        {
            if (this.State == State.停用)
            {
                throw new DomainFault("用户所在部门已被停用");
            }
        }
    }
}
