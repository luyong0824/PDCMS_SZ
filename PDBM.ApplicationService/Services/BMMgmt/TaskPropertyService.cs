using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class TaskPropertyService : DataService, ITaskPropertyService
    {
        private readonly IRepository<TaskProperty> taskPropertyRepository;
        private readonly IRepository<ConstructionTask> constructionTaskRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<AddressExplor> addressExplorRepository;
        private readonly IRepository<FoundationTest> foundationTestRepository;
        private readonly IRepository<TaskPropertyLog> taskPropertyLogRepository;
        private readonly IRepository<TowerLog> towerLogRepository;
        private readonly IRepository<TowerBaseLog> towerBaseLogRepository;
        private readonly IRepository<MachineRoomLog> machineRoomLogRepository;
        private readonly IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository;
        private readonly IRepository<EquipmentInstallLog> equipmentInstallLogRepository;
        private readonly IRepository<AddressExplorLog> addressExplorLogRepository;
        private readonly IRepository<FoundationTestLog> foundationTestLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<Customer> customerRepository;

        public TaskPropertyService(IRepositoryContext context,
            IRepository<TaskProperty> taskPropertyRepository,
            IRepository<ConstructionTask> constructionTaskRepository,
            IRepository<Place> placeRepository,
            IRepository<Project> projectRepository,
            IRepository<User> userRepository,
            IRepository<Tower> towerRepository,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<AddressExplor> addressExplorRepository,
            IRepository<FoundationTest> foundationTestRepository,
            IRepository<TaskPropertyLog> taskPropertyLogRepository,
            IRepository<TowerLog> towerLogRepository,
            IRepository<TowerBaseLog> towerBaseLogRepository,
            IRepository<MachineRoomLog> machineRoomLogRepository,
            IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository,
            IRepository<EquipmentInstallLog> equipmentInstallLogRepository,
            IRepository<AddressExplorLog> addressExplorLogRepository,
            IRepository<FoundationTestLog> foundationTestLogRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<Customer> customerRepository)
            : base(context)
        {
            this.taskPropertyRepository = taskPropertyRepository;
            this.constructionTaskRepository = constructionTaskRepository;
            this.placeRepository = placeRepository;
            this.projectRepository = projectRepository;
            this.userRepository = userRepository;
            this.towerRepository = towerRepository;
            this.towerBaseRepository = towerBaseRepository;
            this.machineRoomRepository = machineRoomRepository;
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.addressExplorRepository = addressExplorRepository;
            this.foundationTestRepository = foundationTestRepository;
            this.taskPropertyLogRepository = taskPropertyLogRepository;
            this.towerLogRepository = towerLogRepository;
            this.towerBaseLogRepository = towerBaseLogRepository;
            this.machineRoomLogRepository = machineRoomLogRepository;
            this.externalElectricPowerLogRepository = externalElectricPowerLogRepository;
            this.equipmentInstallLogRepository = equipmentInstallLogRepository;
            this.addressExplorLogRepository = addressExplorLogRepository;
            this.foundationTestLogRepository = foundationTestLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// 根据任务属性Id获取任务
        /// </summary>
        /// <param name="id">任务属性Id</param>
        /// <returns>任务属性维护对象</returns>
        public TaskPropertyMaintObject GetTaskPropertyById(Guid id)
        {
            TaskProperty taskProperty = taskPropertyRepository.FindByKey(id);
            if (taskProperty != null)
            {
                TaskPropertyMaintObject taskPropertyMaintObject = MapperHelper.Map<TaskProperty, TaskPropertyMaintObject>(taskProperty);
                taskPropertyMaintObject.Id = id;
                taskPropertyMaintObject.ConstructionProgress = (int)taskProperty.ConstructionProgress;
                taskPropertyMaintObject.ProgressMemos = taskProperty.ProgressMemos;
                if (taskProperty.ProgressUserId != Guid.Empty)
                {
                    User user = userRepository.FindByKey(taskProperty.ProgressUserId.Value);
                    taskPropertyMaintObject.ProgressFullName = user.FullName;
                }
                else
                {
                    taskPropertyMaintObject.ProgressFullName = "";
                }
                taskPropertyMaintObject.ProgressModifyDate = taskProperty.ProgressModifyDate.ToShortDateString();
                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == taskProperty.Id && entity.EntityName == "Progress"));
                if (fileAssociation != null)
                {
                    int count = 0;
                    if (fileAssociation.FileIdList != "")
                    {
                        if (fileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = fileAssociation.FileIdList.Split(',');
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
                    taskPropertyMaintObject.Count1 = count;
                    taskPropertyMaintObject.Count2 = count;
                    taskPropertyMaintObject.Count3 = count;
                    taskPropertyMaintObject.Count4 = count;
                    taskPropertyMaintObject.Count5 = count;
                    taskPropertyMaintObject.Count6 = count;
                    taskPropertyMaintObject.Count7 = count;
                    taskPropertyMaintObject.FileIdList = fileAssociation.FileIdList;
                }
                else
                {
                    taskPropertyMaintObject.Count1 = 0;
                    taskPropertyMaintObject.Count2 = 0;
                    taskPropertyMaintObject.Count3 = 0;
                    taskPropertyMaintObject.Count4 = 0;
                    taskPropertyMaintObject.Count5 = 0;
                    taskPropertyMaintObject.Count6 = 0;
                    taskPropertyMaintObject.Count7 = 0;
                    taskPropertyMaintObject.FileIdList = "";
                }
                taskPropertyMaintObject.SubmitState = (int)taskProperty.SubmitState;
                if (taskProperty.SubmitUserId != Guid.Empty)
                {
                    User user = userRepository.FindByKey(taskProperty.SubmitUserId.Value);
                    taskPropertyMaintObject.SubmitFullName = user.FullName;
                }
                else
                {
                    taskPropertyMaintObject.SubmitFullName = "";
                }
                taskPropertyMaintObject.SubmitModifyDate = taskProperty.SubmitModifyDate.ToShortDateString();

                
                return taskPropertyMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改任务属性
        /// </summary>
        /// <param name="taskPropertyMaintObject">要新增或者修改的任务属性对象</param>
        public void AddOrUpdateTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            if (taskPropertyMaintObject.Id == Guid.Empty)
            {
                TaskProperty taskProperty = AggregateFactory.CreateTaskProperty(taskPropertyMaintObject.ConstructionTaskId, (TaskModel)taskPropertyMaintObject.TaskModel, taskPropertyMaintObject.ParentId, taskPropertyMaintObject.ConstructionCustomerId, taskPropertyMaintObject.SupervisorCustomerId, 0);
                taskPropertyRepository.Add(taskProperty);
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
        /// 保存工程信息登记
        /// </summary>
        /// <param name="taskPropertyMaintObject">要修改的任务属性对象</param>
        public void SaveTaskProperty(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            TaskProperty taskProperty = taskPropertyRepository.FindByKey(taskPropertyMaintObject.Id);
            if (taskProperty != null)
            {

                FileAssociation progressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == taskProperty.Id && entity.EntityName == "Progress"));
                if ((progressFileAssociation == null && taskPropertyMaintObject.FileIdList != "") || (progressFileAssociation != null && taskPropertyMaintObject.FileIdList != progressFileAssociation.FileIdList) || taskPropertyMaintObject.ConstructionProgress != (int)taskProperty.ConstructionProgress || taskPropertyMaintObject.ProgressMemos != taskProperty.ProgressMemos)
                {
                    taskProperty.ModifyProgress((EngineeringProgress)taskPropertyMaintObject.ConstructionProgress, taskPropertyMaintObject.ProgressMemos, taskPropertyMaintObject.ModifyUserId);
                    taskPropertyRepository.Update(taskProperty);
                    if (progressFileAssociation == null && taskPropertyMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("Progress", taskProperty.Id, taskPropertyMaintObject.FileIdList, taskPropertyMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (progressFileAssociation != null && taskPropertyMaintObject.FileIdList != progressFileAssociation.FileIdList)
                    {
                        progressFileAssociation.Modify(taskPropertyMaintObject.FileIdList, taskPropertyMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(progressFileAssociation);
                    }

                    TaskPropertyLog taskPropertyLog = AggregateFactory.CreateTaskPropertyLog(RegisterType.进度登记, taskProperty.ConstructionTaskId, taskProperty.TaskModel, taskProperty.ParentId, taskProperty.ConstructionCustomerId, taskProperty.SupervisorCustomerId, (EngineeringProgress)taskPropertyMaintObject.ConstructionProgress, taskPropertyMaintObject.ProgressMemos, taskPropertyMaintObject.ModifyUserId, (SubmitState)taskPropertyMaintObject.SubmitState, taskPropertyMaintObject.ModifyUserId, 0);
                    taskPropertyLogRepository.Add(taskPropertyLog);
                    FileAssociation taskPropertyLogFileAssociation = AggregateFactory.CreateFileAssociation("ProgressLog", taskPropertyLog.Id, taskPropertyMaintObject.FileIdList, taskPropertyMaintObject.ModifyUserId);
                    fileAssociationRepository.Add(taskPropertyLogFileAssociation);
                }
                if (taskPropertyMaintObject.SubmitState != (int)taskProperty.SubmitState)
                {
                    taskProperty.ModifySubmitState((SubmitState)taskPropertyMaintObject.SubmitState, taskPropertyMaintObject.ModifyUserId);
                    taskPropertyRepository.Update(taskProperty);

                    TaskPropertyLog taskPropertyLog = AggregateFactory.CreateTaskPropertyLog(RegisterType.资料登记, taskProperty.ConstructionTaskId, taskProperty.TaskModel, taskProperty.ParentId, taskProperty.ConstructionCustomerId, taskProperty.SupervisorCustomerId, (EngineeringProgress)taskPropertyMaintObject.ConstructionProgress, taskPropertyMaintObject.ProgressMemos, taskPropertyMaintObject.ModifyUserId, (SubmitState)taskPropertyMaintObject.SubmitState, taskPropertyMaintObject.ModifyUserId, 0);
                    taskPropertyLogRepository.Add(taskPropertyLog);
                }

                
            }
            else
            {
                throw new ApplicationFault("选择的任务在系统中不存在");
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
        /// 设置施工单位
        /// </summary>
        /// <param name="taskPropertyMaintObjects">要设置施工单位的任务维护对象列表</param>
        public void SettingConstructionCustomer(IList<TaskPropertyMaintObject> taskPropertyMaintObjects)
        {
            foreach (TaskPropertyMaintObject taskPropertyMaintObject in taskPropertyMaintObjects)
            {
                Customer constructionCustomer = customerRepository.FindByKey(taskPropertyMaintObject.ConstructionCustomerId);
                if (constructionCustomer != null)
                {
                    if (constructionCustomer.CustomerUserId == Guid.Empty)
                    {
                        throw new ApplicationFault("选择的施工单位还未关联登陆人");
                    }
                }
                else
                {
                    throw new ApplicationFault("选择的施工单位在系统中不存在");
                }
                TaskProperty taskProperty = taskPropertyRepository.FindByKey(taskPropertyMaintObject.Id);
                if (taskProperty != null && constructionCustomer != null)
                {
                    taskProperty.SettingConstructionCustomer(taskPropertyMaintObject.ConstructionCustomerId);
                    taskPropertyRepository.Update(taskProperty);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_User_AddressingUserId"))
                //{
                //    throw new ApplicationFault("选择的租赁人在系统中不存在");
                //}
                throw ex;
            }
        }
    }
}
