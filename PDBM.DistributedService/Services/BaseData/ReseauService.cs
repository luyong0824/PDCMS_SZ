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
    /// 网格分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ReseauService : IReseauService
    {
        private readonly IReseauService reseauServiceImpl = ServiceLocator.Instance.GetService<IReseauService>();

        public ReseauMaintObject GetReseauById(Guid id)
        {
            try
            {
                return reseauServiceImpl.GetReseauById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ReseauMaintObject> GetReseaus(Guid areaId)
        {
            try
            {
                return reseauServiceImpl.GetReseaus(areaId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetAllReseaus(Guid areaId)
        {
            try
            {
                return reseauServiceImpl.GetAllReseaus(areaId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ReseauSelectObject> GetUsedReseaus(Guid areaId)
        {
            try
            {
                return reseauServiceImpl.GetUsedReseaus(areaId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ReseauSelectObject> GetAllUsedReseaus()
        {
            try
            {
                return reseauServiceImpl.GetAllUsedReseaus();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateReseau(ReseauMaintObject reseauMaintObject)
        {
            try
            {
                reseauServiceImpl.AddOrUpdateReseau(reseauMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveReseaus(IList<ReseauMaintObject> reseauMaintObjects)
        {
            try
            {
                reseauServiceImpl.RemoveReseaus(reseauMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            reseauServiceImpl.Dispose();
        }
    }
}
