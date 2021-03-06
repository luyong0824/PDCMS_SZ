﻿using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 外电引入历史记录实体
    /// </summary>
    public class ExternalElectricPowerLog : AggregateRoot
    {
        protected ExternalElectricPowerLog()
        {
        }

        /// <summary>
        /// 构造外电引入历史记录实体
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="externalElectric">引入方式</param>
        /// <param name="budgetPrice">预算价格</param>
        /// <param name="memos">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal ExternalElectricPowerLog(OperationType operationType, Guid parentId, PropertyType propertyType, ExternalElectric externalElectric, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            operationType.IsInvalid("操作类型");
            propertyType.IsInvalid("资源类型");
            //ExternalElectric.IsInvalid("引入方式");

            this.Id = Guid.NewGuid();
            this.OperationType = operationType;
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.ExternalElectric = externalElectric;
            this.BudgetPrice = budgetPrice;
            this.CustomerId = Guid.Empty;
            this.CustomerId = Guid.Empty;
            this.TimeLimit = timeLimit;
            this.Memos = memos;
            this.State = State.使用;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType
        {
            get;
            set;
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
    }
}
