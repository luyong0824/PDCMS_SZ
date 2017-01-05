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
    /// 铁塔基础维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class TowerBaseMaintObject
    {
        /// <summary>
        /// 铁塔基础资源Id
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
        /// 铁塔类型
        /// </summary>
        [DataMember, ProtoMember(4)]
        public int TowerBaseType
        {
            get;
            set;
        }

        /// <summary>
        /// 预算价
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid? CustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(7)]
        public string Memos
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
