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
    /// 规格类型信息表维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MaterialSpecListMaintObject
    {
        /// <summary>
        /// 规格类型表Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 立项编号
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 规格型号
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string MaterialSpecName
        {
            get;
            set;
        }

        /// <summary>
        /// 单价
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal UnitPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember, ProtoMember(6)]
        public decimal SpecNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal TotalPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string OrderCode
        {
            get;
            set;
        }

        /// <summary>
        /// 使用状态
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
        /// 创建日期
        /// </summary>
        [DataMember, ProtoMember(12)]
        public DateTime CreateDate
        {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        [DataMember, ProtoMember(13)]
        public DateTime ModifyDate
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string CreateFullName
        {
            get;
            set;
        }

        /// <summary>
        /// 规格型号分类
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int MaterialSpecType
        {
            get;
            set;
        }
    }
}
