using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
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
    /// 物资清单应用层服务
    /// </summary>
    public class MaterialListService : DataService, IMaterialListService
    {
        private readonly IRepository<MaterialList> materialListRepository;
        private readonly IRepository<Material> materialRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IRepository<MaterialSpec> materialSpecRepository;
        private readonly IRepository<Customer> customerRepository;

        public MaterialListService(IRepositoryContext context,
            IRepository<MaterialList> materialListRepository,
            IRepository<Material> materialRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IRepository<MaterialSpec> materialSpecRepository,
            IRepository<Customer> customerRepository)
            : base(context)
        {
            this.materialListRepository = materialListRepository;
            this.materialRepository = materialRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.materialSpecRepository = materialSpecRepository;
            this.customerRepository = customerRepository;
        }

        /// <summary>
        /// 根据物资清单Id获取物资清单
        /// </summary>
        /// <param name="id">物资清单Id</param>
        /// <returns>物资清单维护对象</returns>
        public MaterialListMaintObject GetMaterialListById(Guid id)
        {
            MaterialList materialList = materialListRepository.FindByKey(id);
            if (materialList != null)
            {
                Material material = materialRepository.FindByKey(materialList.MaterialId);
                MaterialListMaintObject materialListMaintObject = MapperHelper.Map<MaterialList, MaterialListMaintObject>(materialList);
                materialListMaintObject.Id = id;
                materialListMaintObject.MaterialCategoryId = material.MaterialCategoryId;
                materialListMaintObject.MaterialId = materialList.MaterialId;
                materialListMaintObject.MaterialSpecId = materialList.MaterialSpecId;
                materialListMaintObject.SpecNumber = materialList.SpecNumber;
                materialListMaintObject.BudgetPrice = materialList.BudgetPrice;
                materialListMaintObject.Memos = materialList.Memos;
                materialListMaintObject.MaterialListId = id;
                materialListMaintObject.MaterialSpecId = materialList.MaterialSpecId;
                if (materialList.MaterialSpecId != Guid.Empty)
                {
                    MaterialSpec materialSpec = materialSpecRepository.FindByKey(materialList.MaterialSpecId.Value);
                    if (materialSpec.CustomerId != Guid.Empty)
                    {
                        Customer customer = customerRepository.FindByKey(materialSpec.CustomerId.Value);
                        materialListMaintObject.SupplierCustomerName = customer.CustomerName;
                    }
                    else
                    {
                        materialListMaintObject.SupplierCustomerName = "";
                    }
                }
                else
                {
                    materialListMaintObject.SupplierCustomerName = "";
                }
                return materialListMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的物资清单在系统中不存在");
            }
        }

        /// <summary>
        /// 获取物资清单
        /// </summary>
        /// <param name="parentId">父表Id</param>
        /// <param name="propertyType">资源类型</param>
        /// <returns></returns>
        public string GetMaterialList(Guid parentId, int propertyType)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetMaterialList", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 新增或者修改物资清单
        /// </summary>
        /// <param name="materialListMaintObject">要新增或者修改的物资清单对象</param>
        public void AddOrUpdateMaterialList(MaterialListMaintObject materialListMaintObject)
        {
            if (materialListMaintObject.Id == Guid.Empty)
            {
                MaterialList materialList = AggregateFactory.CreateMaterialList(materialListMaintObject.ParentId, (PropertyType)materialListMaintObject.PropertyType, materialListMaintObject.MaterialId, materialListMaintObject.MaterialSpecId.Value, materialListMaintObject.BudgetPrice, materialListMaintObject.SpecNumber, materialListMaintObject.Memos, materialListMaintObject.CreateUserId);
                materialListRepository.Add(materialList);
            }
            else
            {
                MaterialList materialList = materialListRepository.FindByKey(materialListMaintObject.Id);
                if (materialList != null)
                {
                    materialList.Modify(materialListMaintObject.MaterialId, materialListMaintObject.MaterialSpecId.Value, materialListMaintObject.BudgetPrice, materialListMaintObject.SpecNumber, materialListMaintObject.Memos, materialListMaintObject.ModifyUserId);
                    materialListRepository.Update(materialList);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_MaterialListParentIdPropertyTypeMaterialIdMaterialSpecId"))
                {
                    throw new ApplicationFault("同一物资名称下设计规格只能添加一次");
                }
                throw ex;
            }
        }

        public void RemoveMaterialList(IList<MaterialListMaintObject> materialListMaintObjects)
        {
            foreach (MaterialListMaintObject materialListMaintObject in materialListMaintObjects)
            {
                MaterialList materialList = materialListRepository.FindByKey(materialListMaintObject.Id);
                if (materialList != null)
                {
                    materialListRepository.Remove(materialList);
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
        /// 指定供应商
        /// </summary>
        /// <param name="materialListMaintObject"></param>
        public void SaveMaterialSpec(MaterialListMaintObject materialListMaintObject)
        {
            MaterialList materialList = materialListRepository.FindByKey(materialListMaintObject.Id);
            if (materialList != null)
            {
                MaterialSpec materialSpec = materialSpecRepository.FindByKey(materialListMaintObject.MaterialSpecId.Value);
                materialList.ModifySpec(materialListMaintObject.MaterialSpecId, materialSpec.CustomerId, materialListMaintObject.ModifyUserId);
                materialListRepository.Update(materialList);
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_MaterialListParentIdPropertyTypeMaterialIdMaterialSpecId"))
                {
                    throw new ApplicationFault("同一物资名称下设计规格只能添加一次");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 分页显示申购清单
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="placeName">基站名称</param>
        /// <param name="placeCategoryId">基站类型Id</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="reseauId">网格Id</param>
        /// <param name="materialName">物资名称</param>
        /// <param name="doState">申购状态</param>
        /// <returns></returns>
        public string GetMaterialPurchasePage(int pageIndex, int pageSize, string placeName, Guid placeCategoryId, Guid areaId, Guid reseauId, string materialName, int doState)
        {
            List<Parameter> parameters = new List<Parameter>(13);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "PlaceCategoryId", Type = SqlDbType.UniqueIdentifier, Value = placeCategoryId });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "ReseauId", Type = SqlDbType.UniqueIdentifier, Value = reseauId });
            parameters.Add(new Parameter() { Name = "MaterialName", Type = SqlDbType.NVarChar, Value = materialName });
            parameters.Add(new Parameter() { Name = "DoState", Type = SqlDbType.Int, Value = doState });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryMaterialPurchasePage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 申购确认
        /// </summary>
        /// <param name="materialListMaintObjects"></param>
        public void DoStateConfirm(IList<MaterialListMaintObject> materialListMaintObjects)
        {
            foreach (MaterialListMaintObject materialListMaintObject in materialListMaintObjects)
            {
                MaterialList materialList = materialListRepository.GetByKey(materialListMaintObject.Id);
                materialList.ModifyState((DoState)materialListMaintObject.DoState);
                materialListRepository.Update(materialList);
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
