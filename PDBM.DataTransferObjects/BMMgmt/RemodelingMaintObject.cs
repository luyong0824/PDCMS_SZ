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
    /// 改造维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class RemodelingMaintObject
    {
        /// <summary>
        /// 改造Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 专业
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int Profession
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string PlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid PlaceId
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
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 站点类型Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid PlaceCategoryId
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
        /// 紧要程度
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int Urgency
        {
            get;
            set;
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 是否下达
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int Issued
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理名字
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string ProjectManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// 改造流程状态
        /// </summary>
        [DataMember, ProtoMember(25)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 站点编码
        /// </summary>
        [DataMember, ProtoMember(26)]
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 改造状态
        /// </summary>
        [DataMember, ProtoMember(27)]
        public string OrderStateName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算
        /// </summary>
        [DataMember, ProtoMember(28)]
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember, ProtoMember(29)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 公文流转Id
        /// </summary>
        [DataMember, ProtoMember(30)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        [DataMember, ProtoMember(31)]
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        [DataMember, ProtoMember(32)]
        public int ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(33)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(34)]
        public int Count
        {
            get;
            set;
        }
    }
}
