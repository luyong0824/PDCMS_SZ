using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.BMMgmt;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;

namespace PDBM.ApplicationService.Services.BaseData
{
    /// <summary>
    /// 产权应用层服务
    /// </summary>
    public class PlaceOwnerService : DataService, IPlaceOwnerService
    {
        private readonly IRepository<PlaceOwner> placeOwnerRepository;
        private readonly IRepository<Place> placeRepository;
        private readonly IRepository<Addressing> addressingRepository;
        private readonly IRepository<Purchase> purchaseRepository;

        public PlaceOwnerService(IRepositoryContext context,
            IRepository<PlaceOwner> placeOwnerRepository,
            IRepository<Place> placeRepository,
            IRepository<Addressing> addressingRepository,
            IRepository<Purchase> purchaseRepository)
            : base(context)
        {
            this.placeOwnerRepository = placeOwnerRepository;
            this.placeRepository = placeRepository;
            this.addressingRepository = addressingRepository;
            this.purchaseRepository = purchaseRepository;
        }

        /// <summary>
        /// 根据产权Id获取产权
        /// </summary>
        /// <param name="id">产权Id</param>
        /// <returns>产权维护对象</returns>
        public PlaceOwnerMaintObject GetPlaceOwnerById(Guid id)
        {
            PlaceOwner placeOwner = placeOwnerRepository.FindByKey(id);
            if (placeOwner != null)
            {
                PlaceOwnerMaintObject placeOwnerMaintObject = MapperHelper.Map<PlaceOwner, PlaceOwnerMaintObject>(placeOwner);
                return placeOwnerMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的产权在系统中不存在");
            }
        }

        /// <summary>
        /// 获取产权列表
        /// </summary>
        /// <returns>产权维护对象列表</returns>
        public IList<PlaceOwnerMaintObject> GetPlaceOwners()
        {
            IList<PlaceOwnerMaintObject> placeOwnerMaintObjects = new List<PlaceOwnerMaintObject>();
            IEnumerable<PlaceOwner> placeOwners = placeOwnerRepository.FindAll(null, "PlaceOwnerCode");
            if (placeOwners != null)
            {
                foreach (var placeOwner in placeOwners)
                {
                    placeOwnerMaintObjects.Add(MapperHelper.Map<PlaceOwner, PlaceOwnerMaintObject>(placeOwner));
                }
            }
            return placeOwnerMaintObjects;
        }

        /// <summary>
        /// 获取状态为使用的产权列表
        /// </summary>
        /// <returns>产权选择对象列表</returns>
        public IList<PlaceOwnerSelectObject> GetUsedPlaceOwners()
        {
            IList<PlaceOwnerSelectObject> placeOwnerSelectObjects = new List<PlaceOwnerSelectObject>();
            IEnumerable<PlaceOwner> placeOwners = placeOwnerRepository.FindAll(Specification<PlaceOwner>.Eval(entity => entity.State == State.使用), "PlaceOwnerCode");
            if (placeOwners != null)
            {
                foreach (var placeOwner in placeOwners)
                {
                    placeOwnerSelectObjects.Add(MapperHelper.Map<PlaceOwner, PlaceOwnerSelectObject>(placeOwner));
                }
            }
            return placeOwnerSelectObjects;
        }

        /// <summary>
        /// 新增或者修改产权
        /// </summary>
        /// <param name="placeOwnerMaintObject">要新增或者修改的产权维护对象</param>
        public void AddOrUpdatePlaceOwner(PlaceOwnerMaintObject placeOwnerMaintObject)
        {
            if (placeOwnerMaintObject.Id == Guid.Empty)
            {
                PlaceOwner placeOwner = AggregateFactory.CreatePlaceOwner(placeOwnerMaintObject.PlaceOwnerCode, placeOwnerMaintObject.PlaceOwnerName,
                    placeOwnerMaintObject.Remarks, (State)placeOwnerMaintObject.State, placeOwnerMaintObject.CreateUserId);
                placeOwnerRepository.Add(placeOwner);
            }
            else
            {
                PlaceOwner placeOwner = placeOwnerRepository.FindByKey(placeOwnerMaintObject.Id);
                if (placeOwner != null)
                {
                    placeOwner.Modify(placeOwnerMaintObject.PlaceOwnerCode, placeOwnerMaintObject.PlaceOwnerName, placeOwnerMaintObject.Remarks,
                        (State)placeOwnerMaintObject.State, placeOwnerMaintObject.ModifyUserId);
                    placeOwnerRepository.Update(placeOwner);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_PlaceOwnerCode"))
                {
                    throw new ApplicationFault("产权编码重复");
                }
                else if (ex.Message.Contains("IX_UQ_PlaceOwnerName"))
                {
                    throw new ApplicationFault("产权名称重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除产权
        /// </summary>
        /// <param name="placeOwnerMaintObjects">要删除的产权维护对象列表</param>
        public void RemovePlaceOwners(IList<PlaceOwnerMaintObject> placeOwnerMaintObjects)
        {
            foreach (PlaceOwnerMaintObject placeOwnerMaintObject in placeOwnerMaintObjects)
            {
                PlaceOwner placeOwner = placeOwnerRepository.FindByKey(placeOwnerMaintObject.Id);
                if (placeOwner != null)
                {
                    //if (placeRepository.Exists(Specification<Place>.Eval(entity => entity.PlaceOwnerId == placeOwner.Id)))
                    //{
                    //    throw new ApplicationFault("{0}<br>已在站点中使用", placeOwner.PlaceOwnerCode);
                    //}
                    //else if (addressingRepository.Exists(Specification<Addressing>.Eval(entity => entity.PlaceOwnerId == placeOwner.Id)))
                    //{
                    //    throw new ApplicationFault("{0}<br>已在寻址确认中使用", placeOwner.PlaceOwnerCode);
                    //}
                    //else if (purchaseRepository.Exists(Specification<Purchase>.Eval(entity => entity.PlaceOwnerId == placeOwner.Id)))
                    //{
                    //    throw new ApplicationFault("{0}<br>已在购置站点中使用", placeOwner.PlaceOwnerCode);
                    //}
                    placeOwnerRepository.Remove(placeOwner);
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                //if (ex.Message.Contains("FK_dbo.tbl_Place_dbo.tbl_PlaceOwner_PlaceOwnerId"))
                //{
                //    throw new ApplicationFault("已在站点中使用");
                //}
                //else if (ex.Message.Contains("FK_dbo.tbl_Addressing_dbo.tbl_PlaceOwner_PlaceOwnerId"))
                //{
                //    throw new ApplicationFault("已在寻址确认中使用");
                //}
                //else if (ex.Message.Contains("FK_dbo.tbl_Purchase_dbo.tbl_PlaceOwner_PlaceOwnerId"))
                //{
                //    throw new ApplicationFault("已在购置站点中使用");
                //}
                throw ex;
            }
        }
    }
}
