using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.Map
{
    /// <summary>
    /// 点对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PointObject
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int DataType
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int Profession
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
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ReseauName
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
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int PropertyRight
        {
            get;
            set;
        }

        /// <summary>
        /// 电信共享
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int TelecomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 移动共享
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int MobileShare
        {
            get;
            set;
        }

        /// <summary>
        /// 联通共享
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int UnicomShare
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系人
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 业主联系电话
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 规划公司Id
        /// </summary>
        [DataMember, ProtoMember(18)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划公司名称
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划列表
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string OperatorsPlannings
        {
            get;
            set;
        }

        /// <summary>
        /// 基站清单
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string Places
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划清单
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string Operators
        {
            get;
            set;
        }

        [DataMember, ProtoMember(23)]
        public string Plannings
        {
            get;
            set;
        }

        /// <summary>
        /// 运营商规划列表及显示周边已有站点
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string OperatorsPlanningsAndPlaces
        {
            get;
            set;
        }

        /// <summary>
        /// 是否下达
        /// </summary>
        [DataMember, ProtoMember(25)]
        public int Issued
        {
            get;
            set;
        }

        /// <summary>
        /// 寻址状态
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string AddressingStateName
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(27)]
        public string PlaceOwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 已有网络
        /// </summary>
        [DataMember, ProtoMember(28)]
        public string NetWorks
        {
            get;
            set;
        }

        /// <summary>
        /// 站点地图状态
        /// </summary>
        [DataMember, ProtoMember(29)]
        public int PlaceMapState
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是从规划页面查看
        /// </summary>
        [DataMember, ProtoMember(30)]
        public int IsFromPlanning
        {
            get;
            set;
        }

        /// <summary>
        /// 规划及已有站点清单
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string PlanningsAndPlaces
        {
            get;
            set;
        }
    }
}
