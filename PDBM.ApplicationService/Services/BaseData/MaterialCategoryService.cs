using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 物资类别应用层服务
    /// </summary>
    public class MaterialCategoryService : DataService, IMaterialCategoryService
    {
        private readonly IRepository<MaterialCategory> materialCategoryRepository;
        private readonly IRepository<Material> materialRepository;

        public MaterialCategoryService(IRepositoryContext context,
            IRepository<MaterialCategory> materialCategoryRepository,
            IRepository<Material> materialRepository)
            : base(context)
        {
            this.materialCategoryRepository = materialCategoryRepository;
            this.materialRepository = materialRepository;
        }

        /// <summary>
        /// 根据物资类别Id获取物资类别
        /// </summary>
        /// <param name="id">物资类别Id</param>
        /// <returns>物资类别维护对象</returns>
        public MaterialCategoryMaintObject GetMaterialCategoryById(Guid id)
        {
            MaterialCategory materialCategory = materialCategoryRepository.FindByKey(id);
            if (materialCategory != null)
            {
                MaterialCategoryMaintObject materialCategoryMaintObject = MapperHelper.Map<MaterialCategory, MaterialCategoryMaintObject>(materialCategory);
                return materialCategoryMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的物资类别在系统中不存在");
            }
        }

        /// <summary>
        /// 获取物资类别列表
        /// </summary>
        /// <returns>物资类别维护对象列表</returns>
        public IList<MaterialCategoryMaintObject> GetMaterialCategorys()
        {
            IList<MaterialCategoryMaintObject> materialCategoryMaintObjects = new List<MaterialCategoryMaintObject>();
            IEnumerable<MaterialCategory> materialCategorys = materialCategoryRepository.FindAll(null, "MaterialCategoryCode");
            if (materialCategorys != null)
            {
                foreach (var materialCategory in materialCategorys)
                {
                    materialCategoryMaintObjects.Add(MapperHelper.Map<MaterialCategory, MaterialCategoryMaintObject>(materialCategory));
                }
            }
            return materialCategoryMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的物资类别列表
        /// </summary>
        /// <returns>物资类别选择对象列表</returns>
        public IList<MaterialCategorySelectObject> GetUsedMaterialCategorys()
        {
            IList<MaterialCategorySelectObject> materialCategorySelectObjects = new List<MaterialCategorySelectObject>();
            IEnumerable<MaterialCategory> materialCategorys = materialCategoryRepository.FindAll(Specification<MaterialCategory>.Eval(entity => entity.State == State.使用), "MaterialCategoryCode");
            if (materialCategorys != null)
            {
                foreach (var materialCategory in materialCategorys)
                {
                    materialCategorySelectObjects.Add(MapperHelper.Map<MaterialCategory, MaterialCategorySelectObject>(materialCategory));
                }
            }
            return materialCategorySelectObjects;
        }

        /// <summary>
        /// 新增或者修改物资类别
        /// </summary>
        /// <param name="materialCategoryMaintObject">要新增或者修改的物资类别维护对象</param>
        public void AddOrUpdateMaterialCategory(MaterialCategoryMaintObject materialCategoryMaintObject)
        {
            if (materialCategoryMaintObject.Id == Guid.Empty)
            {
                MaterialCategory materialCategory = AggregateFactory.CreateMaterialCategory(materialCategoryMaintObject.MaterialCategoryCode, materialCategoryMaintObject.MaterialCategoryName,
                    (State)materialCategoryMaintObject.State, materialCategoryMaintObject.CreateUserId);
                materialCategoryRepository.Add(materialCategory);
            }
            else
            {
                MaterialCategory materialCategory = materialCategoryRepository.FindByKey(materialCategoryMaintObject.Id);
                if (materialCategory != null)
                {
                    materialCategory.Modify(materialCategoryMaintObject.MaterialCategoryCode, materialCategoryMaintObject.MaterialCategoryName,
                        (State)materialCategoryMaintObject.State, materialCategoryMaintObject.ModifyUserId);
                    materialCategoryRepository.Update(materialCategory);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_MaterialCategoryCode"))
                {
                    throw new ApplicationFault("类别编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_MaterialCategoryName"))
                {
                    throw new ApplicationFault("类别名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除物资类别
        /// </summary>
        /// <param name="materialCategoryMaintObjects">要删除的物资类别维护对象列表</param>
        public void RemoveMaterialCategorys(IList<MaterialCategoryMaintObject> materialCategoryMaintObjects)
        {
            foreach (MaterialCategoryMaintObject materialCategoryMaintObject in materialCategoryMaintObjects)
            {
                MaterialCategory materialCategory = materialCategoryRepository.FindByKey(materialCategoryMaintObject.Id);
                if (materialCategory != null)
                {
                    if (materialRepository.Exists(Specification<Material>.Eval(entity => entity.MaterialCategoryId == materialCategory.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在物资名称", materialCategory.MaterialCategoryCode);
                    }
                    materialCategoryRepository.Remove(materialCategory);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Material_dbo.tbl_MaterialCategory_MaterialCategoryId"))
                {
                    throw new ApplicationFault("已存在物资名称");
                }
                throw ex;
            }
        }
    }
}
