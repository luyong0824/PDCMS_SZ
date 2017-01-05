using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 规格型号信息表实体
    /// </summary>
    public class MaterialSpecList : AggregateRoot
    {
        protected MaterialSpecList()
        { 
        }

        /// <summary>
        /// 构造规格型号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">规格型号分类</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="specNumber">数量</param>
        /// <param name="totalPrice">金额</param>
        /// <param name="orderCode">订单编号</param>
        /// <param name="state">使用状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal MaterialSpecList(string projectCode, string customerName, MaterialSpecType materialSpecType, string materialSpecName, decimal unitPrice, decimal specNumber, decimal totalPrice, string orderCode, State state, Guid createUserId)
        {
            projectCode.IsNullOrTooLong("立项编号", true, 50);
            customerName.IsNullOrTooLong("供应商", true, 50);
            materialSpecType.IsInvalid("规格型号分类");
            materialSpecName.IsNullOrTooLong("规格型号", true, 50);
            orderCode.IsNullOrTooLong("订单编号", true, 50);
            state.IsInvalid("使用状态");

            this.Id = Guid.NewGuid();
            this.ProjectCode = projectCode;
            this.CustomerName = customerName;
            this.MaterialSpecType = materialSpecType;
            this.MaterialSpecName = materialSpecName;
            this.UnitPrice = unitPrice;
            this.SpecNumber = specNumber;
            this.TotalPrice = totalPrice;
            this.OrderCode = orderCode;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 立项编号
        /// </summary>
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 规格型号分类
        /// </summary>
        public MaterialSpecType MaterialSpecType
        {
            get;
            set;
        }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string MaterialSpecName
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal SpecNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal TotalPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 使用状态
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

        /// <summary>
        /// 修改规格型号信息表实体
        /// </summary>
        /// <param name="projectCode">立项编号</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">规格型号分类</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="unitPrice">单价</param>
        /// <param name="specNumber">数量</param>
        /// <param name="totalPrice">金额</param>
        /// <param name="orderCode">订单编号</param>
        /// <param name="state">使用状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string projectCode, string customerName, MaterialSpecType materialSpecType, string materialSpecName, decimal unitPrice, decimal specNumber, decimal totalPrice, string orderCode, State state, Guid modifyUserId)
        {
            projectCode.IsNullOrTooLong("立项编号", true, 50);
            customerName.IsNullOrTooLong("供应商", true, 50);
            materialSpecType.IsInvalid("规格型号分类");
            materialSpecName.IsNullOrTooLong("规格型号", true, 50);
            orderCode.IsNullOrTooLong("订单编号", true, 50);
            state.IsInvalid("使用状态");

            this.ProjectCode = projectCode;
            this.CustomerName = customerName;
            this.MaterialSpecType = materialSpecType;
            this.MaterialSpecName = materialSpecName;
            this.UnitPrice = unitPrice;
            this.SpecNumber = specNumber;
            this.TotalPrice = totalPrice;
            this.OrderCode = orderCode;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
