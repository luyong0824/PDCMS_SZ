using System;
using System.Collections;
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
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.DataOutput;

namespace PDBM.ApplicationService.Services.DataOutput
{
    public class DataOutputService : DataService, IDataOutputService
    {
        public DataOutputService(IRepositoryContext context)
            : base(context)
        {
        }

        /// <summary>
        /// 导出租赁人月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁人月报Json字符串</returns>
        public string ExportAddressingMonthUserExcel(DateTime beginDate, DateTime endDate, Guid departmentId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportAddressingMonthUserExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出租赁月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁月报Json字符串</returns>
        public string ExportAddressingMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportAddressingMonthReseauExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出部门建设月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门建设月报Json字符串</returns>
        public string ExportProjectTaskDepartmentExcel(DateTime beginDate, DateTime beginDateYear, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "BeginDateYear", Type = SqlDbType.DateTime, Value = beginDateYear });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectTaskDepartmentExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出项目经理月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目经理月报Json字符串</returns>
        public string ExportProjectTaskProjectManagerExcel(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "BeginDateYear", Type = SqlDbType.DateTime, Value = beginDateYear });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectTaskProjectManagerExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出公司年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司年度成长(话务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthCompanyTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthCompanyExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 导出公司年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司年度成长(业务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthCompanyBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthCompanyExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[1]);
            }
        }

        /// <summary>
        /// 导出区域年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域年度成长(话务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthAreaTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthAreaExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 导出区域年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域年度成长(业务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthAreaBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthAreaExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[1]);
            }
        }

        /// <summary>
        /// 导出网格年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格年度成长(话务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthReseauTVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthReseauExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 导出网格年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格年度成长(业务量)Json字符串</returns>
        public string ExportBusinessVolumeYearGrowthReseauBVExcel(DateTime beginDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_ExportBusinessVolumeYearGrowthReseauExcel", parameters))
            {
                return ExportHelper.DataTableToString(ds.Tables[1]);
            }
        }

        /// <summary>
        /// 导出公司业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>公司业务月清单Json字符串</returns>
        public string ExportBusinessVolumeMonthCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeMonthCompanyExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出区域业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>区域业务月清单Json字符串</returns>
        public string ExportBusinessVolumeMonthAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeMonthAreaExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出网格业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>网格业务月清单Json字符串</returns>
        public string ExportBusinessVolumeMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeMonthReseauExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出基站业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>基站业务月清单Json字符串</returns>
        public string ExportBusinessVolumeMonthPlaceExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeMonthPlaceExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出公司业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>公司业务清单Json字符串</returns>
        public string ExportBusinessVolumeCompanyExcel(DateTime beginDate, DateTime endDate, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(4);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeCompanyExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出区域业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>区域业务清单Json字符串</returns>
        public string ExportBusinessVolumeAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(5);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeAreaExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出网格业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>网格业务清单Json字符串</returns>
        public string ExportBusinessVolumeReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeReseauExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出基站业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>基站业务清单Json字符串</returns>
        public string ExportBusinessVolumeExcel(DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(7);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportBusinessVolumeExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出逻辑号业务清单
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="logicalType">逻辑号类型</param>
        /// <param name="logicalNumber">逻辑号</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>逻辑号业务清单Json字符串</returns>
        public string ExportLogicalBusinessVolumeExcel(DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "LogicalType", Type = SqlDbType.Int, Value = logicalType });
            parameters.Add(new Parameter() { Name = "LogicalNumber", Type = SqlDbType.NVarChar, Value = logicalNumber });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportLogicalBusinessVolumeExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出站点
        /// </summary>
        /// <param name="profession">专业</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="state">状态</param>
        /// <returns>站点的Json字符串</returns>
        public string ExportPlaceAllExcel(int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "PlaceOwner", Type = SqlDbType.UniqueIdentifier, Value = placeOwner });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportPlaceAllExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportLogicalNumbersExcel(string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark)
        {
            List<Parameter> parameters = new List<Parameter>(14);
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "G2Mark", Type = SqlDbType.Int, Value = g2Mark });
            parameters.Add(new Parameter() { Name = "D2Mark", Type = SqlDbType.Int, Value = d2Mark });
            parameters.Add(new Parameter() { Name = "G3Mark", Type = SqlDbType.Int, Value = g3Mark });
            parameters.Add(new Parameter() { Name = "G4Mark", Type = SqlDbType.Int, Value = g4Mark });
            parameters.Add(new Parameter() { Name = "G2Number", Type = SqlDbType.NVarChar, Value = g2Number });
            parameters.Add(new Parameter() { Name = "D2Number", Type = SqlDbType.NVarChar, Value = d2Number });
            parameters.Add(new Parameter() { Name = "G3Number", Type = SqlDbType.NVarChar, Value = g3Number });
            parameters.Add(new Parameter() { Name = "G4Number", Type = SqlDbType.NVarChar, Value = g4Number });
            parameters.Add(new Parameter() { Name = "AllMark", Type = SqlDbType.Int, Value = allMark });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportLogicalNumbersExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportPlacesBaseStationExcel(string placeCode, string placeName, Guid placeCategoryId, Guid placeOwner, Guid areaId, Guid reseauId, int importance, int state, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "PlaceCode", Type = SqlDbType.NVarChar, Value = placeCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "PlaceOwner", Type = SqlDbType.UniqueIdentifier, Value = placeOwner });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportPlacesBaseStationExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportOperatorsPlanningsExcel()
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = 1 });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportOperatorsPlanningsExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportConstructionTaskPlanningsExcel()
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = 1 });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportConstructionTaskPlanningsExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportConstructionTaskRemodeingsExcel()
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = 1 });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportConstructionTaskRemodeingsExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportMaterialPurchaseExcel(string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "MaterialName", Type = SqlDbType.NVarChar, Value = materialName });
            parameters.Add(new Parameter() { Name = "DoState", Type = SqlDbType.Int, Value = doState });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportMaterialPurchaseExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportProjectInformationExcel(DateTime beginDate, DateTime endDate, string propertyRightSql, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PropertyRightSql", Type = SqlDbType.NVarChar, Value = propertyRightSql });
            parameters.Add(new Parameter() { Name = "GroupPlaceCode", Type = SqlDbType.NVarChar, Value = groupPlaceCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ConstructionMethod", Type = SqlDbType.Int, Value = constructionMethod });
            parameters.Add(new Parameter() { Name = "ConstructionProgress", Type = SqlDbType.Int, Value = constructionProgress });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectInformationExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportWorkApplysExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "IsSoved", Type = SqlDbType.Int, Value = isSoved });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportWorkApplysExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportWorkOrdersExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(12);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "WorkBigClassId", Type = SqlDbType.UniqueIdentifier, Value = workBigClassId });
            parameters.Add(new Parameter() { Name = "WorkSmallClassId", Type = SqlDbType.UniqueIdentifier, Value = workSmallClassId });
            parameters.Add(new Parameter() { Name = "CustomerId", Type = SqlDbType.UniqueIdentifier, Value = customerId });
            parameters.Add(new Parameter() { Name = "MaintainContactMan", Type = SqlDbType.NVarChar, Value = maintainContactMan });
            parameters.Add(new Parameter() { Name = "SendUserId", Type = SqlDbType.UniqueIdentifier, Value = sendUserId });
            parameters.Add(new Parameter() { Name = "IsFinish", Type = SqlDbType.Int, Value = isFinish });
            parameters.Add(new Parameter() { Name = "OrderState", Type = SqlDbType.Int, Value = orderState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportWorkOrdersExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportWorkApplyProjectsExcel(DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId)
        {
            List<Parameter> parameters = new List<Parameter>(6);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "Title", Type = SqlDbType.NVarChar, Value = title });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "IsProject", Type = SqlDbType.Int, Value = isProject });
            parameters.Add(new Parameter() { Name = "SendUserId", Type = SqlDbType.UniqueIdentifier, Value = sendUserId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportWorkApplyProjectsExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        public string ExportProjectMaterial(DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode)
        {
            List<Parameter> parameters = new List<Parameter>(11);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "CustomerName", Type = SqlDbType.NVarChar, Value = customerName });
            parameters.Add(new Parameter() { Name = "MaterialSpecType", Type = SqlDbType.Int, Value = materialSpecType });
            parameters.Add(new Parameter() { Name = "MaterialSpecName", Type = SqlDbType.NVarChar, Value = materialSpecName });
            parameters.Add(new Parameter() { Name = "OrderCode", Type = SqlDbType.NVarChar, Value = orderCode });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectMaterial", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 根据条件导出租赁进度表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="planningCode">规划编码</param>
        /// <param name="planningName">规划名称</param>
        /// <param name="profession">专业</param>
        /// <param name="placeCategoryId">站点类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="addressingState">寻址状态</param>
        /// <param name="addressingDepartmentId">租赁部门</param>
        /// <param name="addressingUserId">租赁人</param>
        /// <param name="isAppoint">指定租赁</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>租赁进度表的Json字符串</returns>
        public string ExportAddressingReportExcel(DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession, Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(14);
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "PlanningCode", Type = SqlDbType.NVarChar, Value = planningCode });
            parameters.Add(new Parameter() { Name = "PlanningName", Type = SqlDbType.NVarChar, Value = planningName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "Importance", Type = SqlDbType.Int, Value = importance });
            parameters.Add(new Parameter() { Name = "AddressingState", Type = SqlDbType.Int, Value = addressingState });
            parameters.Add(new Parameter() { Name = "AddressingDepartmentId", Type = SqlDbType.UniqueIdentifier, Value = addressingDepartmentId });
            parameters.Add(new Parameter() { Name = "AddressingUserId", Type = SqlDbType.UniqueIdentifier, Value = addressingUserId });
            parameters.Add(new Parameter() { Name = "IsAppoint", Type = SqlDbType.Int, Value = isAppoint });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportAddressingReportExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出项目进度表
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="projectProgress">项目进度</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="isOverTime">是否超时</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目进度表的Json字符串</returns>
        public string ExportProjectProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(10);
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "ProjectType", Type = SqlDbType.Int, Value = projectType });
            parameters.Add(new Parameter() { Name = "ProjectProgress", Type = SqlDbType.Int, Value = projectProgress });
            parameters.Add(new Parameter() { Name = "ProjectManagerId", Type = SqlDbType.UniqueIdentifier, Value = projectManagerId });
            parameters.Add(new Parameter() { Name = "IsOverTime", Type = SqlDbType.Int, Value = isOverTime });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectProgressReportExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出项目设计清单
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="generalDesignId">总设单位Id</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>项目设计清单的Json字符串</returns>
        public string ExportProjectDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId, string designRealName,
            int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "GeneralDesignId", Type = SqlDbType.UniqueIdentifier, Value = generalDesignId });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportProjectDesignReportExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出工程进度表
        /// </summary>
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
        /// <param name="companyId">公司Id</param>
        /// <returns>工程进度表的Json字符串</returns>
        public string ExportEngineeringProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel, int engineeringProgress, int projectType,
            Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(12);
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
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportEngineeringProgressReportExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }

        /// <summary>
        /// 导出工程设计清单
        /// </summary>
        /// <param name="projectCode">项目编号</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="taskModel">工程名称</param>
        /// <param name="designRealName">设计人</param>
        /// <param name="designCustomerId">设计单位</param>
        /// <param name="profession">专业</param>
        /// <param name="companyId">公司Id</param>
        /// <returns>工程设计清单的Json字符串</returns>
        public string ExportEngineeringDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession,Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "TaskModel", Type = SqlDbType.Int, Value = taskModel });
            parameters.Add(new Parameter() { Name = "DesignRealName", Type = SqlDbType.NVarChar, Value = designRealName });
            parameters.Add(new Parameter() { Name = "DesignCustomerId", Type = SqlDbType.UniqueIdentifier, Value = designCustomerId });
            parameters.Add(new Parameter() { Name = "Profession", Type = SqlDbType.Int, Value = profession });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_ExportEngineeringDesignReportExcel", parameters))
            {
                return ExportHelper.DataTableToString(dt);
            }
        }
    }
}
