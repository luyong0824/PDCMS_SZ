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
    public class MachineRoomService : DataService, IMachineRoomService
    {
        private readonly IRepository<MachineRoom> machineRoomRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public MachineRoomService(IRepositoryContext context,
            IRepository<MachineRoom> machineRoomRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.machineRoomRepository = machineRoomRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据机房Id获取机房
        /// </summary>
        /// <param name="id">机房Id</param>
        /// <returns>机房维护对象</returns>
        public MachineRoomMaintObject GetMachineRoomById(Guid id)
        {
            MachineRoom machineRoom = machineRoomRepository.FindByKey(id);
            if (machineRoom != null)
            {
                MachineRoomMaintObject machineRoomMaintObject = MapperHelper.Map<MachineRoom, MachineRoomMaintObject>(machineRoom);
                return machineRoomMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的机房在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改机房
        /// </summary>
        /// <param name="machineRoomMaintObject">要新增或者修改的机房对象</param>
        public void AddOrUpdateMachineRoom(MachineRoomMaintObject machineRoomMaintObject)
        {
            if (machineRoomMaintObject.Id == Guid.Empty)
            {
                MachineRoom machineRoom = AggregateFactory.CreateMachineRoom(machineRoomMaintObject.ParentId, (PropertyType)machineRoomMaintObject.PropertyType, (MachineRoomType)machineRoomMaintObject.MachineRoomType, machineRoomMaintObject.MachineRoomArea, machineRoomMaintObject.BudgetPrice, 0, machineRoomMaintObject.Memos, machineRoomMaintObject.CreateUserId);
                machineRoomRepository.Add(machineRoom);
            }
            else
            {
                MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomMaintObject.Id);
                if (machineRoom != null)
                {
                    machineRoom.Modify((MachineRoomType)machineRoomMaintObject.MachineRoomType, machineRoomMaintObject.MachineRoomArea, machineRoomMaintObject.BudgetPrice, 0, machineRoomMaintObject.Memos, machineRoomMaintObject.ModifyUserId);
                    machineRoomRepository.Update(machineRoom);
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

        public void RemoveMachineRoom(IList<MachineRoomMaintObject> machineRoomMaintObjects)
        {
            foreach (MachineRoomMaintObject machineRoomMaintObject in machineRoomMaintObjects)
            {
                MachineRoom machineRoom = machineRoomRepository.FindByKey(machineRoomMaintObject.Id);
                if (machineRoom != null)
                {
                    machineRoomRepository.Remove(machineRoom);
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
