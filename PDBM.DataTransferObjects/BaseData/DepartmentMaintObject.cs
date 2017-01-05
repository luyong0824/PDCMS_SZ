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
    /// 部门维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class DepartmentMaintObject
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string DepartmentCode
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 部门经理用户Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid ManagerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门经理姓名
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(8)]
        public int State
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
    }
}
