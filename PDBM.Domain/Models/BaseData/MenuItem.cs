using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.Domain.Models.Enum;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 菜单项实体
    /// </summary>
    public class MenuItem : AggregateRoot
    {
        protected MenuItem()
        {
        }

        /// <summary>
        /// 子菜单Id
        /// </summary>
        public Guid MenuSubId
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项名称
        /// </summary>
        public string MenuItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项路径
        /// </summary>
        public string MenuItemPath
        {
            get;
            set;
        }

        /// <summary>
        /// 索引编号
        /// </summary>
        public int IndexId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        public Bool IsDisplay
        {
            get;
            set;
        }

        /// <summary>
        /// 创建人用户Id
        /// </summary>
        public Guid CreateUserId
        {
            get;
            set;
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            get;
            set;
        }

        #region Relations
        /// <summary>
        /// 子菜单实体
        /// </summary>
        protected virtual MenuSub MenuSub
        {
            get;
            set;
        }

        /// <summary>
        /// 角色菜单实体列表
        /// </summary>
        protected virtual ICollection<RoleMenuItem> RoleMenuItems
        {
            get;
            set;
        }
        #endregion
    }
}
