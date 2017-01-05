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
    public class TowerService : DataService, ITowerService
    {
        private readonly IRepository<Tower> towerRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public TowerService(IRepositoryContext context,
            IRepository<Tower> towerRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.towerRepository = towerRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据铁塔资源Id获取铁塔资源
        /// </summary>
        /// <param name="id">铁塔资源Id</param>
        /// <returns>铁塔资源维护对象</returns>
        public TowerMaintObject GetTowerById(Guid id)
        {
            Tower tower = towerRepository.FindByKey(id);
            if (tower != null)
            {
                TowerMaintObject towerMaintObject = MapperHelper.Map<Tower, TowerMaintObject>(tower);
                return towerMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的铁塔资源在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改铁塔资源
        /// </summary>
        /// <param name="towerMaintObject">要新增或者修改的铁塔资源对象</param>
        public void AddOrUpdateTower(TowerMaintObject towerMaintObject)
        {
            if (towerMaintObject.Id == Guid.Empty)
            {
                Tower tower = AggregateFactory.CreateTower(towerMaintObject.ParentId, (PropertyType)towerMaintObject.PropertyType, (TowerType)towerMaintObject.TowerType, towerMaintObject.TowerHeight, towerMaintObject.PlatFormNumber, towerMaintObject.PoleNumber, towerMaintObject.BudgetPrice, 0, towerMaintObject.Memos, towerMaintObject.CreateUserId);
                towerRepository.Add(tower);
            }
            else
            {
                Tower tower = towerRepository.FindByKey(towerMaintObject.Id);
                if (tower != null)
                {
                    tower.Modify((TowerType)towerMaintObject.TowerType, towerMaintObject.TowerHeight, towerMaintObject.PlatFormNumber, towerMaintObject.PoleNumber, towerMaintObject.BudgetPrice, 0, towerMaintObject.Memos, towerMaintObject.ModifyUserId);
                    towerRepository.Update(tower);
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

        public void RemoveTower(IList<TowerMaintObject> towerMaintObjects)
        {
            foreach (TowerMaintObject towerMaintObject in towerMaintObjects)
            {
                Tower tower = towerRepository.FindByKey(towerMaintObject.Id);
                if (tower != null)
                {
                    towerRepository.Remove(tower);
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
