using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 运营商规划维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class OperatorsPlanningMaintObject
    {
        /// <summary>
        /// 运营商规划Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规划编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PlanningCode
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlanningName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(8)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 天线挂高(米)
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal AntennaHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int PoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 紧要程度
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int Urgency
        {
            get;
            set;
        }

        /// <summary>
        /// 是否采纳
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int Solved
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid PlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人所在公司性质
        /// </summary>
        [DataMember, ProtoMember(20)]
        public int CurrentCompanyNature
        {
            get;
            set;
        }
    }
}
