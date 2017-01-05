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
    public class TowerBaseLogService : DataService, ITowerBaseLogService
    {
        private readonly IRepository<TowerBaseLog> towerBaseLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public TowerBaseLogService(IRepositoryContext context,
            IRepository<TowerBaseLog> towerBaseLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.towerBaseLogRepository = towerBaseLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改铁塔基础
        /// </summary>
        /// <param name="towerBaseMaintObject">要新增或者修改的铁塔基础对象</param>
        public void AddOrUpdateTowerBaseLog(TowerBaseMaintObject towerBaseMaintObject)
        {
            if (towerBaseMaintObject.Id == Guid.Empty)
            {
                TowerBaseLog towerBaseLog = AggregateFactory.CreateTowerBaseLog(OperationType.新增, towerBaseMaintObject.ParentId, (PropertyType)towerBaseMaintObject.PropertyType, (TowerBaseType)towerBaseMaintObject.TowerBaseType, towerBaseMaintObject.BudgetPrice, 0, towerBaseMaintObject.Memos, towerBaseMaintObject.CreateUserId);
                towerBaseLogRepository.Add(towerBaseLog);
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
        /// 根据资源类型获取铁塔基础历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetTowerBaseLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetTowerBaseLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
