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
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BMMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    public class DelayApplyService : DataService, IDelayApplyService
    {
        private readonly IRepository<DelayApply> delayApplyRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;
        private readonly IOrderCodeSeedRepository orderCodeSeedRepository;

        public DelayApplyService(IRepositoryContext context,
            IRepository<DelayApply> delayApplyRepository,
            IRepository<FileAssociation> fileAssociationRepository,
            IOrderCodeSeedRepository orderCodeSeedRepository)
            : base(context)
        {
            this.delayApplyRepository = delayApplyRepository;
            this.fileAssociationRepository = fileAssociationRepository;
            this.orderCodeSeedRepository = orderCodeSeedRepository;
        }

        /// <summary>
        /// 根据工期延误申请Id获取工期延误申请
        /// </summary>
        /// <param name="id">工期延误申请Id</param>
        /// <returns>工期延误申请维护对象</returns>
        public DelayApplyMaintObject GetDelayApplyById(Guid id)
        {
            DelayApply delayApply = delayApplyRepository.FindByKey(id);
            if (delayApply != null)
            {
                DelayApplyMaintObject delayApplyMaintObject = MapperHelper.Map<DelayApply, DelayApplyMaintObject>(delayApply);
                //workApplyMaintObject.Id = id;
                //Customer customer = customerRepository.FindByKey(workApply.CustomerId);
                //workApplyMaintObject.CustomerName = customer.CustomerName;
                //workApplyMaintObject.Count = 0;
                //FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == workApply.Id && entity.EntityName == "WorkApply"));
                //if (fileAssociation != null)
                //{
                //    int count = 0;
                //    if (fileAssociation.FileIdList != "")
                //    {
                //        if (fileAssociation.FileIdList.Contains(","))
                //        {
                //            string[] strFileList = fileAssociation.FileIdList.Split(',');
                //            foreach (string i in strFileList)
                //            {
                //                count += 1;
                //            }
                //        }
                //        else
                //        {
                //            count = 1;
                //        }
                //    }
                //    workApplyMaintObject.Count = count;
                //    workApplyMaintObject.FileIdList = fileAssociation.FileIdList;
                //}
                //else
                //{
                //    workApplyMaintObject.Count = 0;
                //    workApplyMaintObject.FileIdList = "";
                //}
                return delayApplyMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的工期延误申请单在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改工期延误申请
        /// </summary>
        /// <param name="delayApplyMaintObject">要新增或者修改的工期延误申请对象</param>
        public void AddOrUpdateDelayApply(DelayApplyMaintObject delayApplyMaintObject)
        {
            if (delayApplyMaintObject.Id == Guid.Empty)
            {
                DelayApply delayApply = AggregateFactory.CreateDelayApply(delayApplyMaintObject.ConstructionTaskId, delayApplyMaintObject.Title, (EngineeringProgress)delayApplyMaintObject.ConstructionProgress,
                    delayApplyMaintObject.DelayDays, delayApplyMaintObject.Remarks, delayApplyMaintObject.CreateUserId);
                delayApplyRepository.Add(delayApply);

                if (delayApplyMaintObject.FileIdList != "")
                {
                    FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("DelayApply", delayApply.Id, delayApplyMaintObject.FileIdList, delayApplyMaintObject.CreateUserId);
                    fileAssociationRepository.Add(fileAssociation);
                }
            }
            else
            {
                DelayApply delayApply = delayApplyRepository.FindByKey(delayApplyMaintObject.Id);
                if (delayApply != null)
                {
                    //delayApply.CheckByUpdate(workApplyMaintObject.ModifyUserId);
                    delayApply.Modify(delayApplyMaintObject.Title, (EngineeringProgress)delayApplyMaintObject.ConstructionProgress, delayApplyMaintObject.DelayDays, delayApplyMaintObject.Remarks, delayApplyMaintObject.ModifyUserId);
                    delayApplyRepository.Update(delayApply);

                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == delayApply.Id && entity.EntityName == "DelayApply"));
                    if (fileAssociation == null && delayApplyMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("DelayApply", delayApply.Id, delayApplyMaintObject.FileIdList, delayApplyMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (fileAssociation != null && delayApplyMaintObject.FileIdList != fileAssociation.FileIdList)
                    {
                        fileAssociation.Modify(delayApplyMaintObject.FileIdList, delayApplyMaintObject.ModifyUserId);
                        fileAssociationRepository.Update(fileAssociation);
                    }
                }
            }
            try
            {
                this.Context.Commit();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_UQ_DelayApplyTitle"))
                {
                    throw new ApplicationFault("标题重复");
                }
                throw ex;
            }
        }

        /// <summary>
        /// 删除工期延误申请
        /// </summary>
        /// <param name="delayApplyMaintObjects">要删除的工期延误申请维护对象</param>
        public void RemoveDelayApplys(IList<DelayApplyMaintObject> delayApplyMaintObjects)
        {
            foreach (DelayApplyMaintObject delayApplyMaintObject in delayApplyMaintObjects)
            {
                DelayApply delayApply = delayApplyRepository.FindByKey(delayApplyMaintObject.Id);
                if (delayApply != null)
                {
                    if (delayApply.OrderState != WFProcessInstanceState.未发送)
                    {
                        throw new ApplicationFault("只能删除申请状态为未发送的工期延误申请单");
                    }
                    delayApplyRepository.Remove(delayApply);
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
