using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PDBM.DataTransferObjects.FileMgmt;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.FileMgmt;
using PDBM.Web.Filters;
using PDBM.DataTransferObjects.BaseData;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    public class FileController : BaseController
    {
        /// <summary>
        /// 文件管理器
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public ActionResult FileManager()
        {
            return View();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        [HttpPost]
        public async Task<ActionResult> UploadFile()
        {
            if (Request.Files.Count == 0)
            {
                throw new ArgumentNullException("File");
            }

            var file = Request.Files[0];
            if (file.ContentLength <= 15728640)
            {
                Guid fileId = Guid.NewGuid();
                using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
                {
                    await Task.Factory.StartNew(() => proxy.Channel.UploadFile(new FileUploadObject()
                    {
                        Id = fileId,
                        FileName = file.FileName,
                        FileSize = file.ContentLength,
                        FileType = file.ContentType,
                        FileExtension = Path.GetExtension(file.FileName),
                        FileData = file.InputStream,
                        UploadUserId = this.UserId
                    }));
                    return Sucess(fileId.ToString());
                }
            }
            else
            {
                return this.Error("单个文件大小不能超过15MB");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id">文件Id</param>
        /// <returns></returns>
        public async Task<ActionResult> DownloadFile(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                FileDownloadObject fileDownloadObject = await Task.Factory.StartNew(() => proxy.Channel.GetFileDownloadByFileId(id));
                using (Stream source = proxy.Channel.DownloadFile(fileDownloadObject.FilePath))
                {
                    if (source.CanRead)
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + StringHelper.UrlEncode(fileDownloadObject.FileName));
                        Response.AppendHeader("content-length", fileDownloadObject.FileSize.ToString());
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int perCopied = 0;
                        while ((perCopied = source.Read(buffer, 0, bufferSize)) > 0)
                        {
                            Response.OutputStream.Write(buffer, 0, perCopied);
                            Response.Flush();
                        }
                    }
                    return new EmptyResult();
                }
            }
        }

        /// <summary>
        /// 下载本地示例及模板
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadTemplatesAndSamples()
        {
            if (Request["FileType"] == null)
            {
                throw new ArgumentNullException("FileType");
            }

            string fileName = "";
            switch (Request["FileType"])
            {
                case "1": fileName = "基站规划导入模板.xlsx"; break;
                case "2": fileName = "基站规划导入示例.xlsx"; break;
                case "3": fileName = "基站改造导入模板.xlsx"; break;
                case "4": fileName = "基站改造导入示例.xlsx"; break;
                case "5": fileName = "基站导入模板.xlsx"; break;
                case "6": fileName = "基站导入示例.xlsx"; break;
                case "7": fileName = "逻辑号导入模板.xlsx"; break;
                case "8": fileName = "逻辑号导入示例.xlsx"; break;
                case "9": fileName = "业务量导入模板.xlsx"; break;
                case "10": fileName = "业务量导入示例.xlsx"; break;
                case "11": fileName = "基站建设导入模板.xlsx"; break;
                case "12": fileName = "基站建设导入示例.xlsx"; break;
                case "13": fileName = "室分建设导入模板.xlsx"; break;
                case "14": fileName = "室分建设导入示例.xlsx"; break;
                case "15": fileName = "室分规划导入模板.xlsx"; break;
                case "16": fileName = "室分规划导入示例.xlsx"; break;
                case "17": fileName = "室分改造导入模板.xlsx"; break;
                case "18": fileName = "室分改造导入示例.xlsx"; break;
                case "19": fileName = "室分导入模板.xlsx"; break;
                case "20": fileName = "室分导入示例.xlsx"; break;
            }

            if (fileName.Trim() != "")
            {
                string filePath = Server.MapPath("~/Files/") + fileName;
                using (Stream source = FileHelper.FileToStream(filePath))
                {
                    if (source.CanRead)
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + StringHelper.UrlEncode(fileName));
                        Response.AppendHeader("content-length", source.Length.ToString());
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int perCopied = 0;
                        while ((perCopied = source.Read(buffer, 0, bufferSize)) > 0)
                        {
                            Response.OutputStream.Write(buffer, 0, perCopied);
                            Response.Flush();
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 下载帮助文档
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadHelpFiles()
        {
            if (Request["FileName"] == null)
            {
                throw new ArgumentNullException("FileName");
            }

            string fileName = "";
            switch (Request["FileName"])
            {
                case "Company": fileName = "公司.pdf"; break;
                case "Department": fileName = "部门.pdf"; break;
                case "Post": fileName = "岗位.pdf"; break;
                case "UserAccount": fileName = "用户账号.pdf"; break;
                case "UserInfo": fileName = "用户信息.pdf"; break;
                case "RoleUser": fileName = "角色用户.pdf"; break;
                case "PostUser": fileName = "岗位用户.pdf"; break;
                case "UserDepartmentChange": fileName = "部门调动.pdf"; break;
                case "Role": fileName = "角色.pdf"; break;
                case "RoleMenuItem": fileName = "角色菜单.pdf"; break;
                case "Project": fileName = "项目信息.pdf"; break;
                case "Area": fileName = "区域.pdf"; break;
                case "Reseau": fileName = "网格.pdf"; break;
                case "Scene": fileName = "周边场景.pdf"; break;
                case "PlaceCategory": fileName = "站点类型.pdf"; break;
                case "Place": fileName = "站点信息.pdf"; break;
                case "Unit": fileName = "计量单位.pdf"; break;
                case "MaterialCategory": fileName = "物资类别.pdf"; break;
                case "Material": fileName = "物资名称.pdf"; break;
                case "MaterialSpec": fileName = "设计规格.pdf"; break;
                case "MaterialPurchase": fileName = "物资申购.pdf"; break;
                case "Customer": fileName = "往来单位.pdf"; break;
                case "WorkBigClass": fileName = "工单大类.pdf"; break;
                case "WorkSmallClass": fileName = "工单小类.pdf"; break;
                case "WFProcess": fileName = "流程定义.pdf"; break;
                case "WFActivity": fileName = "流程步骤.pdf"; break;
                case "MyWFInstance": fileName = "我的公文.pdf"; break;
                case "WFInstanceQuery": fileName = "公文查询.pdf"; break;
                case "OperatorsPlanning": fileName = "运营商基站规划.pdf"; break;
                case "Planning": fileName = "基站规划.pdf"; break;
                case "OperatorsConfirm": fileName = "运营商需求确认.pdf"; break;
                case "OperatorsPlanningDemand": fileName = "改造站需求确认.pdf"; break;
                case "Addressing": fileName = "寻址确认.pdf"; break;
                case "OperatorsSharing": fileName = "运营商共享基站.pdf"; break;
                case "ShareSummary": fileName = "共享基站汇总.pdf"; break;
                case "NewPlanning": fileName = "新增基站.pdf"; break;
                case "NewRemodeling": fileName = "改造基站.pdf"; break;
                case "RegisterPlanning": fileName = "新增安装登记.pdf"; break;
                case "RegisterRemodeing": fileName = "改造安装登记.pdf"; break;
                case "ProjectManagement": fileName = "项目管理.pdf"; break;
                case "EngineeringManagement": fileName = "工程管理.pdf"; break;
                case "ProjectSetting": fileName = "项目设置.pdf"; break;
                case "EngineeringSetting": fileName = "工程设置.pdf"; break;
                case "Purchase": fileName = "站点导入.pdf"; break;
                case "ResourceImport": fileName = "资源导入.pdf"; break;
                case "PlaceMaintenance": fileName = "站点维护.pdf"; break;
                case "PlaceReport": fileName = "基站清单.pdf"; break;
                case "OperatorsPlanningReport": fileName = "运营商规划清单.pdf"; break;
                case "ConstructionPlanningReport": fileName = "新增基站建设进度表.pdf"; break;
                case "ConstructionRemodeingReport": fileName = "改造基站建设进度表.pdf"; break;
                case "ProjectInformation": fileName = "项目信息表.pdf"; break;
                case "WorkApply": fileName = "隐患上报.pdf"; break;
                case "WorkOrder": fileName = "零星用工.pdf"; break;
                case "WorkApplyReport": fileName = "隐患上报清单.pdf"; break;
                case "WorkOrderReport": fileName = "零星派工清单.pdf"; break;
                case "HelpSummary": fileName = "通信基础设施建设辅助管理系统使用手册.pdf"; break;
            }

            if (fileName.Trim() != "")
            {
                string filePath = Server.MapPath("~/HelpFiles/") + fileName;
                using (Stream source = FileHelper.FileToStream(filePath))
                {
                    if (source.CanRead)
                    {
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + StringHelper.UrlEncode(fileName));
                        Response.AppendHeader("content-length", source.Length.ToString());
                        int bufferSize = 4096;
                        byte[] buffer = new byte[bufferSize];
                        int perCopied = 0;
                        while ((perCopied = source.Read(buffer, 0, bufferSize)) > 0)
                        {
                            Response.OutputStream.Write(buffer, 0, perCopied);
                            Response.Flush();
                        }
                    }
                }
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetFiles()
        {
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetFiles(Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取盲点反馈附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetBlindSpotFeedBackFiles()
        {
            if (Request["BlindSpotFeedBackId"] == null)
            {
                throw new ArgumentNullException("BlindSpotFeedBackId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetBlindSpotFeedBackFiles(Guid.Parse(Request["BlindSpotFeedBackId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取基站改造附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetRemodelingFiles()
        {
            if (Request["RemodelingId"] == null)
            {
                throw new ArgumentNullException("RemodelingId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetRemodelingFiles(Guid.Parse(Request["RemodelingId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取工程进度现场摄像附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetEngineeringProgressFiles()
        {
            if (Request["EngineeringTaskId"] == null)
            {
                throw new ArgumentNullException("EngineeringTaskId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringProgressFiles(Guid.Parse(Request["EngineeringTaskId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取项目进度现场摄像附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetProjectProgressFiles()
        {
            if (Request["ProjectTaskId"] == null)
            {
                throw new ArgumentNullException("ProjectTaskId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProjectProgressFiles(Guid.Parse(Request["ProjectTaskId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取施工图附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetEngineeringDesignFiles()
        {
            if (Request["EngineeringTaskId"] == null)
            {
                throw new ArgumentNullException("EngineeringTaskId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEngineeringDesignFiles(Guid.Parse(Request["EngineeringTaskId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取总设图附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetGeneralDesignFiles()
        {
            if (Request["ProjectTaskId"] == null)
            {
                throw new ArgumentNullException("ProjectTaskId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetGeneralDesignFiles(Guid.Parse(Request["ProjectTaskId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取建设申请附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetPlanningApplyFiles()
        {
            if (Request["PlanningApplyId"] == null)
            {
                throw new ArgumentNullException("PlanningApplyId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningApplyFiles(Guid.Parse(Request["PlanningApplyId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取规划附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetPlanningFiles()
        {
            if (Request["PlanningId"] == null)
            {
                throw new ArgumentNullException("PlanningId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlanningFiles(Guid.Parse(Request["PlanningId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取寻址确认示意图
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetAddressingFiles()
        {
            if (Request["AddressingId"] == null)
            {
                throw new ArgumentNullException("AddressingId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAddressingFiles(Guid.Parse(Request["AddressingId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取购置站点示意图
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetPurchaseFiles()
        {
            if (Request["PurchaseId"] == null)
            {
                throw new ArgumentNullException("PurchaseId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPurchaseFiles(Guid.Parse(Request["PurchaseId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取站点示意图
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetPlaceFiles()
        {
            if (Request["PlaceId"] == null)
            {
                throw new ArgumentNullException("PlaceId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetPlaceFiles(Guid.Parse(Request["PlaceId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取公文附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetWFProcessInstanceFiles()
        {
            if (Request["WFProcessInstanceId"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceId");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWFProcessInstanceFiles(Guid.Parse(Request["WFProcessInstanceId"]))),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取公文流程步骤附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetWFActivityInstanceFiles()
        {
            if (Request["WFActivityInstanceId"] == null)
            {
                throw new ArgumentNullException("WFActivityInstanceId");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWFActivityInstanceFiles(Guid.Parse(Request["WFActivityInstanceId"]))),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetTowerFiles()
        {
            if (Request["TowerId"] == null)
            {
                throw new ArgumentNullException("TowerId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetTowerFiles(Guid.Parse(Request["TowerId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetTowerBaseFiles()
        {
            if (Request["TowerBaseId"] == null)
            {
                throw new ArgumentNullException("TowerBaseId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetTowerBaseFiles(Guid.Parse(Request["TowerBaseId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetMachineRoomFiles()
        {
            if (Request["MachineRoomId"] == null)
            {
                throw new ArgumentNullException("MachineRoomId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMachineRoomFiles(Guid.Parse(Request["MachineRoomId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetExternalElectricPowerFiles()
        {
            if (Request["ExternalElectricPowerId"] == null)
            {
                throw new ArgumentNullException("ExternalElectricPowerId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetExternalElectricPowerFiles(Guid.Parse(Request["ExternalElectricPowerId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetEquipmentInstallFiles()
        {
            if (Request["EquipmentInstallId"] == null)
            {
                throw new ArgumentNullException("EquipmentInstallId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEquipmentInstallFiles(Guid.Parse(Request["EquipmentInstallId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetAddressExplorFiles()
        {
            if (Request["AddressExplorId"] == null)
            {
                throw new ArgumentNullException("AddressExplorId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAddressExplorFiles(Guid.Parse(Request["AddressExplorId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetFoundationTestFiles()
        {
            if (Request["FoundationTestId"] == null)
            {
                throw new ArgumentNullException("FoundationTestId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetFoundationTestFiles(Guid.Parse(Request["FoundationTestId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取铁塔图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetTowerLogFiles()
        {
            if (Request["TowerLogId"] == null)
            {
                throw new ArgumentNullException("TowerLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetTowerLogFiles(Guid.Parse(Request["TowerLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取塔基图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetTowerBaseLogFiles()
        {
            if (Request["TowerBaseLogId"] == null)
            {
                throw new ArgumentNullException("TowerBaseLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetTowerBaseLogFiles(Guid.Parse(Request["TowerBaseLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取机房图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetMachineRoomLogFiles()
        {
            if (Request["MachineRoomLogId"] == null)
            {
                throw new ArgumentNullException("MachineRoomLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetMachineRoomLogFiles(Guid.Parse(Request["MachineRoomLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取路由图
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetExternalElectricPowerLogFiles()
        {
            if (Request["ExternalElectricPowerLogId"] == null)
            {
                throw new ArgumentNullException("ExternalElectricPowerLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetExternalElectricPowerLogFiles(Guid.Parse(Request["ExternalElectricPowerLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取安装图纸
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetEquipmentInstallLogFiles()
        {
            if (Request["EquipmentInstallLogId"] == null)
            {
                throw new ArgumentNullException("EquipmentInstallLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetEquipmentInstallLogFiles(Guid.Parse(Request["EquipmentInstallLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取地勘报告
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetAddressExplorLogFiles()
        {
            if (Request["AddressExplorLogId"] == null)
            {
                throw new ArgumentNullException("AddressExplorLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetAddressExplorLogFiles(Guid.Parse(Request["AddressExplorLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取动测报告
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetFoundationTestLogFiles()
        {
            if (Request["FoundationTestLogId"] == null)
            {
                throw new ArgumentNullException("FoundationTestLogId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetFoundationTestLogFiles(Guid.Parse(Request["FoundationTestLogId"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取现场影像
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetProgressFiles()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProgressFiles(Guid.Parse(Request["Id"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取现场影像历史记录
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetProgressLogFiles()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetProgressLogFiles(Guid.Parse(Request["Id"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取隐患上报单附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetWorkApplyFiles()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkApplyFiles(Guid.Parse(Request["Id"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取零星派工单附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetWorkOrderFiles()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderFiles(Guid.Parse(Request["Id"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取零星派工单执行说明附件
        /// </summary>
        /// <returns></returns>
        [AuthorizeFilter]
        public async Task<ActionResult> GetWorkOrderWFFiles()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }

            using (ServiceProxy<IFileService> proxy = new ServiceProxy<IFileService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWorkOrderWFFiles(Guid.Parse(Request["Id"]), Request["FileIdList"])),
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}