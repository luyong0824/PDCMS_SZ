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
    /// 网格维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class ReseauMaintObject
    {
        /// <summary>
        /// 网格Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid AreaId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格编码
        /// </summary>
        [DataMember, ProtoMember(3)]
        public string ReseauCode
        {
            get;
            set;
        }

        /// <summary>
        /// 网格名称
        /// </summary>
        [DataMember, ProtoMember(4)]
        public string ReseauName
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember, ProtoMember(5)]
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember, ProtoMember(6)]
        public int State
        {
            get;
            set;
        }

        /// <summary>
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

        /// <summary>
        /// 网格经理Id
        /// </summary>
        [DataMember, ProtoMember(9)]
        public Guid? ReseauManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 网格经理名称
        /// </summary>
        [DataMember, ProtoMember(10)]
        public string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// 规划经理Id
        /// </summary>
        [DataMember, ProtoMember(11)]
        public Guid? PlanningManagerId
        {
            get;
            set;
        }

        /// <summary>
        /// 规划经理名称
        /// </summary>
        [DataMember, ProtoMember(12)]
        public string PlanningFullName
        {
            get;
            set;
        }
    }
}
