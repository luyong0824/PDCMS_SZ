using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 铁塔实体
    /// </summary>
    public class Tower : AggregateRoot
    {
        protected Tower()
        {
        }

        /// <summary>
        /// 构造铁塔实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="towerType">铁塔类型</param>
        /// <param name="towerHeight">铁塔高度</param>
        /// <param name="platFormNumber">平台数量</param>
        /// <param name="poleNumber">抱杆数量</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal Tower(Guid parentId, PropertyType propertyType, TowerType towerType, decimal towerHeight, int platFormNumber, int poleNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            propertyType.IsInvalid("资源类型");
            towerType.IsInvalid("铁塔类型");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.TowerType = towerType;
            this.TowerHeight = towerHeight;
            this.PlatFormNumber = platFormNumber;
            this.PoleNumber = poleNumber;
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
        /// 铁塔类型
        /// </summary>
        public TowerType TowerType
        {
            get;
            set;
        }

        /// <summary>
        /// 铁塔高度
        /// </summary>
        public decimal TowerHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 平台数量
        /// </summary>
        public int PlatFormNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量
        /// </summary>
        public int PoleNumber
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
        /// 修改铁塔实体
        /// </summary>
        /// <param name="towerType">铁塔类型</param>
        /// <param name="towerHeight">铁塔高度</param>
        /// <param name="platFormNumber">平台数量</param>
        /// <param name="holdingPoleNumber">抱杆数量</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(TowerType towerType, decimal towerHeight, int platFormNumber, int poleNumber, decimal budgetPrice, int timeLimit, string memos, Guid modifyUserId)
        {
            towerType.IsInvalid("铁塔类型");

            this.TowerType = towerType;
            this.TowerHeight = towerHeight;
            this.PlatFormNumber = platFormNumber;
            this.PoleNumber = poleNumber;
            this.BudgetPrice = budgetPrice;
            this.TimeLimit = timeLimit;
            this.Memos = memos;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 指定铁塔施工单位
        /// </summary>
        /// <param name="customerId">施工单位Id</param>
        public void ModifyCustomer(Guid? customerId)
        {
            this.CustomerId = customerId;
        }
    }
}
