using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 业务量实体
    /// </summary>
    public class BusinessVolume : AggregateRoot
    {
        protected BusinessVolume()
        {
        }

        /// <summary>
        /// 构造业务量实体
        /// </summary>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="trafficVolumes">话务量</param>
        /// <param name="businessVolumes">业务量</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="createDate">创建日期</param>
        /// <param name="createUserId">创建人用户Id</param>
        internal BusinessVolume(LogicalType logicalType, string logicalNumber, decimal trafficVolumes, decimal businessVolumes, Profession profession, Guid companyId, DateTime createDate, Guid createUserId)
        {
            logicalType.IsInvalid("逻辑号类型");
            logicalNumber.IsNullOrTooLong("逻辑号", true, 150);
            this.Id = Guid.NewGuid();
            this.LogicalType = logicalType;
            this.LogicalNumber = logicalNumber;
            this.TrafficVolumes = trafficVolumes;
            this.BusinessVolumes = businessVolumes;
            this.Profession = profession;
            this.CompanyId = companyId;
            this.CreateUserId = createUserId;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Parse(createDate.ToShortDateString());
            this.ModifyDate = this.CreateDate;
        }

        /// <summary>
        /// 逻辑号类型
        /// </summary>
        public LogicalType LogicalType
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑号
        /// </summary>
        public string LogicalNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 话务量
        /// </summary>
        public decimal TrafficVolumes
        {
            get;
            set;
        }

        /// <summary>
        /// 业务量
        /// </summary>
        public decimal BusinessVolumes
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        public Profession Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 分公司Id
        /// </summary>
        public Guid CompanyId
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
        /// 修改业务量实体
        /// </summary>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="trafficVolumes">话务量</param>
        /// <param name="businessVolumes">业务量</param>
        /// <param name="modifyUserId">修改人用户Id</param>
        public void Modify(string logicalNumber, decimal trafficVolumes, decimal businessVolumes, Guid modifyUserId)
        {
            logicalNumber.IsNullOrTooLong("逻辑号", true, 150);
            this.LogicalNumber = logicalNumber;
            this.TrafficVolumes = trafficVolumes;
            this.BusinessVolumes = businessVolumes;
            this.ModifyUserId = modifyUserId;
            this.ModifyDate = DateTime.Now;
        }
    }
}
