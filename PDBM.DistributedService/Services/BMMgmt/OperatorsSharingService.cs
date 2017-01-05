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
    /// 运营商共享分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class OperatorsSharingService : IOperatorsSharingService
    {
        private readonly IOperatorsSharingService operatorsSharingServiceImpl = ServiceLocator.Instance.GetService<IOperatorsSharingService>();

        public OperatorsSharingMaintObject GetOperatorsSharingById(Guid id)
        {
            try
            {
                return operatorsSharingServiceImpl.GetOperatorsSharingById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsSharingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeCode, string placeName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency, int solved)
        {
            try
            {
                return operatorsSharingServiceImpl.GetOperatorsSharingsPage(pageIndex, pageSize, beginDate, endDate, placeCode, placeName, companyId, profession, placeCategoryId, areaId, reseauId, urgency, solved);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsSharingsPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency)
        {
            try
            {
                return operatorsSharingServiceImpl.GetOperatorsSharingsPageBySelect(pageIndex, pageSize, placeCode, placeName, companyId, profession, placeCategoryId, areaId, reseauId, urgency);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetOperatorsSharingsByPlace(Guid operatorsSharingId, Guid remodelingId)
        {
            try
            {
                return operatorsSharingServiceImpl.GetOperatorsSharingsByPlace(operatorsSharingId, remodelingId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateOperatorsSharing(OperatorsSharingMaintObject operatorsSharingMaintObject)
        {
            try
            {
                operatorsSharingServiceImpl.AddOrUpdateOperatorsSharing(operatorsSharingMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Associate(Guid remodelingId, Guid remodelingCreateUserId, Guid currentUserId, IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects)
        {
            try
            {
                operatorsSharingServiceImpl.Associate(remodelingId, remodelingCreateUserId, currentUserId, operatorsSharingMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveOperatorsSharings(IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects)
        {
            try
            {
                operatorsSharingServiceImpl.RemoveOperatorsSharings(operatorsSharingMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            operatorsSharingServiceImpl.Dispose();
        }
    }
}
