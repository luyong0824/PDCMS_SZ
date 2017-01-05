using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Domain.Models;
using PDBM.Domain.Models.WorkFlow;
using PDBM.Domain.Repositories;
using PDBM.Infrastructure.Common;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.WorkFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.WorkFlow
{
    public class WFActivityInstanceEditorService : DataService, IWFActivityInstanceEditorService
    {
        private readonly IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository;

        public WFActivityInstanceEditorService(IRepositoryContext context,
            IRepository<WFActivityInstanceEditor> wfActivityInstanceEditorRepository)
            : base(context)
        {
            this.wfActivityInstanceEditorRepository = wfActivityInstanceEditorRepository;
        }

        public WFActivityInstanceEditorMaintObject GetWFActivityInstanceEditorById(Guid id)
        {
            WFActivityInstanceEditor wfActivityInstanceEditor = wfActivityInstanceEditorRepository.FindByKey(id);
            if (wfActivityInstanceEditor != null)
            {
                WFActivityInstanceEditorMaintObject wfActivityInstanceEditorMaintObject = MapperHelper.Map<WFActivityInstanceEditor, WFActivityInstanceEditorMaintObject>(wfActivityInstanceEditor);
                return wfActivityInstanceEditorMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的流程步骤在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改任务
        /// </summary>
        /// <param name="constructionTaskMaintObject">要新增对象</param>
        public void AddWFActivityInstanceEditor(WFActivityInstanceEditorMaintObject wfActivityInstanceEditorMaintObject)
        {
            if (wfActivityInstanceEditorMaintObject.Id == Guid.Empty)
            {
                WFActivityInstanceEditor wfActivityInstanceEditor = AggregateFactory.CreateWFActivityInstanceEditor(wfActivityInstanceEditorMaintObject.WFActivityInstanceId);
                wfActivityInstanceEditorRepository.Add(wfActivityInstanceEditor);
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
