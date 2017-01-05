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
    /// 公司维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class CompanyMaintObject
    {
        /// <summary>
        /// 公司Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 公司编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string CompanyCode
        {
            get;
            set;
        }

        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CompanyName
        {
            get;
            set;
        }

        /// <summary>
        /// 公司全称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string CompanyFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 生成的单据编码前缀
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string ApplyCodePrefix
        {
            get;
            set;
        }

        /// <summary>
        /// 公司性质
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int CompanyNature
        {
            get;
            set;
        }

        // <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
