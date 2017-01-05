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
    /// 任务维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ConstructionTaskMaintObject
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 建设方式
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int ConstructionMethod
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid PlaceId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid ProjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 建设进度
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int ConstructionProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string ProjectManagerName
        {
            get;
            set;
        }

        /// <summary>
        /// 站点名称
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string PlaceName
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位名称
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string SupervisorCustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 监理人员Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid SupervisorUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理人员名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string SupervisorUserName
        {
            get;
            set;
        }
    }
}
