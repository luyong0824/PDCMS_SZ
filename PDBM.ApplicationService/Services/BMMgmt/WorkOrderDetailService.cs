using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
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
    public class WorkOrderDetailService : DataService, IWorkOrderDetailService
    {
        private readonly IRepository<WorkOrder> workOrderRepository;
        private readonly IRepository<WorkOrderDetail> workOrderDetailRepository;
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;

        public WorkOrderDetailService(IRepositoryContext context,
            IRepository<WorkOrder> workOrderRepository,
            IRepository<WorkOrderDetail> workOrderDetailRepository,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository)
            : base(context)
        {
            this.workOrderRepository = workOrderRepository;
            this.workOrderDetailRepository = workOrderDetailRepository;
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
        }

        /// <summary>
        /// 根据派工单明细Id获取派工单明细
        /// </summary>
        /// <param name="id">派工单明细Id</param>
        /// <returns>派工单明细维护对象</returns>
        public WorkOrderDetailMaintObject GetWorkOrderDetailById(Guid id)
        {
            WorkOrderDetail workOrderDetail = workOrderDetailRepository.FindByKey(id);
            if (workOrderDetail != null)
            {
                WorkOrderDetailMaintObject workOrderDetailMaintObject = MapperHelper.Map<WorkOrderDetail, WorkOrderDetailMaintObject>(workOrderDetail);
                workOrderDetailMaintObject.WorkOrderDetailId = id;
                workOrderDetailMaintObject.dp_WorkBeginDateText = workOrderDetail.WorkBeginDate.ToShortDateString();
                workOrderDetailMaintObject.sp_BeginHour = workOrderDetail.BeginHour;
                workOrderDetailMaintObject.sp_BeginMinute = workOrderDetail.BeginMinute;
                workOrderDetailMaintObject.dp_WorkEndDateText = workOrderDetail.WorkEndDate.ToShortDateString();
                workOrderDetailMaintObject.sp_EndHour = workOrderDetail.EndHour;
                workOrderDetailMaintObject.sp_EndMinute = workOrderDetail.EndMinute;
                workOrderDetailMaintObject.txt_ExecuteSituation = workOrderDetail.ExecuteSituation;
                workOrderDetailMaintObject.txt_MaterialConsumption = workOrderDetail.MaterialConsumption;
                workOrderDetailMaintObject.txt_PersonnelNumber = workOrderDetail.PersonnelNumber;
                workOrderDetailMaintObject.txt_CarType = workOrderDetail.CarType;
                workOrderDetailMaintObject.cb_IsFinish = workOrderDetail.IsFinish;
                return workOrderDetailMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的派工单明细明细在系统中不存在");
            }
        }

        public string GetWorkOrderDetail(Guid workOrderId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "WorkOrderId", Type = SqlDbType.UniqueIdentifier, Value = workOrderId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetWorkOrderDetail", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改派工单明细
        /// </summary>
        /// <param name="workOrderDetailMaintObject">要新增或者修改的派工单明细对象</param>
        public void AddOrUpdateWorkOrderDetail(WorkOrderDetailMaintObject workOrderDetailMaintObject)
        {
            if (workOrderDetailMaintObject.WorkOrderDetailId == Guid.Empty)
            {
                WorkOrderDetail workOrderDetail = AggregateFactory.CreateWorkOrderDetail(workOrderDetailMaintObject.WorkOrderId, workOrderDetailMaintObject.dp_WorkBeginDate, workOrderDetailMaintObject.sp_BeginHour,
                    workOrderDetailMaintObject.sp_BeginMinute, workOrderDetailMaintObject.dp_WorkEndDate, workOrderDetailMaintObject.sp_EndHour, workOrderDetailMaintObject.sp_EndMinute, workOrderDetailMaintObject.cb_IsFinish,
                    workOrderDetailMaintObject.txt_ExecuteSituation, workOrderDetailMaintObject.txt_MaterialConsumption, workOrderDetailMaintObject.txt_PersonnelNumber, workOrderDetailMaintObject.txt_CarType, workOrderDetailMaintObject.CreateUserId);
                workOrderDetailRepository.Add(workOrderDetail);
            }
            else
            {
                WorkOrderDetail workOrderDetail = workOrderDetailRepository.FindByKey(workOrderDetailMaintObject.WorkOrderDetailId);
                if (workOrderDetail != null)
                {
                    workOrderDetail.Modify(workOrderDetailMaintObject.dp_WorkBeginDate, workOrderDetailMaintObject.sp_BeginHour, workOrderDetailMaintObject.sp_BeginMinute, workOrderDetailMaintObject.dp_WorkEndDate,
                        workOrderDetailMaintObject.sp_EndHour, workOrderDetailMaintObject.sp_EndMinute, workOrderDetailMaintObject.cb_IsFinish, workOrderDetailMaintObject.txt_ExecuteSituation, workOrderDetailMaintObject.txt_MaterialConsumption,
                        workOrderDetailMaintObject.txt_PersonnelNumber, workOrderDetailMaintObject.txt_CarType, workOrderDetailMaintObject.ModifyUserId);
                    workOrderDetailRepository.Update(workOrderDetail);
                }
            }

            WFActivityInstanceEditor wfActivityInstanceEditors = wfActivityInstanceEditorRepository.Find(Specification<WFActivityInstanceEditor>.Eval(entity => entity.WFActivityInstanceId == workOrderDetailMaintObject.txt_WFActivityInstanceId));
            if (wfActivityInstanceEditors == null)
            {
                WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(workOrderDetailMaintObject.txt_WFActivityInstanceId);
                wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
        /// 删除各天执行情况
        /// </summary>
        /// <param name="workOrderDetailMaintObjects"></param>
        public void RemoveWorkOrderDetail(IList<WorkOrderDetailMaintObject> workOrderDetailMaintObjects)
        {
            foreach (WorkOrderDetailMaintObject workOrderDetailMaintObject in workOrderDetailMaintObjects)
            {
                WorkOrderDetail workOrderDetail = workOrderDetailRepository.FindByKey(workOrderDetailMaintObject.WorkOrderDetailId);
                if (workOrderDetail != null)
                {
                    workOrderDetailRepository.Remove(workOrderDetail);
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
    }
}
