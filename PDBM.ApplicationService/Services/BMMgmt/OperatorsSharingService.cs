using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
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
    /// 运营商共享应用层服务
    /// </summary>
    public class OperatorsSharingService : DataService, IOperatorsSharingService
    {
        private readonly IRepository<OperatorsSharing> operatorsSharingRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IBMMgmtService bmMgmtService;

        public OperatorsSharingService(IRepositoryContext context,
            IRepository<OperatorsSharing> operatorsSharingRepository,
            IRepository<Place> placeRepository,
            IRepository<Reseau> reseauRepository,
            IBMMgmtService bmMgmtService)
            : base(context)
        {
            this.operatorsSharingRepository = operatorsSharingRepository;
            this.placeRepository = placeRepository;
            this.reseauRepository = reseauRepository;
            this.bmMgmtService = bmMgmtService;
        }

        /// <summary>
        /// 根据运营商共享Id获取运营商共享
        /// </summary>
        /// <param name="id">运营商共享Id</param>
        /// <returns>运营商共享维护对象</returns>
        public OperatorsSharingMaintObject GetOperatorsSharingById(Guid id)
        {
            OperatorsSharing operatorsSharing = operatorsSharingRepository.FindByKey(id);
            if (operatorsSharing != null)
            {
                OperatorsSharingMaintObject operatorsSharingMaintObject = MapperHelper.Map<OperatorsSharing, OperatorsSharingMaintObject>(operatorsSharing);
                operatorsSharingMaintObject.CreateDateText = operatorsSharing.CreateDate.ToShortDateString();
                Place place = placeRepository.GetByKey(operatorsSharing.PlaceId);
                Reseau reseau = reseauRepository.GetByKey(place.ReseauId);
                operatorsSharingMaintObject.PlaceName = place.PlaceName;
                operatorsSharingMaintObject.AreaId = reseau.AreaId;
                operatorsSharingMaintObject.ReseauId = place.ReseauId;
                operatorsSharingMaintObject.PlaceCategoryId = place.PlaceCategoryId;
                operatorsSharingMaintObject.Lng = place.Lng;
                operatorsSharingMaintObject.Lat = place.Lat;
                //operatorsSharingMaintObject.PropertyRight = (int)place.PropertyRight;
                //operatorsSharingMaintObject.TelecomShare = (int)place.TelecomShare;
                //operatorsSharingMaintObject.MobileShare = (int)place.MobileShare;
                //operatorsSharingMaintObject.UnicomShare = (int)place.UnicomShare;
                return operatorsSharingMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的共享申请在系统中不存在");
            }
        }

        /// <summary>
        /// 根据条件获取分页运营商共享列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <param name="solved">是否采纳</param>
        /// <returns>分页运营商共享列表的Json字符串</returns>
        public string GetOperatorsSharingsPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeCode, string placeName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency, int solved)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Urgency", Type = SqlDbType.Int, Value = urgency });
            parameters.Add(new Parameter() { Name = "Solved", Type = SqlDbType.Int, Value = solved });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsSharingsPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页运营商共享列表，用于选择运营商共享申请
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeCode">站点编码</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="companyId">公司Id</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="urgency">紧要程度</param>
        /// <returns>分页运营商共享列表的Json字符串</returns>
        public string GetOperatorsSharingsPageBySelect(int pageIndex, int pageSize, string placeCode, string placeName, Guid companyId, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int urgency)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Urgency", Type = SqlDbType.Int, Value = urgency });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryOperatorsSharingsPageBySelect", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取指定站点的运营商共享列表
        /// </summary>
        /// <param name="operatorsSharingId">运营商共享Id</param>
        /// <param name="remodelingId">改造Id</param>
        /// <returns>运营商共享列表的Json字符串</returns>
        public string GetOperatorsSharingsByPlace(Guid operatorsSharingId, Guid remodelingId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "OperatorsSharingId", Type = SqlDbType.UniqueIdentifier, Value = operatorsSharingId });
            parameters.Add(new Parameter() { Name = "RemodelingId", Type = SqlDbType.UniqueIdentifier, Value = remodelingId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryOperatorsSharingsByPlace", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者修改运营商共享
        /// </summary>
        /// <param name="operatorsSharingMaintObject">要新增或者修改的运营商共享维护对象</param>
        public void AddOrUpdateOperatorsSharing(OperatorsSharingMaintObject operatorsSharingMaintObject)
        {
            if (operatorsSharingMaintObject.Id == Guid.Empty)
            {
                Place place = placeRepository.FindByKey(operatorsSharingMaintObject.PlaceId);
                OperatorsSharing operatorsSharing = AggregateFactory.CreateOperatorsSharing((Profession)operatorsSharingMaintObject.Profession, place.PlaceCode, operatorsSharingMaintObject.PlaceId, operatorsSharingMaintObject.PowerUsed,
                    operatorsSharingMaintObject.PoleNumber, operatorsSharingMaintObject.CabinetNumber, (Urgency)operatorsSharingMaintObject.Urgency, Bool.否, operatorsSharingMaintObject.Remarks, operatorsSharingMaintObject.CompanyId, null, Guid.Empty,
                    operatorsSharingMaintObject.CreateUserId, (CompanyNature)operatorsSharingMaintObject.CurrentCompanyNature);
                operatorsSharingRepository.Add(operatorsSharing);
            }
            else
            {
                OperatorsSharing operatorsSharing = operatorsSharingRepository.FindByKey(operatorsSharingMaintObject.Id);
                if (operatorsSharing != null)
                {
                    operatorsSharing.CheckByUpdate(operatorsSharingMaintObject.ModifyUserId);
                    operatorsSharing.Modify(operatorsSharingMaintObject.PlaceCode, operatorsSharingMaintObject.PlaceId, operatorsSharingMaintObject.PowerUsed, operatorsSharingMaintObject.PoleNumber, operatorsSharingMaintObject.CabinetNumber,
                        (Urgency)operatorsSharingMaintObject.Urgency, operatorsSharingMaintObject.Remarks, operatorsSharingMaintObject.ModifyUserId);
                    operatorsSharingRepository.Update(operatorsSharing);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsSharing_dbo.tbl_Place_PlaceId"))
                {
                    throw new ApplicationFault("选择的站点在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 运营商共享关联改造
        /// </summary>
        /// <param name="remodelingId">改造Id</param>
        /// <param name="remodelingCreateUserId">改造创建人用户Id</param>
        /// <param name="currentUserId">当前操作人用户Id</param>
        /// <param name="operatorsSharingMaintObjects">要关联的运营商共享维护对象列表</param>
        public void Associate(Guid remodelingId, Guid remodelingCreateUserId, Guid currentUserId, IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects)
        {
            bmMgmtService.CheckByRemodelingAssociate(remodelingCreateUserId, currentUserId);
            foreach (OperatorsSharingMaintObject operatorsSharingMaintObject in operatorsSharingMaintObjects)
            {
                if (operatorsSharingMaintObject.Id != Guid.Empty)
                {
                    OperatorsSharing operatorsSharing = operatorsSharingRepository.FindByKey(operatorsSharingMaintObject.Id);
                    if (operatorsSharing != null)
                    {
                        if (operatorsSharingMaintObject.RemodelingId == Guid.Empty)
                        {
                            operatorsSharing.Associate(remodelingId);
                        }
                        else
                        {
                            operatorsSharing.CancelAssociate();
                        }
                        operatorsSharingRepository.Update(operatorsSharing);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_OperatorsSharing_dbo.tbl_Remodeling_RemodelingId"))
                {
                    throw new ApplicationFault("选择的站点改造在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除运营商共享
        /// </summary>
        /// <param name="operatorsSharingMaintObjects">要删除的运营商共享维护对象列表</param>
        public void RemoveOperatorsSharings(IList<OperatorsSharingMaintObject> operatorsSharingMaintObjects)
        {
            foreach (OperatorsSharingMaintObject operatorsSharingMaintObject in operatorsSharingMaintObjects)
            {
                OperatorsSharing operatorsSharing = operatorsSharingRepository.FindByKey(operatorsSharingMaintObject.Id);
                if (operatorsSharing != null)
                {
                    operatorsSharing.CheckByRemove(operatorsSharingMaintObject.ModifyUserId);
                    operatorsSharingRepository.Remove(operatorsSharing);
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
