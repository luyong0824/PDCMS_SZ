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
    /// 用户登录对象
    /// </summary>
    [DataContract, ProtoContract]
    public class UserLoginObject
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
        /// 用户名
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// 部门Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid DepartmentId
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid CompanyId
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司性质
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int CompanyNature
        {
            get;
            set;
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string PhoneNumber
        {
            get;
            set;
        }
    }
}
