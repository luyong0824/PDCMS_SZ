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
    /// 物资清单维护实体
    /// </summary>
    [DataContract, ProtoContract]
    public class MaterialListMaintObject
    {
        /// <summary>
        /// 地质勘探资源Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 父表Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 资源类型
        /// </summary>
        [DataMember, ProtoMember(3)]
        public int PropertyType
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
        /// 设计规格Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid? MaterialSpecId
        {
            get;
            set;
        }

        /// <summary>
        /// 预算金额
        /// </summary>
        [DataMember, ProtoMember(6)]
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal SpecNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 供应商Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid? SupplierId
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string Memos
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
        /// 物资类别Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid MaterialCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 物资清单Id
        /// </summary>
        [DataMember, ProtoMember(13)]
        public Guid MaterialListId
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

        /// <summary>
        /// 申购状态
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int DoState
        {
            get;
            set;
        }
    }
}
