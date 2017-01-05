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
    /// 菜单选择对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MenuSelectObject
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string MenuName
        {
            get;
            set;
        }
    }
}
