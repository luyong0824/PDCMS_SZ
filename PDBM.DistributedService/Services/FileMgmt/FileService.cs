using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.FileMgmt;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.FileMgmt;
using PDBM.DataTransferObjects.BaseData;

namespace PDBM.DistributedService.Services.FileMgmt
{
    /// <summary>
    /// 文件分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class FileService : IFileService
    {
        private readonly IFileService fileServiceImpl = ServiceLocator.Instance.GetService<IFileService>();

        public void UploadFile(FileUploadObject fileUploadObject)
        {
            try
            {
                fileServiceImpl.UploadFile(fileUploadObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public Stream DownloadFile(string filePath)
        {
            try
            {
                return fileServiceImpl.DownloadFile(filePath);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public FileDownloadObject GetFileDownloadByFileId(Guid fileId)
        {
            try
            {
                return fileServiceImpl.GetFileDownloadByFileId(fileId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetFiles(string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetFiles(fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetBlindSpotFeedBackFiles(Guid blindSpotFeedBackId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetBlindSpotFeedBackFiles(blindSpotFeedBackId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetRemodelingFiles(Guid remodelingId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetRemodelingFiles(remodelingId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetEngineeringProgressFiles(Guid projectTaskId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetEngineeringProgressFiles(projectTaskId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetProjectProgressFiles(Guid projectTaskId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetProjectProgressFiles(projectTaskId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetEngineeringDesignFiles(Guid projectTaskId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetEngineeringDesignFiles(projectTaskId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetGeneralDesignFiles(Guid projectTaskId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetGeneralDesignFiles(projectTaskId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetPlanningApplyFiles(Guid planningApplyId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetPlanningApplyFiles(planningApplyId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetPlanningFiles(Guid planningId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetPlanningFiles(planningId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetAddressingFiles(Guid addressingId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetAddressingFiles(addressingId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetPurchaseFiles(Guid purchaseId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetPurchaseFiles(purchaseId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetPlaceFiles(Guid placeId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetPlaceFiles(placeId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetWFProcessInstanceFiles(Guid wfProcessInstanceId)
        {
            try
            {
                return fileServiceImpl.GetWFProcessInstanceFiles(wfProcessInstanceId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetWFActivityInstanceFiles(Guid wfActivityInstanceId)
        {
            try
            {
                return fileServiceImpl.GetWFActivityInstanceFiles(wfActivityInstanceId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetTowerFiles(Guid towerId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetTowerFiles(towerId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetTowerBaseFiles(Guid towerBaseId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetTowerBaseFiles(towerBaseId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetMachineRoomFiles(Guid machineRoomId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetMachineRoomFiles(machineRoomId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetExternalElectricPowerFiles(Guid externalElectricPowerId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetExternalElectricPowerFiles(externalElectricPowerId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetEquipmentInstallFiles(Guid equipmentInstallId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetEquipmentInstallFiles(equipmentInstallId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetAddressExplorFiles(Guid addressExplorId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetAddressExplorFiles(addressExplorId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetFoundationTestFiles(Guid foundationTestId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetFoundationTestFiles(foundationTestId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetTowerLogFiles(Guid towerLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetTowerLogFiles(towerLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetTowerBaseLogFiles(Guid towerBaseLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetTowerBaseLogFiles(towerBaseLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetMachineRoomLogFiles(Guid machineRoomLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetMachineRoomLogFiles(machineRoomLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetExternalElectricPowerLogFiles(Guid externalElectricPowerLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetExternalElectricPowerLogFiles(externalElectricPowerLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetEquipmentInstallLogFiles(Guid equipmentInstallLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetEquipmentInstallLogFiles(equipmentInstallLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetAddressExplorLogFiles(Guid addressExplorLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetAddressExplorLogFiles(addressExplorLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetFoundationTestLogFiles(Guid foundationTestLogId, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetFoundationTestLogFiles(foundationTestLogId, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetProgressFiles(Guid id, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetProgressFiles(id, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetProgressLogFiles(Guid id, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetProgressLogFiles(id, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetWorkApplyFiles(Guid id, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetWorkApplyFiles(id, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetWorkOrderFiles(Guid id, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetWorkOrderFiles(id, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<FileObject> GetWorkOrderWFFiles(Guid id, string fileIdList)
        {
            try
            {
                return fileServiceImpl.GetWorkOrderWFFiles(id, fileIdList);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            fileServiceImpl.Dispose();
        }
    }
}
