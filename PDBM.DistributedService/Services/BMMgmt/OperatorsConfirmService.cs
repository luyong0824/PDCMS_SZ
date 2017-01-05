using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.DistributedService.Services.BMMgmt
{
    /// <summary>
    /// 运营商确认分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class OperatorsConfirmService : IOperatorsConfirmService
    {
        private readonly IOperatorsConfirmService operatorsConfirmServiceImpl = ServiceLocator.Instance.GetService<IOperatorsConfirmService>();

        public string GetOperatorsConfirmDetailsPage(int pageIndex, int pageSize, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId)
        {
            try
            {
                return operatorsConfirmServiceImpl.GetOperatorsConfirmDetailsPage(pageIndex, pageSize, planningCode, planningName, profession, placeCategoryId, areaId, reseauId, demand, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOperatorsConfirm(Guid createUserId, IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects)
        {
            try
            {
                operatorsConfirmServiceImpl.AddOperatorsConfirm(createUserId, operatorsConfirmDetailMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Confirm(Guid currentUserId, Guid currentCompanyId, int currentCompanyNature, int demand, IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects)
        {
            try
            {
                operatorsConfirmServiceImpl.Confirm(currentUserId, currentCompanyId, currentCompanyNature, demand, operatorsConfirmDetailMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            operatorsConfirmServiceImpl.Dispose();
        }
    }
}
