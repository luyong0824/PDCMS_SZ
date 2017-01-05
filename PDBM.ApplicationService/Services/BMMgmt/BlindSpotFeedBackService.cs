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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.ApplicationService.Services.BMMgmt
{
    /// <summary>
    /// 盲点反馈应用层服务
    /// </summary>
    public class BlindSpotFeedBackService : DataService, IBlindSpotFeedBackService
    {
        private const int bufferSize = 4096;
        private string baseUploadFolder = ConfigurationManager.AppSettings["baseUploadFolder"];
        private readonly IRepository<File> fileRepository;
        private readonly IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository;
        private readonly IRepository<FileAssociation> fileAssociationRepository;

        public BlindSpotFeedBackService(IRepositoryContext context,
            IRepository<File> fileRepository,
            IRepository<BlindSpotFeedBack> blindSpotFeedBackRepository,
            IRepository<FileAssociation> fileAssociationRepository)
            : base(context)
        {
            this.fileRepository = fileRepository;
            this.blindSpotFeedBackRepository = blindSpotFeedBackRepository;
            this.fileAssociationRepository = fileAssociationRepository;
        }

        /// <summary>
        /// 根据盲点反馈Id获取盲点反馈
        /// </summary>
        /// <param name="id">盲点反馈Id</param>
        /// <returns>盲点反馈维护对象</returns>
        public BlindSpotFeedBackMaintObject GetBlindSpotFeedBackById(Guid id)
        {
            BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(id);
            if (blindSpotFeedBack != null)
            {
                BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject = MapperHelper.Map<BlindSpotFeedBack, BlindSpotFeedBackMaintObject>(blindSpotFeedBack);
                blindSpotFeedBackMaintObject.Id = blindSpotFeedBack.Id;
                blindSpotFeedBackMaintObject.AreaId = blindSpotFeedBack.AreaId;
                blindSpotFeedBackMaintObject.PlaceName = blindSpotFeedBack.PlaceName;
                blindSpotFeedBackMaintObject.Lng = blindSpotFeedBack.Lng;
                blindSpotFeedBackMaintObject.Lat = blindSpotFeedBack.Lat;
                blindSpotFeedBackMaintObject.FeedBackContent = blindSpotFeedBack.FeedBackContent;

                FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == blindSpotFeedBack.Id && entity.EntityName == "BlindSpotFeedBack"));
                if (fileAssociation != null)
                {
                    int count = 0;
                    if (fileAssociation.FileIdList != "")
                    {
                        if (fileAssociation.FileIdList.Contains(","))
                        {
                            string[] strFileList = fileAssociation.FileIdList.Split(',');
                            foreach (string i in strFileList)
                            {
                                count += 1;
                            }
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    blindSpotFeedBackMaintObject.Count = count;
                    blindSpotFeedBackMaintObject.FileIdList = fileAssociation.FileIdList;
                }
                else
                {
                    blindSpotFeedBackMaintObject.Count = 0;
                    blindSpotFeedBackMaintObject.FileIdList = "";
                }
                return blindSpotFeedBackMaintObject;
            }
            else
            {
                throw new ApplicationFault("选择的盲点反馈在系统中不存在");
            }
        }

        /// <summary>
        /// 新增或者修改盲点反馈
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要新增或者修改的盲点反馈对象</param>
        public void AddOrUpdateBlindSpotFeedBack(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            if (blindSpotFeedBackMaintObject.Id == Guid.Empty)
            {
                BlindSpotFeedBack blindSpotFeedBack = AggregateFactory.CreateBlindSpotFeedBack(blindSpotFeedBackMaintObject.PlaceName, blindSpotFeedBackMaintObject.AreaId, blindSpotFeedBackMaintObject.Lng,
                    blindSpotFeedBackMaintObject.Lat, blindSpotFeedBackMaintObject.FeedBackContent, blindSpotFeedBackMaintObject.CreateUserId);
                blindSpotFeedBackRepository.Add(blindSpotFeedBack);

                if (blindSpotFeedBackMaintObject.FileIdList != "")
                {
                    FileAssociation fileAssociation = AggregateFactory.CreateFileAssociation("BlindSpotFeedBack", blindSpotFeedBack.Id, blindSpotFeedBackMaintObject.FileIdList, blindSpotFeedBackMaintObject.CreateUserId);
                    fileAssociationRepository.Add(fileAssociation);
                }
            }
            else
            {
                BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(blindSpotFeedBackMaintObject.Id);
                if (blindSpotFeedBack != null)
                {
                    blindSpotFeedBack.Modify(blindSpotFeedBackMaintObject.PlaceName, blindSpotFeedBackMaintObject.AreaId, blindSpotFeedBackMaintObject.Lng, blindSpotFeedBackMaintObject.Lat, blindSpotFeedBackMaintObject.FeedBackContent, blindSpotFeedBackMaintObject.ModifyUserId);
                    blindSpotFeedBackRepository.Update(blindSpotFeedBack);

                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == blindSpotFeedBack.Id && entity.EntityName == "BlindSpotFeedBack"));
                    if (fileAssociation == null && blindSpotFeedBackMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("BlindSpotFeedBack", blindSpotFeedBack.Id, blindSpotFeedBackMaintObject.FileIdList, blindSpotFeedBackMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (fileAssociation != null && blindSpotFeedBackMaintObject.FileIdList != fileAssociation.FileIdList)
                    {
                        fileAssociation.Modify(blindSpotFeedBackMaintObject.FileIdList, blindSpotFeedBackMaintObject.ModifyUserId);
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
                throw ex;
            }
        }

        /// <summary>
        /// 删除盲点反馈
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObjects">要删除的盲点反馈维护对象</param>
        public void RemoveBlindSpotFeedBacks(IList<BlindSpotFeedBackMaintObject> blindSpotFeedBackMaintObjects)
        {
            foreach (BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject in blindSpotFeedBackMaintObjects)
            {
                BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(blindSpotFeedBackMaintObject.Id);
                if (blindSpotFeedBack != null)
                {
                    if (blindSpotFeedBack.DoState != DoState.未处理)
                    {
                        throw new ApplicationFault("只能删除处理状态为未处理的盲点反馈");
                    }
                    blindSpotFeedBackRepository.Remove(blindSpotFeedBack);
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

        /// <summary>
        /// 根据条件获取分页盲点反馈导入列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="beginDate">起始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="areaId">区域Id</param>
        /// <param name="placeName">盲点地名</param>
        /// <param name="doState">处理状态</param>
        /// <param name="createUserId">创建人用户Id</param>
        /// <param name="companyId">分公司Id</param>
        /// <returns>分页盲点反馈导入列表的Json字符串</returns>
        public string GetBlindSpotFeedBacksPage(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate, Guid areaId, string placeName, int doState, Guid createUserId, Guid companyId)
        {
            List<Parameter> parameters = new List<Parameter>(9);
            parameters.Add(new Parameter() { Name = "PageIndex", Type = SqlDbType.Int, Value = pageIndex });
            parameters.Add(new Parameter() { Name = "PageSize", Type = SqlDbType.Int, Value = pageSize });
            parameters.Add(new Parameter() { Name = "BeginDate", Type = SqlDbType.DateTime, Value = beginDate });
            parameters.Add(new Parameter() { Name = "EndDate", Type = SqlDbType.DateTime, Value = endDate });
            parameters.Add(new Parameter() { Name = "AreaId", Type = SqlDbType.UniqueIdentifier, Value = areaId });
            parameters.Add(new Parameter() { Name = "PlaceName", Type = SqlDbType.NVarChar, Value = placeName });
            parameters.Add(new Parameter() { Name = "DoState", Type = SqlDbType.Int, Value = doState });
            parameters.Add(new Parameter() { Name = "CreateUserId", Type = SqlDbType.UniqueIdentifier, Value = createUserId });
            parameters.Add(new Parameter() { Name = "CompanyId", Type = SqlDbType.UniqueIdentifier, Value = companyId });
            using (var ds = SqlHelper.ExecuteDataSet("prc_QueryBlindSpotFeedBacksPage", parameters))
            {
                Dictionary<string, object> result = new Dictionary<string, object>(2);
                result["data"] = ds.Tables[0];
                result["total"] = ds.Tables[1].Rows[0][0].ToString();
                return JsonHelper.Encode(result);
            }
        }

        /// <summary>
        /// 保存反馈处理
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要修改的盲点反馈对象</param>
        public void SaveBlindSpotHanding(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            if (blindSpotFeedBackMaintObject.Id != Guid.Empty)
            {
                BlindSpotFeedBack blindSpotFeedBack = blindSpotFeedBackRepository.FindByKey(blindSpotFeedBackMaintObject.Id);
                if (blindSpotFeedBack != null)
                {
                    blindSpotFeedBack.SaveBlindSpotHanding(blindSpotFeedBackMaintObject.Lng, blindSpotFeedBackMaintObject.Lat, blindSpotFeedBackMaintObject.FeedBackResult, blindSpotFeedBackMaintObject.ModifyUserId);
                    blindSpotFeedBackRepository.Update(blindSpotFeedBack);

                    FileAssociation fileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == blindSpotFeedBack.Id && entity.EntityName == "BlindSpotFeedBack"));
                    if (fileAssociation == null && blindSpotFeedBackMaintObject.FileIdList != "")
                    {
                        FileAssociation newFileAssociation = AggregateFactory.CreateFileAssociation("BlindSpotFeedBack", blindSpotFeedBack.Id, blindSpotFeedBackMaintObject.FileIdList, blindSpotFeedBackMaintObject.ModifyUserId);
                        fileAssociationRepository.Add(newFileAssociation);
                    }
                    else if (fileAssociation != null && blindSpotFeedBackMaintObject.FileIdList != fileAssociation.FileIdList)
                    {
                        fileAssociation.Modify(blindSpotFeedBackMaintObject.FileIdList, blindSpotFeedBackMaintObject.ModifyUserId);
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
                throw ex;
            }
        }

        /// <summary>
        /// 保存盲点反馈(移动端)
        /// </summary>
        /// <param name="blindSpotFeedBackMaintObject">要保存的盲点反馈对象</param>
        public void SaveBlindSpotFeedBackMobile(BlindSpotFeedBackMaintObject blindSpotFeedBackMaintObject)
        {
            if (blindSpotFeedBackMaintObject.Id == Guid.Empty)
            {
                BlindSpotFeedBack blindSpotFeedBack = AggregateFactory.CreateBlindSpotFeedBack(blindSpotFeedBackMaintObject.PlaceName, blindSpotFeedBackMaintObject.AreaId, blindSpotFeedBackMaintObject.Lng,
                blindSpotFeedBackMaintObject.Lat, blindSpotFeedBackMaintObject.FeedBackContent, blindSpotFeedBackMaintObject.CreateUserId);
                blindSpotFeedBackRepository.Add(blindSpotFeedBack);

                if (blindSpotFeedBackMaintObject.Base64String.Length > 0)
                {
                    string fileIdList = "";
                    foreach (string base64 in blindSpotFeedBackMaintObject.Base64String)
                    {
                        if (base64.Contains(","))
                        {
                            string base64Content = base64.Split(',')[1];
                            byte[] bt = Convert.FromBase64String(base64Content);
                            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bt))
                            {
                                Guid fileId = Guid.NewGuid();
                                DateTime today = DateTime.Now;
                                string uploadFolder = System.IO.Path.Combine(baseUploadFolder, string.Format("{0}-{1}-{2}", today.Year, today.Month, today.Day));
                                FileHelper.CreateDirectory(uploadFolder);
                                string filePath = System.IO.Path.Combine(uploadFolder, Guid.NewGuid() + ".jpeg");
                                FileHelper.UploadFile(stream, filePath, bufferSize);
                                string fileName = fileId.ToString().Replace("-", "");
                                File file = AggregateFactory.CreateFile(fileId, fileName, "application/octet-stream",
                                    ".jpeg", bt.Length, filePath, blindSpotFeedBackMaintObject.CreateUserId);
                                fileRepository.Add(file);

                                fileIdList += fileId + ",";
                            }
                        }
                    }
                    FileAssociation blindSpotFeedFileAssociation = fileAssociationRepository.Find(Specification<FileAssociation>.Eval(entity => entity.EntityId == blindSpotFeedBack.Id && entity.EntityName == "BlindSpotFeedBack"));
                    if (blindSpotFeedFileAssociation == null && fileIdList.ToString() != "")
                    {
                        FileAssociation newblindSpotFeedFileAssociation = AggregateFactory.CreateFileAssociation("BlindSpotFeedBack", blindSpotFeedBack.Id, fileIdList.Substring(0, fileIdList.Length - 1), blindSpotFeedBackMaintObject.CreateUserId);
                        fileAssociationRepository.Add(newblindSpotFeedFileAssociation);
                    }
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
