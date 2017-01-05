using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 站点业务量维护实体
    /// </summary>
    [DataContract, ProtoContract]
    public class PlaceBusinessVolumeMaintObject
    {
        /// <summary>
        /// 站点业务量Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 2G业务量Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid G2BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 2D业务量Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid D2BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 3G业务量Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid G3BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 4G业务量Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid G4BusinessVolumeId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(9)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(10)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 分公司Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑号类型
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int LogicalType
        {
            get;
            set;
        }

        /// <summary>
        /// 业务量Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid BusinessVolumeId
        {
            get;
            set;
        }
    }
}
