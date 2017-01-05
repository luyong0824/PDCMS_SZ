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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 业务量应用层服务
    /// </summary>
    public class BusinessVolumeService : DataService, IBusinessVolumeService
    {
        private readonly IRepository<BusinessVolume> businessVolumeRepository;

        public BusinessVolumeService(IRepositoryContext context,
            IRepository<BusinessVolume> businessVolumeRepository)
            : base(context)
        {
            this.businessVolumeRepository = businessVolumeRepository;
        }

        /// <summary>
        /// 根据业务量Id获取业务量
        /// </summary>
        /// <param name="id">业务量Id</param>
        /// <returns>业务量维护对象</returns>
        public BusinessVolumeMaintObject GetBusinessVolumeById(Guid id)
        {
            BusinessVolume businessVolume = businessVolumeRepository.FindByKey(id);
            if (businessVolume != null)
            {
                BusinessVolumeMaintObject businessVolumeMaintObject = MapperHelper.Map<BusinessVolume, BusinessVolumeMaintObject>(businessVolume);
                return businessVolumeMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的业务量在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改业务量
        /// </summary>
        /// <param name="businessVolumeMaintObject">要新增或者修改的业务量对象</param>
        public void AddOrUpdateBusinessVolume(BusinessVolumeMaintObject businessVolumeMaintObject)
        {
            if (businessVolumeMaintObject.Id != Guid.Empty)
            {
                BusinessVolume businessVolume = businessVolumeRepository.FindByKey(businessVolumeMaintObject.Id);
                if (businessVolume != null)
                {
                    businessVolume.Modify(businessVolumeMaintObject.LogicalNumber, businessVolumeMaintObject.TrafficVolumes, businessVolumeMaintObject.BusinessVolumes, businessVolumeMaintObject.ModifyUserId);
                    businessVolumeRepository.Update(businessVolume);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_BusinessVolumeLogicalTypeLogicalNumberCreateDate"))
                {
                    throw new ApplicationFault("逻辑号重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除业务量
        /// </summary>
        /// <param name="businessVolumeMaintObjects">要删除的业务量维护对象</param>
        public void RemoveBusinessVolumes(IList<BusinessVolumeMaintObject> businessVolumeMaintObjects)
        {
            foreach (BusinessVolumeMaintObject businessVolumeMaintObject in businessVolumeMaintObjects)
            {
                BusinessVolume businessVolume = businessVolumeRepository.FindByKey(businessVolumeMaintObject.Id);
                if (businessVolume != null)
                {
                    businessVolumeRepository.Remove(businessVolume);
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
        /// 根据条件获取分页业务量导入列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页业务量导入列表的Json字符串</returns>
        public string GetBusinessVolumesPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "LogicalType", Type = SqlDbType.Int, Value = logicalType });
            parameters.Add(new Parameter() { Name = "LogicalNumber", Type = SqlDbType.NVarChar, Value = logicalNumber });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBusinessVolumesPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取分页基站业务报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页基站业务报表的Json字符串</returns>
        public string GetBusinessVolumeReportPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBusinessVolumeReportPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取网格业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务报表的Json字符串</returns>
        public string GetBusinessVolumeReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeReseau", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取区域业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务报表的Json字符串</returns>
        public string GetBusinessVolumeArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeArea", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取公司业务报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务报表的Json字符串</returns>
        public string GetBusinessVolumeCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeCompany", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取基站业务月报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortOrder">排序方向</param>
        /// <returns>基站业务月报表的Json字符串</returns>
        public string GetBusinessVolumeMonthPlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId, string sortField, string sortOrder)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "SortField", Type = SqlDbType.VarChar, Value = sortField });
            parameters.Add(new Parameter() { Name = "SortOrder", Type = SqlDbType.VarChar, Value = sortOrder });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBusinessVolumeMonthPlace", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取网格业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务月报表的Json字符串</returns>
        public string GetBusinessVolumeMonthReseau(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthReseau", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取区域业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务月报表的Json字符串</returns>
        public string GetBusinessVolumeMonthArea(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthArea", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取公司业务月报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务月报表的Json字符串</returns>
        public string GetBusinessVolumeMonthCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthCompany", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取基站业务月增量报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务月增量报表的Json字符串</returns>
        public string GetBusinessVolumeMonthRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBusinessVolumeMonthRisePlace", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取基站业务年增量报表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务年增量报表的Json字符串</returns>
        public string GetBusinessVolumeYearRisePlace(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBusinessVolumeYearRisePlace", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取网格业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务月增量报表的Json字符串</returns>
        public string GetBusinessVolumeMonthRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthRiseReseau", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取网格业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务年增量报表的Json字符串</returns>
        public string GetBusinessVolumeYearRiseReseau(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearRiseReseau", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取区域业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务月增量报表的Json字符串</returns>
        public string GetBusinessVolumeMonthRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthRiseArea", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取区域业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务年增量报表的Json字符串</returns>
        public string GetBusinessVolumeYearRiseArea(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearRiseArea", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取公司业务月增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务月增量报表的Json字符串</returns>
        public string GetBusinessVolumeMonthRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeMonthRiseCompany", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取公司业务年增量报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务年增量报表的Json字符串</returns>
        public string GetBusinessVolumeYearRiseCompany(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearRiseCompany", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取网格年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="profession">专业</param>
        /// <returns>网格年度成长报表的Json字符串</returns>
        public string GetBusinessVolumeYearGrowthReseau(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearGrowthReseau", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取区域年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="profession">专业</param>
        /// <returns>区域年度成长报表的Json字符串</returns>
        public string GetBusinessVolumeYearGrowthArea(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearGrowthArea", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 根据条件获取公司年度成长报表
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="companyId">分公司Id</param>
        /// <param name="profession">专业</param>
        /// <returns>公司年度成长报表的Json字符串</returns>
        public string GetBusinessVolumeYearGrowthCompany(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryBusinessVolumeYearGrowthCompany", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = dt;
                return JsonHelper.Encode(result);
            }
        }
    }
}
