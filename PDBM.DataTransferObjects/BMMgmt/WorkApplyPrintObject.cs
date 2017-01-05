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
    /// 派单申请打印对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WorkApplyPrintObject
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 申请部门
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 申请人
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 派单人
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string SendFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 申请日期
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 申请事由
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string ApplyReason
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动实例信息HTML字符串
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string WFActivityInstancesInfoHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 网格
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 申请单位
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 隐患上报Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 附件数量
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// 现场联系人
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string SceneContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人电话
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string SceneContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 退回原因
        /// </summary>
        [DataMember, ProtoMember(15)]
        public string ReturnReason
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember, ProtoMember(16)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否立项
        /// </summary>
        [DataMember, ProtoMember(17)]
        public string IsProjectName
        {
            get;
            set;
        }
    }
}
