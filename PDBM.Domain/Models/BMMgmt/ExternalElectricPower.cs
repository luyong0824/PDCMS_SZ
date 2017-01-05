﻿using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 外电引入实体
    /// </summary>
    public class ExternalElectricPower : AggregateRoot
    {
        protected ExternalElectricPower()
        {
        }

        /// <summary>
        /// 构造外电引入实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="externalElectric">外电引入方式</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal ExternalElectricPower(Guid parentId, PropertyType propertyType, ExternalElectric externalElectric, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            propertyType.IsInvalid("资源类型");
            //ExternalElectric.IsInvalid("引入方式");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.ExternalElectric = externalElectric;
            this.BudgetPrice = budgetPrice;
            this.CustomerId = Guid.Empty;
            this.CustomerUserId = Guid.Empty;
            this.TimeLimit = timeLimit;
            this.Memos = memos;
            this.State = State.使用;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        public PropertyType PropertyType
        {
            get;
            set;
        }

        /// <summary>
        /// 外电引入方式
        /// </summary>
        public ExternalElectric ExternalElectric
        {
            get;
            set;
        }

        /// <summary>
        /// 预算价
        /// </summary>
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位
        /// </summary>
        public Guid? CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 施工人员用户Id
        /// </summary>
        public Guid? CustomerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 完成时限(天)
        /// </summary>
        public int TimeLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memos
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
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
        /// 创建时间
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

        /// <summary>
        /// 修改外电引入实体
        /// </summary>
        /// <param name="externalElectric">外电引入方式</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(ExternalElectric externalElectric, decimal budgetPrice, int timeLimit, string memos, Guid modifyUserId)
        {
            externalElectric.IsInvalid("引入方式");

            this.ExternalElectric = externalElectric;
            this.BudgetPrice = budgetPrice;
            this.TimeLimit = timeLimit;
            this.Memos = memos;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 指定外电引入施工单位
        /// </summary>
        /// <param name="customerId"></param>
        public void ModifyCustomer(Guid? customerId)
        {
            this.CustomerId = customerId;
        }
    }
}
