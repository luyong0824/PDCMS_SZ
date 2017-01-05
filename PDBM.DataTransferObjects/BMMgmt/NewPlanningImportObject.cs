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
    /// 新模式规划批量导入对象
    /// </summary>
    [DataContract, ProtoContract]
    public class NewPlanningImportObject
    {
        /// <summary>
        /// 移动需求
        /// </summary>
        [DataMember, ProtoMember(1)]
        public int MobileDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 电信需求
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int TelecomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 联通需求
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int UnicomDemand
        {
            get;
            set;
        }

        /// <summary>
        /// 规划名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlanningName
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid PlaceCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 周边场景Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid SceneId
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 移动天线挂高(米)
        /// </summary>
        [DataMember, ProtoMember(17)]
        public decimal MobileAntennaHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 移动抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int MobilePoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int MobileCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 移动确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid MobileUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 电信天线挂高(米)
        /// </summary>
        [DataMember, ProtoMember(21)]
        public decimal TelecomAntennaHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 电信抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int TelecomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(23)]
        public int TelecomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 电信确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid TelecomUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 联通天线挂高(米)
        /// </summary>
        [DataMember, ProtoMember(25)]
        public decimal UnicomAntennaHeight
        {
            get;
            set;
        }

        /// <summary>
        /// 联通抱杆数量(根)
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int UnicomPoleNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通机柜数量(个)
        /// </summary>
        [DataMember, ProtoMember(27)]
        public int UnicomCabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 联通确认人用户Id
        /// </summary>
        [DataMember, ProtoMember(28)]
        public Guid UnicomUserId
        {
            get;
            set;
        }
    }
}
