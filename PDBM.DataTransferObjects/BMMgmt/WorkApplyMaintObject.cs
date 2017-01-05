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
    /// 隐患上报维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkApplyMaintObject
    {
        /// <summary>
        /// 隐患上报Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单位Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 申请事由
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ApplyReason
        {
            get;
            set;
        }

        /// <summary>
        /// 是否解决
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int IsSoved
        {
            get;
            set;
        }

        /// <summary>
        /// 派工单Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid? WorkOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单审批状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int OrderState
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 派单人名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string SendFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid ReseauId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格经理Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid ReseauManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格经理名称
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string ReseauFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单位名称
        /// </summary>
        [DataMember, ProtoMember(18)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 附件Id列表
        /// </summary>
        [DataMember, ProtoMember(20)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        [DataMember, ProtoMember(21)]
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember, ProtoMember(22)]
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 退回原因
        /// </summary>
        [DataMember, ProtoMember(23)]
        public string ReturnReason
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember, ProtoMember(24)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否立项
        /// </summary>
        [DataMember, ProtoMember(25)]
        public int IsProject
        {
            get;
            set;
        }
    }
}
