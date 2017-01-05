using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 职务用户
    /// </summary>
    public class DutyUser : AggregateRoot
    {
        protected DutyUser()
        { 
        }

        /// <summary>
        /// 构造职务用户
        /// </summary>
        /// <param name="duty">职务</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal DutyUser(Duty duty, Guid userId, Guid createUserId)
        {
            duty.IsInvalid("职务");
            userId.IsEmpty("用户Id");

            this.Id = Guid.NewGuid();
            this.Duty = duty;
            this.UserId = userId;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 职务
        /// </summary>
        public Duty Duty
        {
            get;
            set;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId
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
    }
}
