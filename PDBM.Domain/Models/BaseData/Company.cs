using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 公司实体
    /// </summary>
    public class Company : AggregateRoot
    {
        protected Company()
        {
        }

        /// <summary>
        /// 公司编码
        /// </summary>
        public string CompanyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司全称
        /// </summary>
        public string CompanyFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 生成的单据编码前缀
        /// </summary>
        public string ApplyCodePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 公司性质
        /// </summary>
        public CompanyNature CompanyNature
        {
            get;
            set;
        }

        /// <summary>
        /// 公司状态
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
        /// 部门实体列表
        /// </summary>
        protected virtual ICollection<Department> Departments
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
        /// 用户登录检查
        /// </summary>
        public void CheckByLogin()
        {
            if (this.State == State.停用)
            {
                throw new DomainFault("用户所在公司已被停用");
            }
        }
    }
}
