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
    public class UnitService : DataService, IUnitService
    {
        private readonly IRepository<Unit> unitRepository;

        public UnitService(IRepositoryContext context,
            IRepository<Unit> unitRepository)
            : base(context)
        {
            this.unitRepository = unitRepository;
        }

        /// <summary>
        /// 根据计量单位Id获取计量单位
        /// </summary>
        /// <param name="id">计量单位Id</param>
        /// <returns>计量单位维护对象</returns>
        public UnitMaintObject GetUnitById(Guid id)
        {
            Unit unit = unitRepository.FindByKey(id);
            if (unit != null)
            {
                UnitMaintObject unitMaintObject = MapperHelper.Map<Unit, UnitMaintObject>(unit);
                return unitMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的计量单位在系统中不存在");
            }
        }

        /// <summary>
        /// 获取计量单位列表
        /// </summary>
        /// <returns>计量单位维护对象列表</returns>
        public IList<UnitMaintObject> GetUnits()
        {
            IList<UnitMaintObject> unitMaintObjects = new List<UnitMaintObject>();
            IEnumerable<Unit> units = unitRepository.FindAll(null, "UnitName");
            if (units != null)
            {
                foreach (var unit in units)
                {
                    unitMaintObjects.Add(MapperHelper.Map<Unit, UnitMaintObject>(unit));
                }
            }
            return unitMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的计量单位列表
        /// </summary>
        /// <returns>计量单位选择对象列表</returns>
        public IList<UnitMaintObject> GetUsedUnits()
        {
            IList<UnitMaintObject> unitMaintObjects = new List<UnitMaintObject>();
            IEnumerable<Unit> units = unitRepository.FindAll(Specification<Unit>.Eval(entity => entity.State == State.使用), "UnitName");
            if (units != null)
            {
                foreach (var unit in units)
                {
                    unitMaintObjects.Add(MapperHelper.Map<Unit, UnitMaintObject>(unit));
                }
            }
            return unitMaintObjects;
        }

        /// <summary>
        /// 新增或者修改计量单位
        /// </summary>
        /// <param name="unitMaintObject">要新增或者修改的计量单位维护对象</param>
        public void AddOrUpdateUnit(UnitMaintObject unitMaintObject)
        {
            if (unitMaintObject.Id == Guid.Empty)
            {
                Unit unit = AggregateFactory.CreateUnit(unitMaintObject.UnitName, (State)unitMaintObject.State, unitMaintObject.CreateUserId);
                unitRepository.Add(unit);
            }
            else
            {
                Unit unit = unitRepository.FindByKey(unitMaintObject.Id);
                if (unit != null)
                {
                    unit.Modify(unitMaintObject.UnitName, (State)unitMaintObject.State, unitMaintObject.ModifyUserId);
                    unitRepository.Update(unit);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_UnitName"))
                {
                    throw new ApplicationFault("计量单位名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除计量单位
        /// </summary>
        /// <param name="unitMaintObjects">要删除的计量单位维护对象列表</param>
        public void RemoveUnits(IList<UnitMaintObject> unitMaintObjects)
        {
            foreach (UnitMaintObject unitMaintObject in unitMaintObjects)
            {
                Unit unit = unitRepository.FindByKey(unitMaintObject.Id);
                if (unit != null)
                {
                    //if (materialSpecRepository.Exists(Specification<MaterialSpec>.Eval(entity => entity.UnitId == unit.Id)))
                    //{
                    //    throw new ApplicationFault("{0}<br>已在设计规格中使用", unit.UnitName);
                    //}
                    unitRepository.Remove(unit);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_MaterialSpec_dbo.tbl_Unit_UnitId"))
                {
                    throw new ApplicationFault("已在设计规格中使用");
                }
                throw ex;
            }
        }
    }
}
