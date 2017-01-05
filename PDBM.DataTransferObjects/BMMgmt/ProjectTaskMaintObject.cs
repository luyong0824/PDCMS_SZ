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
    /// 项目任务实体
    /// </summary>
    [DataContract, ProtoContract]
    public class ProjectTaskMaintObject
    {
        /// <summary>
        /// 项目任务Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        [DataMember, ProtoMember(2)]
        public int ProjectType
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid AreaManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 总设单位Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid GeneralDesignId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string DesignRealName
        {
            get;
            set;
        }

        /// <summary>
        /// 设计日期
        /// </summary>
        [DataMember, ProtoMember(7)]
        public DateTime DesignDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编号
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目进度
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int ProjectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 进度简述
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string ProgressMemos
        {
            get;
            set;
        }

        /// <summary>
        /// 项目开通日期
        /// </summary>
        [DataMember, ProtoMember(11)]
        public DateTime ProjectDate
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(14)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目启动日期
        /// </summary>
        [DataMember, ProtoMember(15)]
        public DateTime ProjectBeginDate
        {
            get;
            set;
        }

        /// <summary>
        /// 项目启动日期
        /// </summary>
        [DataMember, ProtoMember(16)]
        public DateTime ProjectBeginDateText
        {
            get;
            set;
        }

        /// <summary>
        /// 站点Id
        /// </summary>
        [DataMember, ProtoMember(17)]
        public Guid PlaceId
        {
            get;
            set;
        }
    }
}
