using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Domain.Models.BaseData
{
    /// <summary>
    /// 子菜单实体
    /// </summary>
    public class MenuSub : AggregateRoot
    {
        protected MenuSub()
        {
        }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid MenuId
        {
            get;
            set;
        }

        /// <summary>
        /// 子菜单名称
        /// </summary>
        public string MenuSubName
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
        /// 菜单实体
        /// </summary>
        protected virtual Menu Menu
        {
            get;
            set;
        }

        /// <summary>
        /// 菜单项实体列表
        /// </summary>
        protected virtual ICollection<MenuItem> MenuItems
        {
            get;
            set;
        }
        #endregion
    }
}
