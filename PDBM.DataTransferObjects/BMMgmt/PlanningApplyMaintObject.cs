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
    /// 建设申请维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlanningApplyMaintObject
    {
        /// <summary>
        /// 建设申请Id
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
        /// 公文审批Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid WFActivityInstanceId
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
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int Importance
        {
            get;
            set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string DetailedAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 建设理由
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(14)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期文本
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string CreateDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 拟建网络
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string ProposedNetwork
        {
            get;
            set;
        }

        /// <summary>
        /// 可选位置
        /// </summary>
        [DataMember, ProtoMember(19)]
        public string OptionalAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 产权
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid PlaceOwner
        {
            get;
            set;
        }

        /// <summary>
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(22)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 区域
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 重要性程度
        /// </summary>
        [DataMember, ProtoMember(25)]
        public string ImportanceText
        {
            get;
            set;
        }

        /// <summary>
        /// 规划用户Id
        /// </summary>
        [DataMember, ProtoMember(26)]
        public Guid PlanningUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 建设申请单Id
        /// </summary>
        [DataMember, ProtoMember(27)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划意见
        /// </summary>
        [DataMember, ProtoMember(28)]
        public int PlanningAdvice
        {
            get;
            set;
        }
    }
}
