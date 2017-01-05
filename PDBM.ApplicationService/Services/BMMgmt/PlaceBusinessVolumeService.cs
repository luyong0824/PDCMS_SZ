using PDBM.DataTransferObjects.BMMgmt;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.FileMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Repositories.BaseData;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
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
    /// <summary>
    /// 站点业务量应用层服务
    /// </summary>
    public class PlaceBusinessVolumeService : DataService, IPlaceBusinessVolumeService
    {
        private readonly IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository;

        public PlaceBusinessVolumeService(IRepositoryContext context,
            IRepository<PlaceBusinessVolume> placeBusinessVolumeRepository)
            : base(context)
        {
            this.placeBusinessVolumeRepository = placeBusinessVolumeRepository;
        }

        /// <summary>
        /// 新增或者修改站点业务量
        /// </summary>
        /// <param name="placeBusinessVolumeMaintObject">要新增或者修改的站点业务量对象</param>
        public void AddOrUpdatePlaceBusinessVolume(PlaceBusinessVolumeMaintObject placeBusinessVolumeMaintObject)
        {
            if (placeBusinessVolumeMaintObject.Id == Guid.Empty)
            {
                PlaceBusinessVolume placeBusinessVolume = AggregateFactory.CreatePlaceBusinessVolume(placeBusinessVolumeMaintObject.PlaceId, placeBusinessVolumeMaintObject.G2BusinessVolumeId,
                    placeBusinessVolumeMaintObject.D2BusinessVolumeId, placeBusinessVolumeMaintObject.G3BusinessVolumeId, placeBusinessVolumeMaintObject.G4BusinessVolumeId, placeBusinessVolumeMaintObject.CompanyId);
                placeBusinessVolumeRepository.Add(placeBusinessVolume);
            }
            else
            {
                PlaceBusinessVolume placeBusinessVolume = placeBusinessVolumeRepository.FindByKey(placeBusinessVolumeMaintObject.Id);
                if (placeBusinessVolume != null)
                {
                    placeBusinessVolume.Modify((LogicalType)placeBusinessVolumeMaintObject.LogicalType, placeBusinessVolumeMaintObject.BusinessVolumeId);
                    placeBusinessVolumeRepository.Update(placeBusinessVolume);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlaceBusinessVolumePlaceIdCreateDate"))
                {
                    throw new ApplicationFault("站点业务量重复");
                }
                throw ex;
            }
        }
    }
}
