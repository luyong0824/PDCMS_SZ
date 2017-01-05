using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 物资清单实体
    /// </summary>
    public class MaterialList : AggregateRoot
    {
        protected MaterialList()
        { 
        }

        /// <summary>
        /// 构造物资清单实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="materialId">物资名称Id</param>
        /// <param name="materialSpecId">设计规格Id</param>
        /// <param name="budgetPrice">预算价格</param>
        /// <param name="specNumber">数量</param>
        /// <param name="memos">备注</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal MaterialList(Guid parentId, PropertyType propertyType, Guid materialId, Guid materialSpecId, decimal budgetPrice, decimal specNumber, string memos, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            propertyType.IsInvalid("资源类型");
            materialId.IsEmpty("物资名称Id");
            materialSpecId.IsEmpty("设计规格Id");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.MaterialId = materialId;
            this.MaterialSpecId = materialSpecId;
            this.BudgetPrice = budgetPrice;
            this.SpecNumber = specNumber;
            this.SupplierId = Guid.Empty;
            this.Memos = memos;
            this.DoState = DoState.未处理;
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
        /// 物资名称Id
        /// </summary>
        public Guid MaterialId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计规格Id
        /// </summary>
        public Guid? MaterialSpecId
        {
            get;
            set;
        }

        /// <summary>
        /// 预算金额
        /// </summary>
        public decimal BudgetPrice
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
        /// 供应商
        /// </summary>
        public Guid? SupplierId
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
        /// 处理状态
        /// </summary>
        public DoState DoState
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
        /// 修改物资清单实体
        /// </summary>
        /// <param name="materialId">物资名称Id</param>
        /// <param name="materialSpecId">设计规格Id</param>
        /// <param name="budgetPrice">预算价格</param>
        /// <param name="specNumber">数量</param>
        /// <param name="memos">备注</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(Guid materialId, Guid materialSpecId, decimal budgetPrice, decimal specNumber, string memos, Guid modifyUserId)
        {
            materialId.IsEmpty("物资名称Id");
            materialSpecId.IsEmpty("物资名称Id");

            this.MaterialId = materialId;
            this.MaterialSpecId = materialSpecId;
            this.BudgetPrice = budgetPrice;
            this.SpecNumber = specNumber;
            this.Memos = memos;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 指定规格及供应商
        /// </summary>
        /// <param name="materialSpecId"></param>
        /// <param name="supplierId"></param>
        /// <param name="modifyUserId"></param>
        public void ModifySpec(Guid? materialSpecId, Guid? supplierId, Guid modifyUserId)
        {
            this.MaterialSpecId = materialSpecId;
            this.SupplierId = supplierId;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 修改物资清单状态
        /// </summary>
        /// <param name="state"></param>
        public void ModifyState(DoState doState)
        {
            this.DoState = doState;
        }
    }
}
