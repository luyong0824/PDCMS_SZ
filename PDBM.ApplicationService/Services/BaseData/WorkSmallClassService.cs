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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 派工小类应用层服务
    /// </summary>
    public class WorkSmallClassService : DataService, IWorkSmallClassService
    {
        private readonly IRepository<WorkSmallClass> workSmallClassRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;

        public WorkSmallClassService(IRepositoryContext context,
            IRepository<WorkSmallClass> workSmallClassRepository,
            IRepository<WorkOrder> workOrderRepository)
            : base(context)
        {
            this.workSmallClassRepository = workSmallClassRepository;
            this.workOrderRepository = workOrderRepository;
        }

        /// <summary>
        /// 根据派工小类Id获取派工小类
        /// </summary>
        /// <param name="id">派工小类Id</param>
        /// <returns>派工小类维护对象</returns>
        public WorkSmallClassMaintObject GetWorkSmallClassById(Guid id)
        {
            WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(id);
            if (workSmallClass != null)
            {
                WorkSmallClassMaintObject workSmallClassMaintObject = MapperHelper.Map<WorkSmallClass, WorkSmallClassMaintObject>(workSmallClass);
                return workSmallClassMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的派工小类在系统中不存在");
            }
        }

        /// <summary>
        /// 根据派工大类Id获取派工小类列表
        /// </summary>
        /// <param name="workBigClassId">派工大类Id</param>
        /// <returns>派工小类维护对象列表</returns>
        public IList<WorkSmallClassMaintObject> GetWorkSmallClasss(Guid workBigClassId)
        {
            IList<WorkSmallClassMaintObject> workSmallClassMaintObjects = new List<WorkSmallClassMaintObject>();
            IEnumerable<WorkSmallClass> workSmallClasss = workSmallClassRepository.FindAll(Specification<WorkSmallClass>.Eval(entity => entity.WorkBigClassId == workBigClassId), "SmallClassCode");
            if (workSmallClasss != null)
            {
                foreach (var workSmallClass in workSmallClasss)
                {
                    workSmallClassMaintObjects.Add(MapperHelper.Map<WorkSmallClass, WorkSmallClassMaintObject>(workSmallClass));
                }
            }
            return workSmallClassMaintObjects;
        }

        /// <summary>
        /// 根据派工大类Id获取状态为使用的派工小类列表
        /// </summary>
        /// <param name="workBigClassId">派工大类Id</param>
        /// <returns>派工小类选择对象列表</returns>
        public IList<WorkSmallClassSelectObject> GetUsedWorkSmallClass(Guid workBigClassId)
        {
            IList<WorkSmallClassSelectObject> workSmallClassSelectObjects = new List<WorkSmallClassSelectObject>();
            IEnumerable<WorkSmallClass> workSmallClasss = workSmallClassRepository.FindAll(Specification<WorkSmallClass>.Eval(entity => entity.WorkBigClassId == workBigClassId && entity.State == State.使用), "SmallClassCode");
            if (workSmallClasss != null)
            {
                foreach (var workSmallClass in workSmallClasss)
                {
                    workSmallClassSelectObjects.Add(MapperHelper.Map<WorkSmallClass, WorkSmallClassSelectObject>(workSmallClass));
                }
            }
            return workSmallClassSelectObjects;
        }

        /// <summary>
        /// 新增或者修改派工小类
        /// </summary>
        /// <param name="workSmallClassMaintObject">要新增或者修改的派工小类维护对象</param>
        public void AddOrUpdateWorkSmallClass(WorkSmallClassMaintObject workSmallClassMaintObject)
        {
            if (workSmallClassMaintObject.Id == Guid.Empty)
            {
                WorkSmallClass workSmallClass = AggregateFactory.CreateWorkSmallClass(workSmallClassMaintObject.WorkBigClassId, workSmallClassMaintObject.SmallClassCode, workSmallClassMaintObject.SmallClassName,
                    workSmallClassMaintObject.Remarks, (State)workSmallClassMaintObject.State, workSmallClassMaintObject.CreateUserId);
                workSmallClassRepository.Add(workSmallClass);
            }
            else
            {
                WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workSmallClassMaintObject.Id);
                if (workSmallClass != null)
                {
                    workSmallClass.Modify(workSmallClassMaintObject.SmallClassCode, workSmallClassMaintObject.SmallClassName, workSmallClassMaintObject.Remarks,
                        (State)workSmallClassMaintObject.State, workSmallClassMaintObject.ModifyUserId);
                    workSmallClassRepository.Update(workSmallClass);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_WorkBigClassIdSmallClassCode"))
                {
                    throw new ApplicationFault("小类编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_WorkBigClassIdSmallClassName"))
                {
                    throw new ApplicationFault("小类名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WorkSmallClass_dbo.tbl_WorkBigClass_WorkBigClassId"))
                {
                    throw new ApplicationFault("选择的工单大类在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除派工小类
        /// </summary>
        /// <param name="workSmallClassMaintObjects">要删除的派工小类维护对象列表</param>
        public void RemoveWorkSmallClasss(IList<WorkSmallClassMaintObject> workSmallClassMaintObjects)
        {
            foreach (WorkSmallClassMaintObject workSmallClassMaintObject in workSmallClassMaintObjects)
            {
                WorkSmallClass workSmallClass = workSmallClassRepository.FindByKey(workSmallClassMaintObject.Id);
                if (workSmallClass != null)
                {
                    if (workOrderRepository.Exists(Specification<WorkOrder>.Eval(entity => entity.WorkSmallClassId == workSmallClass.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在零星派工单中使用", workSmallClass.SmallClassCode);
                    }
                    workSmallClassRepository.Remove(workSmallClass);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_WorkOrder_dbo.tbl_WorkSmallClass_WorkSmallClassId"))
                {
                    throw new ApplicationFault("已在零星派工单中使用");
                }
                throw ex;
            }
        }
    }
}
