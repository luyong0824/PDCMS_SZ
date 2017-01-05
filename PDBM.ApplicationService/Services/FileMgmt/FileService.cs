using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.FileMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.FileMgmt;
using PDBM.DataTransferObjects.BaseData;

namespace PDBM.ApplicationService.Services.FileMgmt
{
    /// <summary>
    /// 文件应用层服务
    /// </summary>
    public class FileService : DataService, IFileService
    {
        private const int bufferSize = 4096;
        private string baseUploadFolder = ConfigurationManager.AppSettings["baseUploadFolder"];
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<TowerLog> towerLogRepository;
        private readonly IRepository<TowerBaseLog> towerBaseLogRepository;
        private readonly IRepository<MachineRoomLog> machineRoomLogRepository;
        private readonly IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository;
        private readonly IRepository<EquipmentInstallLog> equipmentInstallLogRepository;
        private readonly IRepository<AddressExplorLog> addressExplorLogRepository;
        private readonly IRepository<FoundationTestLog> foundationTestLogRepository;
        private readonly IRepository<TaskProperty> taskPropertyRepository;
        private readonly IRepository<TaskPropertyLog> taskPropertyLogRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<PlanningApply> planningApplyRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository;

        public FileService(IRepositoryContext context,
            IRepository<File> fileRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<Purchase> purchaseRepository,
            IRepository<Place> placeRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<TowerLog> towerLogRepository,
            IRepository<TowerBaseLog> towerBaseLogRepository,
            IRepository<MachineRoomLog> machineRoomLogRepository,
            IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository,
            IRepository<EquipmentInstallLog> equipmentInstallLogRepository,
            IRepository<AddressExplorLog> addressExplorLogRepository,
            IRepository<FoundationTestLog> foundationTestLogRepository,
            IRepository<TaskProperty> taskPropertyRepository,
            IRepository<TaskPropertyLog> taskPropertyLogRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<PlanningApply> planningApplyRepository,
            IRepository<Planning> planningRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository)
            : base(context)
        {
            this.fileRepository = fileRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.purchaseRepository = purchaseRepository;
            this.placeRepository = placeRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.towerLogRepository = towerLogRepository;
            this.towerBaseLogRepository = towerBaseLogRepository;
            this.machineRoomLogRepository = machineRoomLogRepository;
            this.externalElectricPowerLogRepository = externalElectricPowerLogRepository;
            this.equipmentInstallLogRepository = equipmentInstallLogRepository;
            this.addressExplorLogRepository = addressExplorLogRepository;
            this.foundationTestLogRepository = foundationTestLogRepository;
            this.taskPropertyRepository = taskPropertyRepository;
            this.taskPropertyLogRepository = taskPropertyLogRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
            this.planningApplyRepository = planningApplyRepository;
            this.planningRepository = planningRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.blindSpotFeedBackRepository = blindSpotFeedBackRepository;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileUploadObject">文件上传对象</param>
        public void UploadFile(FileUploadObject fileUploadObject)
        {
            DateTime today = DateTime.Now;
            string uploadFolder = System.IO.Path.Combine(baseUploadFolder, string.Format("{0}-{1}-{2}", today.Year, today.Month, today.Day));
            FileHelper.CreateDirectory(uploadFolder);
            string filePath = System.IO.Path.Combine(uploadFolder, Guid.NewGuid() + fileUploadObject.FileExtension);
            FileHelper.UploadFile(fileUploadObject.FileData, filePath, bufferSize);
            File file = AggregateFactory.CreateFile(fileUploadObject.Id, fileUploadObject.FileName, fileUploadObject.FileType,
                fileUploadObject.FileExtension, fileUploadObject.FileSize, filePath, fileUploadObject.UploadUserId);
            fileRepository.Add(file);
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath">要下载的文件存储路径</param>
        /// <returns>要下载的文件流</returns>
        public System.IO.Stream DownloadFile(string filePath)
        {
            return FileHelper.FileToStream(filePath);
        }

        /// <summary>
        /// 根据文件Id获取要下载的文件对象
        /// </summary>
        /// <param name="fileId">文件Id</param>
        /// <returns>要下载的文件对象</returns>
        public FileDownloadObject GetFileDownloadByFileId(Guid fileId)
        {
            return MapperHelper.Map<File, FileDownloadObject>(fileRepository.GetByKey(fileId));
        }

        /// <summary>
        /// 根据文件Id列表获取文件列表
        /// </summary>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetFiles(string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (fileIdList != "")
            {
                string[] fileIds = fileIdList.Split(',');
                foreach (string fileId in fileIds)
                {
                    File file = fileRepository.GetByKey(Guid.Parse(fileId));
                    FileObject fileObject = new FileObject()
                    {
                        Id = file.Id,
                        WebUploaderFileId = "",
                        FileStatus = 2,
                        FileName = file.FileName,
                        FileSize = ConvertFileSize(file.FileSize),
                        UploadProgress = 100,
                        UploadStatus = "上传完成",
                        UploadDate = file.UploadDate
                    };
                    fileObjects.Add(fileObject);
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取盲点反馈附件
        /// </summary>
        /// <param name="planningId">盲点反馈Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetBlindSpotFeedBackFiles(Guid blindSpotFeedBackId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (blindSpotFeedBackId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(blindSpotFeedBackId);
                if (blindSpotFeedBack != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == blindSpotFeedBackId && entity.EntityName == "BlindSpotFeedBack"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的盲点反馈在系统中不存在");
                }
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取改造附件
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetRemodelingFiles(Guid remodelingId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (remodelingId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Remodeling remodeling = remodelingRepository.FindByKey(remodelingId);
                if (remodeling != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == remodelingId && entity.EntityName == "Remodeling"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的基站改造在系统中不存在");
                }
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取工程现场摄像附件
        /// </summary>
        /// <param name="engineeringTaskId">工程任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetEngineeringProgressFiles(Guid engineeringTaskId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(engineeringTaskId);
            if (engineeringTask != null)
            {
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringProgress"));
                if (fileAssociation != null && fileAssociation.FileIdList != "")
                {
                    string[] fileIds = fileAssociation.FileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取项目现场摄像附件
        /// </summary>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetProjectProgressFiles(Guid projectTaskId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            ProjectTask projectTask = projectTaskRepository.FindByKey(projectTaskId);
            if (projectTask != null)
            {
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                if (fileAssociation != null && fileAssociation.FileIdList != "")
                {
                    string[] fileIds = fileAssociation.FileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取施工图附件
        /// </summary>
        /// <param name="engineeringTaskId">工程任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetEngineeringDesignFiles(Guid engineeringTaskId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(engineeringTaskId);
            if (engineeringTask != null)
            {
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringDesign"));
                if (fileAssociation != null && fileAssociation.FileIdList != "")
                {
                    string[] fileIds = fileAssociation.FileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取总设图附件
        /// </summary>
        /// <param name="projectTaskId">项目任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetGeneralDesignFiles(Guid projectTaskId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            ProjectTask projectTask = projectTaskRepository.FindByKey(projectTaskId);
            if (projectTask != null)
            {
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (fileAssociation != null && fileAssociation.FileIdList != "")
                {
                    string[] fileIds = fileAssociation.FileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的建设申请在系统中不存在");
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取建设申请附件
        /// </summary>
        /// <param name="planningApplyId">建设申请Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetPlanningApplyFiles(Guid planningApplyId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            PlanningApply planningApply = planningApplyRepository.FindByKey(planningApplyId);
            if (planningApply != null)
            {
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planningApplyId && entity.EntityName == "PlanningApply"));
                if (fileAssociation != null && fileAssociation.FileIdList != "")
                {
                    string[] fileIds = fileAssociation.FileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的建设申请在系统中不存在");
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取规划附件
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetPlanningFiles(Guid planningId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (planningId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Planning planning = planningRepository.FindByKey(planningId);
                if (planning != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == planningId && entity.EntityName == "Planning"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的规划在系统中不存在");
                }
            }
            return fileObjects;
        }

        /// <summary>
        /// 获取寻址确认示意图
        /// </summary>
        /// <param name="addressingId">寻址确认Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetAddressingFiles(Guid addressingId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (addressingId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Addressing addressing = addressingRepository.FindByKey(addressingId);
                if (addressing != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressingId && entity.EntityName == "Addressing"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的寻址确认在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取购置站点示意图
        /// </summary>
        /// <param name="purchaseId">购置站点Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetPurchaseFiles(Guid purchaseId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (purchaseId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Purchase purchase = purchaseRepository.FindByKey(purchaseId);
                if (purchase != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == purchaseId && entity.EntityName == "Purchase"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的购置站点在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取站点示意图
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetPlaceFiles(Guid placeId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (placeId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Place place = placeRepository.FindByKey(placeId);
                if (place != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == placeId && entity.EntityName == "Place"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取工作流过程实例附件
        /// </summary>
        /// <param name="wfProcessInstanceId">工作流过程实例Id</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetWFProcessInstanceFiles(Guid wfProcessInstanceId)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == wfProcessInstanceId && entity.EntityName == "WFProcessInstance"));
            if (fileAssociation != null && fileAssociation.FileIdList != "")
            {
                string[] fileIds = fileAssociation.FileIdList.Split(',');
                foreach (string fileId in fileIds)
                {
                    File file = fileRepository.GetByKey(Guid.Parse(fileId));
                    FileObject fileObject = new FileObject()
                    {
                        Id = file.Id,
                        WebUploaderFileId = "",
                        FileStatus = 2,
                        FileName = file.FileName,
                        FileSize = ConvertFileSize(file.FileSize),
                        UploadProgress = 100,
                        UploadStatus = "上传完成",
                        UploadDate = file.UploadDate
                    };
                    fileObjects.Add(fileObject);
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取工作流活动实例附件
        /// </summary>
        /// <param name="wfActivityInstanceId">工作流活动实例Id</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetWFActivityInstanceFiles(Guid wfActivityInstanceId)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == wfActivityInstanceId && entity.EntityName == "WFActivityInstance"));
            if (fileAssociation != null && fileAssociation.FileIdList != "")
            {
                string[] fileIds = fileAssociation.FileIdList.Split(',');
                foreach (string fileId in fileIds)
                {
                    File file = fileRepository.GetByKey(Guid.Parse(fileId));
                    FileObject fileObject = new FileObject()
                    {
                        Id = file.Id,
                        WebUploaderFileId = "",
                        FileStatus = 2,
                        FileName = file.FileName,
                        FileSize = ConvertFileSize(file.FileSize),
                        UploadProgress = 100,
                        UploadStatus = "上传完成",
                        UploadDate = file.UploadDate
                    };
                    fileObjects.Add(fileObject);
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <param name="towerId">铁塔Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetTowerFiles(Guid towerId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (towerId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                Tower tower = towerRepository.FindByKey(towerId);
                if (tower != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == tower.Id && entity.EntityName == "Tower"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的铁塔在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <param name="towerBaseId">铁塔基础Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetTowerBaseFiles(Guid towerBaseId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (towerBaseId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseId);
                if (towerBase != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBase.Id && entity.EntityName == "TowerBase"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的铁塔基础在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <param name="machineRoomId">机房Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetMachineRoomFiles(Guid machineRoomId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (machineRoomId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomId);
                if (machineRoom != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoom.Id && entity.EntityName == "MachineRoom"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的机房在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <param name="externalElectricPowerId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetExternalElectricPowerFiles(Guid externalElectricPowerId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (externalElectricPowerId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerId);
                if (externalElectricPower != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPower.Id && entity.EntityName == "ExternalElectricPower"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的外电引入在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <param name="externalElectricPowerId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetEquipmentInstallFiles(Guid equipmentInstallId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (equipmentInstallId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallId);
                if (equipmentInstall != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstall.Id && entity.EntityName == "EquipmentInstall"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的设备安装在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <param name="addressExplorId">地质勘探Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetAddressExplorFiles(Guid addressExplorId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (addressExplorId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                AddressExplor addressExplor = addressExplorRepository.FindByKey(addressExplorId);
                if (addressExplor != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplor.Id && entity.EntityName == "AddressExplor"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的地质勘探在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <param name="foundationTestId">桩基动测Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetFoundationTestFiles(Guid foundationTestId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (foundationTestId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                FoundationTest foundationTest = foundationTestRepository.FindByKey(foundationTestId);
                if (foundationTest != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTest.Id && entity.EntityName == "FoundationTest"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的桩基动测在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <param name="towerLogId">铁塔Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetTowerLogFiles(Guid towerLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (towerLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                TowerLog towerLog = towerLogRepository.FindByKey(towerLogId);
                if (towerLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerLog.Id && entity.EntityName == "TowerLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的铁塔历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <param name="towerBaseLogId">铁塔基础Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetTowerBaseLogFiles(Guid towerBaseLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (towerBaseLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                TowerBaseLog towerBaseLog = towerBaseLogRepository.FindByKey(towerBaseLogId);
                if (towerBaseLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == towerBaseLog.Id && entity.EntityName == "TowerBaseLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的铁塔基础历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <param name="machineRoomLogId">机房Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetMachineRoomLogFiles(Guid machineRoomLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (machineRoomLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                MachineRoomLog machineRoomLog = machineRoomLogRepository.FindByKey(machineRoomLogId);
                if (machineRoomLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == machineRoomLog.Id && entity.EntityName == "MachineRoomLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的机房历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <param name="externalElectricPowerLogId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetExternalElectricPowerLogFiles(Guid externalElectricPowerLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (externalElectricPowerLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                ExternalElectricPowerLog externalElectricPowerLog = externalElectricPowerLogRepository.FindByKey(externalElectricPowerLogId);
                if (externalElectricPowerLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == externalElectricPowerLog.Id && entity.EntityName == "ExternalElectricPowerLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的外电引入历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <param name="equipmentInstallLogId">外电引入Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetEquipmentInstallLogFiles(Guid equipmentInstallLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (equipmentInstallLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                EquipmentInstallLog equipmentInstallLog = equipmentInstallLogRepository.FindByKey(equipmentInstallLogId);
                if (equipmentInstallLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == equipmentInstallLog.Id && entity.EntityName == "EquipmentInstallLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的设备安装历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <param name="addressExplorLogId">地质勘探Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetAddressExplorLogFiles(Guid addressExplorLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (addressExplorLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                AddressExplorLog addressExplorLog = addressExplorLogRepository.FindByKey(addressExplorLogId);
                if (addressExplorLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == addressExplorLog.Id && entity.EntityName == "AddressExplorLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的地质勘探历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <param name="foundationTestLogId">桩基动测Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetFoundationTestLogFiles(Guid foundationTestLogId, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (foundationTestLogId == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                FoundationTestLog foundationTestLog = foundationTestLogRepository.FindByKey(foundationTestLogId);
                if (foundationTestLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == foundationTestLog.Id && entity.EntityName == "FoundationTestLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的桩基动测历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取现场影像
        /// </summary>
        /// <param name="id">子任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetProgressFiles(Guid id, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (id == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                TaskProperty taskProperty = taskPropertyRepository.FindByKey(id);
                if (taskProperty != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == taskProperty.Id && entity.EntityName == "Progress"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的任务在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取现场影像历史记录
        /// </summary>
        /// <param name="id">子任务历史记录Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetProgressLogFiles(Guid id, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (id == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                TaskPropertyLog taskPropertyLog = taskPropertyLogRepository.FindByKey(id);
                if (taskPropertyLog != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == taskPropertyLog.Id && entity.EntityName == "ProgressLog"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的任务历史记录在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取隐患上报单附件
        /// </summary>
        /// <param name="id">隐患上报单Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetWorkApplyFiles(Guid id, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (id == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                WorkApply workApply = workApplyRepository.FindByKey(id);
                if (workApply != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workApply.Id && entity.EntityName == "WorkApply"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的隐患上报单在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取零星派工单附件
        /// </summary>
        /// <param name="id">子任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetWorkOrderFiles(Guid id, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (id == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                WorkOrder workOrder = workOrderRepository.FindByKey(id);
                if (workOrder != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrder"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的零星派工单在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 获取零星派工单执行说明附件
        /// </summary>
        /// <param name="id">子任务Id</param>
        /// <param name="fileIdList">文件Id列表</param>
        /// <returns>文件对象列表</returns>
        public IList<FileObject> GetWorkOrderWFFiles(Guid id, string fileIdList)
        {
            IList<FileObject> fileObjects = new List<FileObject>();
            if (id == Guid.Empty)
            {
                if (fileIdList != "")
                {
                    string[] fileIds = fileIdList.Split(',');
                    foreach (string fileId in fileIds)
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(fileId));
                        FileObject fileObject = new FileObject()
                        {
                            Id = file.Id,
                            WebUploaderFileId = "",
                            FileStatus = 2,
                            FileName = file.FileName,
                            FileSize = ConvertFileSize(file.FileSize),
                            UploadProgress = 100,
                            UploadStatus = "上传完成",
                            UploadDate = file.UploadDate
                        };
                        fileObjects.Add(fileObject);
                    }
                }
            }
            else
            {
                WorkOrder workOrder = workOrderRepository.FindByKey(id);
                if (workOrder != null)
                {
                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workOrder.Id && entity.EntityName == "WorkOrderWF"));
                    if (fileAssociation != null && fileAssociation.FileIdList != "")
                    {
                        string[] fileIds = fileAssociation.FileIdList.Split(',');
                        foreach (string fileId in fileIds)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileId));
                            FileObject fileObject = new FileObject()
                            {
                                Id = file.Id,
                                WebUploaderFileId = "",
                                FileStatus = 2,
                                FileName = file.FileName,
                                FileSize = ConvertFileSize(file.FileSize),
                                UploadProgress = 100,
                                UploadStatus = "上传完成",
                                UploadDate = file.UploadDate
                            };
                            fileObjects.Add(fileObject);
                        }
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的零星派工单在系统中不存在");
                }
            }

            return fileObjects;
        }

        /// <summary>
        /// 转换文件大小
        /// </summary>
        /// <param name="fileSize">文件字节数</param>
        /// <returns></returns>
        private string ConvertFileSize(long fileSize)
        {
            if (fileSize >= 1024000)
            {
                return Math.Round(fileSize / 1024000.0) + "MB";
            }
            else
            {
                return Math.Round(fileSize / 1024.0) + "KB";
            }
        }
    }
}
