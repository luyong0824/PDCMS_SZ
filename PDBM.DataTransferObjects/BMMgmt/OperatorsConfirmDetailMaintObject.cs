using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace PDBM.DataTransferObjects.BMMgmt
{
    /// <summary>
    /// 运营商确认明细维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class OperatorsConfirmDetailMaintObject
    {
        /// <summary>
        /// 运营商确认明细Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 规划Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid PlanningId
        {
            get;
            set;
        }
    }
}
