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
    public class EquipmentInstallLogService : DataService, IEquipmentInstallLogService
    {
        private readonly IRepository<EquipmentInstallLog> equipmentInstallLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public EquipmentInstallLogService(IRepositoryContext context,
            IRepository<EquipmentInstallLog> equipmentInstallLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.equipmentInstallLogRepository = equipmentInstallLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改设备安装
        /// </summary>
        /// <param name="equipmentInstallMaintObject">要新增或者修改的设备安装对象</param>
        public void AddOrUpdateEquipmentInstallLog(EquipmentInstallMaintObject equipmentInstallMaintObject)
        {
            if (equipmentInstallMaintObject.Id == Guid.Empty)
            {
                EquipmentInstallLog equipmentInstallLog = AggregateFactory.CreateEquipmentInstallLog(OperationType.新增, equipmentInstallMaintObject.ParentId, (PropertyType)equipmentInstallMaintObject.PropertyType, equipmentInstallMaintObject.SwitchPower, equipmentInstallMaintObject.Battery, equipmentInstallMaintObject.CabinetNumber, equipmentInstallMaintObject.BudgetPrice, 0, equipmentInstallMaintObject.Memos, equipmentInstallMaintObject.CreateUserId);
                equipmentInstallLogRepository.Add(equipmentInstallLog);
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
        /// 根据资源类型获取设备安装历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetEquipmentInstallLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetEquipmentInstallLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
