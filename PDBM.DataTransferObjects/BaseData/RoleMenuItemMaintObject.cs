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
    /// 角色菜单维护对象
    /// </summary>
    [DataContract, ProtoContract]
    public class RoleMenuItemMaintObject
    {
        /// <summary>
        /// 角色菜单Id
        /// </summary>
        [DataMember, ProtoMember(1)]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        [DataMember, ProtoMember(2)]
        public Guid RoleId
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项Id
        /// </summary>
        [DataMember, ProtoMember(3)]
        public Guid MenuItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        [DataMember, ProtoMember(4)]
        public Guid CreateUserId
        {
            get;
            set;
        }
    }
}
