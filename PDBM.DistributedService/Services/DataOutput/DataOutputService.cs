using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.ServiceContracts.DataOutput;
using System.ServiceModel;
using PDBM.Infrastructure.IoC;
using PDBM.DataTransferObjects;

namespace PDBM.DistributedService.Services.DataOutput
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DataOutputService : IDataOutputService
    {
        private readonly IDataOutputService dataImportServiceImpl = ServiceLocator.Instance.GetService<IDataOutputService>();

        public string ExportAddressingMonthUserExcel(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportAddressingMonthUserExcel(beginDate, endDate, departmentId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportAddressingMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportAddressingMonthReseauExcel(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectTaskDepartmentExcel(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectTaskDepartmentExcel(beginDate, beginDateYear, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectTaskProjectManagerExcel(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectTaskProjectManagerExcel(beginDate, beginDateYear, departmentId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthCompanyTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthCompanyTVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthCompanyBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthCompanyBVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthAreaTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthAreaTVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthAreaBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthAreaBVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthReseauTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthReseauTVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeYearGrowthReseauBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeYearGrowthReseauBVExcel(beginDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeMonthCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeMonthCompanyExcel(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeMonthAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeMonthAreaExcel(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeMonthReseauExcel(beginDate, endDate, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeMonthPlaceExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeMonthPlaceExcel(beginDate, endDate, areaId, reseauId, placeName, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeCompanyExcel(beginDate, endDate, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeAreaExcel(beginDate, endDate, areaId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeReseauExcel(beginDate, endDate, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportBusinessVolumeExcel(DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportBusinessVolumeExcel(beginDate, endDate, placeName, areaId, reseauId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportLogicalBusinessVolumeExcel(DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportLogicalBusinessVolumeExcel(beginDate, endDate, logicalType, logicalNumber, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportPlaceAllExcel(int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state)
        {
            try
            {
                return dataImportServiceImpl.ExportPlaceAllExcel(profession, placeName, areaId, reseauId, placeOwner, state);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportEngineeringDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportEngineeringDesignReportExcel(projectCode, placeName, areaId, reseauId, taskModel, designRealName, designCustomerId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportEngineeringProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel, int engineeringProgress, int projectType,
            Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportEngineeringProgressReportExcel(projectCode, placeName, areaId, reseauId, taskModel, engineeringProgress, projectType, projectManagerId, constructionCustomerId, supervisionCustomerId, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId, string designRealName, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectDesignReportExcel(projectCode, placeName, areaId, reseauId, generalDesignId, designRealName, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectProgressReportExcel(projectCode, placeName, areaId, reseauId, projectType, projectProgress, projectManagerId, isOverTime, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportAddressingReportExcel(DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId,
            Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportAddressingReportExcel(beginDate, endDate, planningCode, planningName, profession, placeCategoryId, areaId, reseauId, importance, addressingState, addressingDepartmentId, addressingUserId, isAppoint, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportLogicalNumbersExcel(string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark)
        {
            try
            {
                return dataImportServiceImpl.ExportLogicalNumbersExcel(placeCode, placeName, profession, areaId, reseauId, g2Mark, d2Mark, g3Mark, g4Mark, g2Number, d2Number, g3Number, g4Number, allMark);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportPlacesBaseStationExcel(string placeCode, string placeName, Guid placeCategoryId, Guid placeOwner, Guid areaId, Guid reseauId, int importance, int state, int profession, Guid companyId)
        {
            try
            {
                return dataImportServiceImpl.ExportPlacesBaseStationExcel(placeCode, placeName, placeCategoryId, placeOwner, areaId, reseauId, importance, state, profession, companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportOperatorsPlanningsExcel()
        {
            try
            {
                return dataImportServiceImpl.ExportOperatorsPlanningsExcel();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportConstructionTaskPlanningsExcel()
        {
            try
            {
                return dataImportServiceImpl.ExportConstructionTaskPlanningsExcel();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportConstructionTaskRemodeingsExcel()
        {
            try
            {
                return dataImportServiceImpl.ExportConstructionTaskRemodeingsExcel();
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportMaterialPurchaseExcel(string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState)
        {
            try
            {
                return dataImportServiceImpl.ExportMaterialPurchaseExcel(placeName, placeCategoryId, areaId, reseauId, materialName, doState);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectInformationExcel(DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectInformationExcel(beginDate, endDate, propertyRightSql, groupPlaceCode, placeName, areaId, reseauId, constructionMethod, constructionProgress);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportWorkApplysExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ExportWorkApplysExcel(beginDate, endDate, title, reseauId, customerId, orderState, isSoved, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportWorkOrdersExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId)
        {
            try
            {
                return dataImportServiceImpl.ExportWorkOrdersExcel(beginDate, endDate, title, reseauId, workBigClassId, workSmallClassId, customerId, maintainContactMan, sendUserId, isFinish, orderState, createUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportWorkApplyProjectsExcel(DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId)
        {
            try
            {
                return dataImportServiceImpl.ExportWorkApplyProjectsExcel(beginDate, endDate, title, projectCode, isProject, sendUserId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string ExportProjectMaterial(DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode)
        {
            try
            {
                return dataImportServiceImpl.ExportProjectMaterial(beginDate, endDate, projectCode, projectType, placeName, reseauId, projectManagerId, customerName, materialSpecType, materialSpecName, orderCode);
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
