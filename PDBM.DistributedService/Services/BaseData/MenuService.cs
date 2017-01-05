using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 菜单分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MenuService : IMenuService
    {
        private readonly IMenuService menuServiceImpl = ServiceLocator.Instance.GetService<IMenuService>();

        public string GetMenuInfo(Guid userId)
        {
            try
            {
                return menuServiceImpl.GetMenuInfo(userId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<MenuSelectObject> GetMenus()
        {
            try
            {
                return menuServiceImpl.GetMenus();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            menuServiceImpl.Dispose();
        }
    }
}
