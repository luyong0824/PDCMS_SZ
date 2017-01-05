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
    /// 用户信息维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class UserInfoMaintObject
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
        /// 手机号
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string PhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称列表
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 部门名称列表
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string DepartmentName
        {
            get;
            set;
        }

        /// <summary>
        /// 角色名称列表
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string RoleNameList
        {
            get;
            set;
        }

        /// <summary>
        /// 岗位名称列表
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string PostNameList
        {
            get;
            set;
        }

        /// <summary>
        /// 当前密码
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string OldUserPassword
        {
            get;
            set;
        }

        /// <summary>
        /// 新密码，为空则不修改密码
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string NewUserPassword
        {
            get;
            set;
        }

        /// <summary>
        /// 确认密码
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string ConfirmNewUserPassword
        {
            get;
            set;
        }
    }
}
