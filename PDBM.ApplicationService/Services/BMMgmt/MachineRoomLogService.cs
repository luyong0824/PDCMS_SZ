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
    public class MachineRoomLogService : DataService, IMachineRoomLogService
    {
        private readonly IRepository<MachineRoomLog> machineRoomLogRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public MachineRoomLogService(IRepositoryContext context,
            IRepository<MachineRoomLog> machineRoomLogRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.machineRoomLogRepository = machineRoomLogRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 新增或者修改机房
        /// </summary>
        /// <param name="machineRoomMaintObject">要新增或者修改的机房对象</param>
        public void AddOrUpdateMachineRoomLog(MachineRoomMaintObject machineRoomMaintObject)
        {
            if (machineRoomMaintObject.Id == Guid.Empty)
            {
                MachineRoomLog machineRoomLog = AggregateFactory.CreateMachineRoomLog(OperationType.新增, machineRoomMaintObject.ParentId, (PropertyType)machineRoomMaintObject.PropertyType, (MachineRoomType)machineRoomMaintObject.MachineRoomType, machineRoomMaintObject.MachineRoomArea, machineRoomMaintObject.BudgetPrice, 0, machineRoomMaintObject.Memos, machineRoomMaintObject.CreateUserId);
                machineRoomLogRepository.Add(machineRoomLog);
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
        /// 根据资源类型获取机房历史记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <returns></returns>
        public string GetMachineRoomLog(int propertyType, Guid parentId)
        {
            List<Parameter> parameters = new List<Parameter>(2);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetMachineRoomLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
