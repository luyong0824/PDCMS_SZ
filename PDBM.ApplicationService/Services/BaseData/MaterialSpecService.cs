using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BaseData
{
    public class MaterialSpecService : DataService, IMaterialSpecService
    {
        private readonly IRepository<MaterialSpec> materialSpecRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Material> materialRepository;
        private readonly IRepository<MaterialList> materialListRepository;

        public MaterialSpecService(IRepositoryContext context,
            IRepository<MaterialSpec> materialSpecRepository,
            IRepository<Customer> customerRepository,
            IRepository<Material> materialRepository,
            IRepository<MaterialList> materialListRepository)
            : base(context)
        {
            this.materialSpecRepository = materialSpecRepository;
            this.customerRepository = customerRepository;
            this.materialRepository = materialRepository;
            this.materialListRepository = materialListRepository;
        }

        /// <summary>
        /// 根据设计规格Id获取设计规格
        /// </summary>
        /// <param name="id">设计规格Id</param>
        /// <returns>设计规格维护对象</returns>
        public MaterialSpecMaintObject GetMaterialSpecById(Guid id)
        {
            MaterialSpec materialSpec = materialSpecRepository.FindByKey(id);
            if (materialSpec != null)
            {
                MaterialSpecMaintObject materialSpecMaintObject = MapperHelper.Map<MaterialSpec, MaterialSpecMaintObject>(materialSpec);

                if (materialSpec.CustomerId != Guid.Empty)
                {
                    Customer customer = customerRepository.GetByKey(materialSpec.CustomerId.Value);
                    materialSpecMaintObject.CustomerName = customer.CustomerName;
                }
                else
                {
                    materialSpecMaintObject.CustomerName = "";
                }
                return materialSpecMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的设计规格在系统中不存在");
            }
        }

        /// <summary>
        /// 根据区域Id获取设计规格列表
        /// </summary>
        /// <param name="materialId">区域Id</param>
        /// <returns>设计规格维护对象列表</returns>
        public string GetMaterialSpecs(Guid materialId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "MaterialId", Type = SqlDbType.UniqueIdentifier, Value = materialId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_GetMaterialSpecs", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(1);
                result["data"] = ds.Tables[0];
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 获取状态为使用的设计规格列表
        /// </summary>
        /// <returns>设计规格选择对象列表</returns>
        public IList<MaterialSpecMaintObject> GetUsedMaterialSpecs(Guid materialId)
        {
            IList<MaterialSpecMaintObject> materialSpecMaintObjects = new List<MaterialSpecMaintObject>();
            IEnumerable<MaterialSpec> materialSpecs = materialSpecRepository.FindAll(Specification<MaterialSpec>.Eval(entity => entity.MaterialId == materialId && entity.State == State.使用), "MaterialSpecCode");
            if (materialSpecs != null)
            {
                foreach (var materialSpec in materialSpecs)
                {
                    materialSpecMaintObjects.Add(MapperHelper.Map<MaterialSpec, MaterialSpecMaintObject>(materialSpec));
                }
            }
            return materialSpecMaintObjects;
        }

        /// <summary>
        /// 新增或者修改设计规格
        /// </summary>
        /// <param name="materialSpecMaintObject">要新增或者修改的设计规格维护对象</param>
        public void AddOrUpdateMaterialSpec(MaterialSpecMaintObject materialSpecMaintObject)
        {
            if (materialSpecMaintObject.Id == Guid.Empty)
            {
                MaterialSpec materialSpec = AggregateFactory.CreateMaterialSpec(materialSpecMaintObject.MaterialSpecCode, materialSpecMaintObject.MaterialSpecName,
                    materialSpecMaintObject.MaterialId, materialSpecMaintObject.UnitId, materialSpecMaintObject.Price, materialSpecMaintObject.CustomerId, materialSpecMaintObject.Remarks, (State)materialSpecMaintObject.State, materialSpecMaintObject.CreateUserId);
                materialSpecRepository.Add(materialSpec);
            }
            else
            {
                MaterialSpec materialSpec = materialSpecRepository.FindByKey(materialSpecMaintObject.Id);
                if (materialSpec != null)
                {
                    materialSpec.Modify(materialSpecMaintObject.MaterialSpecCode, materialSpecMaintObject.MaterialSpecName, materialSpecMaintObject.UnitId, materialSpecMaintObject.Price, materialSpecMaintObject.CustomerId, materialSpecMaintObject.Remarks,
                        (State)materialSpecMaintObject.State, materialSpecMaintObject.ModifyUserId);
                    materialSpecRepository.Update(materialSpec);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_MaterialSpecCode"))
                {
                    throw new ApplicationFault("物资编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_MaterialSpecName"))
                {
                    throw new ApplicationFault("设计规格重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_MaterialSpec_dbo.tbl_MaterialSpecCategory_MaterialSpecCategoryId"))
                {
                    throw new ApplicationFault("选择的物资类别在系统中不存在");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_MaterialSpec_dbo.tbl_Unit_UnitId"))
                {
                    throw new ApplicationFault("选择的计量单位在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除设计规格
        /// </summary>
        /// <param name="materialSpecMaintObjects">要删除的设计规格维护对象列表</param>
        public void RemoveMaterialSpecs(IList<MaterialSpecMaintObject> materialSpecMaintObjects)
        {
            foreach (MaterialSpecMaintObject materialSpecMaintObject in materialSpecMaintObjects)
            {
                MaterialSpec materialSpec = materialSpecRepository.FindByKey(materialSpecMaintObject.Id);
                if (materialSpec != null)
                {
                    if (materialListRepository.Exists(Specification<MaterialList>.Eval(entity => entity.MaterialSpecId == materialSpec.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在物资清单中使用", materialSpec.MaterialSpecCode);
                    }
                    materialSpecRepository.Remove(materialSpec);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_MaterialList_dbo.tbl_MaterialSpec_MaterialSpecId"))
                {
                    throw new ApplicationFault("已在物资清单中使用");
                }
                throw ex;
            }
        }

        public MaterialSpecMaintObject GetSupplierCustomerNameByMaterialSpecId(Guid id)
        {
            MaterialSpec materialSpec = materialSpecRepository.FindByKey(id);
            if (materialSpec != null)
            {
                MaterialSpecMaintObject materialSpecMaintObject = MapperHelper.Map<MaterialSpec, MaterialSpecMaintObject>(materialSpec);

                if (materialSpec.CustomerId != Guid.Empty)
                {
                    Customer customer = customerRepository.GetByKey(materialSpec.CustomerId.Value);
                    materialSpecMaintObject.SupplierCustomerName = customer.CustomerName;
                }
                else
                {
                    materialSpecMaintObject.SupplierCustomerName = "";
                }
                return materialSpecMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的设计规格在系统中不存在");
            }
        }
    }
}
