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
    /// 设备安装资源维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class EquipmentInstallMaintObject
    {
        /// <summary>
        /// 设备安装资源Id
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
        /// 开关电源
        /// </summary>
        [DataMember, ProtoMember(4)]
        public decimal SwitchPower
        {
            get;
            set;
        }

        /// <summary>
        /// 蓄电池
        /// </summary>
        [DataMember, ProtoMember(5)]
        public decimal Battery
        {
            get;
            set;
        }

        /// <summary>
        /// 机柜数量
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int CabinetNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 预算价
        /// </summary>
        [DataMember, ProtoMember(7)]
        public decimal BudgetPrice
        {
            get;
            set;
        }

        /// <summary>
        /// 施工单位Id
        /// </summary>
        [DataMember, ProtoMember(8)]
        public Guid? CustomerId
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
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(10)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(12)]
        public Guid ModifyUserId
        {
            get;
            set;
        }
    }
}
