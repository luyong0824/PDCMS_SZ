using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Infrastructure.IoC;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.DistributedService.Services.BaseData
{
    /// <summary>
    /// 部门分布式服务
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentService departmentServiceImpl = ServiceLocator.Instance.GetService<IDepartmentService>();

        public DepartmentMaintObject GetDepartmentById(Guid id)
        {
            try
            {
                return departmentServiceImpl.GetDepartmentById(id);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetDepartments(Guid companyId)
        {
            try
            {
                return departmentServiceImpl.GetDepartments(companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public IList<DepartmentSelectObject> GetUsedDepartments(Guid companyId)
        {
            try
            {
                return departmentServiceImpl.GetUsedDepartments(companyId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public string GetUsedDepartmentsBySend(Guid companyId, Guid postId)
        {
            try
            {
                return departmentServiceImpl.GetUsedDepartmentsBySend(companyId, postId);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void AddOrUpdateDepartment(DepartmentMaintObject departmentMaintObject)
        {
            try
            {
                departmentServiceImpl.AddOrUpdateDepartment(departmentMaintObject);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void RemoveDepartments(IList<DepartmentMaintObject> departmentMaintObjects)
        {
            try
            {
                departmentServiceImpl.RemoveDepartments(departmentMaintObjects);
            }
            catch (Exception ex)
            {
                throw new FaultException<FaultObject>(FaultObject.CreateFromException(ex), FaultObject.CreateFaultReason(ex));
            }
        }

        public void Dispose()
        {
            departmentServiceImpl.Dispose();
        }
    }
}
