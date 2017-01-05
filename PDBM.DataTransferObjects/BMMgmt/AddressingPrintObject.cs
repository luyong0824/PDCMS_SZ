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
    /// 寻址确认打印对象
    /// </summary>
    [DataContract, ProtoContract]
    public class AddressingPrintObject
    {
        /// <summary>
        /// 寻址确认Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 申请日期
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CreateDateText
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
        /// 区域名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string PlaceCategoryName
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string ImportanceName
        {
            get;
            set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal Lng
        {
            get;
            set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [DataMember, ProtoMember(11)]
        public decimal Lat
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string PlaceOwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁部门
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string AddressingDepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 租赁人
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string AddressingRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 业主名称
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string OwnerName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string OwnerContact
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string OwnerPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例信息HTML字符串
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string WFActivityInstancesInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid PlanningId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(24)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(26)]
        public int Count
        {
            get;
            set;
        }
    }
}
