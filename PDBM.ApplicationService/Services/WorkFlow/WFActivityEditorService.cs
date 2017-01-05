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
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    /// <summary>
    /// 工作流活动编辑器应用层服务
    /// </summary>
    public class WFActivityEditorService : DataService, IWFActivityEditorService
    {
        private readonly IRepository<WFActivityEditor> wfActivityEditorRepository;

        public WFActivityEditorService(IRepositoryContext context,
            IRepository<WFActivityEditor> wfActivityEditorRepository)
            : base(context)
        {
            this.wfActivityEditorRepository = wfActivityEditorRepository;
        }

        /// <summary>
        /// 根据工作流类型Id获取状态为使用的工作流活动编辑器列表
        /// </summary>
        /// <param name="wfCategoryId">工作流类型Id</param>
        /// <returns>工作流活动编辑器选择对象列表</returns>
        public IList<WFActivityEditorSelectObject> GetUsedWFActivityEditors(Guid wfCategoryId)
        {
            IList<WFActivityEditorSelectObject> wfActivityEditorSelectObjects = new List<WFActivityEditorSelectObject>();
            IEnumerable<WFActivityEditor> wfActivityEditors = wfActivityEditorRepository.FindAll(Specification<WFActivityEditor>.Eval(entity => entity.WFCategoryId == wfCategoryId && entity.State == State.使用), "WFActivityEditorCode");
            if (wfActivityEditors != null)
            {
                foreach (var wfActivityEditor in wfActivityEditors)
                {
                    wfActivityEditorSelectObjects.Add(MapperHelper.Map<WFActivityEditor, WFActivityEditorSelectObject>(wfActivityEditor));
                }
            }
            return wfActivityEditorSelectObjects;
        }
    }
}
