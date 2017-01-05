using PDBM.DataTransferObjects.BaseData;
using PDBM.Domain.Models;
using PDBM.Domain.Models.BaseData;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.DataAccess.EnterpriseLibrary;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BaseData
{
    public class PlacePropertyLogService : DataService, IPlacePropertyLogService
    {
        private readonly IRepository<PlacePropertyLog> placePropertyLogRepository;

        public PlacePropertyLogService(IRepositoryContext context,
            IRepository<PlacePropertyLog> placePropertyLogRepository)
            : base(context)
        {
            this.placePropertyLogRepository = placePropertyLogRepository;
        }

        public PlacePropertyLogMaintObject GetPlacePropertyLogById(Guid id)
        {
            PlacePropertyLog placePropertyLog = placePropertyLogRepository.FindByKey(id);
            if (placePropertyLog != null)
            {
                PlacePropertyLogMaintObject placePropertyLogMaintObject = MapperHelper.Map<PlacePropertyLog, PlacePropertyLogMaintObject>(placePropertyLog);
                return placePropertyLogMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的站点属性在系统中不存在");
            }
        }

        /// <summary>
        /// 获取运营商共享记录
        /// </summary>
        /// <param name="propertyType">资源类型</param>
        /// <param name="parentId">父表Id</param>
        /// <param name="companyNameId">运营商名称Id</param>
        /// <returns></returns>
        public string GetPlacePropertyLog(int propertyType, Guid parentId, int companyNameId)
        {
            List<Parameter> parameters = new List<Parameter>(3);
            parameters.Add(new Parameter() { Name = "PropertyType", Type = SqlDbType.Int, Value = propertyType });
            parameters.Add(new Parameter() { Name = "ParentId", Type = SqlDbType.UniqueIdentifier, Value = parentId });
            parameters.Add(new Parameter() { Name = "CompanyNameId", Type = SqlDbType.Int, Value = companyNameId });
            using (var dt = SqlHelper.ExecuteDataTable("prc_GetPlacePropertyLog", parameters))
            {
                return JsonHelper.Encode(dt);
            }
        }
    }
}
