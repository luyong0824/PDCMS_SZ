using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
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
    /// 规格型号信息应用层服务
    /// </summary>
    public class MaterialSpecListService : DataService, IMaterialSpecListService
    {
        private readonly IRepository<MaterialSpecList> materialSpecListRepository;

        public MaterialSpecListService(IRepositoryContext context,
            IRepository<MaterialSpecList> materialSpecListRepository)
            : base(context)
        {
            this.materialSpecListRepository = materialSpecListRepository;
        }

        /// <summary>
        /// 根据规格型号信息表Id获取规格型号信息
        /// </summary>
        /// <param name="id">规格型号信息表Id</param>
        /// <returns>规格型号信息维护对象</returns>
        public MaterialSpecListMaintObject GetMaterialSpecListById(Guid id)
        {
            MaterialSpecList materialSpecList = materialSpecListRepository.FindByKey(id);
            if (materialSpecList != null)
            {
                MaterialSpecListMaintObject materialSpecListMaintObject = MapperHelper.Map<MaterialSpecList, MaterialSpecListMaintObject>(materialSpecList);
                return materialSpecListMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的规格型号信息在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改规格型号信息
        /// </summary>
        /// <param name="materialSpecListMaintObject">要新增或者修改的规格型号信息维护对象</param>
        public void AddOrUpdateMaterialSpecList(MaterialSpecListMaintObject materialSpecListMaintObject)
        {
            if (materialSpecListMaintObject.Id == Guid.Empty)
            {
                MaterialSpecList materialSpecList = AggregateFactory.CreateMaterialSpecList(materialSpecListMaintObject.ProjectCode, materialSpecListMaintObject.CustomerName, (MaterialSpecType)materialSpecListMaintObject.MaterialSpecType, materialSpecListMaintObject.MaterialSpecName,
                    materialSpecListMaintObject.UnitPrice, materialSpecListMaintObject.SpecNumber, materialSpecListMaintObject.TotalPrice, materialSpecListMaintObject.OrderCode, State.使用, materialSpecListMaintObject.CreateUserId);
                materialSpecListRepository.Add(materialSpecList);
            }
            else
            {
                MaterialSpecList materialSpecList = materialSpecListRepository.FindByKey(materialSpecListMaintObject.Id);
                if (materialSpecList != null)
                {
                    materialSpecList.Modify(materialSpecListMaintObject.ProjectCode, materialSpecListMaintObject.CustomerName, (MaterialSpecType)materialSpecListMaintObject.MaterialSpecType, materialSpecListMaintObject.MaterialSpecName,
                    materialSpecListMaintObject.UnitPrice, materialSpecListMaintObject.SpecNumber, materialSpecListMaintObject.TotalPrice, materialSpecListMaintObject.OrderCode, State.使用, materialSpecListMaintObject.ModifyUserId);
                    materialSpecListRepository.Update(materialSpecList);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_MaterialSpecList_dbo.tbl_ProjectCodeList_ProjectCode"))
                {
                    throw new ApplicationFault("立项编号在立项信息表中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 获取分页导入采购清单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">型号类别</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="orderCode">订单编号</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <returns></returns>
        public string GetMaterialSpecListPage(int pageIndex, int pageSize, string projectCode, string customerName, int materialSpecType, string materialSpecName, string orderCode, Guid createUserId)
        {
            List<Parameter> parameters = new List<Parameter>(8);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "ProjectCode", Type = SqlDbType.NVarChar, Value = projectCode });
            parameters.Add(new Parameter() { Name = "CustomerName", Type = SqlDbType.NVarChar, Value = customerName });
            parameters.Add(new Parameter() { Name = "MaterialSpecType", Type = SqlDbType.Int, Value = materialSpecType });
            parameters.Add(new Parameter() { Name = "MaterialSpecName", Type = SqlDbType.NVarChar, Value = materialSpecName });
            parameters.Add(new Parameter() { Name = "OrderCode", Type = SqlDbType.NVarChar, Value = orderCode });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryMaterialSpecListPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 获取分页导出清单列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="projectCode">立项编号</param>
        /// <param name="projectType">建设方式</param>
        /// <param name="placeName">站点名称</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="projectManagerId">工程经理Id</param>
        /// <param name="customerName">供应商</param>
        /// <param name="materialSpecType">型号类别</param>
        /// <param name="materialSpecName">规格型号</param>
        /// <param name="orderCode">订单编号</param>
        /// <returns></returns>
        public string GetProjectCodeListAndMaterialSpecListPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, string projectCode, int projectType, string placeName, Guid reseauId, Guid projectManagerId, string customerName, int materialSpecType, string materialSpecName, string orderCode)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
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
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryProjectCodeListAndMaterialSpecListPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }
    }
}
