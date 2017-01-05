using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;
using PDBM.Infrastructure.Common;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 运营商确认明细实体
    /// </summary>
    public class OperatorsConfirmDetail : AggregateRoot
    {
        protected OperatorsConfirmDetail()
        {
        }

        /// <summary>
        /// 构造运营商确认明细实体
        /// </summary>
        /// <param name="operatorsConfirmId">运营商确认Id</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="mobileDemand">移动需求确认</param>
        /// <param name="telecomDemand">电信需求确认</param>
        /// <param name="unicomDemand">联通需求确认</param>
        /// <param name="mobileConfirmUserId">移动确认人用户Id</param>
        /// <param name="telecomConfirmUserId">电信确认人用户Id</param>
        /// <param name="unicomConfirmUserId">联通确认人用户Id</param>
        internal OperatorsConfirmDetail(Guid operatorsConfirmId, Guid planningId, Demand mobileDemand, Demand telecomDemand, Demand unicomDemand, Guid mobileConfirmUserId,
            Guid telecomConfirmUserId, Guid unicomConfirmUserId, Guid createUserId)
        {
            operatorsConfirmId.IsEmpty("运营商确认Id");
            planningId.IsEmpty("规划Id");

            this.Id = Guid.NewGuid();
            this.OperatorsConfirmId = operatorsConfirmId;
            this.PlanningId = planningId;
            this.TelecomDemand = telecomDemand;
            this.MobileDemand = mobileDemand;
            this.UnicomDemand = unicomDemand;
            this.TelecomConfirmUserId = telecomConfirmUserId;
            this.MobileConfirmUserId = mobileConfirmUserId;
            this.UnicomConfirmUserId = unicomConfirmUserId;
            this.TelecomConfirmDate = DateTime.Now;
            this.MobileConfirmDate = DateTime.Now;
            this.UnicomConfirmDate = DateTime.Now;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
        }

        /// <summary>
        /// 运营商确认Id
        /// </summary>
        public Guid OperatorsConfirmId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        public Guid PlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信需求确认
        /// </summary>
        public Demand TelecomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 移动需求确认
        /// </summary>
        public Demand MobileDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求确认
        /// </summary>
        public Demand UnicomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认人用户Id
        /// </summary>
        public Guid TelecomConfirmUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认人用户Id
        /// </summary>
        public Guid MobileConfirmUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认人用户Id
        /// </summary>
        public Guid UnicomConfirmUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认日期
        /// </summary>
        public DateTime TelecomConfirmDate
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认日期
        /// </summary>
        public DateTime MobileConfirmDate
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认日期
        /// </summary>
        public DateTime UnicomConfirmDate
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

        #region Relations
        /// <summary>
        /// 运营商确认实体
        /// </summary>
        protected virtual OperatorsConfirm OperatorsConfirm
        {
            get;
            set;
        }

        /// <summary>
        /// 规划实体
        /// </summary>
        protected virtual Planning Planning
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="currentCompanyId">当前操作人所在公司Id</param>
        /// <param name="currentCompanyNature">当前操作人所在公司性质</param>
        /// <param name="demand">是否需要</param>
        public void Confirm(Guid currentUserId, Guid currentCompanyId, CompanyNature currentCompanyNature, Demand demand)
        {
            currentUserId.IsEmpty("当前操作人用户Id");
            if (currentCompanyNature != CompanyNature.运营商)
            {
                throw new DomainFault("只能由运营商用户进行操作");
            }
            if (demand != Demand.需要 && demand != Demand.不需要)
            {
                throw new DomainFault("确认无效");
            }

            if (currentCompanyId == Guid.Parse("2E0FFE5F-C03A-4767-9915-9683F0DB0B53"))
            {
                this.TelecomDemand = demand;
                this.TelecomConfirmUserId = currentUserId;
                this.TelecomConfirmDate = DateTime.Now;
            }
            else if (currentCompanyId == Guid.Parse("6365F3DE-0FC5-4930-A321-2350EE6269BB"))
            {
                this.MobileDemand = demand;
                this.MobileConfirmUserId = currentUserId;
                this.MobileConfirmDate = DateTime.Now;
            }
            else if (currentCompanyId == Guid.Parse("0B2A7F2D-B623-4EF2-A89D-FF4EAB0A1600"))
            {
                this.UnicomDemand = demand;
                this.UnicomConfirmUserId = currentUserId;
                this.UnicomConfirmDate = DateTime.Now;
            }
            else
            {
                throw new DomainFault("当前操作人所在公司Id无效");
            }
        }
    }
}
