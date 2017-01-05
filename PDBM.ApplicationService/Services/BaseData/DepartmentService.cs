using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 部门应用层服务
    /// </summary>
    public class DepartmentService : DataService, IDepartmentService
    {
        private readonly IRepository<Department> departmentRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<WFActivity> wfActivityRepository;

        public DepartmentService(IRepositoryContext context,
            IRepository<Department> departmentRepository,
            IRepository<User> userRepository,
            IRepository<WFActivity> wfActivityRepository)
            : base(context)
        {
            this.departmentRepository = departmentRepository;
            this.userRepository = userRepository;
            this.wfActivityRepository = wfActivityRepository;
        }

        /// <summary>
        /// 根据部门Id获取部门
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>部门维护对象</returns>
        public DepartmentMaintObject GetDepartmentById(Guid id)
        {
            Department department = departmentRepository.FindByKey(id);
            if (department != null)
            {
                DepartmentMaintObject departmentMaintObject = MapperHelper.Map<Department, DepartmentMaintObject>(department);
                if (department.ManagerUserId != null)
                {
                    User user = userRepository.GetByKey((Guid)department.ManagerUserId);
                    departmentMaintObject.FullName = user.FullName;
                }
                else
                {
                    departmentMaintObject.FullName = "无";
                }
                return departmentMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的部门在系统中不存在");
            }
        }

        /// <summary>
        /// 根据公司Id获取部门列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门列表的Json字符串</returns>
        public string GetDepartments(Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(1);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryDepartments", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 根据公司Id获取状态为使用的部门列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <returns>部门选择对象列表</returns>
        public IList<DepartmentSelectObject> GetUsedDepartments(Guid companyId)
        {
            IList<DepartmentSelectObject> departmentSelectObjects = new List<DepartmentSelectObject>();
            IEnumerable<Department> departments = departmentRepository.FindAll(Specification<Department>.Eval(entity => entity.CompanyId == companyId && entity.State == State.使用), "DepartmentCode");
            if (departments != null)
            {
                foreach (var department in departments)
                {
                    departmentSelectObjects.Add(MapperHelper.Map<Department, DepartmentSelectObject>(department));
                }
            }
            return departmentSelectObjects;
        }

        /// <summary>
        /// 根据公司Id和岗位Id获取状态为使用的部门列表，用于发送工作流实例
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="postId">岗位Id</param>
        /// <returns>部门列表的Json字符串</returns>
        public string GetUsedDepartmentsBySend(Guid companyId, Guid postId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "PostId", Type = SqlDbType.UniqueIdentifier, Value = postId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetUsedDepartmentsBySend", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者修改部门
        /// </summary>
        /// <param name="departmentMaintObject">要新增或者修改的部门维护对象</param>
        public void AddOrUpdateDepartment(DepartmentMaintObject departmentMaintObject)
        {
            if (departmentMaintObject.Id == Guid.Empty)
            {
                Department department = AggregateFactory.CreateDepartment(departmentMaintObject.CompanyId, departmentMaintObject.DepartmentCode,
                    departmentMaintObject.DepartmentName, departmentMaintObject.ManagerUserId, departmentMaintObject.Remarks, (State)departmentMaintObject.State, departmentMaintObject.CreateUserId);
                departmentRepository.Add(department);
            }
            else
            {
                Department department = departmentRepository.FindByKey(departmentMaintObject.Id);
                if (department != null)
                {
                    department.Modify(departmentMaintObject.DepartmentCode, departmentMaintObject.DepartmentName, departmentMaintObject.ManagerUserId, departmentMaintObject.Remarks,
                        (State)departmentMaintObject.State, departmentMaintObject.ModifyUserId);
                    departmentRepository.Update(department);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_CompanyIdDepartmentCode"))
                {
                    throw new ApplicationFault("部门编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_CompanyIdDepartmentName"))
                {
                    throw new ApplicationFault("部门名称重复");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_Department_dbo.tbl_Company_CompanyId"))
                {
                    throw new ApplicationFault("选择的公司在系统中不存在");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentMaintObjects">要删除的部门维护对象列表</param>
        public void RemoveDepartments(IList<DepartmentMaintObject> departmentMaintObjects)
        {
            foreach (DepartmentMaintObject departmentMaintObject in departmentMaintObjects)
            {
                Department department = departmentRepository.FindByKey(departmentMaintObject.Id);
                if (department != null)
                {
                    if (userRepository.Exists(Specification<User>.Eval(entity => entity.DepartmentId == department.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已存在用户账号", department.DepartmentCode);
                    }
                    if (wfActivityRepository.Exists(Specification<WFActivity>.Eval(entity => entity.DepartmentId == department.Id)))
                    {
                        throw new ApplicationFault("{0}<br>已在流程步骤中使用", department.DepartmentCode);
                    }
                    departmentRepository.Remove(department);
                }
            }
            try
            {
                Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FK_dbo.tbl_User_dbo.tbl_Department_DepartmentId"))
                {
                    throw new ApplicationFault("已存在用户账号");
                }
                else if (ex.Message.Contains("FK_dbo.tbl_WFActivity_dbo.tbl_Department_DepartmentId"))
                {
                    throw new ApplicationFault("已在流程步骤中使用");
                }
                throw ex;
            }
        }
    }
}
