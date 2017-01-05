using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class TowerBaseService : DataService, ITowerBaseService
    {
        private readonly IRepository<TowerBase> towerBaseRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public TowerBaseService(IRepositoryContext context,
            IRepository<TowerBase> towerBaseRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.towerBaseRepository = towerBaseRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据铁塔基础Id获取铁塔基础
        /// </summary>
        /// <param name="id">铁塔基础Id</param>
        /// <returns>铁塔基础维护对象</returns>
        public TowerBaseMaintObject GetTowerBaseById(Guid id)
        {
            TowerBase towerBase = towerBaseRepository.FindByKey(id);
            if (towerBase != null)
            {
                TowerBaseMaintObject towerBaseMaintObject = MapperHelper.Map<TowerBase, TowerBaseMaintObject>(towerBase);
                return towerBaseMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的铁塔基础在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改铁塔基础
        /// </summary>
        /// <param name="towerBaseMaintObject">要新增或者修改的铁塔基础对象</param>
        public void AddOrUpdateTowerBase(TowerBaseMaintObject towerBaseMaintObject)
        {
            if (towerBaseMaintObject.Id == Guid.Empty)
            {
                TowerBase towerBase = AggregateFactory.CreateTowerBase(towerBaseMaintObject.ParentId, (PropertyType)towerBaseMaintObject.PropertyType, (TowerBaseType)towerBaseMaintObject.TowerBaseType, towerBaseMaintObject.BudgetPrice, 0, towerBaseMaintObject.Memos, towerBaseMaintObject.CreateUserId);
                towerBaseRepository.Add(towerBase);
            }
            else
            {
                TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseMaintObject.Id);
                if (towerBase != null)
                {
                    towerBase.Modify((TowerBaseType)towerBaseMaintObject.TowerBaseType, towerBaseMaintObject.BudgetPrice, 0, towerBaseMaintObject.Memos, towerBaseMaintObject.ModifyUserId);
                    towerBaseRepository.Update(towerBase);
                }
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

        public void RemoveTowerBase(IList<TowerBaseMaintObject> towerBaseMaintObjects)
        {
            foreach (TowerBaseMaintObject towerBaseMaintObject in towerBaseMaintObjects)
            {
                TowerBase towerBase = towerBaseRepository.FindByKey(towerBaseMaintObject.Id);
                if (towerBase != null)
                {
                    towerBaseRepository.Remove(towerBase);
                }
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
    }
}
