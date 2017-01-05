using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 设计规格实体
    /// </summary>
    public class MaterialSpec : AggregateRoot
    {
        protected MaterialSpec()
        { 
        }

        /// <summary>
        /// 构造设计规格实体
        /// </summary>
        /// <param name="materialSpecCode">规格编码</param>
        /// <param name="materialSpecName">规格名称</param>
        /// <param name="materialId">物资名称Id</param>
        /// <param name="unitId">计量单位Id</param>
        /// <param name="price">参考单价</param>
        /// <param name="customrtId">供应商Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal MaterialSpec(string materialSpecCode, string materialSpecName, Guid materialId, Guid unitId, decimal price, Guid? customrtId, string remarks, State state, Guid createUserId)
        {
            materialSpecCode.IsNullOrEmptyOrTooLong("规格编码", true, 50);
            materialSpecName.IsNullOrEmptyOrTooLong("规格名称", true, 100);
            materialId.IsEmpty("物资名称Id");
            unitId.IsEmpty("计量单位Id");
            price.IsNonnegative("参考单价");
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("规格状态");

            this.Id = Guid.NewGuid();
            this.MaterialSpecCode = materialSpecCode;
            this.MaterialSpecName = materialSpecName;
            this.MaterialId = materialId;
            this.UnitId = unitId;
            this.Price = price;
            this.CustomerId = customrtId;
            this.Remarks = remarks;
            this.State = state;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Now;
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 规格编码
        /// </summary>
        public string MaterialSpecCode
        {
            get;
            set;
        }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string MaterialSpecName
        {
            get;
            set;
        }

        /// <summary>
        /// 物资名称Id
        /// </summary>
        public Guid MaterialId
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位Id
        /// </summary>
        public Guid UnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 参考单价
        /// </summary>
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public Guid? CustomerId
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
        /// 规格状态
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
        /// 修改设计规格实体
        /// </summary>
        /// <param name="materialSpecCode">规格编码</param>
        /// <param name="materialSpecName">规格名称</param>
        /// <param name="unitId">计量单位Id</param>
        /// <param name="price">参考单价</param>
        /// <param name="customerId">共供应商Id</param>
        /// <param name="remarks">备注</param>
        /// <param name="state">状态</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string materialSpecCode, string materialSpecName, Guid unitId, decimal price, Guid? customerId, string remarks, State state, Guid modifyUserId)
        {
            materialSpecCode.IsNullOrEmptyOrTooLong("规格编码", true, 50);
            materialSpecName.IsNullOrEmptyOrTooLong("规格名称", true, 100);
            unitId.IsEmpty("计量单位Id");
            price.IsNonnegative("参考单价");
            remarks.IsNullOrTooLong("备注", true, 150);
            state.IsInvalid("状态");

            this.MaterialSpecCode = materialSpecCode;
            this.MaterialSpecName = materialSpecName;
            this.UnitId = unitId;
            this.Price = price;
            this.CustomerId = customerId;
            this.Remarks = remarks;
            this.State = state;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
