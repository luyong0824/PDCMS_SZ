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
    /// 业务量分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class BusinessVolumeService : IBusinessVolumeService
    {
        private readonly IBusinessVolumeService businessVolumeServiceImpl = ServiceLocator.Instance.GetService<IBusinessVolumeService>();

        public BusinessVolumeMaintObject GetBusinessVolumeById(Guid id)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateBusinessVolume(BusinessVolumeMaintObject businessVolumeMaintObject)
        {
            try
            {
                businessVolumeServiceImpl.AddOrUpdateBusinessVolume(businessVolumeMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveBusinessVolumes(IList<BusinessVolumeMaintObject> businessVolumeMaintObjects)
        {
            try
            {
                businessVolumeServiceImpl.RemoveBusinessVolumes(businessVolumeMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumesPage(pageIndex, pageSize, beginDate, endDate, logicalType, logicalNumber, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeReportPage(pageIndex, pageSize, beginDate, endDate, placeName, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeReseau(beginDate, endDate, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeArea(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeCompany(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthPlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId, string sortField, string sortOrder)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthPlace(pageIndex, pageSize, beginDate, endDate, areaId, reseauId, placeName, profession, companyId, sortField, sortOrder);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthReseau(beginDate, endDate, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthArea(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthCompany(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthRisePlace(pageIndex, pageSize, beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearRisePlace(pageIndex, pageSize, beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthRiseReseau(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearRiseReseau(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthRiseArea(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearRiseArea(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeMonthRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeMonthRiseCompany(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearRiseCompany(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearGrowthReseau(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearGrowthReseau(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearGrowthArea(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearGrowthArea(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetBusinessVolumeYearGrowthCompany(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return businessVolumeServiceImpl.GetBusinessVolumeYearGrowthCompany(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            businessVolumeServiceImpl.Dispose();
        }
    }
}
