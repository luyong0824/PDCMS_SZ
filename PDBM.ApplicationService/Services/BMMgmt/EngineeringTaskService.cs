using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
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

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class EngineeringTaskService : DataService, IEngineeringTaskService
    {
        private const int bufferSize = 4096;
        private string baseUploadFolder = ConfigurationManager.AppSettings["baseUploadFolder"];
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<EngineeringTask> engineeringTaskRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public EngineeringTaskService(IRepositoryContext context,
            IRepository<File> fileRepository,
            IRepository<EngineeringTask> engineeringTaskRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.fileRepository = fileRepository;
            this.engineeringTaskRepository = engineeringTaskRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据工程任务Id获取工程任务信息
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        public EngineeringTaskEditObject GetEngineeringTaskEditById(Guid id)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(id);
            if (engineeringTask != null)
            {
                EngineeringTaskEditObject engineeringTaskEditObject = MapperHelper.Map<EngineeringTask, EngineeringTaskEditObject>(engineeringTask);
                return engineeringTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
        }

        /// <summary>
        /// 新增工程任务
        /// </summary>
        /// <param name="projectTaskMaintObject">要新增的工程任务对象</param>
        public void AddEngineeringTask(EngineeringTaskMaintObject engineeringTaskMaintObject)
        {
            if (engineeringTaskMaintObject.Id == Guid.Empty)
            {
                EngineeringTask engineeringTask = AggregateFactory.CreateEngineeringTask((TaskModel)engineeringTaskMaintObject.TaskModel, engineeringTaskMaintObject.ProjectTaskId, engineeringTaskMaintObject.CreateUserId);
                engineeringTaskRepository.Add(engineeringTask);
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
        /// 删除工程任务
        /// </summary>
        /// <param name="projectTaskMaintObjects">要删除的工程任务维护对象列表</param>
        public void RemoveEngineeringTasks(IList<EngineeringTaskMaintObject> engineeringTaskMaintObjects)
        {
            foreach (EngineeringTaskMaintObject engineeringTaskMaintObject in engineeringTaskMaintObjects)
            {
                EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(engineeringTaskMaintObject.Id);
                if (engineeringTask != null)
                {
                    engineeringTaskRepository.Remove(engineeringTask);
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
        /// 根据条件获取分页项目设计任务列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designState">设计完成</param>
        /// <param name="profession">专业</param>
        /// <param name="customerUserId">设计单位登陆人Id</param>
        /// <returns></returns>
        public string GetEngineeringDesignsPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int taskModel, string designRealName, int designState, int profession, Guid customerUserId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "DesignState", Type = SqlDbType.Int, Value = designState });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CustomerUserId", Type = SqlDbType.UniqueIdentifier, Value = customerUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryEngineeringDesignsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据工程任务Id获取施工设计
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        public EngineeringTaskEditObject GetEngineeringDesignById(Guid id)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(id);
            if (engineeringTask != null)
            {
                EngineeringTaskEditObject engineeringTaskEditObject = MapperHelper.Map<EngineeringTask, EngineeringTaskEditObject>(engineeringTask);
                engineeringTaskEditObject.Id = engineeringTask.Id;
                engineeringTaskEditObject.DesignRealName = engineeringTask.DesignRealName;
                engineeringTaskEditObject.DesignMemos = engineeringTask.DesignMemos;
                engineeringTaskEditObject.DesignDateText = engineeringTask.DesignDate.ToShortDateString();

                FileAssociation engineeringDesignFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringDesign"));
                if (engineeringDesignFileAssociation != null)
                {
                    int count = 0;
                    if (engineeringDesignFileAssociation.FileIdList != "")
                    {
                        if (engineeringDesignFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = engineeringDesignFileAssociation.FileIdList.Split(',');
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
                    engineeringTaskEditObject.Count = count;
                    engineeringTaskEditObject.FileIdList = engineeringDesignFileAssociation.FileIdList;
                }
                else
                {
                    engineeringTaskEditObject.Count = 0;
                    engineeringTaskEditObject.FileIdList = "";
                }
                return engineeringTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
        }

        /// <summary>
        /// 保存施工设计
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务对象</param>
        public void SaveEngineeringDesign(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.GetByKey(engineeringTaskEditObject.Id);
            engineeringTask.DesignRealName = engineeringTaskEditObject.DesignRealName;
            engineeringTask.DesignMemos = engineeringTaskEditObject.DesignMemos;
            engineeringTask.DesignDate = DateTime.Now;
            engineeringTask.DesignState = (Bool)engineeringTaskEditObject.DesignState;
            engineeringTaskRepository.Update(engineeringTask);

            FileAssociation engineeringTaskFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringDesign"));
            if (engineeringTaskFileAssociation == null && engineeringTaskEditObject.FileIdList != "")
            {
                FileAssociation newEngineeringTaskFileAssociation = AggregateFactory.CreateFileAssociation("EngineeringDesign", engineeringTask.Id, engineeringTaskEditObject.FileIdList, engineeringTaskEditObject.ModifyUserId);
                fileAssociationRepository.Add(newEngineeringTaskFileAssociation);
            }
            else if (engineeringTaskFileAssociation != null && engineeringTaskEditObject.FileIdList != engineeringTaskFileAssociation.FileIdList)
            {
                engineeringTaskFileAssociation.Modify(engineeringTaskEditObject.FileIdList, engineeringTaskEditObject.ModifyUserId);
                fileAssociationRepository.Update(engineeringTaskFileAssociation);
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
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="profession">专业</param>
        /// <param name="currentUserId">登陆人Id</param>
        /// <returns></returns>
        public string GetEngineeringProgresssPage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId,
             int taskModel, int engineeringProgress, int profession, Guid currentUserId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "EngineeringProgress", Type = SqlDbType.Int, Value = engineeringProgress });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CurrentUserId", Type = SqlDbType.UniqueIdentifier, Value = currentUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryEngineeringProgresssPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页工程进度任务列表(移动端)
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="currentUserId">登陆人Id</param>
        /// <returns></returns>
        public string GetEngineeringProgresssPageMobile(int profession, string projectCode, string placeName, int taskModel, int engineeringProgress, Guid currentUserId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "EngineeringProgress", Type = SqlDbType.Int, Value = engineeringProgress });
            parameters.Add(new Parameter() { Name = "CurrentUserId", Type = SqlDbType.UniqueIdentifier, Value = currentUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryEngineeringProgresssPageMobile", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据工程任务Id获取工程进度登记
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        public EngineeringTaskEditObject GetEngineeringProgressById(Guid id)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(id);
            if (engineeringTask != null)
            {
                EngineeringTaskEditObject engineeringTaskEditObject = MapperHelper.Map<EngineeringTask, EngineeringTaskEditObject>(engineeringTask);
                engineeringTaskEditObject.Id = engineeringTask.Id;
                engineeringTaskEditObject.EngineeringProgress = (int)engineeringTask.EngineeringProgress;
                engineeringTaskEditObject.DesignMemos = engineeringTask.DesignMemos;

                FileAssociation engineeringProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringProgress"));
                if (engineeringProgressFileAssociation != null)
                {
                    int count = 0;
                    if (engineeringProgressFileAssociation.FileIdList != "")
                    {
                        if (engineeringProgressFileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = engineeringProgressFileAssociation.FileIdList.Split(',');
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
                    engineeringTaskEditObject.Count = count;
                    engineeringTaskEditObject.FileIdList = engineeringProgressFileAssociation.FileIdList;
                }
                else
                {
                    engineeringTaskEditObject.Count = 0;
                    engineeringTaskEditObject.FileIdList = "";
                }
                return engineeringTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
        }

        /// <summary>
        /// 根据工程任务Id获取工程进度登记(移动端)
        /// </summary>
        /// <param name="id">工程任务Id</param>
        /// <returns>工程任务修改对象</returns>
        public EngineeringTaskEditObject GetEngineeringProgressByIdMobile(Guid id, string header)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.FindByKey(id);
            if (engineeringTask != null)
            {
                EngineeringTaskEditObject engineeringTaskEditObject = MapperHelper.Map<EngineeringTask, EngineeringTaskEditObject>(engineeringTask);
                engineeringTaskEditObject.Id = engineeringTask.Id;
                engineeringTaskEditObject.EngineeringProgress = (int)engineeringTask.EngineeringProgress;
                engineeringTaskEditObject.DesignMemos = engineeringTask.DesignMemos;

                engineeringTaskEditObject.ImageUrl = "";
                FileAssociation engineeringProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringProgress"));
                if (engineeringProgressFileAssociation != null && engineeringProgressFileAssociation.FileIdList != "")
                {
                    if (engineeringProgressFileAssociation.FileIdList.Contains(","))
                    {
                        string[] strFileList = engineeringProgressFileAssociation.FileIdList.Split(',');
                        foreach (string fileListId in strFileList)
                        {
                            File file = fileRepository.GetByKey(Guid.Parse(fileListId));
                            if (file.FileExtension == ".jpeg")
                            {
                                string url = file.FilePath.Replace("\\", "/");
                                url = url.Replace(url.Substring(0, 2), header);
                                engineeringTaskEditObject.ImageUrl += url + ",";
                            }
                        }
                        engineeringTaskEditObject.ImageUrl = engineeringTaskEditObject.ImageUrl.Substring(0, engineeringTaskEditObject.ImageUrl.Length - 1);
                    }
                    else
                    {
                        File file = fileRepository.GetByKey(Guid.Parse(engineeringProgressFileAssociation.FileIdList));
                        if (file.FileExtension == ".jpeg")
                        {
                            string url = file.FilePath.Replace("\\", "/");
                            url = url.Replace(url.Substring(0, 2), header);
                            engineeringTaskEditObject.ImageUrl = url;
                        }
                    }
                }
                else
                {
                    engineeringTaskEditObject.Count = 0;
                    engineeringTaskEditObject.FileIdList = "";
                }
                return engineeringTaskEditObject;
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
            }
        }

        /// <summary>
        /// 保存工程进度登记
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务对象</param>
        public void SaveEngineeringProgress(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.GetByKey(engineeringTaskEditObject.Id);
            engineeringTask.EngineeringProgress = (EngineeringProgress)engineeringTaskEditObject.EngineeringProgress;
            engineeringTask.ProgressMemos = engineeringTaskEditObject.ProgressMemos;
            engineeringTask.ModifyDate = DateTime.Now;
            engineeringTaskRepository.Update(engineeringTask);

            FileAssociation engineeringProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringProgress"));
            if (engineeringProgressFileAssociation == null && engineeringTaskEditObject.FileIdList != "")
            {
                FileAssociation newEngineeringTaskFileAssociation = AggregateFactory.CreateFileAssociation("EngineeringProgress", engineeringTask.Id, engineeringTaskEditObject.FileIdList, engineeringTaskEditObject.ModifyUserId);
                fileAssociationRepository.Add(newEngineeringTaskFileAssociation);
            }
            else if (engineeringProgressFileAssociation != null && engineeringTaskEditObject.FileIdList != engineeringProgressFileAssociation.FileIdList)
            {
                engineeringProgressFileAssociation.Modify(engineeringTaskEditObject.FileIdList, engineeringTaskEditObject.ModifyUserId);
                fileAssociationRepository.Update(engineeringProgressFileAssociation);
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
        /// 保存工程进度登记(移动端)
        /// </summary>
        /// <param name="engineeringTaskEditObject">要修改的工程任务对象</param>
        public void SaveEngineeringProgressMobile(EngineeringTaskEditObject engineeringTaskEditObject)
        {
            EngineeringTask engineeringTask = engineeringTaskRepository.GetByKey(engineeringTaskEditObject.Id);
            if (engineeringTask != null)
            {
                engineeringTask.EngineeringProgress = (EngineeringProgress)engineeringTaskEditObject.EngineeringProgress;
                engineeringTask.ProgressMemos = engineeringTaskEditObject.ProgressMemos;
                engineeringTask.ModifyDate = DateTime.Now;
                engineeringTaskRepository.Update(engineeringTask);

                if (engineeringTaskEditObject.Base64String.Length > 0)
                {
                    string fileIdList = "";
                    foreach (string base64 in engineeringTaskEditObject.Base64String)
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
                                    ".jpeg", bt.Length, filePath, engineeringTaskEditObject.ModifyUserId);
                                fileRepository.Add(file);

                                fileIdList += fileId + ",";
                            }
                        }
                    }
                    FileAssociation engineeringProgressFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == engineeringTask.Id && entity.EntityName == "EngineeringProgress"));
                    if (engineeringProgressFileAssociation == null && fileIdList != "")
                    {
                        FileAssociation newEngineeringTaskFileAssociation = AggregateFactory.CreateFileAssociation("EngineeringProgress", engineeringTask.Id, fileIdList.Substring(0, fileIdList.Length - 1), engineeringTaskEditObject.ModifyUserId);
                        fileAssociationRepository.Add(newEngineeringTaskFileAssociation);
                    }
                    else if (engineeringProgressFileAssociation != null && fileIdList != "")
                    {
                        if (engineeringProgressFileAssociation.FileIdList != "")
                        {
                            fileIdList = engineeringProgressFileAssociation.FileIdList + "," + fileIdList.Substring(0, fileIdList.Length - 1);
                        }
                        else
                        {
                            fileIdList = fileIdList.Substring(0, fileIdList.Length - 1);
                        }
                        engineeringProgressFileAssociation.Modify(fileIdList, engineeringTaskEditObject.ModifyUserId);
                        fileAssociationRepository.Update(engineeringProgressFileAssociation);
                    }
                }
            }
            else
            {
                throw new ApplicationFault("选择的工程任务在系统中不存在");
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
        /// 根据条件获取分页工程进度表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="engineeringProgress">工程进度</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="constructionCustomerId">施工单位Id</param>
        /// <param name="supervisionCustomerId">监理单位Id</param>
        /// <param name="profession">专业</param>
        /// <returns></returns>
        public string GetEngineeringProgressReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            int engineeringProgress, int projectType, Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(14);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "EngineeringProgress", Type = SqlDbType.Int, Value = engineeringProgress });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "ConstructionCustomerId", Type = SqlDbType.UniqueIdentifier, Value = constructionCustomerId });
            parameters.Add(new Parameter() { Name = "SupervisionCustomerId", Type = SqlDbType.UniqueIdentifier, Value = supervisionCustomerId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryEngineeringProgressReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页工程进度表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designCustomerId">设计单位Id</param>
        /// <param name="profession">专业</param>
        /// <returns></returns>
        public string GetEngineeringDesignReportPage(int pageIndex, int pageSize, string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "DesignCustomerId", Type = SqlDbType.UniqueIdentifier, Value = designCustomerId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryEngineeringDesignReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }
    }
}
