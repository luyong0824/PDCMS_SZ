using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 菜单应用层服务
    /// </summary>
    public class MenuService : DataService, IMenuService
    {
        private readonly IRepository<Menu> menuRepository;

        public MenuService(IRepositoryContext context,
            IRepository<Menu> menuRepository)
            : base(context)
        {
            this.menuRepository = menuRepository;
        }

        /// <summary>
        /// 根据用户Id获取用户所属角色权限内的菜单列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>菜单列表的Json字符串</returns>
        public string GetMenuInfo(Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetMenuInfoByUserId", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>菜单对象列表</returns>
        public IList<MenuSelectObject> GetMenus()
        {
            IList<MenuSelectObject> menuSelectObjects = new List<MenuSelectObject>();
            IEnumerable<Menu> menus = menuRepository.FindAll(null, "IndexId");
            if (menus != null)
            {
                foreach (var menu in menus)
                {
                    menuSelectObjects.Add(MapperHelper.Map<Menu, MenuSelectObject>(menu));
                }
            }
            return menuSelectObjects;
        }
    }
}
