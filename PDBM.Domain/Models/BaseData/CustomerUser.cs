using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    public class CustomerUser : AggregateRoot
    {
        protected CustomerUser()
        { 
        }

        /// <summary>
        /// 构造往来单位用户实体
        /// </summary>
        /// <param name="customerId">往来单位Id</param>
        /// <param name="userId">用户Id</param>
        /// <param name="createUserId"></param>
        internal CustomerUser(Guid customerId, Guid userId,Guid createUserId)
        {
            customerId.IsEmpty("往来单位Id");
            userId.IsEmpty("用户Id");

            this.Id = Guid.NewGuid();
            this.CustomerId = customerId;
            this.UserId = userId;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 往来单位Id
        /// </summary>
        public Guid CustomerId
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
