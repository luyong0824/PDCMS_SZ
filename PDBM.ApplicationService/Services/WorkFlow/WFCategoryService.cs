using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Domain.Models.Enum;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Domain.Specifications;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    /// <summary>
    /// 工作流类型应用层服务
    /// </summary>
    public class WFCategoryService : DataService, IWFCategoryService
    {
        private readonly IRepository<WFCategory> wfCategoryRepository;

        public WFCategoryService(IRepositoryContext context,
            IRepository<WFCategory> wfCategoryRepository)
            : base(context)
        {
            this.wfCategoryRepository = wfCategoryRepository;
        }

        /// <summary>
        /// 根据工作流类型Id获取工作流类型
        /// </summary>
        /// <param name="id">工作流类型Id</param>
        /// <returns>工作流类型选择对象</returns>
        public WFCategorySelectObject GetWFCategoryById(Guid id)
        {
            WFCategory wfCategory = wfCategoryRepository.FindByKey(id);
            if (wfCategory != null)
            {
                WFCategorySelectObject wfCategorySelectObject = MapperHelper.Map<WFCategory, WFCategorySelectObject>(wfCategory);
                return wfCategorySelectObject;
            }
            else
            {
                throw new ApplicationFault("选择的流程类型在系统中不存在");
            }
        }

        /// <summary>
        /// 获取状态为使用的工作流类型列表
        /// </summary>
        /// <returns>工作流类型选择对象列表</returns>
        public IList<WFCategorySelectObject> GetUsedWFCategorys()
        {
            IList<WFCategorySelectObject> wfCategorySelectObjects = new List<WFCategorySelectObject>();
            IEnumerable<WFCategory> wfCategorys = wfCategoryRepository.FindAll(Specification<WFCategory>.Eval(entity => entity.State == State.使用), "WFCategoryCode");
            if (wfCategorys != null)
            {
                foreach (var wfCategory in wfCategorys)
                {
                    wfCategorySelectObjects.Add(MapperHelper.Map<WFCategory, WFCategorySelectObject>(wfCategory));
                }
            }
            return wfCategorySelectObjects;
        }
    }
}
