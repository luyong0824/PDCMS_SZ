using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.Domain.Models.Enum;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 职务用户应用层服务
    /// </summary>
    public class DutyUserService : DataService, IDutyUserService
    {
        private readonly IRepository<DutyUser> dutyUserRepository;

        public DutyUserService(IRepositoryContext context,
            IRepository<DutyUser> dutyUserRepository)
            : base(context)
        {
            this.dutyUserRepository = dutyUserRepository;
        }

        /// <summary>
        /// 根据公司Id，部门Id，职务Id获取职务用户列表
        /// </summary>
        /// <param name="companyId">公司Id</param>
        /// <param name="departmentId">部门Id</param>
        /// <param name="duty">职务Id</param>
        /// <returns>职务用户列表的Json字符串</returns>
        public string GetDutyUsers(Guid companyId, Guid departmentId, int duty)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            parameters.Add(new Parameter() { Name = "DepartmentId", Type = SqlDbType.UniqueIdentifier, Value = departmentId });
            parameters.Add(new Parameter() { Name = "Duty", Type = SqlDbType.Int, Value = duty });
            using (var dt = SqlHelper.ExecuteDataTable("prc_QueryDutyUsers", parameters))
            {
                dt.Columns.Add("isLeaf", typeof(bool), "Convert(IsLeafStr, 'System.Boolean')");
                dt.Columns.Add("asyncLoad", typeof(bool), "Convert(AsyncLoadStr, 'System.Boolean')");
                return JsonHelper.Encode(dt);
            }
        }

        /// <summary>
        /// 新增或者删除职务用户
        /// </summary>
        /// <param name="dutyUserMaintObjects">要新增或者删除的职务用户维护对象列表</param>
        public void AddOrRemoveDutyUsers(IList<DutyUserMaintObject> dutyUserMaintObjects)
        {
            foreach (var dutyUserMaintObject in dutyUserMaintObjects)
            {
                if (dutyUserMaintObject.Id == Guid.Empty)
                {
                    DutyUser dutyUser = AggregateFactory.CreateDutyUser((Duty)dutyUserMaintObject.Duty,
                        dutyUserMaintObject.UserId, dutyUserMaintObject.CreateUserId);
                    dutyUserRepository.Add(dutyUser);
                }
                else
                {
                    DutyUser dutyUser = dutyUserRepository.FindByKey(dutyUserMaintObject.Id);
                    if (dutyUser != null)
                    {
                        dutyUserRepository.Remove(dutyUser);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_DutyUserId"))
                {
                    throw new ApplicationFault("职务用户重复添加");
                }
                throw ex;
            }
        }
    }
}
