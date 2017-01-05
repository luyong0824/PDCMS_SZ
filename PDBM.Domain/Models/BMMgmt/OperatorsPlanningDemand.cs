using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    public class OperatorsPlanningDemand : AggregateRoot
    {
        /// <summary>
        /// 改造站点需求确认实体
        /// </summary>
        protected OperatorsPlanningDemand()
        { 
        }

        /// <summary>
        /// 构造改造站点需求确认实体
        /// </summary>
        /// <param name="operatorsPlanningId">运营商规划Id</param>
        /// <param name="placeId">站点Id</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal OperatorsPlanningDemand(Guid operatorsPlanningId, Guid placeId, Guid createUserId)
        {
            operatorsPlanningId.IsEmpty("运营商规划Id");
            placeId.IsEmpty("站点Id");

            this.Id = Guid.NewGuid();
            this.OperatorsPlanningId = operatorsPlanningId;
            this.PlaceId = placeId;
            this.Demand = Demand.未确认;
            this.CreateUserId = createUserId;
            this.CreateDate = DateTime.Now;
            this.ConfirmDate = this.CreateDate;
        }

        /// <summary>
        /// 运营商规划Id
        /// </summary>
        public Guid OperatorsPlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 需求确认
        /// </summary>
        public Demand Demand
        {
            get;
            set;
        }

        /// <summary>
        /// 确认用户Id
        /// </summary>
        public Guid ConfirmUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime ConfirmDate
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

        /// <summary>
        /// 修改改造站点需求确认实体
        /// </summary>
        /// <param name="demand">需求确认</param>
        /// <param name="confirmUserId">确认用户Id</param>
        public void Modify(Demand demand, Guid confirmUserId)
        {

            this.Demand = demand;
            this.ConfirmUserId = confirmUserId;
            this.ConfirmDate = DateTime.Now;
        }
    }
}
