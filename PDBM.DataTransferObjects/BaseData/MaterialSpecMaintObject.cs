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
    /// 设计规格维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MaterialSpecMaintObject
    {
        /// <summary>
        /// 设计规格Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规格编码
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string MaterialSpecCode
        {
            get;
            set;
        }

        /// <summary>
        /// 规格名称
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string MaterialSpecName
        {
            get;
            set;
        }

        /// <summary>
        /// 物资名称Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid MaterialId
        {
            get;
            set;
        }

        /// <summary>
        /// 计量单位Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid UnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 参考单价
        /// </summary>
        [DataMember, ProtoMember(6)]
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid? CustomerId
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
        /// 文件Id列表
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string FileIdList
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DataMember, ProtoMember(13)]
        public string CustomerName
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [DataMember, ProtoMember(14)]
        public string SupplierCustomerName
        {
            get;
            set;
        }
    }
}
