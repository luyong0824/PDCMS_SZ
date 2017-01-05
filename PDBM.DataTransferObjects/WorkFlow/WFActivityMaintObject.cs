using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.WorkFlow
{
    /// <summary>
    /// 工作流活动维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class WFActivityMaintObject
    {
        /// <summary>
        /// 工作流活动Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流过程Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid WFProcessId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string WFActivityName
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动操作类型
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int WFActivityOperate
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动编辑器Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid WFActivityEditorId
        {
            get;
            set;
        }

        /// <summary>
        /// 工作流活动顺序类型
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int WFActivityOrder
        {
            get;
            set;
        }

        /// <summary>
        /// 序号
        /// </summary>
        [DataMember, ProtoMember(7)]
        public int SerialId
        {
            get;
            set;
        }

        /// <summary>
        /// 行号
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int RowId
        {
            get;
            set;
        }

        /// <summary>
        /// 时限
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int Timelimit
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid DepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid PostId
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
        /// 是否必须编辑
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int IsMustEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 是否必要步骤
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int IsNecessaryStep
        {
            get;
            set;
        }
    }
}
