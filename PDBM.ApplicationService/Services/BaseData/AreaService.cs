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
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using System.Data;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 区域应用层服务
    /// </summary>
    public class AreaService : DataService, IAreaService
    {
        private readonly IRepository<Area> areaRepository;
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<OperatorsPlanning> operatorsPlanningRepository;
        private readonly IRepository<User> userRepository;

        public AreaService(IRepositoryContext context,
            IRepository<Area> areaRepository,
            IRepository<Reseau> reseauRepository,
            IRepository<OperatorsPlanning> operatorsPlanningRepository,
            IRepository<User> userRepository)
            : base(context)
        {
            this.areaRepository = areaRepository;
            this.reseauRepository = reseauRepository;
            this.operatorsPlanningRepository = operatorsPlanningRepository;
            this.userRepository = userRepository;
        }

        /// <summary>
        /// 根据区域Id获取区域
        /// </summary>
        /// <param name="id">区域Id</param>
        /// <returns>区域维护对象</returns>
        public AreaMaintObject GetAreaById(Guid id)
        {
            Area area = areaRepository.FindByKey(id);
            if (area != null)
            {
                AreaMaintObject areaMaintObject = MapperHelper.Map<Area, AreaMaintObject>(area);
                if (area.AreaManagerId != Guid.Empty)
                {
                    if (area.AreaManagerId != null && area.AreaManagerId != Guid.Empty)
                    {
                        User user = userRepository.FindByKey(area.AreaManagerId.Value);
                        areaMaintObject.AreaManagerId = area.AreaManagerId;
                        areaMaintObject.AreaManagerFullName = user.FullName;
                    }
                    else
                    {
                        areaMaintObject.AreaManagerId = Guid.Empty;
                        areaMaintObject.AreaManagerFullName = "请选择";
                    }
                }
                else
                {
                    areaMaintObject.AreaManagerId = Guid.Empty;
                    areaMaintObject.AreaManagerFullName = "请选择";
                }
                return areaMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的区域在系统中不存在");
            }
        }

        /// <summary>
        /// 获取区域列表
        /// </summary>
        /// <returns>区域维护对象列表</returns>
        public IList<AreaMaintObject> GetAreas()
        {
            IList<AreaMaintObject> areaMaintObjects = new List<AreaMaintObject>();
            IEnumerable<Area> areas = areaRepository.FindAll(null, "AreaCode");
            if (areas != null)
            {
                foreach (var area in areas)
                {
                    areaMaintObjects.Add(MapperHelper.Map<Area, AreaMaintObject>(area));
                }
            }
            return areaMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的区域列表
        /// </summary>
        /// <returns>区域选择对象列表</returns>
        public IList<AreaSelectObject> GetUsedAreas()
        {
            IList<AreaSelectObject> areaSelectObjects = new List<AreaSelectObject>();
            IEnumerable<Area> areas = areaRepository.FindAll(Specification<Area>.Eval(entity => entity.State == State.使用), "AreaCode");
            if (areas != null)
            {
                foreach (var area in areas)
                {
                    areaSelectObjects.Add(MapperHelper.Map<Area, AreaSelectObject>(area));
                }
            }
            return areaSelectObjects;
        }

        /// <summary>
        /// 新增或者修改区域
        /// </summary>
        /// <param name="areaMaintObject">要新增或者修改的区域维护对象</param>
        public void AddOrUpdateArea(AreaMaintObject areaMaintObject)
        {
            if (areaMaintObject.Id == Guid.Empty)
            {
                Area area = AggregateFactory.CreateArea(areaMaintObject.AreaCode, areaMaintObject.AreaName, areaMaintObject.Lng,
                    areaMaintObject.Lat, areaMaintObject.AreaManagerId, areaMaintObject.Remarks, (State)areaMaintObject.State, areaMaintObject.CreateUserId);
                areaRepository.Add(area);
            }
            else
            {
                Area area = areaRepository.FindByKey(areaMaintObject.Id);
                if (area != null)
                {
                    area.Modify(areaMaintObject.AreaCode, areaMaintObject.AreaName, areaMaintObject.Lng, areaMaintObject.Lat, areaMaintObject.AreaManagerId,
                        areaMaintObject.Remarks, (State)areaMaintObject.State, areaMaintObject.ModifyUserId);
                    areaRepository.Update(area);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_AreaCode"))
                {
                    throw new ApplicationFault("区域编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_AreaName"))
                {
                    throw new ApplicationFault("区域名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="areaMaintObjects">要删除的区域维护对象列表</param>
        public void RemoveAreas(IList<AreaMaintObject> areaMaintObjects)
        {
            foreach (AreaMaintObject areaMaintObject in areaMaintObjects)
            {
                Area area = areaRepository.FindByKey(areaMaintObject.Id);
                if (area != null)
                {
                    if (reseauRepository.Exists(Specification<Reseau>.Eval(entity => entity.AreaId == area.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在网格", area.AreaCode);
                    }
                    else if (operatorsPlanningRepository.Exists(Specification<OperatorsPlanning>.Eval(entity => entity.AreaId == area.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在运营商规划中使用", area.AreaCode);
                    }
                    areaRepository.Remove(area);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId"))
                {
                    throw new ApplicationFault("已存在网格");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_OperatorsPlanning_dbo.tbl_Area_AreaId"))
                {
                    throw new ApplicationFault("已在运营商规划中使用");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 获取所有区域
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetAllAreas(int state)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "State", Type = SqlDbType.Int, Value = state });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryAreas", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
