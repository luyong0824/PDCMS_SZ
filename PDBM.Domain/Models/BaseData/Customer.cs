using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 往来单位实体
    /// </summary>
    public class Customer : AggregateRoot
    {
        protected Customer()
        {
        }

        /// <summary>
        /// 构造往来单位实体
        /// </summary>
        /// <param name="customerType">往来单位分类</param>
        /// <param name="customerCode">往来单位编码</param>
        /// <param name="customerName">往来单位简称</param>
        /// <param name="customerFullName">往来单位全称</param>
        /// <param name="contactMan">联系人</param>
        /// <param name="contactTel">联系方式</param>
        /// <param name="contactAddr">联系地址</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">使用状态</param>
        /// <param name="createUserId"></param>
        internal Customer(CustomerType customerType, string customerCode, string customerName, string customerFullName, Guid customerUserId, string contactMan, string contactTel, string contactAddr, string remarks, State state, Guid createUserId)
        {
            customerType.IsInvalid("往来单位分类");
            customerCode.IsNullOrEmptyOrTooLong("往来单位编码", true, 50);
            customerName.IsNullOrEmptyOrTooLong("往来单位简称", true, 100);
            customerFullName.IsNullOrEmptyOrTooLong("往来单位全称", true, 100);
            contactMan.IsNullOrTooLong("联系人", true, 50);
            contactTel.IsNullOrTooLong("联系方式", true, 50);
            contactAddr.IsNullOrTooLong("联系地址", true, 150);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("使用状态");

            this.Id = Guid.NewGuid();
            this.CustomerType = customerType;
            this.CustomerCode = customerCode;
            this.CustomerName = customerName;
            this.CustomerFullName = customerFullName;
            this.CustomerUserId = customerUserId;
            this.ContactMan = contactMan;
            this.ContactTel = contactTel;
            this.ContactAddr = contactAddr;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 往来单位分类
        /// </summary>
        public CustomerType CustomerType
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位编码
        /// </summary>
        public string CustomerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位简称
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位全称
        /// </summary>
        public string CustomerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆人
        /// </summary>
        public Guid CustomerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string ContactAddr
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
        /// 往来单位状态
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

        public void Modify(CustomerType customerType, string customerName, string customerFullName, Guid customerUserId, string contactMan, string contactTel, string contactAddr, string remarks, State state, Guid modifyUserId)
        {
            customerType.IsInvalid("往来单位分类");
            customerName.IsNullOrEmptyOrTooLong("往来单位简称", true, 100);
            customerFullName.IsNullOrEmptyOrTooLong("往来单位全称", true, 100);
            contactMan.IsNullOrTooLong("联系人", true, 50);
            contactTel.IsNullOrTooLong("联系方式", true, 50);
            contactAddr.IsNullOrTooLong("联系地址", true, 150);
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("使用状态");

            this.CustomerType = customerType;
            this.CustomerName = customerName;
            this.CustomerFullName = customerFullName;
            this.CustomerUserId = customerUserId;
            this.ContactMan = contactMan;
            this.ContactTel = contactTel;
            this.ContactAddr = contactAddr;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
