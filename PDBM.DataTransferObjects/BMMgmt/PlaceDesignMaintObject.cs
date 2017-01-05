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
    /// 站点设计信息维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class PlaceDesignMaintObject
    {
        /// <summary>
        /// 站点设计信息Id
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
        /// 设计单位Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid? DesignCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 设计人员Id
        /// </summary>
        [DataMember, ProtoMember(5)]
        public Guid? DesignUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理单位Id
        /// </summary>
        [DataMember, ProtoMember(6)]
        public Guid? SupervisorCustomerId
        {
            get;
            set;
        }

        /// <summary>
        /// 监理人员Id
        /// </summary>
        [DataMember, ProtoMember(7)]
        public Guid? SupervisorUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 集团项目编码
        /// </summary>
        [DataMember, ProtoMember(8)]
        public string ProjectCode
        {
            get;
            set;
        }

        /// <summary>
        /// 集团项目名称
        /// </summary>
        [DataMember, ProtoMember(9)]
        public string ProjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 项目预算
        /// </summary>
        [DataMember, ProtoMember(10)]
        public decimal ProjectMoney
        {
            get;
            set;
        }

        /// <summary>
        /// 集团地址编码
        /// </summary>
        [DataMember, ProtoMember(11)]
        public string GroupPlaceCode
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔资源
        /// </summary>
        [DataMember, ProtoMember(12)]
        public int TowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有铁塔基础资源
        /// </summary>
        [DataMember, ProtoMember(13)]
        public int TowerBaseMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有机房资源
        /// </summary>
        [DataMember, ProtoMember(14)]
        public int MachineRoomMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有外电引入资源
        /// </summary>
        [DataMember, ProtoMember(15)]
        public int ExternalElectricPowerMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有设备安装资源
        /// </summary>
        [DataMember, ProtoMember(16)]
        public int EquipmentInstallMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有地质勘探资源
        /// </summary>
        [DataMember, ProtoMember(17)]
        public int AddressExplorMark
        {
            get;
            set;
        }

        /// <summary>
        /// 是否拥有桩基动测资源
        /// </summary>
        [DataMember, ProtoMember(18)]
        public int FoundationTestMark
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(19)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(20)]
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 修改人用户Id
        /// </summary>
        [DataMember, ProtoMember(21)]
        public Guid ModifyUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 公文批复Id
        /// </summary>
        [DataMember, ProtoMember(22)]
        public Guid WFActivityInstanceId
        {
            get;
            set;
        }

        /// <summary>
        /// 工程经理Id
        /// </summary>
        [DataMember, ProtoMember(23)]
        public Guid? ProjectManagerId
        {
            get;
            set;
        }

    }
}
