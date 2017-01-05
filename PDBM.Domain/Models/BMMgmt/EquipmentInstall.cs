using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 设备安装实体
    /// </summary>
    public class EquipmentInstall : AggregateRoot
    {
        protected EquipmentInstall()
        {
        }

        /// <summary>
        /// 构造设备安装实体
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <param name="switchPower">开关电源</param>
        /// <param name="battery">蓄电池</param>
        /// <param name="cabinetNumber">机柜数量</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal EquipmentInstall(Guid parentId, PropertyType propertyType, decimal switchPower, decimal battery, int cabinetNumber, decimal budgetPrice, int timeLimit, string memos, Guid createUserId)
        {
            parentId.IsEmpty("父表Id");
            propertyType.IsInvalid("资源类型");

            this.Id = Guid.NewGuid();
            this.ParentId = parentId;
            this.PropertyType = propertyType;
            this.SwitchPower = switchPower;
            this.Battery = battery;
            this.CabinetNumber = cabinetNumber;
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
        /// 开关电源
        /// </summary>
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        public int CabinetNumber
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
        /// 修改设备安装实体
        /// </summary>
        /// <param name="switchPower">开关电源</param>
        /// <param name="battery">蓄电池</param>
        /// <param name="cabinetNumber">机柜数量</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(decimal switchPower, decimal battery, int cabinetNumber, decimal budgetPrice, int timeLimit, string memos, Guid modifyUserId)
        {
            this.SwitchPower = switchPower;
            this.Battery = battery;
            this.CabinetNumber = cabinetNumber;
            this.BudgetPrice = budgetPrice;
            this.TimeLimit = timeLimit;
            this.Memos = memos;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }

        /// <summary>
        /// 指定设备安装施工单位
        /// </summary>
        /// <param name="customerId"></param>
        public void ModifyCustomer(Guid? customerId)
        {
            this.CustomerId = customerId;
        }
    }
}
