using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
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
    /// <summary>
    /// 物资名称应用层服务
    /// </summary>
    public class MaterialService : DataService, IMaterialService
    {
        private readonly IRepository<Material> materialRepository;
        private readonly IRepository<MaterialSpec> materialSpecRepository;
        private readonly IRepository<MaterialCategory> materialCategoryRepository;

        public MaterialService(IRepositoryContext context,
            IRepository<Material> materialRepository,
            IRepository<MaterialSpec> materialSpecRepository,
            IRepository<MaterialCategory> materialCategoryRepository)
            : base(context)
        {
            this.materialRepository = materialRepository;
            this.materialSpecRepository = materialSpecRepository;
            this.materialCategoryRepository = materialCategoryRepository;
        }

        /// <summary>
        /// 根据物资名称Id获取物资名称
        /// </summary>
        /// <param name="id">物资名称Id</param>
        /// <returns>物资名称维护对象</returns>
        public MaterialMaintObject GetMaterialById(Guid id)
        {
            Material material = materialRepository.FindByKey(id);
            if (material != null)
            {
                MaterialMaintObject materialMaintObject = MapperHelper.Map<Material, MaterialMaintObject>(material);
                return materialMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的物资名称在系统中不存在");
            }
        }

        /// <summary>
        /// 根据物资类别Id获取物资名称列表
        /// </summary>
        /// <param name="materialCategoryId">物资类别Id</param>
        /// <returns>物资名称维护对象列表</returns>
        public IList<MaterialMaintObject> GetMaterials(Guid materialCategoryId)
        {
            IList<MaterialMaintObject> materialMaintObjects = new List<MaterialMaintObject>();
            IEnumerable<Material> materials = materialRepository.FindAll(Specification<Material>.Eval(entity => entity.MaterialCategoryId == materialCategoryId), "MaterialCode");
            if (materials != null)
            {
                foreach (var material in materials)
                {
                    materialMaintObjects.Add(MapperHelper.Map<Material, MaterialMaintObject>(material));
                }
            }
            return materialMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的物资名称列表
        /// </summary>
        /// <returns>物资名称选择对象列表</returns>
        public IList<MaterialSelectObject> GetUsedMaterials(Guid materialCategoryId)
        {
            IList<MaterialSelectObject> materialSelectObjects = new List<MaterialSelectObject>();
            IEnumerable<Material> materials = materialRepository.FindAll(Specification<Material>.Eval(entity => entity.MaterialCategoryId == materialCategoryId && entity.State == State.使用), "MaterialCode");
            if (materials != null)
            {
                foreach (var material in materials)
                {
                    materialSelectObjects.Add(MapperHelper.Map<Material, MaterialSelectObject>(material));
                }
            }
            return materialSelectObjects;
        }

        /// <summary>
        /// 获取所有状态为使用的物资名称列表
        /// </summary>
        /// <returns>物资名称列表Json字符串</returns>
        public string GetAllUsedMaterials()
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = 1 });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetAllUsedMaterials", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据物资名称Id获取相同设计规格下所有状态为使用的物资名称列表
        /// </summary>
        /// <param name="materialId">物资名称Id</param>
        /// <returns>物资名称维护对象列表</returns>
        public IList<MaterialMaintObject> GetUsedMaterialsBySelf(Guid id)
        {
            Guid materialCategoryId = Guid.Empty;
            if (id != Guid.Empty)
            {
                Material m = materialRepository.GetByKey(id);
                materialCategoryId = m.MaterialCategoryId;
            }
            else
            {
                materialCategoryId = Guid.Empty;
            }
            IList<MaterialMaintObject> materialMaintObjects = new List<MaterialMaintObject>();
            IEnumerable<Material> materials = materialRepository.FindAll(Specification<Material>.Eval(entity => entity.MaterialCategoryId == materialCategoryId && entity.State == State.使用), "MaterialCode");
            if (materials != null)
            {
                foreach (var material in materials)
                {
                    materialMaintObjects.Add(MapperHelper.Map<Material, MaterialMaintObject>(material));
                }
            }
            return materialMaintObjects;
        }

        /// <summary>
        /// 新增或者修改物资名称
        /// </summary>
        /// <param name="materialMaintObject">要新增或者修改的物资名称维护对象</param>
        public void AddOrUpdateMaterial(MaterialMaintObject materialMaintObject)
        {
            if (materialMaintObject.Id == Guid.Empty)
            {
                Material material = AggregateFactory.CreateMaterial(materialMaintObject.MaterialCode, materialMaintObject.MaterialName,
                    materialMaintObject.MaterialCategoryId, (State)materialMaintObject.State, materialMaintObject.CreateUserId);
                materialRepository.Add(material);
            }
            else
            {
                Material material = materialRepository.FindByKey(materialMaintObject.Id);
                if (material != null)
                {
                    material.Modify(materialMaintObject.MaterialCode, materialMaintObject.MaterialName,
                        (State)materialMaintObject.State, materialMaintObject.ModifyUserId);
                    materialRepository.Update(material);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_MaterialCode"))
                {
                    throw new ApplicationFault("物资编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_MaterialName"))
                {
                    throw new ApplicationFault("物资名称名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Material_dbo.tbl_MaterialCategory_MaterialCategoryId"))
                {
                    throw new ApplicationFault("选择的物资类别在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除物资名称
        /// </summary>
        /// <param name="materialMaintObjects">要删除的物资名称维护对象列表</param>
        public void RemoveMaterials(IList<MaterialMaintObject> materialMaintObjects)
        {
            foreach (MaterialMaintObject materialMaintObject in materialMaintObjects)
            {
                Material material = materialRepository.FindByKey(materialMaintObject.Id);
                if (material != null)
                {
                    if (materialSpecRepository.Exists(Specification<MaterialSpec>.Eval(entity => entity.MaterialId == material.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在设计规格", material.MaterialCode);
                    }
                    materialRepository.Remove(material);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_MaterialSpec_dbo.tbl_Material_MaterialId"))
                {
                    throw new ApplicationFault("已存在设计规格");
                }
                throw ex;
            }
        }
    }
}
