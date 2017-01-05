using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class FoundationTestLogService : DataService, IFoundationTestLogService
    {
        private readonly IRepository<FoundationTestLog> foundationTestLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public FoundationTestLogService(IRepositoryContext context,
            IRepository<FoundationTestLog> foundationTestLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.foundationTestLogRepository = foundationTestLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改桩基动测
        /// </summary>
        /// <param name="foundationTestMaintObject">要新增或者修改的桩基动测对象</param>
        public void AddOrUpdateFoundationTestLog(FoundationTestMaintObject foundationTestMaintObject)
        {
            if (foundationTestMaintObject.Id == Guid.Empty)
            {
                FoundationTestLog foundationTestLog = AggregateFactory.CreateFoundationTestLog(OperationType.新增, foundationTestMaintObject.ParentId, (PropertyType)foundationTestMaintObject.PropertyType, foundationTestMaintObject.BudgetPrice, 0, foundationTestMaintObject.Memos, foundationTestMaintObject.CreateUserId);
                foundationTestLogRepository.Add(foundationTestLog);
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据资源类型获取桩基动测历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetFoundationTestLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetFoundationTestLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
