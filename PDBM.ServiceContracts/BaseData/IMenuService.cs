using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.Common;

namespace PDBM.ServiceContracts.BaseData
{
    /// <summary>
    /// 菜单服务接口
    /// </summary>
    [ServiceContract]
    public interface IMenuService : IDistributedService
    {
        /// <summary>
        /// 根据用户Id获取用户所属角色权限内的菜单列表
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>菜单列表的Json字符串</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        string GetMenuInfo(Guid userId);

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>菜单对象列表</returns>
        [OperationContract]
        [FaultContract(typeof(FaultObject))]
        IList<MenuSelectObject> GetMenus();
    }
}
