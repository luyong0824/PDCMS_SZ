using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class TaskPropertyLogService : DataService, ITaskPropertyLogService
    {
        private readonly IRepository<TaskPropertyLog> taskPropertyLogRepository;
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
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public TaskPropertyLogService(IRepositoryContext context,
            IRepository<TaskPropertyLog> taskPropertyLogRepository,
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
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.taskPropertyLogRepository = taskPropertyLogRepository;
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
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改任务属性
        /// </summary>
        /// <param name="taskPropertyMaintObject">要新增或者修改的任务属性对象</param>
        public void AddOrUpdateTaskPropertyLog(TaskPropertyMaintObject taskPropertyMaintObject)
        {
            if (taskPropertyMaintObject.Id == Guid.Empty)
            {
                TaskPropertyLog taskPropertyLog = AggregateFactory.CreateTaskPropertyLog(RegisterType.进度登记, taskPropertyMaintObject.ConstructionTaskId, (TaskModel)taskPropertyMaintObject.TaskModel, taskPropertyMaintObject.ParentId, taskPropertyMaintObject.ConstructionCustomerId, taskPropertyMaintObject.SupervisorCustomerId, (EngineeringProgress)taskPropertyMaintObject.ConstructionProgress, taskPropertyMaintObject.ProgressMemos, taskPropertyMaintObject.ProgressUserId.Value, (SubmitState)taskPropertyMaintObject.SubmitState, taskPropertyMaintObject.SubmitUserId.Value, 0);
                taskPropertyLogRepository.Add(taskPropertyLog);
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
        /// 获取子任务历史记录
        /// </summary>
        /// <param name="taskModel">工程名称</param>
        /// <param name="parentId">任务Id</param>
        /// <returns></returns>
        public string GetTaskPropertyLog(int taskModel, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetTaskPropertyLog", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = ds.Tables[0];
                return JsonHelper.Encode(result);
            }
        }
    }
}
