using PDBM.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BMMgmt
{
    /// <summary>
    /// 站点业务量实体
    /// </summary>
    public class PlaceBusinessVolume : AggregateRoot
    {
        protected PlaceBusinessVolume()
        { 
        }

        /// <summary>
        /// 构造站点业务量实体
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="g2BusinessVolumeId">2G业务量Id</param>
        /// <param name="d2BusinessVolumeId">2D业务量Id</param>
        /// <param name="g3BusinessVolumeId">3G业务量Id</param>
        /// <param name="g4BusinessVolumeId">4G业务量Id</param>
        /// <param name="companyId">分公司Id</param>
        internal PlaceBusinessVolume(Guid placeId, Guid g2BusinessVolumeId, Guid d2BusinessVolumeId, Guid g3BusinessVolumeId, Guid g4BusinessVolumeId, Guid companyId)
        {
            this.Id = Guid.NewGuid();
            this.PlaceId = placeId;
            this.G2BusinessVolumeId = g2BusinessVolumeId;
            this.D2BusinessVolumeId = d2BusinessVolumeId;
            this.G3BusinessVolumeId = g3BusinessVolumeId;
            this.G4BusinessVolumeId = g4BusinessVolumeId;
            this.CompanyId = companyId;
            this.CreateUserId = Guid.Empty;
            this.ModifyUserId = this.CreateUserId;
            this.CreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            this.ModifyDate = this.CreateDate;
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
        /// 2G业务量Id
        /// </summary>
        public Guid G2BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 2D业务量Id
        /// </summary>
        public Guid D2BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 3G业务量Id
        /// </summary>
        public Guid G3BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 4G业务量Id
        /// </summary>
        public Guid G4BusinessVolumeId
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
        /// 修改业务量Id
        /// </summary>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="businessVolumeId">业务量Id</param>
        public void Modify(LogicalType logicalType, Guid businessVolumeId)
        {
            if (logicalType == LogicalType.G2)
            {
                this.G2BusinessVolumeId = businessVolumeId;
            }
            if (logicalType == LogicalType.D2)
            {
                this.D2BusinessVolumeId = businessVolumeId;
            }
            if (logicalType == LogicalType.G3)
            {
                this.G3BusinessVolumeId = businessVolumeId;
            }
            if (logicalType == LogicalType.G4)
            {
                this.G4BusinessVolumeId = businessVolumeId;
            }
        }
    }
}
