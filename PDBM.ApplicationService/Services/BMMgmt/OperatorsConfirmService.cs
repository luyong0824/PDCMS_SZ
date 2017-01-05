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
using PDBM.Domain.Services;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 运营商确认应用层服务
    /// </summary>
    public class OperatorsConfirmService : DataService, IOperatorsConfirmService
    {
        private readonly IRepository<OperatorsConfirm> operatorsConfirmRepository;
        private readonly IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IBMMgmtService bmMgmtService;

        public OperatorsConfirmService(IRepositoryContext context,
            IRepository<OperatorsConfirm> operatorsConfirmRepository,
            IRepository<OperatorsConfirmDetail> operatorsConfirmDetailRepository,
            IRepository<Planning> planningRepository,
            IBMMgmtService bmMgmtService)
            : base(context)
        {
            this.operatorsConfirmRepository = operatorsConfirmRepository;
            this.operatorsConfirmDetailRepository = operatorsConfirmDetailRepository;
            this.planningRepository = planningRepository;
            this.bmMgmtService = bmMgmtService;
        }

        /// <summary>
        /// 根据条件获取分页运营商确认明细列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="demand">需求确认</param>
        /// <param name="companyId">运营商公司Id</param>
        /// <returns>分页运营商确认明细列表的Json字符串</returns>
        public string GetOperatorsConfirmDetailsPage(int pageIndex, int pageSize, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Demand", Type = SqlDbType.Int, Value = demand });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsConfirmDetailsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增运营商需求确认
        /// </summary>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="operatorsConfirmDetailMaintObjects">要新增的运营商需求确认明细维护对象列表</param>
        public void AddOperatorsConfirm(Guid createUserId, IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects)
        {
            OperatorsConfirm operatorsConfirm = AggregateFactory.CreateOperatorsConfirm(createUserId);
            foreach (OperatorsConfirmDetailMaintObject operatorsConfirmDetailMaintObject in operatorsConfirmDetailMaintObjects)
            {
                OperatorsConfirmDetail operatorsConfirmDetail = AggregateFactory.CreateOperatorsConfirmDetail(operatorsConfirm.Id, operatorsConfirmDetailMaintObject.PlanningId, 
                    Demand.未确认, Demand.未确认, Demand.未确认, Guid.Empty, Guid.Empty, Guid.Empty, createUserId);
                operatorsConfirmDetailRepository.Add(operatorsConfirmDetail);
                Planning planning = planningRepository.FindByKey(operatorsConfirmDetailMaintObject.PlanningId);
                if (planning != null)
                {
                    //planning.RequestDemand(createUserId, operatorsConfirmDetail.Id);
                    planningRepository.Update(planning);
                }
            }
            operatorsConfirmRepository.Add(operatorsConfirm);
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsConfirmDetail_dbo.tbl_Planning_PlanningId"))
                {
                    throw new ApplicationFault("选择的规划记录在系统中不存在");
                }
                else if (ex.Message.Contains("IX_UQ_OperatorsConfirmIdPlanningId"))
                {
                    throw new ApplicationFault("规划记录重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 需求确认
        /// </summary>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="currentCompanyId">当前操作人所在公司Id</param>
        /// <param name="currentCompanyNature">当前操作人所在公司性质</param>
        /// <param name="demand">是否需要</param>
        /// <param name="operatorsConfirmDetailMaintObjects">要确认的运营商确认明细维护对象列表</param>
        public void Confirm(Guid currentUserId, Guid currentCompanyId, int currentCompanyNature, int demand, IList<OperatorsConfirmDetailMaintObject> operatorsConfirmDetailMaintObjects)
        {
            foreach (OperatorsConfirmDetailMaintObject operatorsConfirmDetailMaintObject in operatorsConfirmDetailMaintObjects)
            {
                OperatorsConfirmDetail operatorsConfirmDetail = operatorsConfirmDetailRepository.GetByKey(operatorsConfirmDetailMaintObject.Id);
                operatorsConfirmDetail.Confirm(currentUserId, currentCompanyId, (CompanyNature)currentCompanyNature, (Demand)demand);
                Planning planning = planningRepository.GetByKey(operatorsConfirmDetail.PlanningId);
                operatorsConfirmDetailRepository.Update(operatorsConfirmDetail);
                planningRepository.Update(bmMgmtService.ModifyPlanningDemand(planning, operatorsConfirmDetail));
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
