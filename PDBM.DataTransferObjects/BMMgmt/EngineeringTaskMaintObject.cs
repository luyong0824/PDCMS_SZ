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
    /// 工程任务实体
    /// </summary>
    [DataContract, ProtoContract]
    public class EngineeringTaskMaintObject
    {
        /// <summary>
        /// 工程任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工程名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int TaskModel
        {
            get;
            set;
        }

        /// <summary>
        /// 项目任务Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid ProjectTaskId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计单位Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid ConstructionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid SupervisionCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ProjectManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计简述
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string DesignMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string DesignRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 设计日期
        /// </summary>
        [DataMember, ProtoMember(10)]
        public DateTime DesignDate
        {
            get;
            set;
        }

        /// <summary>
        /// 工程进度
        /// </summary>
        [DataMember, ProtoMember(11)]
        public int EngineeringProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 进度登记日期
        /// </summary>
        [DataMember, ProtoMember(13)]
        public DateTime ProgressDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(15)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(16)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
