using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class ProjectTaskService : DataService, IProjectTaskService
    {
        private const int bufferSize = 4096;
        private string baseUploadFolder = ConfigurationManager.AppSettings["baseUploadFolder"];
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<ProjectTask> projectTaskRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Remodeling> remodelingRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;
        private readonly IRepository<WFActivityInstance> wfActivityInstanceRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository;
        private readonly ICodeSeedRepository codeSeedRepository;

        public ProjectTaskService(IRepositoryContext context,
            IRepository<File> fileRepository,
            IRepository<ProjectTask> projectTaskRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Remodeling> remodelingRepository,
            IRepository<Planning> planningRepository,
            IRepository<User> userRepository,
            IRepository<Department> departmentRepository,
            IRepository<Customer> customerRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository,
            IRepository<WFActivityInstance> wfActivityInstanceRepository,
            IRepository<Place> placeRepository,
            IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository,
            ICodeSeedRepository codeSeedRepository)
            : base(context)
        {
            this.fileRepository = fileRepository;
            this.projectTaskRepository = projectTaskRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.addressingRepository = addressingRepository;
            this.remodelingRepository = remodelingRepository;
            this.planningRepository = planningRepository;
            this.userRepository = userRepository;
            this.departmentRepository = departmentRepository;
            this.customerRepository = customerRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
            this.wfActivityInstanceRepository = wfActivityInstanceRepository;
            this.placeRepository = placeRepository;
            this.placeBusinessVolumeRepository = placeBusinessVolumeRepository;
            this.codeSeedRepository = codeSeedRepository;
        }

        /// <summary>
        /// 根据项目任务Id获取项目任务信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectTaskById(Guid id)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(id);
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                if (projectTask.AreaManagerId != Guid.Empty)
                {
                    User user = userRepository.GetByKey(projectTask.AreaManagerId);
                    projectTaskEditObject.AreaManagerName = user.FullName;
                }
                else
                {
                    projectTaskEditObject.AreaManagerName = "";
                }
                if (projectTask.GeneralDesignId != Guid.Empty)
                {
                    Customer customer = customerRepository.GetByKey(projectTask.GeneralDesignId);
                    projectTaskEditObject.GeneralDesignName = customer.CustomerName;
                }
                else
                {
                    projectTaskEditObject.GeneralDesignName = "";
                }

                projectTaskEditObject.Id = projectTask.Id;
                projectTaskEditObject.PlaceId = projectTask.PlaceId;
                projectTaskEditObject.DesignRealName = projectTask.DesignRealName;
                projectTaskEditObject.DesignDateText = projectTask.DesignDate.ToShortDateString();
                projectTaskEditObject.ProjectCode = projectTask.ProjectCode;
                projectTaskEditObject.ProjectProgressName = EnumHelper.GetEnumText(typeof(ProjectProgress), projectTask.ProjectProgress);
                projectTaskEditObject.ProjectBeginDateText = projectTask.ProjectBeginDate.ToShortDateString();
                projectTaskEditObject.ProjectDateText = projectTask.ProjectDate.ToShortDateString();
                projectTaskEditObject.ProgressMemos = projectTask.ProgressMemos;

                projectTaskEditObject.DesignCount = 0;
                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation != null)
                {
                    int count = 0;
                    if (generalDesignFileAssociation.FileIdList != "")
                    {
                        if (generalDesignFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = generalDesignFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.DesignCount = count;
                }
                else
                {
                    projectTaskEditObject.DesignCount = 0;
                }

                projectTaskEditObject.ImageCount = 0;
                FileAssociation imageFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                if (imageFileAssociation != null)
                {
                    int count = 0;
                    if (imageFileAssociation.FileIdList != "")
                    {
                        if (imageFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = imageFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.ImageCount = count;
                }
                else
                {
                    projectTaskEditObject.ImageCount = 0;
                }

                EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅 && entity.State == State.使用));
                if (et1 != null)
                {
                    projectTaskEditObject.Id1 = et1.Id;
                    projectTaskEditObject.Mark1 = 1;
                    if (et1.ProjectManagerId != Guid.Empty)
                    {
                        User pm1 = userRepository.GetByKey(et1.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName1 = pm1.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName1 = "";
                    }
                    if (et1.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc1 = customerRepository.GetByKey(et1.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName1 = dc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName1 = "";
                    }
                    if (et1.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc1 = customerRepository.GetByKey(et1.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName1 = cc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName1 = "";
                    }
                    if (et1.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc1 = customerRepository.GetByKey(et1.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName1 = sc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName1 = "";
                    }
                    projectTaskEditObject.DesignRealName1 = et1.DesignRealName;
                    projectTaskEditObject.DesignDateText1 = et1.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName1 = EnumHelper.GetEnumText(typeof(Bool), et1.DesignState);
                    projectTaskEditObject.EngineeringProgressName1 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et1.EngineeringProgress);
                    projectTaskEditObject.DesignMemos1 = et1.DesignMemos;
                    projectTaskEditObject.ProgressMemos1 = et1.ProgressMemos;

                    projectTaskEditObject.DesignCount1 = 0;
                    FileAssociation designFileAssociation1 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et1.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation1 != null)
                    {
                        int count = 0;
                        if (designFileAssociation1.FileIdList != "")
                        {
                            if (designFileAssociation1.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation1.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount1 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount1 = 0;
                    }

                    projectTaskEditObject.ImageCount1 = 0;
                    FileAssociation imageFileAssociation1 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et1.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation1 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation1.FileIdList != "")
                        {
                            if (imageFileAssociation1.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation1.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount1 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount1 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id1 = Guid.Empty;
                    projectTaskEditObject.Mark1 = 0;
                    projectTaskEditObject.ProjectManagerName1 = "";
                    projectTaskEditObject.DesignCustomerName1 = "";
                    projectTaskEditObject.ConstructionCustomerName1 = "";
                    projectTaskEditObject.SupervisionCustomerName1 = "";
                    projectTaskEditObject.DesignRealName1 = "";
                    projectTaskEditObject.DesignDateText1 = "";
                    projectTaskEditObject.DesignStateName1 = "";
                    projectTaskEditObject.EngineeringProgressName1 = "";
                    projectTaskEditObject.DesignMemos1 = "";
                    projectTaskEditObject.ProgressMemos1 = "";
                    projectTaskEditObject.DesignCount1 = 0;
                    projectTaskEditObject.ImageCount1 = 0;
                }
                EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础 && entity.State == State.使用));
                if (et2 != null)
                {
                    projectTaskEditObject.Id2 = et2.Id;
                    projectTaskEditObject.Mark2 = 1;
                    if (et2.ProjectManagerId != Guid.Empty)
                    {
                        User pm2 = userRepository.GetByKey(et2.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName2 = pm2.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName2 = "";
                    }
                    if (et2.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc2 = customerRepository.GetByKey(et2.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName2 = dc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName2 = "";
                    }
                    if (et2.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc2 = customerRepository.GetByKey(et2.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName2 = cc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName2 = "";
                    }
                    if (et2.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc2 = customerRepository.GetByKey(et2.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName2 = sc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName2 = "";
                    }
                    projectTaskEditObject.DesignRealName2 = et2.DesignRealName;
                    projectTaskEditObject.DesignDateText2 = et2.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName2 = EnumHelper.GetEnumText(typeof(Bool), et2.DesignState);
                    projectTaskEditObject.EngineeringProgressName2 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et2.EngineeringProgress);
                    projectTaskEditObject.DesignMemos2 = et2.DesignMemos;
                    projectTaskEditObject.ProgressMemos2 = et2.ProgressMemos;

                    projectTaskEditObject.DesignCount2 = 0;
                    FileAssociation designFileAssociation2 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et2.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation2 != null)
                    {
                        int count = 0;
                        if (designFileAssociation2.FileIdList != "")
                        {
                            if (designFileAssociation2.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation2.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount2 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount2 = 0;
                    }

                    projectTaskEditObject.ImageCount2 = 0;
                    FileAssociation imageFileAssociation2 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et2.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation2 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation2.FileIdList != "")
                        {
                            if (imageFileAssociation2.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation2.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount2 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount2 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id2 = Guid.Empty;
                    projectTaskEditObject.Mark2 = 0;
                    projectTaskEditObject.ProjectManagerName2 = "";
                    projectTaskEditObject.DesignCustomerName2 = "";
                    projectTaskEditObject.ConstructionCustomerName2 = "";
                    projectTaskEditObject.SupervisionCustomerName2 = "";
                    projectTaskEditObject.DesignRealName2 = "";
                    projectTaskEditObject.DesignDateText2 = "";
                    projectTaskEditObject.DesignStateName2 = "";
                    projectTaskEditObject.EngineeringProgressName2 = "";
                    projectTaskEditObject.DesignMemos2 = "";
                    projectTaskEditObject.ProgressMemos2 = "";
                    projectTaskEditObject.DesignCount2 = 0;
                    projectTaskEditObject.ImageCount2 = 0;
                }
                EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房 && entity.State == State.使用));
                if (et3 != null)
                {
                    projectTaskEditObject.Id3 = et3.Id;
                    projectTaskEditObject.Mark3 = 1;
                    if (et3.ProjectManagerId != Guid.Empty)
                    {
                        User pm3 = userRepository.GetByKey(et3.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName3 = pm3.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName3 = "";
                    }
                    if (et3.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc3 = customerRepository.GetByKey(et3.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName3 = dc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName3 = "";
                    }
                    if (et3.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc3 = customerRepository.GetByKey(et3.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName3 = cc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName3 = "";
                    }
                    if (et3.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc3 = customerRepository.GetByKey(et3.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName3 = sc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName3 = "";
                    }
                    projectTaskEditObject.DesignRealName3 = et3.DesignRealName;
                    projectTaskEditObject.DesignDateText3 = et3.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName3 = EnumHelper.GetEnumText(typeof(Bool), et3.DesignState);
                    projectTaskEditObject.EngineeringProgressName3 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et3.EngineeringProgress);
                    projectTaskEditObject.DesignMemos3 = et3.DesignMemos;
                    projectTaskEditObject.ProgressMemos3 = et3.ProgressMemos;

                    projectTaskEditObject.DesignCount3 = 0;
                    FileAssociation designFileAssociation3 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et3.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation3 != null)
                    {
                        int count = 0;
                        if (designFileAssociation3.FileIdList != "")
                        {
                            if (designFileAssociation3.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation3.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount3 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount3 = 0;
                    }

                    projectTaskEditObject.ImageCount3 = 0;
                    FileAssociation imageFileAssociation3 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et3.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation3 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation3.FileIdList != "")
                        {
                            if (imageFileAssociation3.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation3.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount3 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount3 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id3 = Guid.Empty;
                    projectTaskEditObject.Mark3 = 0;
                    projectTaskEditObject.ProjectManagerName3 = "";
                    projectTaskEditObject.DesignCustomerName3 = "";
                    projectTaskEditObject.ConstructionCustomerName3 = "";
                    projectTaskEditObject.SupervisionCustomerName3 = "";
                    projectTaskEditObject.DesignRealName3 = "";
                    projectTaskEditObject.DesignDateText3 = "";
                    projectTaskEditObject.DesignStateName3 = "";
                    projectTaskEditObject.EngineeringProgressName3 = "";
                    projectTaskEditObject.DesignMemos3 = "";
                    projectTaskEditObject.ProgressMemos3 = "";
                    projectTaskEditObject.DesignCount3 = 0;
                    projectTaskEditObject.ImageCount3 = 0;
                }
                EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入 && entity.State == State.使用));
                if (et4 != null)
                {
                    projectTaskEditObject.Id4 = et4.Id;
                    projectTaskEditObject.Mark4 = 1;
                    if (et4.ProjectManagerId != Guid.Empty)
                    {
                        User pm4 = userRepository.GetByKey(et4.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName4 = pm4.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName4 = "";
                    }
                    if (et4.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc4 = customerRepository.GetByKey(et4.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName4 = dc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName4 = "";
                    }
                    if (et4.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc4 = customerRepository.GetByKey(et4.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName4 = cc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName4 = "";
                    }
                    if (et4.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc4 = customerRepository.GetByKey(et4.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName4 = sc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName4 = "";
                    }
                    projectTaskEditObject.DesignRealName4 = et4.DesignRealName;
                    projectTaskEditObject.DesignDateText4 = et4.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName4 = EnumHelper.GetEnumText(typeof(Bool), et4.DesignState);
                    projectTaskEditObject.EngineeringProgressName4 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et4.EngineeringProgress);
                    projectTaskEditObject.DesignMemos4 = et4.DesignMemos;
                    projectTaskEditObject.ProgressMemos4 = et4.ProgressMemos;

                    projectTaskEditObject.DesignCount4 = 0;
                    FileAssociation designFileAssociation4 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et4.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation4 != null)
                    {
                        int count = 0;
                        if (designFileAssociation4.FileIdList != "")
                        {
                            if (designFileAssociation4.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation4.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount4 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount4 = 0;
                    }

                    projectTaskEditObject.ImageCount4 = 0;
                    FileAssociation imageFileAssociation4 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et4.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation4 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation4.FileIdList != "")
                        {
                            if (imageFileAssociation4.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation4.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount4 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount4 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id4 = Guid.Empty;
                    projectTaskEditObject.Mark4 = 0;
                    projectTaskEditObject.ProjectManagerName4 = "";
                    projectTaskEditObject.DesignCustomerName4 = "";
                    projectTaskEditObject.ConstructionCustomerName4 = "";
                    projectTaskEditObject.SupervisionCustomerName4 = "";
                    projectTaskEditObject.DesignRealName4 = "";
                    projectTaskEditObject.DesignDateText4 = "";
                    projectTaskEditObject.DesignStateName4 = "";
                    projectTaskEditObject.EngineeringProgressName4 = "";
                    projectTaskEditObject.DesignMemos4 = "";
                    projectTaskEditObject.ProgressMemos4 = "";
                    projectTaskEditObject.DesignCount4 = 0;
                    projectTaskEditObject.ImageCount4 = 0;
                }
                EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                if (et5 != null)
                {
                    projectTaskEditObject.Id5 = et5.Id;
                    projectTaskEditObject.Mark5 = 1;
                    if (et5.ProjectManagerId != Guid.Empty)
                    {
                        User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName5 = "";
                    }
                    if (et5.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName5 = "";
                    }
                    if (et5.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName5 = "";
                    }
                    if (et5.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName5 = "";
                    }
                    projectTaskEditObject.DesignRealName5 = et5.DesignRealName;
                    projectTaskEditObject.DesignDateText5 = et5.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName5 = EnumHelper.GetEnumText(typeof(Bool), et5.DesignState);
                    projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                    projectTaskEditObject.DesignMemos5 = et5.DesignMemos;
                    projectTaskEditObject.ProgressMemos5 = et5.ProgressMemos;

                    projectTaskEditObject.DesignCount5 = 0;
                    FileAssociation designFileAssociation5 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et5.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation5 != null)
                    {
                        int count = 0;
                        if (designFileAssociation5.FileIdList != "")
                        {
                            if (designFileAssociation5.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation5.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount5 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount5 = 0;
                    }

                    projectTaskEditObject.ImageCount5 = 0;
                    FileAssociation imageFileAssociation5 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et5.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation5 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation5.FileIdList != "")
                        {
                            if (imageFileAssociation5.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation5.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount5 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount5 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id5 = Guid.Empty;
                    projectTaskEditObject.Mark5 = 0;
                    projectTaskEditObject.ProjectManagerName5 = "";
                    projectTaskEditObject.DesignCustomerName5 = "";
                    projectTaskEditObject.ConstructionCustomerName5 = "";
                    projectTaskEditObject.SupervisionCustomerName5 = "";
                    projectTaskEditObject.DesignRealName5 = "";
                    projectTaskEditObject.DesignDateText5 = "";
                    projectTaskEditObject.DesignStateName5 = "";
                    projectTaskEditObject.EngineeringProgressName5 = "";
                    projectTaskEditObject.DesignMemos5 = "";
                    projectTaskEditObject.ProgressMemos5 = "";
                    projectTaskEditObject.DesignCount5 = 0;
                    projectTaskEditObject.ImageCount5 = 0;
                }
                EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                if (et6 != null)
                {
                    projectTaskEditObject.Id6 = et6.Id;
                    projectTaskEditObject.Mark6 = 1;
                    if (et6.ProjectManagerId != Guid.Empty)
                    {
                        User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName6 = "";
                    }
                    if (et6.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName6 = "";
                    }
                    if (et6.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName6 = "";
                    }
                    if (et6.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName6 = "";
                    }
                    projectTaskEditObject.DesignRealName6 = et6.DesignRealName;
                    projectTaskEditObject.DesignDateText6 = et6.DesignDate.ToShortDateString();
                    projectTaskEditObject.DesignStateName6 = EnumHelper.GetEnumText(typeof(Bool), et6.DesignState);
                    projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                    projectTaskEditObject.DesignMemos6 = et6.DesignMemos;
                    projectTaskEditObject.ProgressMemos6 = et6.ProgressMemos;

                    projectTaskEditObject.DesignCount6 = 0;
                    FileAssociation designFileAssociation6 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et6.Id && entity.EntityName == "EngineeringDesign"));
                    if (designFileAssociation6 != null)
                    {
                        int count = 0;
                        if (designFileAssociation6.FileIdList != "")
                        {
                            if (designFileAssociation6.FileIdList.Contains(","))
                            {
                                string[] strFileList = designFileAssociation6.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.DesignCount6 = count;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCount6 = 0;
                    }

                    projectTaskEditObject.ImageCount6 = 0;
                    FileAssociation imageFileAssociation6 = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == et6.Id && entity.EntityName == "EngineeringProgress"));
                    if (imageFileAssociation6 != null)
                    {
                        int count = 0;
                        if (imageFileAssociation6.FileIdList != "")
                        {
                            if (imageFileAssociation6.FileIdList.Contains(","))
                            {
                                string[] strFileList = imageFileAssociation6.FileIdList.Split(',');
                                foreach (string i in strFileList)
                                {
                                    count += 1;
                                }
                            }
                            else
                            {
                                count = 1;
                            }
                        }
                        projectTaskEditObject.ImageCount6 = count;
                    }
                    else
                    {
                        projectTaskEditObject.ImageCount6 = 0;
                    }
                }
                else
                {
                    projectTaskEditObject.Id6 = Guid.Empty;
                    projectTaskEditObject.Mark6 = 0;
                    projectTaskEditObject.ProjectManagerName6 = "";
                    projectTaskEditObject.DesignCustomerName6 = "";
                    projectTaskEditObject.ConstructionCustomerName6 = "";
                    projectTaskEditObject.SupervisionCustomerName6 = "";
                    projectTaskEditObject.DesignRealName6 = "";
                    projectTaskEditObject.DesignDateText6 = "";
                    projectTaskEditObject.DesignStateName6 = "";
                    projectTaskEditObject.EngineeringProgressName6 = "";
                    projectTaskEditObject.DesignMemos6 = "";
                    projectTaskEditObject.ProgressMemos6 = "";
                    projectTaskEditObject.DesignCount6 = 0;
                    projectTaskEditObject.ImageCount6 = 0;
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 根据寻址确认Id获取项目任务信息
        /// </summary>
        /// <param name="id">寻址确认Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectTaskEditById(Guid id)
        {
            Addressing addressing = addressingRepository.GetByKey(id);
            Planning planning = planningRepository.GetByKey(addressing.PlanningId);
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                if (projectTask.AreaManagerId != Guid.Empty)
                {
                    User user = userRepository.GetByKey(projectTask.AreaManagerId);
                    projectTaskEditObject.AreaManagerName = user.FullName;
                }
                else
                {
                    projectTaskEditObject.AreaManagerName = "请选择";
                }
                if (projectTask.GeneralDesignId != Guid.Empty)
                {
                    Customer customer = customerRepository.GetByKey(projectTask.GeneralDesignId);
                    projectTaskEditObject.GeneralDesignName = customer.CustomerName;
                }
                else
                {
                    projectTaskEditObject.GeneralDesignName = "请选择";
                }

                projectTaskEditObject.ProjectProgressName = EnumHelper.GetEnumText(typeof(ProjectProgress), projectTask.ProjectProgress);
                projectTaskEditObject.ProjectDateText = projectTask.ProjectDate.ToShortDateString();
                projectTaskEditObject.ProjectCode = projectTask.ProjectCode;
                projectTaskEditObject.DesignRealName = projectTask.DesignRealName;
                projectTaskEditObject.ProjectBeginDateText = projectTask.ProjectBeginDate.ToShortDateString();

                if (planning.Profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅 && entity.State == State.使用));
                    if (et1 != null)
                    {
                        projectTaskEditObject.Id1 = et1.Id;
                        projectTaskEditObject.Mark1 = 1;
                        projectTaskEditObject.ProjectManagerId1 = et1.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId1 = et1.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId1 = et1.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId1 = et1.SupervisionCustomerId;
                        if (et1.ProjectManagerId != Guid.Empty)
                        {
                            User pm1 = userRepository.GetByKey(et1.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName1 = pm1.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName1 = "请选择";
                        }
                        if (et1.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc1 = customerRepository.GetByKey(et1.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName1 = dc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName1 = "请选择";
                        }
                        if (et1.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc1 = customerRepository.GetByKey(et1.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName1 = cc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                        }
                        if (et1.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc1 = customerRepository.GetByKey(et1.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName1 = sc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName1 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et1.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id1 = Guid.Empty;
                        projectTaskEditObject.Mark1 = 0;
                        projectTaskEditObject.ProjectManagerId1 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId1 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId1 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId1 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName1 = "请选择";
                        projectTaskEditObject.DesignCustomerName1 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                        projectTaskEditObject.EngineeringProgressName1 = "";
                    }
                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础 && entity.State == State.使用));
                    if (et2 != null)
                    {
                        projectTaskEditObject.Id2 = et2.Id;
                        projectTaskEditObject.Mark2 = 1;
                        projectTaskEditObject.ProjectManagerId2 = et2.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId2 = et2.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId2 = et2.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId2 = et2.SupervisionCustomerId;
                        if (et2.ProjectManagerId != Guid.Empty)
                        {
                            User pm2 = userRepository.GetByKey(et2.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName2 = pm2.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName2 = "请选择";
                        }
                        if (et2.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc2 = customerRepository.GetByKey(et2.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName2 = dc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName2 = "请选择";
                        }
                        if (et2.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc2 = customerRepository.GetByKey(et2.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName2 = cc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                        }
                        if (et2.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc2 = customerRepository.GetByKey(et2.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName2 = sc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName2 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et2.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id2 = Guid.Empty;
                        projectTaskEditObject.Mark2 = 0;
                        projectTaskEditObject.ProjectManagerId2 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId2 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId2 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId2 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName2 = "请选择";
                        projectTaskEditObject.DesignCustomerName2 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                        projectTaskEditObject.EngineeringProgressName2 = "";
                    }
                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房 && entity.State == State.使用));
                    if (et3 != null)
                    {
                        projectTaskEditObject.Id3 = et3.Id;
                        projectTaskEditObject.Mark3 = 1;
                        projectTaskEditObject.ProjectManagerId3 = et3.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId3 = et3.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId3 = et3.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId3 = et3.SupervisionCustomerId;
                        if (et3.ProjectManagerId != Guid.Empty)
                        {
                            User pm3 = userRepository.GetByKey(et3.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName3 = pm3.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName3 = "请选择";
                        }
                        if (et3.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc3 = customerRepository.GetByKey(et3.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName3 = dc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName3 = "请选择";
                        }
                        if (et3.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc3 = customerRepository.GetByKey(et3.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName3 = cc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                        }
                        if (et3.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc3 = customerRepository.GetByKey(et3.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName3 = sc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName3 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et3.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id3 = Guid.Empty;
                        projectTaskEditObject.Mark3 = 0;
                        projectTaskEditObject.ProjectManagerId3 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId3 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId3 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId3 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName3 = "请选择";
                        projectTaskEditObject.DesignCustomerName3 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                        projectTaskEditObject.EngineeringProgressName3 = "";
                    }
                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入 && entity.State == State.使用));
                    if (et4 != null)
                    {
                        projectTaskEditObject.Id4 = et4.Id;
                        projectTaskEditObject.Mark4 = 1;
                        projectTaskEditObject.ProjectManagerId4 = et4.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId4 = et4.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId4 = et4.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId4 = et4.SupervisionCustomerId;
                        if (et4.ProjectManagerId != Guid.Empty)
                        {
                            User pm4 = userRepository.GetByKey(et4.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName4 = pm4.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName4 = "请选择";
                        }
                        if (et4.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc4 = customerRepository.GetByKey(et4.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName4 = dc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName4 = "请选择";
                        }
                        if (et4.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc4 = customerRepository.GetByKey(et4.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName4 = cc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                        }
                        if (et4.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc4 = customerRepository.GetByKey(et4.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName4 = sc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName4 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et4.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id4 = Guid.Empty;
                        projectTaskEditObject.Mark4 = 0;
                        projectTaskEditObject.ProjectManagerId4 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId4 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId4 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId4 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName4 = "请选择";
                        projectTaskEditObject.DesignCustomerName4 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                        projectTaskEditObject.EngineeringProgressName4 = "";
                    }
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                    if (et5 != null)
                    {
                        projectTaskEditObject.Id5 = et5.Id;
                        projectTaskEditObject.Mark5 = 1;
                        projectTaskEditObject.ProjectManagerId5 = et5.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId5 = et5.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId5 = et5.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId5 = et5.SupervisionCustomerId;
                        if (et5.ProjectManagerId != Guid.Empty)
                        {
                            User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName5 = "请选择";
                        }
                        if (et5.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName5 = "请选择";
                        }
                        if (et5.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        }
                        if (et5.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id5 = Guid.Empty;
                        projectTaskEditObject.Mark5 = 0;
                        projectTaskEditObject.ProjectManagerId5 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName5 = "请选择";
                        projectTaskEditObject.DesignCustomerName5 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        projectTaskEditObject.EngineeringProgressName5 = "";
                    }
                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                    if (et6 != null)
                    {
                        projectTaskEditObject.Id6 = et6.Id;
                        projectTaskEditObject.Mark6 = 1;
                        projectTaskEditObject.ProjectManagerId6 = et6.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId6 = et6.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId6 = et6.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId6 = et6.SupervisionCustomerId;
                        if (et6.ProjectManagerId != Guid.Empty)
                        {
                            User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName6 = "请选择";
                        }
                        if (et6.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName6 = "请选择";
                        }
                        if (et6.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        }
                        if (et6.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id6 = Guid.Empty;
                        projectTaskEditObject.Mark6 = 0;
                        projectTaskEditObject.ProjectManagerId6 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName6 = "请选择";
                        projectTaskEditObject.DesignCustomerName6 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        projectTaskEditObject.EngineeringProgressName6 = "";
                    }
                }
                else if (planning.Profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                    if (et5 != null)
                    {
                        projectTaskEditObject.Id5 = et5.Id;
                        projectTaskEditObject.Mark5 = 1;
                        projectTaskEditObject.ProjectManagerId5 = et5.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId5 = et5.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId5 = et5.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId5 = et5.SupervisionCustomerId;
                        if (et5.ProjectManagerId != Guid.Empty)
                        {
                            User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName5 = "请选择";
                        }
                        if (et5.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName5 = "请选择";
                        }
                        if (et5.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        }
                        if (et5.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id5 = Guid.Empty;
                        projectTaskEditObject.Mark5 = 0;
                        projectTaskEditObject.ProjectManagerId5 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName5 = "请选择";
                        projectTaskEditObject.DesignCustomerName5 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        projectTaskEditObject.EngineeringProgressName5 = "";
                    }
                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                    if (et6 != null)
                    {
                        projectTaskEditObject.Id6 = et6.Id;
                        projectTaskEditObject.Mark6 = 1;
                        projectTaskEditObject.ProjectManagerId6 = et6.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId6 = et6.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId6 = et6.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId6 = et6.SupervisionCustomerId;
                        if (et6.ProjectManagerId != Guid.Empty)
                        {
                            User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName6 = "请选择";
                        }
                        if (et6.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName6 = "请选择";
                        }
                        if (et6.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        }
                        if (et6.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id6 = Guid.Empty;
                        projectTaskEditObject.Mark6 = 0;
                        projectTaskEditObject.ProjectManagerId6 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName6 = "请选择";
                        projectTaskEditObject.DesignCustomerName6 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        projectTaskEditObject.EngineeringProgressName6 = "";
                    }
                }

                projectTaskEditObject.Id = projectTask.Id;
                projectTaskEditObject.DesignDateText = projectTask.DesignDate.ToShortDateString();
                projectTaskEditObject.DesignFileIdList = "";
                projectTaskEditObject.DesignCount = 0;
                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation != null)
                {
                    int count = 0;
                    if (generalDesignFileAssociation.FileIdList != "")
                    {
                        if (generalDesignFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = generalDesignFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.DesignCount = count;
                    projectTaskEditObject.DesignFileIdList = generalDesignFileAssociation.FileIdList;
                }
                else
                {
                    projectTaskEditObject.DesignCount = 0;
                    projectTaskEditObject.DesignFileIdList = "";
                }

                if (planning.AddressingState == AddressingState.已寻址确认 && planning.PlaceId != Guid.Empty)
                {
                    Place place = placeRepository.GetByKey(planning.PlaceId);
                    projectTaskEditObject.G2Number = place.G2Number;
                    projectTaskEditObject.D2Number = place.D2Number;
                    projectTaskEditObject.G3Number = place.G3Number;
                    projectTaskEditObject.G4Number = place.G4Number;
                    projectTaskEditObject.G5Number = place.G5Number;
                }
                else
                {
                    projectTaskEditObject.G2Number = "";
                    projectTaskEditObject.D2Number = "";
                    projectTaskEditObject.G3Number = "";
                    projectTaskEditObject.G4Number = "";
                    projectTaskEditObject.G5Number = "";
                }

                projectTaskEditObject.WFActivityInstancesInfoHtml = "";
                if (addressing.OrderCode != "")
                {
                    List<Parameter> parameters = new List<Parameter>(1);
                    parameters.Add(new Parameter() { Name = "WFProcessInstanceCode", Type = SqlDbType.NVarChar, Value = addressing.OrderCode });
                    using (var ds = SqlHelper.ExecuteDataSet("prc_GetWFActivityInstancesInfo", parameters))
                    {
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<table class='table' cellpadding='0' cellspacing='0' style='margin:auto;'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:800px;' colspan='7'>发送人：" + ds.Tables[0].Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;发送日期：" + DateTime.Parse(ds.Tables[0].Rows[0][1].ToString()).ToShortDateString() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td style='width:60px;'>步骤顺序</td>");
                            sb.Append("<td style='width:150px;'>步骤名称</td>");
                            sb.Append("<td style='width:200px;'>用户</td>");
                            sb.Append("<td style='width:60px;'>操作类型</td>");
                            sb.Append("<td style='width:60px;'>操作结果</td>");
                            sb.Append("<td style='width:190px;'>内容</td>");
                            sb.Append("<td style='width:80px;'>操作日期</td>");
                            sb.Append("</tr>");
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                sb.Append("<tr>");
                                sb.Append("<td>" + dr["SerialId"].ToString() + "</td>");
                                sb.Append("<td>" + dr["WFActivityInstanceName"].ToString() + "</td>");
                                sb.Append("<td>" + dr["FullName"].ToString() + "</td>");
                                sb.Append("<td>" + EnumHelper.GetEnumText(typeof(WFActivityOperate), int.Parse(dr["WFActivityOperate"].ToString())) + "</td>");
                                sb.Append("<td>" + EnumHelper.GetEnumText(typeof(WFActivityInstanceResult), int.Parse(dr["WFActivityInstanceResult"].ToString())) + "</td>");
                                sb.Append("<td>" + dr["Content"].ToString() + "</td>");
                                if (dr["WFActivityInstanceResult"].ToString() != "0")
                                {
                                    sb.Append("<td>" + DateTime.Parse(dr["OperateDate"].ToString()).ToShortDateString() + "</td>");
                                }
                                else
                                {
                                    sb.Append("<td></td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("</table>");
                            projectTaskEditObject.WFActivityInstancesInfoHtml = sb.ToString();
                        }
                    }
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 根据改造确认Id获取项目任务信息
        /// </summary>
        /// <param name="id">改造确认Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectTaskEditByRemodelingId(Guid id)
        {
            Remodeling remodeling = remodelingRepository.GetByKey(id);
            Place place = placeRepository.GetByKey(remodeling.PlaceId);
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                if (projectTask.AreaManagerId != Guid.Empty)
                {
                    User user = userRepository.GetByKey(projectTask.AreaManagerId);
                    projectTaskEditObject.AreaManagerName = user.FullName;
                }
                else
                {
                    projectTaskEditObject.AreaManagerName = "请选择";
                }
                if (projectTask.GeneralDesignId != Guid.Empty)
                {
                    Customer customer = customerRepository.GetByKey(projectTask.GeneralDesignId);
                    projectTaskEditObject.GeneralDesignName = customer.CustomerName;
                }
                else
                {
                    projectTaskEditObject.GeneralDesignName = "请选择";
                }

                projectTaskEditObject.ProjectProgressName = EnumHelper.GetEnumText(typeof(ProjectProgress), projectTask.ProjectProgress);
                projectTaskEditObject.ProjectDateText = projectTask.ProjectDate.ToShortDateString();
                projectTaskEditObject.ProjectCode = projectTask.ProjectCode;
                projectTaskEditObject.DesignRealName = projectTask.DesignRealName;
                projectTaskEditObject.ProjectBeginDateText = projectTask.ProjectBeginDate.ToShortDateString();
                projectTaskEditObject.ProjectTypeName = EnumHelper.GetEnumText(typeof(ProjectType), projectTask.ProjectType);

                if (remodeling.Profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅 && entity.State == State.使用));
                    if (et1 != null)
                    {
                        projectTaskEditObject.Id1 = et1.Id;
                        projectTaskEditObject.Mark1 = 1;
                        projectTaskEditObject.ProjectManagerId1 = et1.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId1 = et1.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId1 = et1.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId1 = et1.SupervisionCustomerId;
                        if (et1.ProjectManagerId != Guid.Empty)
                        {
                            User pm1 = userRepository.GetByKey(et1.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName1 = pm1.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName1 = "请选择";
                        }
                        if (et1.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc1 = customerRepository.GetByKey(et1.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName1 = dc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName1 = "请选择";
                        }
                        if (et1.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc1 = customerRepository.GetByKey(et1.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName1 = cc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                        }
                        if (et1.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc1 = customerRepository.GetByKey(et1.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName1 = sc1.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName1 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et1.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id1 = Guid.Empty;
                        projectTaskEditObject.Mark1 = 0;
                        projectTaskEditObject.ProjectManagerId1 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId1 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId1 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId1 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName1 = "请选择";
                        projectTaskEditObject.DesignCustomerName1 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                        projectTaskEditObject.EngineeringProgressName1 = "";
                    }
                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础 && entity.State == State.使用));
                    if (et2 != null)
                    {
                        projectTaskEditObject.Id2 = et2.Id;
                        projectTaskEditObject.Mark2 = 1;
                        projectTaskEditObject.ProjectManagerId2 = et2.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId2 = et2.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId2 = et2.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId2 = et2.SupervisionCustomerId;
                        if (et2.ProjectManagerId != Guid.Empty)
                        {
                            User pm2 = userRepository.GetByKey(et2.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName2 = pm2.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName2 = "请选择";
                        }
                        if (et2.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc2 = customerRepository.GetByKey(et2.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName2 = dc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName2 = "请选择";
                        }
                        if (et2.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc2 = customerRepository.GetByKey(et2.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName2 = cc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                        }
                        if (et2.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc2 = customerRepository.GetByKey(et2.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName2 = sc2.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName2 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et2.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id2 = Guid.Empty;
                        projectTaskEditObject.Mark2 = 0;
                        projectTaskEditObject.ProjectManagerId2 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId2 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId2 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId2 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName2 = "请选择";
                        projectTaskEditObject.DesignCustomerName2 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                        projectTaskEditObject.EngineeringProgressName2 = "";
                    }
                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房 && entity.State == State.使用));
                    if (et3 != null)
                    {
                        projectTaskEditObject.Id3 = et3.Id;
                        projectTaskEditObject.Mark3 = 1;
                        projectTaskEditObject.ProjectManagerId3 = et3.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId3 = et3.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId3 = et3.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId3 = et3.SupervisionCustomerId;
                        if (et3.ProjectManagerId != Guid.Empty)
                        {
                            User pm3 = userRepository.GetByKey(et3.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName3 = pm3.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName3 = "请选择";
                        }
                        if (et3.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc3 = customerRepository.GetByKey(et3.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName3 = dc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName3 = "请选择";
                        }
                        if (et3.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc3 = customerRepository.GetByKey(et3.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName3 = cc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                        }
                        if (et3.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc3 = customerRepository.GetByKey(et3.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName3 = sc3.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName3 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et3.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id3 = Guid.Empty;
                        projectTaskEditObject.Mark3 = 0;
                        projectTaskEditObject.ProjectManagerId3 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId3 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId3 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId3 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName3 = "请选择";
                        projectTaskEditObject.DesignCustomerName3 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                        projectTaskEditObject.EngineeringProgressName3 = "";
                    }
                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入 && entity.State == State.使用));
                    if (et4 != null)
                    {
                        projectTaskEditObject.Id4 = et4.Id;
                        projectTaskEditObject.Mark4 = 1;
                        projectTaskEditObject.ProjectManagerId4 = et4.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId4 = et4.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId4 = et4.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId4 = et4.SupervisionCustomerId;
                        if (et4.ProjectManagerId != Guid.Empty)
                        {
                            User pm4 = userRepository.GetByKey(et4.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName4 = pm4.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName4 = "请选择";
                        }
                        if (et4.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc4 = customerRepository.GetByKey(et4.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName4 = dc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName4 = "请选择";
                        }
                        if (et4.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc4 = customerRepository.GetByKey(et4.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName4 = cc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                        }
                        if (et4.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc4 = customerRepository.GetByKey(et4.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName4 = sc4.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName4 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et4.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id4 = Guid.Empty;
                        projectTaskEditObject.Mark4 = 0;
                        projectTaskEditObject.ProjectManagerId4 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId4 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId4 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId4 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName4 = "请选择";
                        projectTaskEditObject.DesignCustomerName4 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                        projectTaskEditObject.EngineeringProgressName4 = "";
                    }
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                    if (et5 != null)
                    {
                        projectTaskEditObject.Id5 = et5.Id;
                        projectTaskEditObject.Mark5 = 1;
                        projectTaskEditObject.ProjectManagerId5 = et5.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId5 = et5.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId5 = et5.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId5 = et5.SupervisionCustomerId;
                        if (et5.ProjectManagerId != Guid.Empty)
                        {
                            User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName5 = "请选择";
                        }
                        if (et5.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName5 = "请选择";
                        }
                        if (et5.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        }
                        if (et5.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id5 = Guid.Empty;
                        projectTaskEditObject.Mark5 = 0;
                        projectTaskEditObject.ProjectManagerId5 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName5 = "请选择";
                        projectTaskEditObject.DesignCustomerName5 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        projectTaskEditObject.EngineeringProgressName5 = "";
                    }
                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                    if (et6 != null)
                    {
                        projectTaskEditObject.Id6 = et6.Id;
                        projectTaskEditObject.Mark6 = 1;
                        projectTaskEditObject.ProjectManagerId6 = et6.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId6 = et6.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId6 = et6.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId6 = et6.SupervisionCustomerId;
                        if (et6.ProjectManagerId != Guid.Empty)
                        {
                            User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName6 = "请选择";
                        }
                        if (et6.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName6 = "请选择";
                        }
                        if (et6.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        }
                        if (et6.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id6 = Guid.Empty;
                        projectTaskEditObject.Mark6 = 0;
                        projectTaskEditObject.ProjectManagerId6 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName6 = "请选择";
                        projectTaskEditObject.DesignCustomerName6 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        projectTaskEditObject.EngineeringProgressName6 = "";
                    }
                }
                else if (remodeling.Profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                    if (et5 != null)
                    {
                        projectTaskEditObject.Id5 = et5.Id;
                        projectTaskEditObject.Mark5 = 1;
                        projectTaskEditObject.ProjectManagerId5 = et5.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId5 = et5.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId5 = et5.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId5 = et5.SupervisionCustomerId;
                        if (et5.ProjectManagerId != Guid.Empty)
                        {
                            User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName5 = "请选择";
                        }
                        if (et5.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName5 = "请选择";
                        }
                        if (et5.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        }
                        if (et5.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id5 = Guid.Empty;
                        projectTaskEditObject.Mark5 = 0;
                        projectTaskEditObject.ProjectManagerId5 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId5 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName5 = "请选择";
                        projectTaskEditObject.DesignCustomerName5 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                        projectTaskEditObject.EngineeringProgressName5 = "";
                    }
                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                    if (et6 != null)
                    {
                        projectTaskEditObject.Id6 = et6.Id;
                        projectTaskEditObject.Mark6 = 1;
                        projectTaskEditObject.ProjectManagerId6 = et6.ProjectManagerId;
                        projectTaskEditObject.DesignCustomerId6 = et6.DesignCustomerId;
                        projectTaskEditObject.ConstructionCustomerId6 = et6.ConstructionCustomerId;
                        projectTaskEditObject.SupervisionCustomerId6 = et6.SupervisionCustomerId;
                        if (et6.ProjectManagerId != Guid.Empty)
                        {
                            User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                            projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                        }
                        else
                        {
                            projectTaskEditObject.ProjectManagerName6 = "请选择";
                        }
                        if (et6.DesignCustomerId != Guid.Empty)
                        {
                            Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                            projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.DesignCustomerName6 = "请选择";
                        }
                        if (et6.ConstructionCustomerId != Guid.Empty)
                        {
                            Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                            projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        }
                        if (et6.SupervisionCustomerId != Guid.Empty)
                        {
                            Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                            projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                        }
                        else
                        {
                            projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        }
                        projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                    }
                    else
                    {
                        projectTaskEditObject.Id6 = Guid.Empty;
                        projectTaskEditObject.Mark6 = 0;
                        projectTaskEditObject.ProjectManagerId6 = Guid.Empty;
                        projectTaskEditObject.DesignCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ConstructionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.SupervisionCustomerId6 = Guid.Empty;
                        projectTaskEditObject.ProjectManagerName6 = "请选择";
                        projectTaskEditObject.DesignCustomerName6 = "请选择";
                        projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                        projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                        projectTaskEditObject.EngineeringProgressName6 = "";
                    }
                }

                projectTaskEditObject.Id = projectTask.Id;
                projectTaskEditObject.DesignDateText = projectTask.DesignDate.ToShortDateString();
                projectTaskEditObject.DesignFileIdList = "";
                projectTaskEditObject.DesignCount = 0;
                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation != null)
                {
                    int count = 0;
                    if (generalDesignFileAssociation.FileIdList != "")
                    {
                        if (generalDesignFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = generalDesignFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.DesignCount = count;
                    projectTaskEditObject.DesignFileIdList = generalDesignFileAssociation.FileIdList;
                }
                else
                {
                    projectTaskEditObject.DesignCount = 0;
                    projectTaskEditObject.DesignFileIdList = "";
                }

                projectTaskEditObject.G2Number = place.G2Number;
                projectTaskEditObject.D2Number = place.D2Number;
                projectTaskEditObject.G3Number = place.G3Number;
                projectTaskEditObject.G4Number = place.G4Number;
                projectTaskEditObject.G5Number = place.G5Number;
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 新增项目任务
        /// </summary>
        /// <param name="projectTaskMaintObject">要新增的项目任务对象</param>
        public void AddProjectTask(ProjectTaskMaintObject projectTaskMaintObject)
        {
            if (projectTaskMaintObject.Id == Guid.Empty)
            {
                ProjectTask projectTask = AggregateFactory.CreateProjectTask((ProjectType)projectTaskMaintObject.ProjectType, projectTaskMaintObject.ParentId, projectTaskMaintObject.PlaceId, projectTaskMaintObject.ProjectCode, projectTaskMaintObject.CreateUserId);
                projectTaskRepository.Add(projectTask);
            }
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
        /// 删除项目任务
        /// </summary>
        /// <param name="projectTaskMaintObjects">要删除的项目任务维护对象列表</param>
        public void RemoveProjectTasks(IList<ProjectTaskMaintObject> projectTaskMaintObjects)
        {
            foreach (ProjectTaskMaintObject projectTaskMaintObject in projectTaskMaintObjects)
            {
                ProjectTask projectTask = projectTaskRepository.FindByKey(projectTaskMaintObject.Id);
                if (projectTask != null)
                {
                    projectTaskRepository.Remove(projectTask);
                }
            }
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
        /// 修改项目任务
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void AppointAreaAndDesignUser(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                Customer customer = customerRepository.GetByKey(projectTaskEditObject.GeneralDesignId);
                if (customer.CustomerUserId == Guid.Empty)
                {
                    throw new ApplicationFault("选择的总设单位还未关联登陆人");
                }
                projectTask.AppointAreaAndDesignUser(projectTaskEditObject.AreaManagerId, projectTaskEditObject.GeneralDesignId, projectTaskEditObject.ModifyUserId);
                projectTaskRepository.Update(projectTask);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(projectTaskEditObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "90c81c32-b84e-46ed-9041-a00cb9b2c04e")//施工设计
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        else if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "97f9aae8-bae1-4fb4-a5d9-061bca9831e4")//提交总设计图
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        else if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "86216182-9bc6-4968-b88b-94de709ed2ea")//施工设计
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        else if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "c6f3e188-896d-4551-968a-a2c8b6fba930")//提交总设计图
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        else if (modifyWFActivityInstance.WFActivityInstanceName == "跟踪项目进度")
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectDesign(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                Addressing addressing = addressingRepository.GetByKey(projectTaskEditObject.Id);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);
                if (projectTask.ProjectBeginDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    projectTask.ProjectCode = orderCodeSeedRepository.GenerateProjectCode((int)planning.Profession, DateTime.Now);
                    projectTask.ProjectBeginDate = DateTime.Now;
                    projectTaskRepository.Update(projectTask);
                }

                if (planning.Profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅));
                    if (projectTaskEditObject.Mark1 == 1)
                    {
                        et1.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId1, projectTaskEditObject.DesignCustomerId1, projectTaskEditObject.ConstructionCustomerId1, projectTaskEditObject.SupervisionCustomerId1);
                        engineeringTaskRepository.Update(et1);
                    }
                    else
                    {
                        if (et1.State == State.使用)
                        {
                            et1.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et1);
                        }
                    }

                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础));
                    if (projectTaskEditObject.Mark2 == 1)
                    {
                        et2.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId2, projectTaskEditObject.DesignCustomerId2, projectTaskEditObject.ConstructionCustomerId2, projectTaskEditObject.SupervisionCustomerId2);
                        engineeringTaskRepository.Update(et2);
                    }
                    else
                    {
                        if (et2.State == State.使用)
                        {
                            et2.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et2);
                        }
                    }

                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房));
                    if (projectTaskEditObject.Mark3 == 1)
                    {
                        et3.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId3, projectTaskEditObject.DesignCustomerId3, projectTaskEditObject.ConstructionCustomerId3, projectTaskEditObject.SupervisionCustomerId3);
                        engineeringTaskRepository.Update(et3);
                    }
                    else
                    {
                        if (et3.State == State.使用)
                        {
                            et3.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et3);
                        }
                    }

                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入));
                    if (projectTaskEditObject.Mark4 == 1)
                    {
                        et4.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId4, projectTaskEditObject.DesignCustomerId4, projectTaskEditObject.ConstructionCustomerId4, projectTaskEditObject.SupervisionCustomerId4);
                        engineeringTaskRepository.Update(et4);
                    }
                    else
                    {
                        if (et4.State == State.使用)
                        {
                            et4.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et4);
                        }
                    }

                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }
                else if (planning.Profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }

                if (planning.AddressingState == AddressingState.流转中)
                {
                    Place place = AggregateFactory.CreatePlace(codeSeedRepository.GenerateCode("Place"), addressing.PlaceName, planning.Profession, planning.PlaceCategoryId, planning.ReseauId,
                        planning.Lng, planning.Lat, planning.PlaceOwner, planning.Importance, addressing.AddressingDepartmentId, addressing.AddressingRealName, addressing.OwnerName, addressing.OwnerContact, addressing.OwnerPhoneNumber,
                        planning.DetailedAddress, "", PlaceMapState.寻址确认, projectTaskEditObject.ModifyUserId);
                    placeRepository.Add(place);

                    User user = userRepository.GetByKey(projectTaskEditObject.ModifyUserId);
                    Department department = departmentRepository.GetByKey(user.DepartmentId);
                    PlaceBusinessVolume placeBusinessVolume = AggregateFactory.CreatePlaceBusinessVolume(place.Id, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, department.CompanyId);
                    placeBusinessVolumeRepository.Add(placeBusinessVolume);

                    planning.AddressingState = AddressingState.已寻址确认;
                    planning.AddressingDate = DateTime.Now;
                    planning.PlaceId = place.Id;
                    planningRepository.Update(planning);

                    addressing.AddressingDate = DateTime.Now;
                    addressingRepository.Update(addressing);

                    projectTask.PlaceId = place.Id;
                    projectTaskRepository.Update(projectTask);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlaceName"))
                {
                    throw new ApplicationFault("站点名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveDesignDrawing(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.DesignDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                //{
                //    throw new ApplicationFault("请选择总设日期");
                //}
                if (projectTaskEditObject.DesignFileIdList == "")
                {
                    throw new ApplicationFault("请上传总设图");
                }
                projectTask.DesignRealName = projectTaskEditObject.DesignRealName;
                projectTask.DesignDate = DateTime.Now;
                projectTaskRepository.Update(projectTask);

                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation == null && projectTaskEditObject.DesignFileIdList != "")
                {
                    FileAssociation newGeneralDesignFileAssociation = AggregateFactory.CreateFileAssociation("GeneralDesign", projectTask.Id, projectTaskEditObject.DesignFileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Add(newGeneralDesignFileAssociation);
                }
                else if (generalDesignFileAssociation != null && projectTaskEditObject.DesignFileIdList != generalDesignFileAssociation.FileIdList)
                {
                    generalDesignFileAssociation.Modify(projectTaskEditObject.DesignFileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Update(generalDesignFileAssociation);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 登记逻辑号
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveLogicalNumber(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.G2Number == "" && projectTaskEditObject.D2Number == "" && projectTaskEditObject.G3Number == "" && projectTaskEditObject.G4Number == "" && projectTaskEditObject.G5Number == "")
                //{
                //    throw new ApplicationFault("请至少填写一个逻辑号");
                //}

                Addressing addressing = addressingRepository.GetByKey(projectTaskEditObject.Id);
                Planning planning = planningRepository.GetByKey(addressing.PlanningId);

                if (projectTaskEditObject.G2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == projectTaskEditObject.G2Number && entity.Id != planning.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2G逻辑号");
                    }
                }
                if (projectTaskEditObject.D2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == projectTaskEditObject.D2Number && entity.Id != planning.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2D逻辑号");
                    }
                }
                if (projectTaskEditObject.G3Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == projectTaskEditObject.G3Number && entity.Id != planning.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该3G逻辑号");
                    }
                }
                if (projectTaskEditObject.G4Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == projectTaskEditObject.G4Number && entity.Id != planning.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该4G逻辑号");
                    }
                }
                if (projectTaskEditObject.G5Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G5Number == projectTaskEditObject.G5Number && entity.Id != planning.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该5G逻辑号");
                    }
                }

                if (planning.AddressingState == AddressingState.已寻址确认)
                {
                    Place place = placeRepository.GetByKey(planning.PlaceId);
                    place.ModifyLogicalNumber(projectTaskEditObject.G2Number, projectTaskEditObject.D2Number, projectTaskEditObject.G3Number, projectTaskEditObject.G4Number, projectTaskEditObject.G5Number);
                    placeRepository.Update(place);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectOpening(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType == ProjectType.新建));
            if (projectTask != null)
            {
                if (projectTaskEditObject.ProjectDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    throw new ApplicationFault("请选择开通时间");
                }

                projectTask.ProjectDate = projectTaskEditObject.ProjectDate;
                projectTaskRepository.Update(projectTask);

                Place place = placeRepository.GetByKey(projectTask.PlaceId);
                place.PlaceMapState = PlaceMapState.项目开通;
                placeRepository.Update(place);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="profession">专业</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        public string GetProjectDesignsPage(int pageIndex, int pageSize, Guid areaId, Guid reseauId, string placeName, string projectCode, int profession, Guid areaManagerId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "AreaManagerId", Type = SqlDbType.UniqueIdentifier, Value = areaManagerId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectDesignsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <param name="userId">当前登陆人Id</param>
        /// <returns></returns>
        public string GetDesignDrawingsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string designRealName, int profession, Guid userId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "UserId", Type = SqlDbType.UniqueIdentifier, Value = userId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryDesignDrawingsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据项目任务Id获取项目任务信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectDesignEditById(Guid id)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(id);
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);

                projectTaskEditObject.Id = projectTask.Id;
                EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅 && entity.State == State.使用));
                if (et1 != null)
                {
                    projectTaskEditObject.Id1 = et1.Id;
                    projectTaskEditObject.Mark1 = 1;
                    projectTaskEditObject.ProjectManagerId1 = et1.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId1 = et1.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId1 = et1.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId1 = et1.SupervisionCustomerId;
                    if (et1.ProjectManagerId != Guid.Empty)
                    {
                        User pm1 = userRepository.GetByKey(et1.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName1 = pm1.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName1 = "请选择";
                    }
                    if (et1.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc1 = customerRepository.GetByKey(et1.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName1 = dc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName1 = "请选择";
                    }
                    if (et1.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc1 = customerRepository.GetByKey(et1.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName1 = cc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                    }
                    if (et1.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc1 = customerRepository.GetByKey(et1.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName1 = sc1.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName1 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et1.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id1 = Guid.Empty;
                    projectTaskEditObject.Mark1 = 0;
                    projectTaskEditObject.ProjectManagerId1 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId1 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId1 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId1 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName1 = "请选择";
                    projectTaskEditObject.DesignCustomerName1 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName1 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName1 = "请选择";
                    projectTaskEditObject.EngineeringProgressName1 = "";
                }
                EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础 && entity.State == State.使用));
                if (et2 != null)
                {
                    projectTaskEditObject.Id2 = et2.Id;
                    projectTaskEditObject.Mark2 = 1;
                    projectTaskEditObject.ProjectManagerId2 = et2.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId2 = et2.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId2 = et2.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId2 = et2.SupervisionCustomerId;
                    if (et2.ProjectManagerId != Guid.Empty)
                    {
                        User pm2 = userRepository.GetByKey(et2.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName2 = pm2.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName2 = "请选择";
                    }
                    if (et2.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc2 = customerRepository.GetByKey(et2.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName2 = dc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName2 = "请选择";
                    }
                    if (et2.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc2 = customerRepository.GetByKey(et2.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName2 = cc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                    }
                    if (et2.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc2 = customerRepository.GetByKey(et2.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName2 = sc2.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName2 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et2.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id2 = Guid.Empty;
                    projectTaskEditObject.Mark2 = 0;
                    projectTaskEditObject.ProjectManagerId2 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId2 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId2 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId2 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName2 = "请选择";
                    projectTaskEditObject.DesignCustomerName2 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName2 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName2 = "请选择";
                    projectTaskEditObject.EngineeringProgressName2 = "";
                }
                EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房 && entity.State == State.使用));
                if (et3 != null)
                {
                    projectTaskEditObject.Id3 = et3.Id;
                    projectTaskEditObject.Mark3 = 1;
                    projectTaskEditObject.ProjectManagerId3 = et3.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId3 = et3.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId3 = et3.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId3 = et3.SupervisionCustomerId;
                    if (et3.ProjectManagerId != Guid.Empty)
                    {
                        User pm3 = userRepository.GetByKey(et3.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName3 = pm3.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName3 = "请选择";
                    }
                    if (et3.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc3 = customerRepository.GetByKey(et3.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName3 = dc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName3 = "请选择";
                    }
                    if (et3.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc3 = customerRepository.GetByKey(et3.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName3 = cc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                    }
                    if (et3.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc3 = customerRepository.GetByKey(et3.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName3 = sc3.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName3 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et3.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id3 = Guid.Empty;
                    projectTaskEditObject.Mark3 = 0;
                    projectTaskEditObject.ProjectManagerId3 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId3 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId3 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId3 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName3 = "请选择";
                    projectTaskEditObject.DesignCustomerName3 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName3 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName3 = "请选择";
                    projectTaskEditObject.EngineeringProgressName3 = "";
                }
                EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入 && entity.State == State.使用));
                if (et4 != null)
                {
                    projectTaskEditObject.Id4 = et4.Id;
                    projectTaskEditObject.Mark4 = 1;
                    projectTaskEditObject.ProjectManagerId4 = et4.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId4 = et4.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId4 = et4.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId4 = et4.SupervisionCustomerId;
                    if (et4.ProjectManagerId != Guid.Empty)
                    {
                        User pm4 = userRepository.GetByKey(et4.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName4 = pm4.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName4 = "请选择";
                    }
                    if (et4.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc4 = customerRepository.GetByKey(et4.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName4 = dc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName4 = "请选择";
                    }
                    if (et4.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc4 = customerRepository.GetByKey(et4.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName4 = cc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                    }
                    if (et4.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc4 = customerRepository.GetByKey(et4.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName4 = sc4.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName4 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et4.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id4 = Guid.Empty;
                    projectTaskEditObject.Mark4 = 0;
                    projectTaskEditObject.ProjectManagerId4 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId4 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId4 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId4 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName4 = "请选择";
                    projectTaskEditObject.DesignCustomerName4 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName4 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName4 = "请选择";
                    projectTaskEditObject.EngineeringProgressName4 = "";
                }
                EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装 && entity.State == State.使用));
                if (et5 != null)
                {
                    projectTaskEditObject.Id5 = et5.Id;
                    projectTaskEditObject.Mark5 = 1;
                    projectTaskEditObject.ProjectManagerId5 = et5.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId5 = et5.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId5 = et5.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId5 = et5.SupervisionCustomerId;
                    if (et5.ProjectManagerId != Guid.Empty)
                    {
                        User pm5 = userRepository.GetByKey(et5.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName5 = pm5.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName5 = "请选择";
                    }
                    if (et5.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc5 = customerRepository.GetByKey(et5.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName5 = dc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName5 = "请选择";
                    }
                    if (et5.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc5 = customerRepository.GetByKey(et5.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName5 = cc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                    }
                    if (et5.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc5 = customerRepository.GetByKey(et5.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName5 = sc5.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName5 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et5.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id5 = Guid.Empty;
                    projectTaskEditObject.Mark5 = 0;
                    projectTaskEditObject.ProjectManagerId5 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId5 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId5 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId5 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName5 = "请选择";
                    projectTaskEditObject.DesignCustomerName5 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName5 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName5 = "请选择";
                    projectTaskEditObject.EngineeringProgressName5 = "";
                }
                EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路 && entity.State == State.使用));
                if (et6 != null)
                {
                    projectTaskEditObject.Id6 = et6.Id;
                    projectTaskEditObject.Mark6 = 1;
                    projectTaskEditObject.ProjectManagerId6 = et6.ProjectManagerId;
                    projectTaskEditObject.DesignCustomerId6 = et6.DesignCustomerId;
                    projectTaskEditObject.ConstructionCustomerId6 = et6.ConstructionCustomerId;
                    projectTaskEditObject.SupervisionCustomerId6 = et6.SupervisionCustomerId;
                    if (et6.ProjectManagerId != Guid.Empty)
                    {
                        User pm6 = userRepository.GetByKey(et6.ProjectManagerId);
                        projectTaskEditObject.ProjectManagerName6 = pm6.FullName;
                    }
                    else
                    {
                        projectTaskEditObject.ProjectManagerName6 = "请选择";
                    }
                    if (et6.DesignCustomerId != Guid.Empty)
                    {
                        Customer dc6 = customerRepository.GetByKey(et6.DesignCustomerId);
                        projectTaskEditObject.DesignCustomerName6 = dc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.DesignCustomerName6 = "请选择";
                    }
                    if (et6.ConstructionCustomerId != Guid.Empty)
                    {
                        Customer cc6 = customerRepository.GetByKey(et6.ConstructionCustomerId);
                        projectTaskEditObject.ConstructionCustomerName6 = cc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                    }
                    if (et6.SupervisionCustomerId != Guid.Empty)
                    {
                        Customer sc6 = customerRepository.GetByKey(et6.SupervisionCustomerId);
                        projectTaskEditObject.SupervisionCustomerName6 = sc6.CustomerName;
                    }
                    else
                    {
                        projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                    }
                    projectTaskEditObject.EngineeringProgressName6 = EnumHelper.GetEnumText(typeof(EngineeringProgress), et6.EngineeringProgress);
                }
                else
                {
                    projectTaskEditObject.Id6 = Guid.Empty;
                    projectTaskEditObject.Mark6 = 0;
                    projectTaskEditObject.ProjectManagerId6 = Guid.Empty;
                    projectTaskEditObject.DesignCustomerId6 = Guid.Empty;
                    projectTaskEditObject.ConstructionCustomerId6 = Guid.Empty;
                    projectTaskEditObject.SupervisionCustomerId6 = Guid.Empty;
                    projectTaskEditObject.ProjectManagerName6 = "请选择";
                    projectTaskEditObject.DesignCustomerName6 = "请选择";
                    projectTaskEditObject.ConstructionCustomerName6 = "请选择";
                    projectTaskEditObject.SupervisionCustomerName6 = "请选择";
                    projectTaskEditObject.EngineeringProgressName6 = "";
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectDesignEdit(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(projectTaskEditObject.Id);
            if (projectTask != null)
            {
                Profession profession = Profession.基站;
                Place place = placeRepository.GetByKey(projectTask.PlaceId);
                if (place != null)
                {
                    profession = place.Profession;
                }

                if (profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅));
                    if (projectTaskEditObject.Mark1 == 1)
                    {
                        et1.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId1, projectTaskEditObject.DesignCustomerId1, projectTaskEditObject.ConstructionCustomerId1, projectTaskEditObject.SupervisionCustomerId1);
                        engineeringTaskRepository.Update(et1);
                    }
                    else
                    {
                        if (et1.State == State.使用)
                        {
                            et1.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et1);
                        }
                    }

                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础));
                    if (projectTaskEditObject.Mark2 == 1)
                    {
                        et2.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId2, projectTaskEditObject.DesignCustomerId2, projectTaskEditObject.ConstructionCustomerId2, projectTaskEditObject.SupervisionCustomerId2);
                        engineeringTaskRepository.Update(et2);
                    }
                    else
                    {
                        if (et2.State == State.使用)
                        {
                            et2.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et2);
                        }
                    }

                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房));
                    if (projectTaskEditObject.Mark3 == 1)
                    {
                        et3.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId3, projectTaskEditObject.DesignCustomerId3, projectTaskEditObject.ConstructionCustomerId3, projectTaskEditObject.SupervisionCustomerId3);
                        engineeringTaskRepository.Update(et3);
                    }
                    else
                    {
                        if (et3.State == State.使用)
                        {
                            et3.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et3);
                        }
                    }

                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入));
                    if (projectTaskEditObject.Mark4 == 1)
                    {
                        et4.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId4, projectTaskEditObject.DesignCustomerId4, projectTaskEditObject.ConstructionCustomerId4, projectTaskEditObject.SupervisionCustomerId4);
                        engineeringTaskRepository.Update(et4);
                    }
                    else
                    {
                        if (et4.State == State.使用)
                        {
                            et4.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et4);
                        }
                    }

                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }
                else if (profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="profession">专业</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        public string GetProjectProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int projectProgress, int profession, Guid areaManagerId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectProgress", Type = SqlDbType.Int, Value = projectProgress });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "AreaManagerId", Type = SqlDbType.UniqueIdentifier, Value = areaManagerId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectProgresssPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页项目进度登记列表(移动端)
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="areaManagerId">项目经理Id</param>
        /// <returns></returns>
        public string GetProjectProgresssPageMobile(int profession, Guid areaId, Guid reseauId, string projectCode, string placeName, int projectProgress, Guid areaManagerId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "ProjectProgress", Type = SqlDbType.Int, Value = projectProgress });
            parameters.Add(new Parameter() { Name = "AreaManagerId", Type = SqlDbType.UniqueIdentifier, Value = areaManagerId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectProgresssPageMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据项目任务Id获取项目进度登记信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectProgressById(Guid id)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(id);
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                projectTaskEditObject.Id = id;
                projectTaskEditObject.ProjectProgress = (int)projectTask.ProjectProgress;
                projectTaskEditObject.ProgressMemos = projectTask.ProgressMemos;
                projectTaskEditObject.ProjectBeginDateText = projectTask.ProjectBeginDate.ToShortDateString();

                projectTaskEditObject.FileIdList = "";
                projectTaskEditObject.Count = 0;
                FileAssociation projectProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                if (projectProgressFileAssociation != null)
                {
                    int count = 0;
                    if (projectProgressFileAssociation.FileIdList != "")
                    {
                        if (projectProgressFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = projectProgressFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.Count = count;
                    projectTaskEditObject.FileIdList = projectProgressFileAssociation.FileIdList;
                }
                else
                {
                    projectTaskEditObject.Count = 0;
                    projectTaskEditObject.FileIdList = "";
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 根据项目任务Id获取项目进度登记信息(移动端)
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetProjectProgressByIdMobile(Guid id, string header)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(id);
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                projectTaskEditObject.Id = id;
                projectTaskEditObject.ProjectProgress = (int)projectTask.ProjectProgress;
                projectTaskEditObject.ProgressMemos = projectTask.ProgressMemos;
                projectTaskEditObject.ProjectBeginDateText = projectTask.ProjectBeginDate.ToShortDateString();

                projectTaskEditObject.ImageUrl = "";
                FileAssociation projectProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                if (projectProgressFileAssociation != null && projectProgressFileAssociation.FileIdList != "")
                {
                    if (projectProgressFileAssociation.FileIdList.Contains(","))
                    {
                        string[] strFileList = projectProgressFileAssociation.FileIdList.Split(',');
                        foreach (string fileListId in strFileList)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileListId));
                            if (file.FileExtension == ".jpeg")
                            {
                                string url = file.FilePath.Replace("\\", "/");
                                url = url.Replace(url.Substring(0, 2), header);
                                projectTaskEditObject.ImageUrl += url + ",";
                            }
                        }
                        projectTaskEditObject.ImageUrl = projectTaskEditObject.ImageUrl.Substring(0, projectTaskEditObject.ImageUrl.Length - 1);
                    }
                    else
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(projectProgressFileAssociation.FileIdList));
                        if (file.FileExtension == ".jpeg")
                        {
                            string url = file.FilePath.Replace("\\", "/");
                            url = url.Replace(url.Substring(0, 2), header);
                            projectTaskEditObject.ImageUrl = url;
                        }
                    }
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 保存项目进度登记
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectProgress(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(projectTaskEditObject.Id);
            if (projectTask != null)
            {
                projectTask.ProjectProgress = (ProjectProgress)projectTaskEditObject.ProjectProgress;
                projectTask.ProgressMemos = projectTaskEditObject.ProgressMemos;
                projectTask.ModifyDate = DateTime.Now;
                projectTaskRepository.Update(projectTask);

                FileAssociation projectProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                if (projectProgressFileAssociation == null && projectTaskEditObject.FileIdList != "")
                {
                    FileAssociation newGeneralDesignFileAssociation = AggregateFactory.CreateFileAssociation("ProjectProgress", projectTask.Id, projectTaskEditObject.FileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Add(newGeneralDesignFileAssociation);
                }
                else if (projectProgressFileAssociation != null && projectTaskEditObject.FileIdList != projectProgressFileAssociation.FileIdList)
                {
                    projectProgressFileAssociation.Modify(projectTaskEditObject.FileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Update(projectProgressFileAssociation);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 保存项目进度登记(移动端)
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectProgressMobile(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(projectTaskEditObject.Id);
            if (projectTask != null)
            {
                projectTask.ProjectProgress = (ProjectProgress)projectTaskEditObject.ProjectProgress;
                projectTask.ProgressMemos = projectTaskEditObject.ProgressMemos;
                projectTask.ModifyDate = DateTime.Now;
                projectTaskRepository.Update(projectTask);

                if (projectTaskEditObject.Base64String.Length > 0)
                {
                    string fileIdList = "";
                    foreach (string base64 in projectTaskEditObject.Base64String)
                    {
                        if (base64.Contains(","))
                        {
                            string base64Content = base64.Split(',')[1];
                            byte[] bt = Convert.FromBase64String(base64Content);
                            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bt))
                            {
                                Guid fileId = Guid.NewGuid();
                                DateTime today = DateTime.Now;
                                string uploadFolder = System.IO.Path.Combine(baseUploadFolder, string.Format("{0}-{1}-{2}", today.Year, today.Month, today.Day));
                                FileHelper.CreateDirectory(uploadFolder);
                                string filePath = System.IO.Path.Combine(uploadFolder, Guid.NewGuid() + ".jpeg");
                                FileHelper.UploadFile(stream, filePath, bufferSize);
                                string fileName = fileId.ToString().Replace("-", "");
                                File file = AggregateFactory.CreateFile(fileId, fileName, "application/octet-stream",
                                    ".jpeg", bt.Length, filePath, projectTaskEditObject.ModifyUserId);
                                fileRepository.Add(file);

                                fileIdList += fileId + ",";
                            }
                        }
                    }
                    FileAssociation projectProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "ProjectProgress"));
                    if (projectProgressFileAssociation == null && fileIdList != "")
                    {
                        FileAssociation newProjectProgressFileAssociation = AggregateFactory.CreateFileAssociation("ProjectProgress", projectTask.Id, fileIdList.Substring(0, fileIdList.Length - 1), projectTaskEditObject.ModifyUserId);
                        fileAssociationRepository.Add(newProjectProgressFileAssociation);
                    }
                    else if (projectProgressFileAssociation != null && fileIdList != "")
                    {
                        if (projectProgressFileAssociation.FileIdList != "")
                        {
                            fileIdList = projectProgressFileAssociation.FileIdList + "," + fileIdList.Substring(0, fileIdList.Length - 1);
                        }
                        else
                        {
                            fileIdList = fileIdList.Substring(0, fileIdList.Length - 1);
                        }
                        projectProgressFileAssociation.Modify(fileIdList, projectTaskEditObject.ModifyUserId);
                        fileAssociationRepository.Update(projectProgressFileAssociation);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 根据项目任务Id获取项目进度登记信息
        /// </summary>
        /// <param name="id">项目任务Id</param>
        /// <returns>项目任务修改对象</returns>
        public ProjectTaskEditObject GetDesignDrawingById(Guid id)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(id);
            if (projectTask != null)
            {
                ProjectTaskEditObject projectTaskEditObject = MapperHelper.Map<ProjectTask, ProjectTaskEditObject>(projectTask);
                projectTaskEditObject.Id = id;
                projectTaskEditObject.DesignRealName = projectTask.DesignRealName;
                projectTaskEditObject.DesignDateText = projectTask.DesignDate.ToShortDateString();

                projectTaskEditObject.FileIdList = "";
                projectTaskEditObject.Count = 0;
                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation != null)
                {
                    int count = 0;
                    if (generalDesignFileAssociation.FileIdList != "")
                    {
                        if (generalDesignFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = generalDesignFileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    projectTaskEditObject.Count = count;
                    projectTaskEditObject.FileIdList = generalDesignFileAssociation.FileIdList;
                }
                else
                {
                    projectTaskEditObject.Count = 0;
                    projectTaskEditObject.FileIdList = "";
                }
                return projectTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
        }

        /// <summary>
        /// 保存项目进度登记
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveDesignDrawingEdit(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.GetByKey(projectTaskEditObject.Id);
            if (projectTask != null)
            {
                //if (projectTaskEditObject.DesignDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                //{
                //    throw new ApplicationFault("请选择设计日期");
                //}
                projectTask.DesignRealName = projectTaskEditObject.DesignRealName;
                projectTask.DesignDate = DateTime.Now;
                projectTask.ModifyDate = DateTime.Now;
                projectTaskRepository.Update(projectTask);

                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation == null && projectTaskEditObject.FileIdList != "")
                {
                    FileAssociation newGeneralDesignFileAssociation = AggregateFactory.CreateFileAssociation("GeneralDesign", projectTask.Id, projectTaskEditObject.FileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Add(newGeneralDesignFileAssociation);
                }
                else if (generalDesignFileAssociation != null && projectTaskEditObject.FileIdList != generalDesignFileAssociation.FileIdList)
                {
                    generalDesignFileAssociation.Modify(projectTaskEditObject.FileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Update(generalDesignFileAssociation);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 改造站修改项目任务
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void AppointAreaAndDesignUserR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                Customer customer = customerRepository.GetByKey(projectTaskEditObject.GeneralDesignId);
                if (customer.CustomerUserId == Guid.Empty)
                {
                    throw new ApplicationFault("选择的总设单位还未关联登陆人");
                }
                projectTask.AppointAreaAndDesignUser(projectTaskEditObject.AreaManagerId, projectTaskEditObject.GeneralDesignId, projectTaskEditObject.ModifyUserId);
                projectTaskRepository.Update(projectTask);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(projectTaskEditObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理 && entity.WFActivityOperate == WFActivityOperate.单据编辑));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "95154645-69f3-4c49-95b1-dc77b8c4c962")//任务分配
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        else if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "f11adb5a-ed98-4320-80b1-d000f60c9bcf")//项目设计
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "47aa34fb-d043-48f9-ab1f-f68ee5b6a01f")//任务分配
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        else if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "9a3469dd-e429-44dd-9caf-f51cd76fff15")//项目设计
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        else if (modifyWFActivityInstance.WFActivityInstanceName == "跟踪项目进度")
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectDesignR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);
                if (projectTask.ProjectBeginDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    projectTask.ProjectCode = orderCodeSeedRepository.GenerateProjectCode((int)remodeling.Profession, DateTime.Now);
                    projectTask.ProjectBeginDate = DateTime.Now;
                    projectTaskRepository.Update(projectTask);
                }

                if (remodeling.Profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅));
                    if (projectTaskEditObject.Mark1 == 1)
                    {
                        et1.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId1, projectTaskEditObject.DesignCustomerId1, projectTaskEditObject.ConstructionCustomerId1, projectTaskEditObject.SupervisionCustomerId1);
                        engineeringTaskRepository.Update(et1);
                    }
                    else
                    {
                        if (et1.State == State.使用)
                        {
                            et1.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et1);
                        }
                    }

                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础));
                    if (projectTaskEditObject.Mark2 == 1)
                    {
                        et2.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId2, projectTaskEditObject.DesignCustomerId2, projectTaskEditObject.ConstructionCustomerId2, projectTaskEditObject.SupervisionCustomerId2);
                        engineeringTaskRepository.Update(et2);
                    }
                    else
                    {
                        if (et2.State == State.使用)
                        {
                            et2.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et2);
                        }
                    }

                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房));
                    if (projectTaskEditObject.Mark3 == 1)
                    {
                        et3.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId3, projectTaskEditObject.DesignCustomerId3, projectTaskEditObject.ConstructionCustomerId3, projectTaskEditObject.SupervisionCustomerId3);
                        engineeringTaskRepository.Update(et3);
                    }
                    else
                    {
                        if (et3.State == State.使用)
                        {
                            et3.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et3);
                        }
                    }

                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入));
                    if (projectTaskEditObject.Mark4 == 1)
                    {
                        et4.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId4, projectTaskEditObject.DesignCustomerId4, projectTaskEditObject.ConstructionCustomerId4, projectTaskEditObject.SupervisionCustomerId4);
                        engineeringTaskRepository.Update(et4);
                    }
                    else
                    {
                        if (et4.State == State.使用)
                        {
                            et4.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et4);
                        }
                    }

                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }
                else if (remodeling.Profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 项目设计
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveDesignDrawingR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.DesignDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                //{
                //    throw new ApplicationFault("请选择总设日期");
                //}
                if (projectTaskEditObject.DesignFileIdList == "")
                {
                    throw new ApplicationFault("请上传总设图");
                }
                projectTask.DesignRealName = projectTaskEditObject.DesignRealName;
                projectTask.DesignDate = DateTime.Now;
                projectTaskRepository.Update(projectTask);

                FileAssociation generalDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == projectTask.Id && entity.EntityName == "GeneralDesign"));
                if (generalDesignFileAssociation == null && projectTaskEditObject.DesignFileIdList != "")
                {
                    FileAssociation newGeneralDesignFileAssociation = AggregateFactory.CreateFileAssociation("GeneralDesign", projectTask.Id, projectTaskEditObject.DesignFileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Add(newGeneralDesignFileAssociation);
                }
                else if (generalDesignFileAssociation != null && projectTaskEditObject.DesignFileIdList != generalDesignFileAssociation.FileIdList)
                {
                    generalDesignFileAssociation.Modify(projectTaskEditObject.DesignFileIdList, projectTaskEditObject.ModifyUserId);
                    fileAssociationRepository.Update(generalDesignFileAssociation);
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 登记逻辑号
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveLogicalNumberR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.G2Number == "" && projectTaskEditObject.D2Number == "" && projectTaskEditObject.G3Number == "" && projectTaskEditObject.G4Number == "" && projectTaskEditObject.G5Number == "")
                //{
                //    throw new ApplicationFault("请至少填写一个逻辑号");
                //}

                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);

                if (projectTaskEditObject.G2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == projectTaskEditObject.G2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2G逻辑号");
                    }
                }
                if (projectTaskEditObject.D2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == projectTaskEditObject.D2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2D逻辑号");
                    }
                }
                if (projectTaskEditObject.G3Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == projectTaskEditObject.G3Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该3G逻辑号");
                    }
                }
                if (projectTaskEditObject.G4Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == projectTaskEditObject.G4Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该4G逻辑号");
                    }
                }
                if (projectTaskEditObject.G5Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G5Number == projectTaskEditObject.G5Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该5G逻辑号");
                    }
                }
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                place.ModifyLogicalNumber(projectTaskEditObject.G2Number, projectTaskEditObject.D2Number, projectTaskEditObject.G3Number, projectTaskEditObject.G4Number, projectTaskEditObject.G5Number);
                placeRepository.Update(place);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveProjectOpeningR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                if (projectTaskEditObject.ProjectDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    throw new ApplicationFault("请选择开通时间");
                }

                projectTask.ProjectDate = projectTaskEditObject.ProjectDate;
                projectTaskRepository.Update(projectTask);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 站点状态变更
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SavePlaceState(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                place.ModifyState((State)projectTaskEditObject.PlaceState);
                placeRepository.Update(place);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 获取建设任务历史记录列表
        /// </summary>
        /// <param name="placeId">站点Id</param>
        /// <returns></returns>
        public string GetProjectTaskHistory(Guid placeId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "PlaceId", Type = SqlDbType.UniqueIdentifier, Value = placeId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryProjectTaskHistory", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取项目进度表分页列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="isOverTime">是否超时</param>
        /// <param name="profession">专业</param>
        /// <returns>项目进度表分页列表</returns>
        public string GetProjectProgresssReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(12);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "ProjectProgress", Type = SqlDbType.Int, Value = projectProgress });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "IsOverTime", Type = SqlDbType.Int, Value = isOverTime });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectProgresssReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 获取项目设计清单分页列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="generalDesignId">总设单位Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <returns>项目设计清单分页列表</returns>
        public string GetProjectDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId,
            string designRealName, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "GeneralDesignId", Type = SqlDbType.UniqueIdentifier, Value = generalDesignId });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectDesignReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 改造站指定项目经理及总设单位和任务分配
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveAreaAndDesignUserAndProjectDesignR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);

                Customer customer = customerRepository.GetByKey(projectTaskEditObject.GeneralDesignId);
                if (customer.CustomerUserId == Guid.Empty)
                {
                    throw new ApplicationFault("选择的总设单位还未关联登陆人");
                }
                projectTask.AppointAreaAndDesignUser(projectTaskEditObject.AreaManagerId, projectTaskEditObject.GeneralDesignId, projectTaskEditObject.ModifyUserId);
                //projectTaskRepository.Update(projectTask);

                WFActivityInstance wfActivityInstance = wfActivityInstanceRepository.FindByKey(projectTaskEditObject.WFActivityInstanceId);

                IEnumerable<WFActivityInstance> wfActivityInstances = wfActivityInstanceRepository.FindAll(Specification<WFActivityInstance>.Eval(entity => entity.WFProcessInstanceId == wfActivityInstance.WFProcessInstanceId &&
                    entity.SerialId >= wfActivityInstance.SerialId && entity.Id != wfActivityInstance.Id && entity.WFActivityInstanceState == WFActivityInstanceState.未处理));
                if (wfActivityInstances != null)
                {
                    foreach (WFActivityInstance modifyWFActivityInstance in wfActivityInstances)
                    {
                        if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "f11adb5a-ed98-4320-80b1-d000f60c9bcf")//项目设计
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        if (modifyWFActivityInstance.WFActivityEditorId.ToString() == "9a3469dd-e429-44dd-9caf-f51cd76fff15")//项目设计
                        {
                            modifyWFActivityInstance.UserId = customer.CustomerUserId;
                        }
                        else if (modifyWFActivityInstance.WFActivityInstanceName == "跟踪项目进度")
                        {
                            modifyWFActivityInstance.UserId = projectTaskEditObject.AreaManagerId;
                        }
                        wfActivityInstanceRepository.Update(modifyWFActivityInstance);
                    }
                }

                if (projectTask.ProjectBeginDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    projectTask.ProjectCode = orderCodeSeedRepository.GenerateProjectCode((int)remodeling.Profession, DateTime.Now);
                    projectTask.ProjectBeginDate = DateTime.Now;
                }
                projectTaskRepository.Update(projectTask);

                if (remodeling.Profession == Profession.基站)
                {
                    EngineeringTask et1 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅));
                    if (projectTaskEditObject.Mark1 == 1)
                    {
                        et1.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId1, projectTaskEditObject.DesignCustomerId1, projectTaskEditObject.ConstructionCustomerId1, projectTaskEditObject.SupervisionCustomerId1);
                        engineeringTaskRepository.Update(et1);
                    }
                    else
                    {
                        if (et1.State == State.使用)
                        {
                            et1.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et1);
                        }
                    }

                    EngineeringTask et2 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.天桅基础));
                    if (projectTaskEditObject.Mark2 == 1)
                    {
                        et2.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId2, projectTaskEditObject.DesignCustomerId2, projectTaskEditObject.ConstructionCustomerId2, projectTaskEditObject.SupervisionCustomerId2);
                        engineeringTaskRepository.Update(et2);
                    }
                    else
                    {
                        if (et2.State == State.使用)
                        {
                            et2.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et2);
                        }
                    }

                    EngineeringTask et3 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.机房));
                    if (projectTaskEditObject.Mark3 == 1)
                    {
                        et3.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId3, projectTaskEditObject.DesignCustomerId3, projectTaskEditObject.ConstructionCustomerId3, projectTaskEditObject.SupervisionCustomerId3);
                        engineeringTaskRepository.Update(et3);
                    }
                    else
                    {
                        if (et3.State == State.使用)
                        {
                            et3.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et3);
                        }
                    }

                    EngineeringTask et4 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.外电引入));
                    if (projectTaskEditObject.Mark4 == 1)
                    {
                        et4.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId4, projectTaskEditObject.DesignCustomerId4, projectTaskEditObject.ConstructionCustomerId4, projectTaskEditObject.SupervisionCustomerId4);
                        engineeringTaskRepository.Update(et4);
                    }
                    else
                    {
                        if (et4.State == State.使用)
                        {
                            et4.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et4);
                        }
                    }

                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }
                else if (remodeling.Profession == Profession.室分)
                {
                    EngineeringTask et5 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.设备安装));
                    if (projectTaskEditObject.Mark5 == 1)
                    {
                        et5.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId5, projectTaskEditObject.DesignCustomerId5, projectTaskEditObject.ConstructionCustomerId5, projectTaskEditObject.SupervisionCustomerId5);
                        engineeringTaskRepository.Update(et5);
                    }
                    else
                    {
                        if (et5.State == State.使用)
                        {
                            et5.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et5);
                        }
                    }

                    EngineeringTask et6 = engineeringTaskRepository.Find(Specification<EngineeringTask>.Eval(entity => entity.ProjectTaskId == projectTask.Id && entity.TaskModel == TaskModel.线路));
                    if (projectTaskEditObject.Mark6 == 1)
                    {
                        et6.SaveProjectDesign(State.使用, projectTaskEditObject.ProjectManagerId6, projectTaskEditObject.DesignCustomerId6, projectTaskEditObject.ConstructionCustomerId6, projectTaskEditObject.SupervisionCustomerId6);
                        engineeringTaskRepository.Update(et6);
                    }
                    else
                    {
                        if (et6.State == State.使用)
                        {
                            et6.SaveProjectDesign(State.停用, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty);
                            engineeringTaskRepository.Update(et6);
                        }
                    }
                }

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 改造站登记逻辑号及项目开通
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveLogicalNumberAndProjectOpeningR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.G2Number == "" && projectTaskEditObject.D2Number == "" && projectTaskEditObject.G3Number == "" && projectTaskEditObject.G4Number == "" && projectTaskEditObject.G5Number == "")
                //{
                //    throw new ApplicationFault("请至少填写一个逻辑号");
                //}

                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);

                if (projectTaskEditObject.G2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == projectTaskEditObject.G2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2G逻辑号");
                    }
                }
                if (projectTaskEditObject.D2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == projectTaskEditObject.D2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2D逻辑号");
                    }
                }
                if (projectTaskEditObject.G3Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == projectTaskEditObject.G3Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该3G逻辑号");
                    }
                }
                if (projectTaskEditObject.G4Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == projectTaskEditObject.G4Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该4G逻辑号");
                    }
                }
                if (projectTaskEditObject.G5Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G5Number == projectTaskEditObject.G5Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该5G逻辑号");
                    }
                }
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                place.ModifyLogicalNumber(projectTaskEditObject.G2Number, projectTaskEditObject.D2Number, projectTaskEditObject.G3Number, projectTaskEditObject.G4Number, projectTaskEditObject.G5Number);
                placeRepository.Update(place);

                if (projectTaskEditObject.ProjectDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    throw new ApplicationFault("请选择开通时间");
                }

                projectTask.ProjectDate = projectTaskEditObject.ProjectDate;
                projectTaskRepository.Update(projectTask);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 改造站登记逻辑号及项目开通和站点状态
        /// </summary>
        /// <param name="projectTaskEditObject">要修改的项目任务对象</param>
        public void SaveLogicalNumberAndProjectOpeningAndPlaceStateR(ProjectTaskEditObject projectTaskEditObject)
        {
            ProjectTask projectTask = projectTaskRepository.Find(Specification<ProjectTask>.Eval(entity => entity.ParentId == projectTaskEditObject.Id && entity.ProjectType != ProjectType.新建));
            if (projectTask != null)
            {
                //if (projectTaskEditObject.G2Number == "" && projectTaskEditObject.D2Number == "" && projectTaskEditObject.G3Number == "" && projectTaskEditObject.G4Number == "" && projectTaskEditObject.G5Number == "")
                //{
                //    throw new ApplicationFault("请至少填写一个逻辑号");
                //}

                Remodeling remodeling = remodelingRepository.GetByKey(projectTaskEditObject.Id);

                if (projectTaskEditObject.G2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G2Number == projectTaskEditObject.G2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2G逻辑号");
                    }
                }
                if (projectTaskEditObject.D2Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.D2Number == projectTaskEditObject.D2Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该2D逻辑号");
                    }
                }
                if (projectTaskEditObject.G3Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G3Number == projectTaskEditObject.G3Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该3G逻辑号");
                    }
                }
                if (projectTaskEditObject.G4Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G4Number == projectTaskEditObject.G4Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该4G逻辑号");
                    }
                }
                if (projectTaskEditObject.G5Number != "")
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.G5Number == projectTaskEditObject.G5Number && entity.Id != remodeling.PlaceId)))
                    {
                        throw new ApplicationFault("系统中已存在该5G逻辑号");
                    }
                }
                Place place = placeRepository.GetByKey(remodeling.PlaceId);
                place.ModifyLogicalNumber(projectTaskEditObject.G2Number, projectTaskEditObject.D2Number, projectTaskEditObject.G3Number, projectTaskEditObject.G4Number, projectTaskEditObject.G5Number);
                place.ModifyState((State)projectTaskEditObject.PlaceState);
                placeRepository.Update(place);

                if (projectTaskEditObject.ProjectDate.ToShortDateString() == DateTime.Parse("2000-01-01").ToShortDateString())
                {
                    throw new ApplicationFault("请选择开通时间");
                }

                projectTask.ProjectDate = projectTaskEditObject.ProjectDate;
                projectTaskRepository.Update(projectTask);

                WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == projectTaskEditObject.WFActivityInstanceId));
                if (wfActivityInstanceEditors == null)
                {
                    WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(projectTaskEditObject.WFActivityInstanceId);
                    wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
                }
            }
            else
            {
                throw new ApplicationFault("选择的项目任务在系统中不存在");
            }
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
        /// 获取项目经理月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="beginDateYear">开始年份</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        public string GetProjectTaskProjectManager(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "BeginDateYear", Type = SqlDbType.DateTime, Value = beginDateYear });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryProjectTaskProjectManager", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取部门建设月报
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="beginDateYear">开始年份</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns></returns>
        public string GetProjectTaskDepartment(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "BeginDateYear", Type = SqlDbType.DateTime, Value = beginDateYear });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryProjectTaskDepartment", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
