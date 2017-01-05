using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 站点信息实体
    /// </summary>
    [DataContract, ProtoContract]
    public class PlaceInfoObject
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string ImportanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(8)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string PlaceOwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 登记人
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string CreateUserName
        {
            get;
            set;
        }

        /// <summary>
        /// 登记日期
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 2G逻辑号
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string G2Number
        {
            get;
            set;
        }

        /// <summary>
        /// D2逻辑号
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string D2Number
        {
            get;
            set;
        }

        /// <summary>
        /// 3G逻辑号
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string G3Number
        {
            get;
            set;
        }

        /// <summary>
        /// 4G逻辑号
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string G4Number
        {
            get;
            set;
        }

        /// <summary>
        /// 5G逻辑号
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string G5Number
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁部门
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string AddressingDepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁人
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string AddressingRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 使用状态
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string StateName
        {
            get;
            set;
        }
    }
}
