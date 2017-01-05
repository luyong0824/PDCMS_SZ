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
    /// 用户维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class UserMaintObject
    {
        /// <summary>
        /// 用户Id
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
        /// 部门Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid DepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string PhoneNumber
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

        /// <summary>
        /// 唯一标识
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid UniqueCode
        {
            get;
            set;
        }
    }
}
