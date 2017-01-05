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
    public class TowerLogService : DataService, ITowerLogService
    {
        private readonly IRepository<TowerLog> towerLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public TowerLogService(IRepositoryContext context,
            IRepository<TowerLog> towerLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.towerLogRepository = towerLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改铁塔资源
        /// </summary>
        /// <param name="towerMaintObject">要新增或者修改的铁塔资源对象</param>
        public void AddOrUpdateTowerLog(TowerMaintObject towerMaintObject)
        {
            if (towerMaintObject.Id == Guid.Empty)
            {
                TowerLog towerLog = AggregateFactory.CreateTowerLog(OperationType.新增, towerMaintObject.ParentId, (PropertyType)towerMaintObject.PropertyType, (TowerType)towerMaintObject.TowerType, towerMaintObject.TowerHeight, towerMaintObject.PlatFormNumber, towerMaintObject.PoleNumber, towerMaintObject.BudgetPrice, 0, towerMaintObject.Memos, towerMaintObject.CreateUserId);
                towerLogRepository.Add(towerLog);
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
        /// 根据资源类型获取铁塔历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetTowerLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetTowerLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
