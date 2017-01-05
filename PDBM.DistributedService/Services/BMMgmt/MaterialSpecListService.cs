using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 规格型号信息分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MaterialSpecListService : IMaterialSpecListService
    {
        private readonly IMaterialSpecListService materialSpecListServiceImpl = ServiceLocator.Instance.GetService<IMaterialSpecListService>();

        public MaterialSpecListMaintObject GetMaterialSpecListById(Guid id)
        {
            try
            {
                return materialSpecListServiceImpl.GetMaterialSpecListById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateMaterialSpecList(MaterialSpecListMaintObject materialSpecListMaintObject)
        {
            try
            {
                materialSpecListServiceImpl.AddOrUpdateMaterialSpecList(materialSpecListMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetMaterialSpecListPage(int pageIndex, int pageSize, string projectCode, string customerName, int materialSpecType, string materialSpecName, string orderCode, Guid createUserId)
        {
            try
            {
                return materialSpecListServiceImpl.GetMaterialSpecListPage(pageIndex, pageSize, projectCode, customerName, materialSpecType, materialSpecName, orderCode, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetProjectCodeListAndMaterialSpecListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode)
        {
            try
            {
                return materialSpecListServiceImpl.GetProjectCodeListAndMaterialSpecListPage(pageIndex, pageSize, beginDate, endDate, projectCode, projectType, placeName, reseauId, projectManagerId, customerName, materialSpecType, materialSpecName, orderCode);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            materialSpecListServiceImpl.Dispose();
        }
    }
}
