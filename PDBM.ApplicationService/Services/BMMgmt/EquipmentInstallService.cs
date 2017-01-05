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
    public class EquipmentInstallService : DataService, IEquipmentInstallService
    {
        private readonly IRepository<EquipmentInstall> equipmentInstallRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public EquipmentInstallService(IRepositoryContext context,
            IRepository<EquipmentInstall> equipmentInstallRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.equipmentInstallRepository = equipmentInstallRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据设备安装Id获取设备安装
        /// </summary>
        /// <param name="id">设备安装Id</param>
        /// <returns>设备安装维护对象</returns>
        public EquipmentInstallMaintObject GetEquipmentInstallById(Guid id)
        {
            EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(id);
            if (equipmentInstall != null)
            {
                EquipmentInstallMaintObject equipmentInstallMaintObject = MapperHelper.Map<EquipmentInstall, EquipmentInstallMaintObject>(equipmentInstall);
                return equipmentInstallMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的设备安装在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改设备安装
        /// </summary>
        /// <param name="equipmentInstallMaintObject">要新增或者修改的设备安装对象</param>
        public void AddOrUpdateEquipmentInstall(EquipmentInstallMaintObject equipmentInstallMaintObject)
        {
            if (equipmentInstallMaintObject.Id == Guid.Empty)
            {
                EquipmentInstall equipmentInstall = AggregateFactory.CreateEquipmentInstall(equipmentInstallMaintObject.ParentId, (PropertyType)equipmentInstallMaintObject.PropertyType, equipmentInstallMaintObject.SwitchPower, equipmentInstallMaintObject.Battery, equipmentInstallMaintObject.CabinetNumber, equipmentInstallMaintObject.BudgetPrice, 0, equipmentInstallMaintObject.Memos, equipmentInstallMaintObject.CreateUserId);
                equipmentInstallRepository.Add(equipmentInstall);
            }
            else
            {
                EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallMaintObject.Id);
                if (equipmentInstall != null)
                {
                    equipmentInstall.Modify(equipmentInstallMaintObject.SwitchPower, equipmentInstallMaintObject.Battery, equipmentInstallMaintObject.CabinetNumber, equipmentInstallMaintObject.BudgetPrice, 0, equipmentInstallMaintObject.Memos, equipmentInstallMaintObject.ModifyUserId);
                    equipmentInstallRepository.Update(equipmentInstall);
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

        public void RemoveEquipmentInstall(IList<EquipmentInstallMaintObject> equipmentInstallMaintObjects)
        {
            foreach (EquipmentInstallMaintObject equipmentInstallMaintObject in equipmentInstallMaintObjects)
            {
                EquipmentInstall equipmentInstall = equipmentInstallRepository.FindByKey(equipmentInstallMaintObject.Id);
                if (equipmentInstall != null)
                {
                    equipmentInstallRepository.Remove(equipmentInstall);
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
