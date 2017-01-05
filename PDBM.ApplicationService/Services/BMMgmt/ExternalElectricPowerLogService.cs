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
    public class ExternalElectricPowerLogService : DataService, IExternalElectricPowerLogService
    {
        private readonly IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public ExternalElectricPowerLogService(IRepositoryContext context,
            IRepository<ExternalElectricPowerLog> externalElectricPowerLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.externalElectricPowerLogRepository = externalElectricPowerLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改外电引入
        /// </summary>
        /// <param name="externalElectricPowerMaintObject">要新增或者修改的外电引入对象</param>
        public void AddOrUpdateExternalElectricPowerLog(ExternalElectricPowerMaintObject externalElectricPowerMaintObject)
        {
            if (externalElectricPowerMaintObject.Id == Guid.Empty)
            {
                ExternalElectricPowerLog externalElectricPowerLog = AggregateFactory.CreateExternalElectricPowerLog(OperationType.新增, externalElectricPowerMaintObject.ParentId, (PropertyType)externalElectricPowerMaintObject.PropertyType, (ExternalElectric)externalElectricPowerMaintObject.ExternalElectric, externalElectricPowerMaintObject.BudgetPrice, 0, externalElectricPowerMaintObject.Memos, externalElectricPowerMaintObject.CreateUserId);
                externalElectricPowerLogRepository.Add(externalElectricPowerLog);
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
        /// 根据资源类型获取外电引入历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetExternalElectricPowerLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetExternalElectricPowerLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
