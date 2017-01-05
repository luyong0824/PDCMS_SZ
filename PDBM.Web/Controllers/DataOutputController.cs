using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.BaseData;
using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.IoC;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.BMMgmt;
using PDBM.ServiceContracts.Enum;
using PDBM.Web.Filters;
using System.Data;
using System.IO;
using System.Text;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.ServiceContracts.DataOutput;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 数据导出控制器
    /// </summary>
    [AuthorizeFilter]
    public class DataOutputController : BaseController
    {
        /// <summary>
        /// 导出租人赁月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <returns>租赁人月报Json字符串</returns>
        public async Task<ActionResult> ExportAddressingMonthUserExcel(DateTime beginDate, DateTime endDate, Guid departmentId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportAddressingMonthUserExcel(beginDate, endDate, departmentId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出租赁月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <returns>租赁月报Json字符串</returns>
        public async Task<ActionResult> ExportAddressingMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportAddressingMonthReseauExcel(beginDate, endDate, areaId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出部门建设月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="profession">专业</param>
        /// <returns>部门建设月报Json字符串</returns>
        public async Task<ActionResult> ExportProjectTaskDepartmentExcel(DateTime beginDate, DateTime beginDateYear, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectTaskDepartmentExcel(beginDate, beginDateYear, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目经理月报
        /// </summary>
        /// <param name="beginDate">起始日期</param>
        /// <param name="beginDateYear">起始年份1月1号</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="profession">专业</param>
        /// <returns>项目经理月报Json字符串</returns>
        public async Task<ActionResult> ExportProjectTaskProjectManagerExcel(DateTime beginDate, DateTime beginDateYear, Guid departmentId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectTaskProjectManagerExcel(beginDate, beginDateYear, departmentId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出公司年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>公司年度成长(话务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthCompanyTVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthCompanyTVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出公司年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>公司年度成长(业务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthCompanyBVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthCompanyBVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出区域年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>区域年度成长(话务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthAreaTVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthAreaTVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出区域年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>区域年度成长(业务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthAreaBVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthAreaBVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出网格年度成长(话务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>网格年度成长(话务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthReseauTVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthReseauTVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出网格年度成长(业务量)
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="profession">专业</param>
        /// <returns>网格年度成长(业务量)Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeYearGrowthReseauBVExcel(DateTime beginDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeYearGrowthReseauBVExcel(beginDate, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出公司业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <returns>公司业务月清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeMonthCompanyExcel(DateTime beginDate, DateTime endDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeMonthCompanyExcel(beginDate, endDate.AddMonths(1), profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出区域业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <returns>区域业务月清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeMonthAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeMonthAreaExcel(beginDate, endDate.AddMonths(1), areaId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出网格业务月清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <returns>网格业务月清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeMonthReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeMonthReseauExcel(beginDate, endDate.AddMonths(1), areaId, reseauId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
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
        /// <returns>基站业务月清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeMonthPlaceExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, string placeName, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeMonthPlaceExcel(beginDate, endDate.AddMonths(1), areaId, reseauId, placeName, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出区域业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="profession">专业</param>
        /// <returns>区域业务清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeCompanyExcel(DateTime beginDate, DateTime endDate, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeCompanyExcel(beginDate, endDate.AddDays(1), profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出区域业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="profession">专业</param>
        /// <returns>区域业务清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeAreaExcel(DateTime beginDate, DateTime endDate, Guid areaId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeAreaExcel(beginDate, endDate.AddDays(1), areaId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出网格业务清单
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="profession">专业</param>
        /// <returns>网格业务清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeReseauExcel(DateTime beginDate, DateTime endDate, Guid areaId, Guid reseauId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeReseauExcel(beginDate, endDate.AddDays(1), areaId, reseauId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
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
        /// <returns>基站业务清单Json字符串</returns>
        public async Task<ActionResult> ExportBusinessVolumeExcel(DateTime beginDate, DateTime endDate, string placeName, Guid areaId, Guid reseauId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportBusinessVolumeExcel(beginDate, endDate.AddDays(1), placeName, areaId, reseauId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
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
        public async Task<ActionResult> ExportLogicalBusinessVolumeExcel(DateTime beginDate, DateTime endDate, int logicalType, string logicalNumber, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportLogicalBusinessVolumeExcel(beginDate, endDate.AddDays(1), logicalType, logicalNumber, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出站点
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportPlaceAllExcel(int profession, string placeName, Guid areaId, Guid reseauId, Guid placeOwner, int state)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportPlaceAllExcel(profession, placeName, areaId, reseauId, placeOwner, state));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目设计清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportEngineeringDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel,
            string designRealName, Guid designCustomerId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportEngineeringDesignReportExcel(projectCode, placeName, areaId, reseauId, taskModel, designRealName, designCustomerId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportEngineeringProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int taskModel, int engineeringProgress, int projectType,
            Guid projectManagerId, Guid constructionCustomerId, Guid supervisionCustomerId, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportEngineeringProgressReportExcel(projectCode, placeName, areaId, reseauId, taskModel, engineeringProgress, projectType, projectManagerId, constructionCustomerId, supervisionCustomerId, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目设计清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportProjectDesignReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, Guid generalDesignId, string designRealName, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectDesignReportExcel(projectCode, placeName, areaId, reseauId, generalDesignId, designRealName, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportProjectProgressReportExcel(string projectCode, string placeName, Guid areaId, Guid reseauId, int projectType,
            int projectProgress, Guid projectManagerId, int isOverTime, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectProgressReportExcel(projectCode, placeName, areaId, reseauId, projectType, projectProgress, projectManagerId, isOverTime, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出租赁进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportAddressingReportExcel(DateTime beginDate, DateTime endDate, string planningCode, string planningName, int profession,
            Guid placeCategoryId, Guid areaId, Guid reseauId, int importance, int addressingState, Guid addressingDepartmentId, Guid addressingUserId, int isAppoint)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportAddressingReportExcel(beginDate, endDate, planningCode, planningName, profession, placeCategoryId, areaId, reseauId, importance, addressingState, addressingDepartmentId, addressingUserId, isAppoint, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出逻辑号
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportLogicalNumbersExcel(string placeCode, string placeName, int profession, Guid areaId, Guid reseauId,
            int g2Mark, int d2Mark, int g3Mark, int g4Mark, string g2Number, string d2Number, string g3Number, string g4Number, int allMark)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportLogicalNumbersExcel(placeCode, placeName, profession, areaId, reseauId, g2Mark, d2Mark, g3Mark, g4Mark, g2Number, d2Number, g3Number, g4Number, allMark));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出基站清单
        /// </summary>
        /// <param name="placeCode">基站编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型</param>
        /// <param name="placeOwner">产权</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="importance">重要性程度</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public async Task<ActionResult> ExportPlacesBaseStationExcel(string placeCode, string placeName, Guid placeCategoryId, Guid placeOwner, Guid areaId, Guid reseauId, int importance, int state, int profession)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportPlacesBaseStationExcel(placeCode, placeName, placeCategoryId, placeOwner, areaId, reseauId, importance, state, profession, this.CompanyId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 新增基站建设进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportConstructionTaskPlanningsExcel()
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportConstructionTaskPlanningsExcel());
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 改造基站建设进度表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportConstructionTaskRemodeingsExcel()
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportConstructionTaskRemodeingsExcel());
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 运营商规划清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportOperatorsPlanningsExcel()
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportOperatorsPlanningsExcel());
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出申购清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportMaterialPurchaseExcel(string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportMaterialPurchaseExcel(placeName, placeCategoryId, areaId, reseauId, materialName, doState));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出项目信息表
        /// </summary>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="propertyRightList">产权</param>
        /// <param name="groupPlaceCode">站点编码</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="constructionMethod">建设方式</param>
        /// <param name="constructionProgress">建设进度</param>
        /// <returns></returns>
        public async Task<ActionResult> ExportProjectInformationExcel(DateTime beginDate, DateTime endDate, string propertyRightList, string groupPlaceCode, string placeName, Guid areaId, Guid reseauId, int constructionMethod, int constructionProgress)
        {
            string propertyRightSql = "";
            if (propertyRightList.Trim() != "")
            {
                string[] strPropertyRightList = propertyRightList.Trim().Split(',');
                for (int i = 0; i < strPropertyRightList.Length; i++)
                {
                    propertyRightSql += "select " + strPropertyRightList[i];
                    if (i != strPropertyRightList.Length - 1)
                    {
                        propertyRightSql += " union ";
                    }
                }
            }

            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectInformationExcel(beginDate, endDate, propertyRightSql, groupPlaceCode, placeName, areaId, reseauId, constructionMethod, constructionProgress));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 隐患上报清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportWorkApplysExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid customerId, int orderState, int isSoved, Guid createUserId)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportWorkApplysExcel(beginDate, endDate, title, reseauId, customerId, orderState, isSoved, createUserId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 零星派工清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportWorkOrdersExcel(DateTime beginDate, DateTime endDate, string title, Guid reseauId, Guid workBigClassId, Guid workSmallClassId, Guid customerId, string maintainContactMan, Guid sendUserId, int isFinish, int orderState, Guid createUserId)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportWorkOrdersExcel(beginDate, endDate, title, reseauId, workBigClassId, workSmallClassId, customerId, maintainContactMan, sendUserId, isFinish, orderState, createUserId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 隐患立项清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportWorkApplyProjectsExcel(DateTime beginDate, DateTime endDate, string title, string projectCode, int isProject, Guid sendUserId)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportWorkApplyProjectsExcel(beginDate, endDate, title, projectCode, isProject, sendUserId));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 导出清单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ExportProjectMaterial(DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode)
        {
            using (ServiceProxy<IDataOutputService> proxy = new ServiceProxy<IDataOutputService>())
            {
                string data = await Task.Factory.StartNew(() => proxy.Channel.ExportProjectMaterial(beginDate, endDate, projectCode, projectType, placeName, reseauId, projectManagerId, customerName, materialSpecType, materialSpecName, orderCode));
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                //Response.Charset = "UTF-8";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Sheet1" + ".xls");
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
                Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
                //EnableViewState = false;
                Response.Write(data);
                Response.End();
            }
            return new EmptyResult();
        }

        public void ExportExcel(ArrayList columns, ArrayList data)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            //Response.Charset = "UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + "grid" + ".xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
            Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
            //EnableViewState = false;
            Response.Write(ExportTable(data, columns));
            Response.End();

        }
        public static string ExportTable(ArrayList data, ArrayList columns)
        {
            StringBuilder sb = new StringBuilder();
            int count = 0;
            sb.AppendLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=gb2312\">");
            sb.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
            //写出列名
            sb.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
            foreach (Hashtable column in columns)
            {
                sb.AppendLine("<td>" + column["header"] + "</td>");
            }
            sb.AppendLine("</tr>");

            //写出数据
            foreach (Hashtable row in data)
            {
                sb.Append("<tr>");
                foreach (Hashtable column in columns)
                {
                    if (column["field"] == null) continue;
                    Object value = row[column["field"]];
                    sb.AppendLine("<td>" + value + "</td>");
                }
                sb.AppendLine("</tr>");
                count++;
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
    }
}