using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 站点类型应用层服务
    /// </summary>
    public class PlaceCategoryService : DataService, IPlaceCategoryService
    {
        private readonly IRepository<PlaceCategory> placeCategoryRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Purchase> purchaseRepository;

        public PlaceCategoryService(IRepositoryContext context,
            IRepository<PlaceCategory> placeCategoryRepository,
            IRepository<Place> placeRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<Planning> planningRepository,
            IRepository<Purchase> purchaseRepository)
            : base(context)
        {
            this.placeCategoryRepository = placeCategoryRepository;
            this.placeRepository = placeRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.planningRepository = planningRepository;
            this.purchaseRepository = purchaseRepository;
        }

        /// <summary>
        /// 根据站点类型Id获取站点类型
        /// </summary>
        /// <param name="id">站点类型Id</param>
        /// <returns>站点类型维护对象</returns>
        public PlaceCategoryMaintObject GetPlaceCategoryById(Guid id)
        {
            PlaceCategory placeCategory = placeCategoryRepository.FindByKey(id);
            if (placeCategory != null)
            {
                PlaceCategoryMaintObject placeCategoryMaintObject = MapperHelper.Map<PlaceCategory, PlaceCategoryMaintObject>(placeCategory);
                return placeCategoryMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点类型在系统中不存在");
            }
        }

        /// <summary>
        /// 根据专业获取站点类型列表
        /// </summary>
        /// <param name="profession">专业</param>
        /// <returns>站点类型维护对象列表</returns>
        public IList<PlaceCategoryMaintObject> GetPlaceCategorys(int profession)
        {
            IList<PlaceCategoryMaintObject> placeCategoryMaintObjects = new List<PlaceCategoryMaintObject>();
            IEnumerable<PlaceCategory> placeCategorys = placeCategoryRepository.FindAll(Specification<PlaceCategory>.Eval(entity => entity.Profession == (Profession)profession), "PlaceCategoryCode");
            if (placeCategorys != null)
            {
                foreach (var placeCategory in placeCategorys)
                {
                    placeCategoryMaintObjects.Add(MapperHelper.Map<PlaceCategory, PlaceCategoryMaintObject>(placeCategory));
                }
            }
            return placeCategoryMaintObjects;
        }

        /// <summary>
        /// 根据专业获取状态为使用的站点类型列表
        /// </summary>
        /// <param name="profession">专业</param>
        /// <returns>站点类型选择对象列表</returns>
        public IList<PlaceCategorySelectObject> GetUsedPlaceCategorys(int profession)
        {
            IList<PlaceCategorySelectObject> placeCategorySelectObjects = new List<PlaceCategorySelectObject>();
            IEnumerable<PlaceCategory> placeCategorys = placeCategoryRepository.FindAll(Specification<PlaceCategory>.Eval(entity => entity.State == State.使用 && entity.Profession == (Profession)profession), "PlaceCategoryCode");
            if (placeCategorys != null)
            {
                foreach (var placeCategory in placeCategorys)
                {
                    placeCategorySelectObjects.Add(MapperHelper.Map<PlaceCategory, PlaceCategorySelectObject>(placeCategory));
                }
            }
            return placeCategorySelectObjects;
        }

        /// <summary>
        /// 新增或者修改站点类型
        /// </summary>
        /// <param name="placeCategoryMaintObject">要新增或者修改的站点类型维护对象</param>
        public void AddOrUpdatePlaceCategory(PlaceCategoryMaintObject placeCategoryMaintObject)
        {
            if (placeCategoryMaintObject.Id == Guid.Empty)
            {
                PlaceCategory placeCategory = AggregateFactory.CreatePlaceCategory((Profession)placeCategoryMaintObject.Profession, placeCategoryMaintObject.PlaceCategoryCode,
                    placeCategoryMaintObject.PlaceCategoryName, placeCategoryMaintObject.Remarks, (State)placeCategoryMaintObject.State, placeCategoryMaintObject.CreateUserId);
                placeCategoryRepository.Add(placeCategory);
            }
            else
            {
                PlaceCategory placeCategory = placeCategoryRepository.FindByKey(placeCategoryMaintObject.Id);
                if (placeCategory != null)
                {
                    placeCategory.Modify(placeCategoryMaintObject.PlaceCategoryCode, placeCategoryMaintObject.PlaceCategoryName, placeCategoryMaintObject.Remarks,
                        (State)placeCategoryMaintObject.State, placeCategoryMaintObject.ModifyUserId);
                    placeCategoryRepository.Update(placeCategory);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_ProfessionPlaceCategoryCode"))
                {
                    throw new ApplicationFault("站点类型编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_ProfessionPlaceCategoryName"))
                {
                    throw new ApplicationFault("站点类型名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除站点类型
        /// </summary>
        /// <param name="placeCategoryMaintObjects">要删除的站点类型维护对象列表</param>
        public void RemovePlaceCategorys(IList<PlaceCategoryMaintObject> placeCategoryMaintObjects)
        {
            foreach (PlaceCategoryMaintObject placeCategoryMaintObject in placeCategoryMaintObjects)
            {
                PlaceCategory placeCategory = placeCategoryRepository.FindByKey(placeCategoryMaintObject.Id);
                if (placeCategory != null)
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceCategoryId == placeCategory.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在站点中使用", placeCategory.PlaceCategoryCode);
                    }
                    if (operatorsPlanningRepository.Exists(Specification<OperatorsPlanning>.Eval(entity => entity.PlaceCategoryId == placeCategory.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在运营商规划中使用", placeCategory.PlaceCategoryCode);
                    }
                    else if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.PlaceCategoryId == placeCategory.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在规划中使用", placeCategory.PlaceCategoryCode);
                    }
                    else if (purchaseRepository.Exists(Specification<Purchase>.Eval(entity => entity.PlaceCategoryId == placeCategory.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在购置站点中使用", placeCategory.PlaceCategoryCode);
                    }
                    placeCategoryRepository.Remove(placeCategory);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("已在站点中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("已在运营商规划中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("已在规划中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_PlaceCategory_PlaceCategoryId"))
                {
                    throw new ApplicationFault("已在购置站点中使用");
                }
                throw ex;
            }
        }
    }
}
