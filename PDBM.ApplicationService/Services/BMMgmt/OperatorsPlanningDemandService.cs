using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
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
    public class OperatorsPlanningDemandService : DataService, IOperatorsPlanningDemandService
    {
        private readonly IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<Place> placeRepository;

        public OperatorsPlanningDemandService(IRepositoryContext context,
            IRepository<OperatorsPlanningDemand> operatorsPlanningDemandRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<Place> placeRepository)
            : base(context)
        {
            this.operatorsPlanningDemandRepository = operatorsPlanningDemandRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.placeRepository = placeRepository;
        }

        /// <summary>
        /// 根据条件获取分页改造站需求确认明细列表
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
        /// <returns>分页改造站需求确认明细列表的Json字符串</returns>
        public string GetOperatorsPlanningDemandsPage(int pageIndex, int pageSize, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int demand, Guid companyId)
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
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsPlanningDemandsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 生成改造站点需求确认任务
        /// </summary>
        /// <param name="operatorsPlanningDemandMaintObjects"></param>
        public void AddOrUpdateOperatorsPlanningDemand(IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects)
        {
            foreach (OperatorsPlanningDemandMaintObject operatorsPlanningDemandMaintObject in operatorsPlanningDemandMaintObjects)
            {
                if (operatorsPlanningDemandMaintObject.Id == Guid.Empty)
                {
                    OperatorsPlanningDemand operatorsPlanningDemand = AggregateFactory.CreateOperatorsPlanningDemand(operatorsPlanningDemandMaintObject.OperatorsPlanningId, operatorsPlanningDemandMaintObject.PlaceId, operatorsPlanningDemandMaintObject.CreateUserId);
                    operatorsPlanningDemandRepository.Add(operatorsPlanningDemand);
                    OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.FindByKey(operatorsPlanningDemandMaintObject.OperatorsPlanningId);
                    operatorsPlanning.DemandSolved(Demand.需要);
                    operatorsPlanningRepository.Update(operatorsPlanning);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_OperatorsPlanningIdPlaceId"))
                {
                    throw new ApplicationFault("不可重复指定相同的共享站点");
                }
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_OperatorsPlanning_OperatorsPlanningId"))
                {
                    throw new ApplicationFault("选择的运营商规划在系统中不存在");
                }
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanningDemand_dbo.tbl_Place_PlaceId"))
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
                throw ex;
            }
        }

        public void OperatorsPlanningDemandConfirm(Guid currentUserId, int demand, IList<OperatorsPlanningDemandMaintObject> operatorsPlanningDemandMaintObjects)
        {
            foreach (OperatorsPlanningDemandMaintObject operatorsPlanningDemandMaintObject in operatorsPlanningDemandMaintObjects)
            {
                OperatorsPlanningDemand operatorsPlanningDemand = operatorsPlanningDemandRepository.GetByKey(operatorsPlanningDemandMaintObject.Id);
                OperatorsPlanning operatorsPlanning = operatorsPlanningRepository.GetByKey(operatorsPlanningDemand.OperatorsPlanningId);
                Place place = placeRepository.GetByKey(operatorsPlanningDemand.PlaceId);
                //if (operatorsPlanning.Solved == Bool.否)
                //{
                if ((Demand)demand == Demand.需要)
                {
                    OperatorsSharing operatorsSharing = AggregateFactory.CreateOperatorsSharing((Profession)operatorsPlanning.Profession, place.PlaceCode, place.Id, 0,
                        operatorsPlanning.PoleNumber, operatorsPlanning.CabinetNumber, Urgency.一级, Bool.否, "", operatorsPlanning.CompanyId, null, operatorsPlanningDemandMaintObject.Id, currentUserId, CompanyNature.运营商);
                    operatorsSharingRepository.Add(operatorsSharing);
                    operatorsPlanning.ModifyIsToShared(Bool.是);
                    operatorsPlanningRepository.Update(operatorsPlanning);
                }
                //}
                //else
                //{
                if ((Demand)demand == Demand.不需要)
                {
                    OperatorsSharing operatorsSharing = operatorsSharingRepository.Find(Specification<OperatorsSharing>.Eval(entity => entity.OperatorsPlanningDemandId == operatorsPlanningDemandMaintObject.Id));
                    if (operatorsSharing != null)
                    {
                        if (operatorsSharing.RemodelingId == null)
                        {
                            operatorsSharingRepository.Remove(operatorsSharing);
                        }
                        else
                        {
                            throw new ApplicationFault("已关联基站改造安排");
                        }
                    }
                }
                //}
                //operatorsPlanning.DemandSolved((Demand)demand);
                operatorsPlanningDemand.Modify((Demand)demand, currentUserId);
                operatorsPlanningDemandRepository.Update(operatorsPlanningDemand);
                //operatorsPlanningRepository.Update(operatorsPlanning);
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
        /// 根据规划获取关联的运营商规划列表
        /// </summary>
        /// <param name="planningId">规划Id</param>
        /// <returns>运营商规划列表的Json字符串</returns>
        public string GetOperatorsPlanningsByOperatorsPlanningDemandId(Guid operatorsPlanningDemandId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "OperatorsPlanningDemandId", Type = SqlDbType.UniqueIdentifier, Value = operatorsPlanningDemandId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetOperatorsPlanningsByOperatorsPlanningDemandId", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
