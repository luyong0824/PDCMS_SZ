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
    public class ExternalElectricPowerService : DataService, IExternalElectricPowerService
    {
        private readonly IRepository<ExternalElectricPower> externalElectricPowerRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public ExternalElectricPowerService(IRepositoryContext context,
            IRepository<ExternalElectricPower> externalElectricPowerRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.externalElectricPowerRepository = externalElectricPowerRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据外电引入Id获取外电引入
        /// </summary>
        /// <param name="id">外电引入Id</param>
        /// <returns>外电引入维护对象</returns>
        public ExternalElectricPowerMaintObject GetExternalElectricPowerById(Guid id)
        {
            ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(id);
            if (externalElectricPower != null)
            {
                ExternalElectricPowerMaintObject externalElectricPowerMaintObject = MapperHelper.Map<ExternalElectricPower, ExternalElectricPowerMaintObject>(externalElectricPower);
                return externalElectricPowerMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的外电引入在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改外电引入
        /// </summary>
        /// <param name="externalElectricPowerMaintObject">要新增或者修改的外电引入对象</param>
        public void AddOrUpdateExternalElectricPower(ExternalElectricPowerMaintObject externalElectricPowerMaintObject)
        {
            if (externalElectricPowerMaintObject.Id == Guid.Empty)
            {
                ExternalElectricPower externalElectricPower = AggregateFactory.CreateExternalElectricPower(externalElectricPowerMaintObject.ParentId, (PropertyType)externalElectricPowerMaintObject.PropertyType, (ExternalElectric)externalElectricPowerMaintObject.ExternalElectric, externalElectricPowerMaintObject.BudgetPrice, 0, externalElectricPowerMaintObject.Memos, externalElectricPowerMaintObject.CreateUserId);
                externalElectricPowerRepository.Add(externalElectricPower);
            }
            else
            {
                ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerMaintObject.Id);
                if (externalElectricPower != null)
                {
                    externalElectricPower.Modify((ExternalElectric)externalElectricPowerMaintObject.ExternalElectric, externalElectricPowerMaintObject.BudgetPrice, 0, externalElectricPowerMaintObject.Memos, externalElectricPowerMaintObject.ModifyUserId);
                    externalElectricPowerRepository.Update(externalElectricPower);
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

        public void RemoveExternalElectricPower(IList<ExternalElectricPowerMaintObject> externalElectricPowerMaintObjects)
        {
            foreach (ExternalElectricPowerMaintObject externalElectricPowerMaintObject in externalElectricPowerMaintObjects)
            {
                ExternalElectricPower externalElectricPower = externalElectricPowerRepository.FindByKey(externalElectricPowerMaintObject.Id);
                if (externalElectricPower != null)
                {
                    externalElectricPowerRepository.Remove(externalElectricPower);
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
