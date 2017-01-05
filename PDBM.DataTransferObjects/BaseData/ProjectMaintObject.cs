using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 项目维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ProjectMaintObject
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 项目简称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目全称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string ProjectFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目类型
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int ProjectCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 会计主体Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid AccountingEntityId
        {
            get;
            set;
        }

        /// <summary>
        /// 分管总经理用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ManagerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 分管总经理姓名
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ManagerUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目负责人用户Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid ResponsibleUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 项目负责人姓名
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string ResponsibleUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 项目进度
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int ProjectProgress
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 所涉专业列表
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string ProfessionList
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

        /// <summary>
        /// 预算金额
        /// </summary>
        [DataMember, ProtoMember(17)]
        public decimal BudgetPrice
        {
            get;
            set;
        }
    }
}
