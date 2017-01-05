using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.BMMgmt;
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
    /// 网格应用层服务
    /// </summary>
    public class ReseauService : DataService, IReseauService
    {
        private readonly IRepository<Reseau> reseauRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Planning> planningRepository;
        private readonly IRepository<Purchase> purchaseRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WorkApply> workApplyRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;

        public ReseauService(IRepositoryContext context,
            IRepository<Reseau> reseauRepository,
            IRepository<Place> placeRepository,
            IRepository<Planning> planningRepository,
            IRepository<Purchase> purchaseRepository,
            IRepository<User> userRepository,
            IRepository<WorkApply> workApplyRepository,
            IRepository<WorkOrder> workOrderRepository)
            : base(context)
        {
            this.reseauRepository = reseauRepository;
            this.placeRepository = placeRepository;
            this.planningRepository = planningRepository;
            this.purchaseRepository = purchaseRepository;
            this.userRepository = userRepository;
            this.workApplyRepository = workApplyRepository;
            this.workOrderRepository = workOrderRepository;
        }

        /// <summary>
        /// 根据网格Id获取网格
        /// </summary>
        /// <param name="id">网格Id</param>
        /// <returns>网格维护对象</returns>
        public ReseauMaintObject GetReseauById(Guid id)
        {
            Reseau reseau = reseauRepository.FindByKey(id);
            if (reseau != null)
            {
                ReseauMaintObject reseauMaintObject = MapperHelper.Map<Reseau, ReseauMaintObject>(reseau);
                if (reseau.ReseauManagerId != null && reseau.ReseauManagerId != Guid.Empty)
                {
                    if (reseau.ReseauManagerId != null && reseau.ReseauManagerId != Guid.Empty)
                    {
                        User user = userRepository.GetByKey(reseau.ReseauManagerId.Value);
                        reseauMaintObject.FullName = user.FullName;
                    }
                    else
                    {
                        reseauMaintObject.FullName = "无";
                    }
                }
                else
                {
                    reseauMaintObject.FullName = "无";
                }
                return reseauMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的网格在系统中不存在");
            }
        }

        /// <summary>
        /// 根据区域Id获取网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格维护对象列表</returns>
        public IList<ReseauMaintObject> GetReseaus(Guid areaId)
        {
            IList<ReseauMaintObject> reseauMaintObjects = new List<ReseauMaintObject>();
            IEnumerable<Reseau> reseaus = reseauRepository.FindAll(Specification<Reseau>.Eval(entity => entity.AreaId == areaId), "ReseauCode");
            if (reseaus != null)
            {
                foreach (var reseau in reseaus)
                {
                    reseauMaintObjects.Add(MapperHelper.Map<Reseau, ReseauMaintObject>(reseau));
                }
            }
            return reseauMaintObjects;
        }

        /// <summary>
        /// 根据区域Id获取网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格列表的Json字符串</returns>
        public string GetAllReseaus(Guid areaId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryReseaus", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据区域Id获取状态为使用的网格列表
        /// </summary>
        /// <param name="areaId">区域Id</param>
        /// <returns>网格选择对象列表</returns>
        public IList<ReseauSelectObject> GetUsedReseaus(Guid areaId)
        {
            IList<ReseauSelectObject> reseauSelectObjects = new List<ReseauSelectObject>();
            IEnumerable<Reseau> reseaus = reseauRepository.FindAll(Specification<Reseau>.Eval(entity => entity.AreaId == areaId && entity.State == State.使用), "ReseauCode");
            if (reseaus != null)
            {
                foreach (var reseau in reseaus)
                {
                    reseauSelectObjects.Add(MapperHelper.Map<Reseau, ReseauSelectObject>(reseau));
                }
            }
            return reseauSelectObjects;
        }

        /// <summary>
        /// 获取所有状态为使用的网格列表
        /// </summary>
        /// <returns>网格选择对象列表</returns>
        public IList<ReseauSelectObject> GetAllUsedReseaus()
        {
            IList<ReseauSelectObject> reseauSelectObjects = new List<ReseauSelectObject>();
            IEnumerable<Reseau> reseaus = reseauRepository.FindAll(Specification<Reseau>.Eval(entity => entity.State == State.使用), "ReseauName");
            if (reseaus != null)
            {
                foreach (var reseau in reseaus)
                {
                    reseauSelectObjects.Add(MapperHelper.Map<Reseau, ReseauSelectObject>(reseau));
                }
            }
            return reseauSelectObjects;
        }

        /// <summary>
        /// 新增或者修改网格
        /// </summary>
        /// <param name="reseauMaintObject">要新增或者修改的网格维护对象</param>
        public void AddOrUpdateReseau(ReseauMaintObject reseauMaintObject)
        {
            if (reseauMaintObject.Id == Guid.Empty)
            {
                Reseau reseau = AggregateFactory.CreateReseau(reseauMaintObject.AreaId, reseauMaintObject.ReseauCode, reseauMaintObject.ReseauName,
                     reseauMaintObject.ReseauManagerId, reseauMaintObject.PlanningManagerId, reseauMaintObject.Remarks, (State)reseauMaintObject.State, reseauMaintObject.CreateUserId);
                reseauRepository.Add(reseau);
            }
            else
            {
                Reseau reseau = reseauRepository.FindByKey(reseauMaintObject.Id);
                if (reseau != null)
                {
                    reseau.Modify(reseauMaintObject.ReseauCode, reseauMaintObject.ReseauName, reseauMaintObject.ReseauManagerId, reseauMaintObject.PlanningManagerId, reseauMaintObject.Remarks,
                        (State)reseauMaintObject.State, reseauMaintObject.ModifyUserId);
                    reseauRepository.Update(reseau);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_AreaIdReseauCode"))
                {
                    throw new ApplicationFault("网格编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_AreaIdReseauName"))
                {
                    throw new ApplicationFault("网格名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Reseau_dbo.tbl_Area_AreaId"))
                {
                    throw new ApplicationFault("选择的区域在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除网格
        /// </summary>
        /// <param name="reseauMaintObjects">要删除的网格维护对象列表</param>
        public void RemoveReseaus(IList<ReseauMaintObject> reseauMaintObjects)
        {
            foreach (ReseauMaintObject reseauMaintObject in reseauMaintObjects)
            {
                Reseau reseau = reseauRepository.FindByKey(reseauMaintObject.Id);
                if (reseau != null)
                {
                    if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.ReseauId == reseau.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在站点中使用", reseau.ReseauCode);
                    }
                    else if (planningRepository.Exists(Specification<Planning>.Eval(entity => entity.ReseauId == reseau.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在规划中使用", reseau.ReseauCode);
                    }
                    else if (purchaseRepository.Exists(Specification<Purchase>.Eval(entity => entity.ReseauId == reseau.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在购置站点中使用", reseau.ReseauCode);
                    }
                    else if (workApplyRepository.Exists(Specification<WorkApply>.Eval(entity => entity.ReseauId == reseau.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在隐患上报单中使用", reseau.ReseauCode);
                    }
                    else if (workOrderRepository.Exists(Specification<WorkOrder>.Eval(entity => entity.ReseauId == reseau.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在零星派工单中使用", reseau.ReseauCode);
                    }
                    reseauRepository.Remove(reseau);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("已在站点中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Planning_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("已在规划中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("已在购置站点中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WorkApply_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("已在隐患上报单中使用");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WorkOrder_dbo.tbl_Reseau_ReseauId"))
                {
                    throw new ApplicationFault("已在零星派工单中使用");
                }
                throw ex;
            }
        }
    }
}
