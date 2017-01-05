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
    public class WorkBigClassService : DataService, IWorkBigClassService
    {
        private readonly IRepository<WorkBigClass> workBigClassRepository;
        private readonly IRepository<WorkSmallClass> workSmallClassRepository;
        private readonly IRepository<WorkOrder> workOrderRepository;

        public WorkBigClassService(IRepositoryContext context,
            IRepository<WorkBigClass> workBigClassRepository,
            IRepository<WorkSmallClass> workSmallClassRepository,
            IRepository<WorkOrder> workOrderRepository)
            : base(context)
        {
            this.workBigClassRepository = workBigClassRepository;
            this.workSmallClassRepository = workSmallClassRepository;
            this.workOrderRepository = workOrderRepository;
        }

        /// <summary>
        /// 根据派工大类Id获取派工大类
        /// </summary>
        /// <param name="id">派工大类Id</param>
        /// <returns>派工大类维护对象</returns>
        public WorkBigClassMaintObject GetWorkBigClassById(Guid id)
        {
            WorkBigClass workBigClass = workBigClassRepository.FindByKey(id);
            if (workBigClass != null)
            {
                WorkBigClassMaintObject workBigClassMaintObject = MapperHelper.Map<WorkBigClass, WorkBigClassMaintObject>(workBigClass);
                return workBigClassMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的派工大类在系统中不存在");
            }
        }

        /// <summary>
        /// 获取派工大类列表
        /// </summary>
        /// <returns>派工大类维护对象列表</returns>
        public IList<WorkBigClassMaintObject> GetWorkBigClasss()
        {
            IList<WorkBigClassMaintObject> workBigClassMaintObjects = new List<WorkBigClassMaintObject>();
            IEnumerable<WorkBigClass> workBigClasss = workBigClassRepository.FindAll(null, "BigClassCode");
            if (workBigClasss != null)
            {
                foreach (var workBigClass in workBigClasss)
                {
                    workBigClassMaintObjects.Add(MapperHelper.Map<WorkBigClass, WorkBigClassMaintObject>(workBigClass));
                }
            }
            return workBigClassMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的派工大类列表
        /// </summary>
        /// <returns>派工大类选择对象列表</returns>
        public IList<WorkBigClassSelectObject> GetUsedWorkBigClasss()
        {
            IList<WorkBigClassSelectObject> workBigClassSelectObjects = new List<WorkBigClassSelectObject>();
            IEnumerable<WorkBigClass> workBigClasss = workBigClassRepository.FindAll(Specification<WorkBigClass>.Eval(entity => entity.State == State.使用), "BigClassCode");
            if (workBigClasss != null)
            {
                foreach (var workBigClass in workBigClasss)
                {
                    workBigClassSelectObjects.Add(MapperHelper.Map<WorkBigClass, WorkBigClassSelectObject>(workBigClass));
                }
            }
            return workBigClassSelectObjects;
        }

        /// <summary>
        /// 新增或者修改派工大类
        /// </summary>
        /// <param name="workBigClassMaintObject">要新增或者修改的派工大类维护对象</param>
        public void AddOrUpdateWorkBigClass(WorkBigClassMaintObject workBigClassMaintObject)
        {
            if (workBigClassMaintObject.Id == Guid.Empty)
            {
                WorkBigClass workBigClass = AggregateFactory.CreateWorkBigClass(workBigClassMaintObject.BigClassCode, workBigClassMaintObject.BigClassName,
                    workBigClassMaintObject.Remarks, (State)workBigClassMaintObject.State, workBigClassMaintObject.CreateUserId);
                workBigClassRepository.Add(workBigClass);
            }
            else
            {
                WorkBigClass workBigClass = workBigClassRepository.FindByKey(workBigClassMaintObject.Id);
                if (workBigClass != null)
                {
                    workBigClass.Modify(workBigClassMaintObject.BigClassCode, workBigClassMaintObject.BigClassName,workBigClassMaintObject.Remarks, 
                        (State)workBigClassMaintObject.State, workBigClassMaintObject.ModifyUserId);
                    workBigClassRepository.Update(workBigClass);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_BigClassCode"))
                {
                    throw new ApplicationFault("大类编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_BigClassName"))
                {
                    throw new ApplicationFault("大类名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除派工大类
        /// </summary>
        /// <param name="workBigClassMaintObjects">要删除的派工大类维护对象列表</param>
        public void RemoveWorkBigClasss(IList<WorkBigClassMaintObject> workBigClassMaintObjects)
        {
            foreach (WorkBigClassMaintObject workBigClassMaintObject in workBigClassMaintObjects)
            {
                WorkBigClass workBigClass = workBigClassRepository.FindByKey(workBigClassMaintObject.Id);
                if (workBigClass != null)
                {
                    if (workSmallClassRepository.Exists(Specification<WorkSmallClass>.Eval(entity => entity.WorkBigClassId == workBigClass.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在工单小类", workBigClass.BigClassCode);
                    }
                    workBigClassRepository.Remove(workBigClass);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_WorkSmallClass_dbo.tbl_WorkBigClass_WorkBigClassId"))
                {
                    throw new ApplicationFault("已存在工单小类");
                }
                throw ex;
            }
        }
    }
}
