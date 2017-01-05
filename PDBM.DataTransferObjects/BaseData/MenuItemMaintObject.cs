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
    /// 菜单维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class MenuItemMaintObject
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        [DataMember, ProtoMember(1)]
        public string MenuItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单路径
        /// </summary>
        [DataMember, ProtoMember(2)]
        public string MenuItemPath
        {
            get;
            set;
        }
    }
}
