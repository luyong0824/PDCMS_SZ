using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DataTransferObjects.BaseData
{
    /// <summary>
    /// 往来单位维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class CustomerMaintObject
    {
        /// <summary>
        /// 往来单位Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string CustomerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位简称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位全称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string CustomerFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ContactMan
        {
            get;
            set;
        }

        /// <summary>
        /// 联系方式
        /// </summary>
        [DataMember, ProtoMember(6)]
        public string ContactTel
        {
            get;
            set;
        }

        /// <summary>
        /// 联系地址
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string ContactAddr
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(9)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(10)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆人用户Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid CustomerUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 登陆人用户名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string CustomerUserFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 往来单位分类
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int CustomerType
        {
            get;
            set;
        }
    }
}
