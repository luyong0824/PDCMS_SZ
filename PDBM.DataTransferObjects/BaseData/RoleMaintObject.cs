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
    /// 角色维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class RoleMaintObject
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 角色编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string RoleCode
        {
            get;
            set;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string RoleName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(5)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
