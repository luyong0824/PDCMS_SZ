using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.DataImport;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.DistributedService.Services.DataImport
{
    /// <summary>
    /// 数据导入分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DataImportService : IDataImportService
    {
        private readonly IDataImportService dataImportServiceImpl = ServiceLocator.Instance.GetService<IDataImportService>();

        public IList<ImportErrorObject> ImportOperatorsPlanningBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature)
        {
            try
            {
                return dataImportServiceImpl.ImportOperatorsPlanningBS(excelFileId, createUserId, companyId, currentCompanyNature);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportOperatorsSharingBS(Guid excelFileId, Guid createUserId, Guid companyId, int currentCompanyNature)
        {
            try
            {
                return dataImportServiceImpl.ImportOperatorsSharingBS(excelFileId, createUserId, companyId, currentCompanyNature);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPurchaseBS(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPurchaseBS(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlanning(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlanning(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportRemodeling(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportRemodeling(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlace(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlace(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> UpdatePlace(Guid excelFileId, Guid modifyUserId)
        {
            try
            {
                return dataImportServiceImpl.UpdatePlace(excelFileId, modifyUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportLogicalNumber(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportLogicalNumber(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportBusinessVolume(Guid excelFileId, int logicalType, int profession, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportBusinessVolume(excelFileId, logicalType, profession, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportResources(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportResources(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportNewPlanningBS(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportNewPlanningBS(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportNewRemodelingBS(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportNewRemodelingBS(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportProjectCodeList(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportProjectCodeList(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportMaterialSpecList(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportMaterialSpecList(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlanningApply(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlanningApply(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlanningApplyID(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlanningApplyID(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlanningID(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlanningID(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportRemodelingID(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportRemodelingID(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> ImportPlaceID(Guid excelFileId, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ImportPlaceID(excelFileId, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<ImportErrorObject> UpdatePlaceID(Guid excelFileId, Guid modifyUserId)
        {
            try
            {
                return dataImportServiceImpl.UpdatePlaceID(excelFileId, modifyUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            dataImportServiceImpl.Dispose();
        }
    }
}
