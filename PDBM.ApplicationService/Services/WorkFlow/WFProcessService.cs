using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Domain.Models;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    /// <summary>
    /// 工作流过程应用层服务
    /// </summary>
    public class WFProcessService : DataService, IWFProcessService
    {
        private readonly IRepository<WFProcess> wfProcessRepository;
        private readonly IRepository<WFActivity> wfActivityRepository;
        private readonly IRepository<WFProcessInstance> wfProcessInstanceRepository;

        public WFProcessService(IRepositoryContext context,
            IRepository<WFProcess> wfProcessRepository,
            IRepository<WFActivity> wfActivityRepository,
            IRepository<WFProcessInstance> wfProcessInstanceRepository)
            : base(context)
        {
            this.wfProcessRepository = wfProcessRepository;
            this.wfActivityRepository = wfActivityRepository;
            this.wfProcessInstanceRepository = wfProcessInstanceRepository;
        }

        /// <summary>
        /// 根据工作流过程Id获取工作流过程
        /// </summary>
        /// <param name="id">工作流过程Id</param>
        /// <returns>工作流过程维护对象</returns>
        public WFProcessMaintObject GetWFProcessById(Guid id)
        {
            WFProcess wfProcess = wfProcessRepository.FindByKey(id);
            if (wfProcess != null)
            {
                WFProcessMaintObject wfProcessMaintObject = MapperHelper.Map<WFProcess, WFProcessMaintObject>(wfProcess);
                return wfProcessMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的流程定义在系统中不存在");
            }
        }

        /// <summary>
        /// 获取工作流过程列表
        /// </summary>
        /// <returns>工作流过程列表的Json字符串</returns>
        public string GetWFProcesses()
        {
            List<Parameter> parameters = new List<Parameter>(0);
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryWFProcesses", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 获取状态为使用的工作流过程列表
        /// </summary>
        /// <returns>工作流过程选择对象列表</returns>
        public IList<WFProcessSelectObject> GetUsedWFProcesses()
        {
            IList<WFProcessSelectObject> wfProcessSelectObjects = new List<WFProcessSelectObject>();
            IEnumerable<WFProcess> wfProcesses = wfProcessRepository.FindAll(Specification<WFProcess>.Eval(entity => entity.State == State.使用), "WFProcessCode");
            if (wfProcesses != null)
            {
                foreach (var wfProcess in wfProcesses)
                {
                    wfProcessSelectObjects.Add(MapperHelper.Map<WFProcess, WFProcessSelectObject>(wfProcess));
                }
            }
            return wfProcessSelectObjects;
        }

        /// <summary>
        /// 根据工作流类型Id获取状态为使用的工作流过程列表
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <returns>工作流过程选择对象列表</returns>
        public IList<WFProcessSelectObject> GetUsedWFProcessesByWFCategoryId(Guid wfCategoryId)
        {
            IList<WFProcessSelectObject> wfProcessSelectObjects = new List<WFProcessSelectObject>();
            IEnumerable<WFProcess> wfProcesses = wfProcessRepository.FindAll(Specification<WFProcess>.Eval(entity => entity.WFCategoryId == wfCategoryId && entity.State == State.使用), "WFProcessCode");
            if (wfProcesses != null)
            {
                foreach (var wfProcess in wfProcesses)
                {
                    wfProcessSelectObjects.Add(MapperHelper.Map<WFProcess, WFProcessSelectObject>(wfProcess));
                }
            }
            return wfProcessSelectObjects;
        }

        /// <summary>
        /// 新增或者修改工作流过程
        /// </summary>
        /// <param name="wfProcessMaintObject">要新增或者修改的工作流过程维护对象</param>
        public void AddOrUpdateWFProcess(WFProcessMaintObject wfProcessMaintObject)
        {
            if (wfProcessMaintObject.Id == Guid.Empty)
            {
                WFProcess wfProcess = AggregateFactory.CreateWFProcess(wfProcessMaintObject.WFCategoryId, wfProcessMaintObject.WFProcessCode, wfProcessMaintObject.WFProcessName,
                    (Bool)wfProcessMaintObject.IsApprovedByManager, wfProcessMaintObject.Remarks, (State)wfProcessMaintObject.State, wfProcessMaintObject.CreateUserId);
                wfProcessRepository.Add(wfProcess);
            }
            else
            {
                WFProcess wfProcess = wfProcessRepository.FindByKey(wfProcessMaintObject.Id);
                if (wfProcess != null)
                {
                    wfProcess.Modify(wfProcessMaintObject.WFProcessCode, wfProcessMaintObject.WFProcessName, (Bool)wfProcessMaintObject.IsApprovedByManager, wfProcessMaintObject.Remarks,
                        (State)wfProcessMaintObject.State, wfProcessMaintObject.ModifyUserId);
                    wfProcessRepository.Update(wfProcess);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_WFProcessCode"))
                {
                    throw new ApplicationFault("流程编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_WFProcessName"))
                {
                    throw new ApplicationFault("流程名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除工作流过程
        /// </summary>
        /// <param name="wfProcessMaintObjects">要删除的工作流过程维护对象列表</param>
        public void RemoveWFProcesses(IList<WFProcessMaintObject> wfProcessMaintObjects)
        {
            foreach (WFProcessMaintObject wfProcessMaintObject in wfProcessMaintObjects)
            {
                WFProcess wfProcess = wfProcessRepository.FindByKey(wfProcessMaintObject.Id);
                if (wfProcess != null)
                {
                    if (wfActivityRepository.Exists(Specification<WFActivity>.Eval(entity => entity.WFProcessId == wfProcess.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在流程步骤", wfProcess.WFProcessCode);
                    }
                    if (wfProcessInstanceRepository.Exists(Specification<WFProcessInstance>.Eval(entity => entity.WFProcessId == wfProcess.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已发送过流程公文", wfProcess.WFProcessCode);
                    }
                    wfProcessRepository.Remove(wfProcess);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_WFProcess_WFProcessId"))
                {
                    throw new ApplicationFault("已存在流程步骤");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFProcessInstance_dbo.tbl_WFProcess_WFProcessId"))
                {
                    throw new ApplicationFault("已发送过流程公文");
                }
                throw ex;
            }
        }
    }
}
