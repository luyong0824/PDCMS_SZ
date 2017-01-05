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
    /// 业务量维护实体
    /// </summary>
    [DataContract, ProtoContract]
    public class BusinessVolumeMaintObject
    {
        /// <summary>
        /// 业务量Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑号类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int LogicalType
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑号
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string LogicalNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 话务量
        /// </summary>
        [DataMember, ProtoMember(4)]
        public decimal TrafficVolumes
        {
            get;
            set;
        }

        /// <summary>
        /// 业务量
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal BusinessVolumes
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(8)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(9)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 分公司Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid CompanyId
        {
            get;
            set;
        }
    }
}
