using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Services;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.Domain.Specifications;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 运营商规划应用层服务
    /// </summary>
    public class OperatorsPlanningService : DataService, IOperatorsPlanningService
    {
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly ICodeSeedRepository codeSeedRepository;
        private readonly IBMMgmtService bmMgmtService;
        private readonly IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository;

        public OperatorsPlanningService(IRepositoryContext context,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            ICodeSeedRepository codeSeedRepository,
            IBMMgmtService bmMgmtService,
            IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository)
            : base(context)
        {
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.codeSeedRepository = codeSeedRepository;
            this.bmMgmtService = bmMgmtService;
            this.operatorsPlanningDemandRepository = operatorsPlanningDemandRepository;
        }

        /// <summary>
        /// 根据运营商规划Id获取运营商规划
        /// </summary>
        /// <param name="id">运营商规划Id</param>
        /// <returns>运营商规划维护对象</returns>
        public OperatorsPlanningMaintObject GetOperatorsPlanningById(Guid id)
        {
            OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(id);
            if (operatorsPlanning != null)
            {
                OperatorsPlanningMaintObject operatorsPlanningMaintObject = MapperHelper.Map<OperatorsPlanning, OperatorsPlanningMaintObject>(operatorsPlanning);
                operatorsPlanningMaintObject.CreateDateText = operatorsPlanning.CreateDate.ToShortDateString();
                return operatorsPlanningMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的运营商规划在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页运营商规划列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否关联</param>
        /// <returns>分页运营商规划列表的Json字符串</returns>
        public string GetOperatorsPlanningsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string planningCode, string planningName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency, int solved, int toShared)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Urgency", Type = SqlDbType.Int, Value = urgency });
            parameters.Add(new Parameter() { Name = "Solved", Type = SqlDbType.Int, Value = solved });
            parameters.Add(new Parameter() { Name = "ToShared", Type = SqlDbType.Int, Value = toShared });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsPlanningsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页运营商规划列表，用于选择运营商规划
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <returns>分页运营商规划列表的Json字符串</returns>
        public string GetOperatorsPlanningsPageBySelect(int pageIndex, int pageSize, string planningCode, string planningName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, int urgency)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Urgency", Type = SqlDbType.Int, Value = urgency });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsPlanningsPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取指定距离内的运营商规划列表
        /// </summary>
        /// <param name="id">运营商规划Id</param>
        /// <param name="planningId">规划Id</param>
        /// <param name="profession">专业</param>
        /// <param name="distance">距离(公里)</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        public string GetOperatorsPlanningsByDistance(Guid id, Guid planningId, int profession, decimal distance)
        {
            if (distance < 0 || distance > 2)
            {
                throw new ApplicationFault("无效的直线距离");
            }

            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "OperatorsPlanningId", Type = SqlDbType.UniqueIdentifier, Value = id });
            parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "Distance", Type = SqlDbType.Decimal, Value = distance });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryOperatorsPlanningsByDistance", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据规划获取关联的运营商规划列表
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        public string GetOperatorsPlanningsByPlanning(Guid planningId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "PlanningId", Type = SqlDbType.UniqueIdentifier, Value = planningId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByPlanning", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者修改运营商规划
        /// </summary>
        /// <param name="operatorsPlanningMaintObject">要新增或者修改的运营商规划维护对象</param>
        public void AddOrUpdateOperatorsPlanning(OperatorsPlanningMaintObject operatorsPlanningMaintObject)
        {
            if (operatorsPlanningMaintObject.Id == Guid.Empty)
            {
                OperatorsPlanning operatorsPlanning = AggregateFactory.CreateOperatorsPlanning(codeSeedRepository.GenerateCode("OperatorsPlanning"), operatorsPlanningMaintObject.PlanningName,
                    (Profession)operatorsPlanningMaintObject.Profession, operatorsPlanningMaintObject.PlaceCategoryId, operatorsPlanningMaintObject.AreaId, operatorsPlanningMaintObject.Lng,
                    operatorsPlanningMaintObject.Lat, operatorsPlanningMaintObject.AntennaHeight, operatorsPlanningMaintObject.PoleNumber, operatorsPlanningMaintObject.CabinetNumber,
                    (Urgency)operatorsPlanningMaintObject.Urgency, Bool.否, operatorsPlanningMaintObject.Remarks, operatorsPlanningMaintObject.CompanyId, null, operatorsPlanningMaintObject.CreateUserId,
                    (CompanyNature)operatorsPlanningMaintObject.CurrentCompanyNature);
                operatorsPlanningRepository.Add(operatorsPlanning);
            }
            else
            {
                OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningMaintObject.Id);
                if (operatorsPlanning != null)
                {
                    operatorsPlanning.CheckByUpdate(operatorsPlanningMaintObject.ModifyUserId);
                    operatorsPlanning.Modify(operatorsPlanningMaintObject.PlanningName, operatorsPlanningMaintObject.PlaceCategoryId, operatorsPlanningMaintObject.AreaId, operatorsPlanningMaintObject.Lng,
                        operatorsPlanningMaintObject.Lat, operatorsPlanningMaintObject.AntennaHeight, operatorsPlanningMaintObject.PoleNumber, operatorsPlanningMaintObject.CabinetNumber,
                        (Urgency)operatorsPlanningMaintObject.Urgency, operatorsPlanningMaintObject.Remarks, operatorsPlanningMaintObject.ModifyUserId);
                    operatorsPlanningRepository.Update(operatorsPlanning);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_OperatorsPlanningCode"))
                {
                    throw new ApplicationFault("规划编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_CompanyIdOperatorsPlanningName"))
                {
                    throw new ApplicationFault("规划名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("选择的站点类型在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId"))
                {
                    throw new ApplicationFault("选择的区域在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 运营商规划关联规划
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <param name="planningCreateUserId">规划创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="operatorsPlanningMaintObjects">要关联的运营商规划维护对象列表</param>
        public void Associate(Guid planningId, Guid planningCreateUserId, Guid currentUserId, IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            bmMgmtService.CheckByAssociate(planningCreateUserId, currentUserId);
            foreach (OperatorsPlanningMaintObject operatorsPlanningMaintObject in operatorsPlanningMaintObjects)
            {
                if (operatorsPlanningMaintObject.Id != Guid.Empty)
                {
                    OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningMaintObject.Id);
                    if (operatorsPlanning != null)
                    {
                        if (operatorsPlanningMaintObject.PlanningId == Guid.Empty)
                        {
                            operatorsPlanning.Associate(planningId);
                        }
                        else
                        {
                            operatorsPlanning.CancelAssociate();
                        }
                        operatorsPlanningRepository.Update(operatorsPlanning);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("选择的规划在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除运营商规划
        /// </summary>
        /// <param name="operatorsPlanningMaintObjects">要删除的运营商规划维护对象列表</param>
        public void RemoveOperatorsPlannings(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            foreach (OperatorsPlanningMaintObject operatorsPlanningMaintObject in operatorsPlanningMaintObjects)
            {
                OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningMaintObject.Id);
                if (operatorsPlanning != null)
                {
                    operatorsPlanning.CheckByRemove(operatorsPlanningMaintObject.ModifyUserId);
                    operatorsPlanningRepository.Remove(operatorsPlanning);
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

        public void DemandSolved(IList<OperatorsPlanningMaintObject> operatorsPlanningMaintObjects)
        {
            foreach (OperatorsPlanningMaintObject operatorsPlanningMaintObject in operatorsPlanningMaintObjects)
            {
                OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningMaintObject.Id);
                if (operatorsPlanning != null)
                {
                    operatorsPlanning.CheckByRemove(operatorsPlanningMaintObject.ModifyUserId);
                    operatorsPlanningRepository.Remove(operatorsPlanning);
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
