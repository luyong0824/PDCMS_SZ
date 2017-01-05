using PDBM.DataTransferObjects.BaseData;
using PDBM.DataTransferObjects.WorkFlow;
using PDBM.Infrastructure.Communication;
using PDBM.Infrastructure.IoC;
using PDBM.Infrastructure.Utils;
using PDBM.ServiceContracts.BaseData;
using PDBM.ServiceContracts.Enum;
using PDBM.ServiceContracts.WorkFlow;
using PDBM.Web.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PDBM.Web.Controllers
{
    /// <summary>
    /// 工作流控制器
    /// </summary>
    [AuthorizeFilter]
    public class WorkFlowController : BaseController
    {
        #region 流程定义

        /// <summary>
        /// 流程定义
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WFProcess()
        {
            using (ServiceProxy<IWFCategoryService> proxy = new ServiceProxy<IWFCategoryService>())
            {
                IList<WFCategorySelectObject> wfCategorySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFCategorys());
                wfCategorySelectObjects.Insert(0, new WFCategorySelectObject() { Id = Guid.Empty, WFCategoryName = "请选择" });
                ViewData["WFCategorysBySelect"] = JsonHelper.Encode(wfCategorySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["State"] = JsonHelper.Encode(enumService.GetStateEnum());
            ViewData["Bool"] = JsonHelper.Encode(enumService.GetBoolEnum());
            return View();
        }

        /// <summary>
        /// 根据流程定义Id获取流程定义
        /// </summary>
        /// <param name="id">流程定义Id</param>
        /// <returns></returns>
        public async Task<ActionResult> GetWFProcessById(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                return Json(await Task.Factory.StartNew(() => proxy.Channel.GetWFProcessById(id)), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取流程定义列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFProcesses()
        {
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFProcesses());
            }
        }

        /// <summary>
        /// 保存流程定义
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWFProcess()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Dictionary<string, object> row = (Dictionary<string, object>)JsonHelper.Decode(Request["data"]);
            WFProcessMaintObject wfProcessMaintObject = null;
            Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
            if (id == Guid.Empty)
            {
                wfProcessMaintObject = new WFProcessMaintObject()
                {
                    Id = Guid.Empty,
                    WFProcessCode = row["WFProcessCode"].ToString().Trim(),
                    WFProcessName = row["WFProcessName"].ToString().Trim(),
                    WFCategoryId = Guid.Parse(row["WFCategoryId"].ToString()),
                    IsApprovedByManager = row["IsApprovedByManager"].ToString() == "true" ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    CreateUserId = this.UserId
                };
            }
            else
            {
                wfProcessMaintObject = new WFProcessMaintObject()
                {
                    Id = id,
                    WFProcessCode = row["WFProcessCode"].ToString().Trim(),
                    WFProcessName = row["WFProcessName"].ToString().Trim(),
                    IsApprovedByManager = row["IsApprovedByManager"].ToString() == "true" ? 1 : 2,
                    Remarks = row["Remarks"].ToString().Trim(),
                    State = int.Parse(row["State"].ToString()),
                    ModifyUserId = this.UserId
                };
            }
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWFProcess(wfProcessMaintObject));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除流程定义
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWFProcesses()
        {
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WFProcessMaintObject> wfProcessMaintObjects = new List<WFProcessMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    wfProcessMaintObjects.Add(new WFProcessMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWFProcesses(wfProcessMaintObjects));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 流程步骤

        /// <summary>
        /// 流程步骤
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WFActivity()
        {
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                IList<WFProcessSelectObject> wfProcessSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFProcesses());
                wfProcessSelectObjects.Add(new WFProcessSelectObject() { Id = Guid.Empty, WFCategoryId = Guid.Empty, WFProcessName = "流程", PId = Guid.NewGuid(), isLeaf = false });
                ViewData["WFProcessesTree"] = JsonHelper.Encode(wfProcessSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                //companySelectObjects.Add(new CompanySelectObject() { Id = Guid.Empty, CompanyName = "请选择" });
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            using (ServiceProxy<IPostService> proxy = new ServiceProxy<IPostService>())
            {
                IList<PostSelectObject> postSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedPosts());
                postSelectObjects.Add(new PostSelectObject() { Id = Guid.Empty, PostName = "无" });
                ViewData["PostsByEmpty"] = JsonHelper.Encode(postSelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["WFActivityOperate"] = JsonHelper.Encode(enumService.GetWFActivityOperateEnum());
            ViewData["WFActivityOrder"] = JsonHelper.Encode(enumService.GetWFActivityOrderEnum());
            ViewData["Bool"] = JsonHelper.Encode(enumService.GetBoolEnum());
            ViewData["CompanyId"] = this.CompanyId;
            return View();
        }

        /// <summary>
        /// 获取流程步骤列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFActivitys()
        {
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }

            using (ServiceProxy<IWFActivityService> proxy = new ServiceProxy<IWFActivityService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFActivitys(Guid.Parse(Request["WFProcessId"])));
            }
        }

        /// <summary>
        /// 获取流程步骤列表，用于发送公文
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFActivitysBySend()
        {
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }

            if (Request["EntityId"] == null)
            {
                throw new ArgumentNullException("EntityId");
            }

            using (ServiceProxy<IWFActivityService> proxy = new ServiceProxy<IWFActivityService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFActivitysBySend(Guid.Parse(Request["WFProcessId"]), this.UserId, Guid.Parse(Request["EntityId"])));
            }
        }

        /// <summary>
        /// 保存流程步骤
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveWFActivitys()
        {
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Guid wfProcessId = Guid.Parse(Request["WFProcessId"]);
            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WFActivityMaintObject> wfActivityMaintObjects = new List<WFActivityMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] == null || row["Id"].ToString() == "" ? Guid.Empty : row["Id"]).ToString());
                if (id == Guid.Empty)
                {
                    wfActivityMaintObjects.Add(new WFActivityMaintObject()
                    {
                        Id = Guid.Empty,
                        WFProcessId = wfProcessId,
                        WFActivityName = row["WFActivityName"].ToString().Trim(),
                        WFActivityOperate = int.Parse(row["WFActivityOperate"].ToString()),
                        WFActivityEditorId = Guid.Parse(row["WFActivityEditorId"].ToString()),
                        WFActivityOrder = int.Parse(row["WFActivityOrder"].ToString()),
                        SerialId = int.Parse(row["SerialId"].ToString()),
                        RowId = int.Parse(row["RowId"].ToString()),
                        Timelimit = int.Parse(row["Timelimit"].ToString()),
                        CompanyId = Guid.Parse(row["CompanyId"].ToString()),
                        DepartmentId = Guid.Parse(row["DepartmentId"].ToString()),
                        UserId = Guid.Parse(row["UserId"].ToString()),
                        PostId = Guid.Parse(row["PostId"].ToString()),
                        IsMustEdit = int.Parse(row["IsMustEdit"].ToString()),
                        IsNecessaryStep = int.Parse(row["IsNecessaryStep"].ToString()),
                        CreateUserId = this.UserId
                    });
                }
                else
                {
                    wfActivityMaintObjects.Add(new WFActivityMaintObject()
                    {
                        Id = id,
                        WFActivityName = row["WFActivityName"].ToString().Trim(),
                        WFActivityOperate = int.Parse(row["WFActivityOperate"].ToString()),
                        WFActivityEditorId = Guid.Parse(row["WFActivityEditorId"].ToString()),
                        WFActivityOrder = int.Parse(row["WFActivityOrder"].ToString()),
                        SerialId = int.Parse(row["SerialId"].ToString()),
                        RowId = int.Parse(row["RowId"].ToString()),
                        Timelimit = int.Parse(row["Timelimit"].ToString()),
                        CompanyId = Guid.Parse(row["CompanyId"].ToString()),
                        DepartmentId = Guid.Parse(row["DepartmentId"].ToString()),
                        UserId = Guid.Parse(row["UserId"].ToString()),
                        PostId = Guid.Parse(row["PostId"].ToString()),
                        IsMustEdit = int.Parse(row["IsMustEdit"].ToString()),
                        IsNecessaryStep = int.Parse(row["IsNecessaryStep"].ToString()),
                        ModifyUserId = this.UserId
                    });
                }
            }
            using (ServiceProxy<IWFActivityService> proxy = new ServiceProxy<IWFActivityService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.AddOrUpdateWFActivitys(wfActivityMaintObjects, wfProcessId));
            }
            return this.Sucess("数据保存成功");
        }

        /// <summary>
        /// 删除流程步骤
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveWFActivitys()
        {
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Guid wfProcessId = Guid.Parse(Request["WFProcessId"]);
            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            IList<WFActivityMaintObject> wfActivityMaintObjects = new List<WFActivityMaintObject>();
            foreach (Dictionary<string, object> row in rows)
            {
                Guid id = Guid.Parse((row["Id"] ?? Guid.Empty).ToString());
                if (id != Guid.Empty)
                {
                    wfActivityMaintObjects.Add(new WFActivityMaintObject() { Id = id });
                }
            }
            using (ServiceProxy<IWFActivityService> proxy = new ServiceProxy<IWFActivityService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.RemoveWFActivitys(wfActivityMaintObjects, wfProcessId));
            }
            return this.Sucess("数据删除成功");
        }

        #endregion

        #region 流程步骤编辑器

        /// <summary>
        /// 获取流程步骤编辑器列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> GetWFActivityEditors()
        {
            if (Request["WFCategoryId"] == null)
            {
                throw new ArgumentNullException("WFCategoryId");
            }

            using (ServiceProxy<IWFActivityEditorService> proxy = new ServiceProxy<IWFActivityEditorService>())
            {
                IList<WFActivityEditorSelectObject> wfActivityEditorSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFActivityEditors(Guid.Parse(Request["WFCategoryId"])));
                wfActivityEditorSelectObjects.Insert(0, new WFActivityEditorSelectObject() { Id = Guid.Empty, WFActivityEditorName = "请选择" });
                return Json(wfActivityEditorSelectObjects, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region 公文处理

        /// <summary>
        /// 发送公文
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WFInstanceSend(Guid WFCategoryId, Guid EntityId)
        {
            if (Request["WFCategoryId"] == null)
            {
                throw new ArgumentNullException("WFCategoryId");
            }
            if (Request["EntityId"] == null)
            {
                throw new ArgumentNullException("EntityId");
            }

            ViewData["WFCategoryId"] = WFCategoryId;
            ViewData["EntityId"] = EntityId;
            ViewData["FullName"] = this.FullName;
            ViewData["SendDate"] = DateTime.Now.ToShortDateString();
            using (ServiceProxy<IWFCategoryService> proxy = new ServiceProxy<IWFCategoryService>())
            {
                WFCategorySelectObject wfCategorySelectObject = await Task.Factory.StartNew(() => proxy.Channel.GetWFCategoryById(Guid.Parse(Request["WFCategoryId"])));
                ViewData["PrintUrl"] = wfCategorySelectObject.PrintUrl + "/" + EntityId;
            }
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                IList<WFProcessSelectObject> wfProcessSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFProcessesByWFCategoryId(Guid.Parse(Request["WFCategoryId"])));
                if (wfProcessSelectObjects.Count == 0)
                {
                    wfProcessSelectObjects.Insert(0, new WFProcessSelectObject() { Id = Guid.Empty, WFProcessName = "暂无流程定义" });
                }
                ViewData["WFProcesses"] = JsonHelper.Encode(wfProcessSelectObjects);
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                string wfProcessInstanceName = await Task.Factory.StartNew(() => proxy.Channel.GetWFProcessInstanceName(WFCategoryId, EntityId));
                ViewData["WFProcessInstanceName"] = JsonHelper.Encode(wfProcessInstanceName);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["WFActivityOperate"] = JsonHelper.Encode(enumService.GetWFActivityOperateEnum());
            ViewData["WFActivityOrder"] = JsonHelper.Encode(enumService.GetWFActivityOrderEnum());
            ViewData["Bool"] = JsonHelper.Encode(enumService.GetBoolEnum());
            ViewData["CompanyId"] = this.CompanyId;
            return View();
        }

        /// <summary>
        /// 公文发送处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WFInstanceSending()
        {
            if (Request["EntityId"] == null)
            {
                throw new ArgumentNullException("EntityId");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }
            if (Request["Content"] == null)
            {
                throw new ArgumentNullException("Content");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            Guid entityId = Guid.Parse(Request["EntityId"]);
            string wfProcessInstanceName = Request["WFProcessInstanceName"].Trim();
            Guid wfProcessId = Guid.Parse(Request["WFProcessId"]);
            string fileIdList = Request["FileIdList"].Trim();
            string content = Request["Content"].Trim();
            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);

            WFProcessInstanceSendObject wfProcessInstanceSendObject = new WFProcessInstanceSendObject()
            {
                WFProcessId = wfProcessId,
                EntityId = entityId,
                WFProcessInstanceName = wfProcessInstanceName,
                Content = content,
                FileIdList = fileIdList,
                CreateUserId = this.UserId
            };
            IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects = new List<WFActivityInstanceSendObject>();
            int rowId = 0;
            foreach (Dictionary<string, object> row in rows)
            {
                wfActivityInstanceSendObjects.Add(new WFActivityInstanceSendObject()
                {
                    WFActivityInstanceName = row["WFActivityName"].ToString().Trim(),
                    WFActivityOperate = int.Parse(row["WFActivityOperate"].ToString()),
                    WFActivityEditorId = Guid.Parse(row["WFActivityEditorId"].ToString()),
                    WFActivityOrder = int.Parse(row["WFActivityOrder"].ToString()),
                    IsMustEdit = int.Parse(row["IsMustEdit"].ToString()),
                    RowId = ++rowId,
                    Timelimit = int.Parse(row["Timelimit"].ToString()),
                    UserId = Guid.Parse(row["UserId"].ToString())
                });
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.SendWFInstance(wfProcessInstanceSendObject, wfActivityInstanceSendObjects));
            }
            return this.Sucess("公文发送成功");
        }

        /// <summary>
        /// 审阅公文
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WFInstanceDo(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            ViewData["Id"] = id;
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                WFActivityInstanceSelectObject wfActivityInstanceSelectObject = await Task.Factory.StartNew(() => proxy.Channel.GetWFActivityInstanceById(id));
                ViewData["WFCategoryId"] = wfActivityInstanceSelectObject.WFCategoryId;
                ViewData["CreateDate"] = wfActivityInstanceSelectObject.CreateDate;
                ViewData["FullName"] = wfActivityInstanceSelectObject.FullName;
                ViewData["WFProcessInstanceName"] = wfActivityInstanceSelectObject.WFProcessInstanceName;
                ViewData["WFProcessName"] = wfActivityInstanceSelectObject.WFProcessName;
                ViewData["WFProcessInstanceCode"] = wfActivityInstanceSelectObject.WFProcessInstanceCode;
                ViewData["WFProcessInstanceContent"] = wfActivityInstanceSelectObject.WFProcessInstanceContent;
                ViewData["WFProcessInstanceId"] = wfActivityInstanceSelectObject.WFProcessInstanceId;
                ViewData["PrintUrl"] = wfActivityInstanceSelectObject.PrintUrl;
                ViewData["EditorUrl"] = wfActivityInstanceSelectObject.EditorUrl;
                ViewData["EntityId"] = wfActivityInstanceSelectObject.EntityId;
                ViewData["WFActivityInstanceId"] = wfActivityInstanceSelectObject.WFActivityInstanceId;
                ViewData["WFActivityInstanceState"] = wfActivityInstanceSelectObject.WFActivityInstanceState;
                ViewData["WFActivityEditorId"] = wfActivityInstanceSelectObject.WFActivityEditorId;
                if (wfActivityInstanceSelectObject.WFActivityOperate == 2)
                {
                    ViewData["OKVisible"] = "false";
                    ViewData["ReadVisible"] = "true";
                }
                else
                {
                    ViewData["OKVisible"] = "true";
                    ViewData["ReadVisible"] = "false";
                }
            }
            using (ServiceProxy<ICompanyService> proxy = new ServiceProxy<ICompanyService>())
            {
                IList<CompanySelectObject> companySelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedCompanys());
                ViewData["Companys"] = JsonHelper.Encode(companySelectObjects);
            }
            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["WFActivityOperate"] = JsonHelper.Encode(enumService.GetWFActivityOperateEnum());
            ViewData["WFActivityInstanceResult"] = JsonHelper.Encode(enumService.GetWFActivityInstanceResultEnum());
            ViewData["WFActivityInstanceFlow"] = JsonHelper.Encode(enumService.GetWFActivityInstanceFlowEnum("1,2,3,4"));
            ViewData["Bool"] = JsonHelper.Encode(enumService.GetBoolEnum());
            ViewData["CompanyId"] = this.CompanyId;
            return View();
        }

        /// <summary>
        /// 审阅公文处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> WFInstanceDoing()
        {
            if (Request["Id"] == null)
            {
                throw new ArgumentNullException("Id");
            }
            if (Request["WFActivityInstanceFlow"] == null)
            {
                throw new ArgumentNullException("WFActivityInstanceFlow");
            }
            if (Request["FileIdList"] == null)
            {
                throw new ArgumentNullException("FileIdList");
            }
            if (Request["Content"] == null)
            {
                throw new ArgumentNullException("Content");
            }
            if (Request["data"] == null)
            {
                throw new ArgumentNullException("data");
            }

            WFActivityInstanceDoObject wfActivityInstanceDoObject = new WFActivityInstanceDoObject()
            {
                Id = Guid.Parse(Request["Id"]),
                WFActivityInstanceFlow = int.Parse(Request["WFActivityInstanceFlow"]),
                Content = Request["Content"].Trim(),
                FileIdList = Request["FileIdList"].Trim()
            };
            IList<WFActivityInstanceSendObject> wfActivityInstanceSendObjects = new List<WFActivityInstanceSendObject>();
            int rowId = 0;
            ArrayList rows = (ArrayList)JsonHelper.Decode(Request["data"]);
            foreach (Dictionary<string, object> row in rows)
            {
                wfActivityInstanceSendObjects.Add(new WFActivityInstanceSendObject()
                {
                    WFActivityInstanceName = row["WFActivityName"].ToString().Trim(),
                    WFActivityOperate = int.Parse(row["WFActivityOperate"].ToString()),
                    WFActivityEditorId = Guid.Parse(row["WFActivityEditorId"].ToString()),
                    IsMustEdit = int.Parse(row["IsMustEdit"].ToString()),
                    RowId = ++rowId,
                    Timelimit = 24,
                    UserId = Guid.Parse(row["UserId"].ToString())
                });
            }
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                await Task.Factory.StartNew(() => proxy.Channel.DoWFInstance(wfActivityInstanceDoObject, wfActivityInstanceSendObjects));
            }
            return this.Sucess("公文处理成功");
        }

        /// <summary>
        /// 获取待办公文
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFInstancesToDo()
        {
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFInstancesToDo(this.UserId));
            }
        }

        /// <summary>
        /// 获取待办任务
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTaskToDo()
        {
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTaskToDo(this.UserId));
            }
        }

        /// <summary>
        /// 获取待办任务(移动端)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTaskToDoMobile()
        {
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetTaskToDoMobile(this.UserId));
            }
        }

        /// <summary>
        /// 获取报表列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetReports()
        {
            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetReports(this.UserId));
            }
        }

        /// <summary>
        /// 获取公文流程步骤
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFActivityInstances()
        {
            if (Request["WFProcessInstanceId"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceId");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFActivityInstances(Guid.Parse(Request["WFProcessInstanceId"])));
            }
        }

        #endregion

        #region 我的公文

        /// <summary>
        /// 我的公文
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MyWFInstance()
        {
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                IList<WFProcessSelectObject> wfProcessSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFProcesses());
                wfProcessSelectObjects.Insert(0, new WFProcessSelectObject() { Id = Guid.Empty, WFProcessName = "全部" });
                ViewData["WFProcesses"] = JsonHelper.Encode(wfProcessSelectObjects);
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            ViewData["WFProcessInstanceState"] = JsonHelper.Encode(enumService.GetWFProcessInstanceStateEnum());
            ViewData["WFActivityOperate"] = JsonHelper.Encode(enumService.GetWFActivityOperateEnum());
            ViewData["WFActivityInstanceResult"] = JsonHelper.Encode(enumService.GetWFActivityInstanceResultEnum());
            IList<Dictionary<string, string>> enumWFProcessInstanceStateByAllList = enumService.GetWFProcessInstanceStateEnum("3,4");
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumWFProcessInstanceStateByAllList.Insert(0, allDict);
            ViewData["WFProcessInstanceStateByAll"] = JsonHelper.Encode(enumWFProcessInstanceStateByAllList);
            return View();
        }

        /// <summary>
        /// 获取分页待处理公文列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMyWFInstancesDoingPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["WFProcessInstanceCode"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceCode");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFInstancesDoingPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["WFProcessInstanceCode"].Trim(), Request["WFProcessInstanceName"].Trim(),
                    Guid.Parse(Request["WFProcessId"]), this.UserId));
            }
        }

        /// <summary>
        /// 获取分页已处理公文列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMyWFInstancesDoedPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["WFProcessInstanceCode"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceCode");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFInstancesDoedPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["WFProcessInstanceCode"].Trim(), Request["WFProcessInstanceName"].Trim(),
                    Guid.Parse(Request["WFProcessId"]), this.UserId));
            }
        }

        /// <summary>
        /// 获取分页已发送公文(待处理)列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMyWFInstancesSendedToDoingPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["WFProcessInstanceCode"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceCode");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFInstancesSendedToDoingPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["WFProcessInstanceCode"].Trim(), Request["WFProcessInstanceName"].Trim(),
                    Guid.Parse(Request["WFProcessId"]), this.UserId));
            }
        }

        /// <summary>
        /// 获取分页已发送公文(已处理)列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMyWFInstancesSendedToDoedPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["WFProcessInstanceCode"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceCode");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }
            if (Request["WFProcessInstanceState"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceState");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFInstancesSendedToDoedPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["WFProcessInstanceCode"].Trim(), Request["WFProcessInstanceName"].Trim(),
                    Guid.Parse(Request["WFProcessId"]), int.Parse(Request["WFProcessInstanceState"]), this.UserId));
            }
        }

        #endregion

        #region 公文查询

        /// <summary>
        /// 公文查询
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> WFInstanceQuery()
        {
            using (ServiceProxy<IWFProcessService> proxy = new ServiceProxy<IWFProcessService>())
            {
                IList<WFProcessSelectObject> wfProcessSelectObjects = await Task.Factory.StartNew(() => proxy.Channel.GetUsedWFProcesses());
                wfProcessSelectObjects.Insert(0, new WFProcessSelectObject() { Id = Guid.Empty, WFProcessName = "全部" });
                ViewData["WFProcesses"] = JsonHelper.Encode(wfProcessSelectObjects);
            }

            IEnumService enumService = ServiceLocator.Instance.GetService<IEnumService>();
            IList<Dictionary<string, string>> enumWFProcessInstanceStateList = enumService.GetWFProcessInstanceStateEnum("2,3,4");

            ViewData["WFProcessInstanceState"] = JsonHelper.Encode(enumWFProcessInstanceStateList);
            Dictionary<string, string> allDict = new Dictionary<string, string>(2);
            allDict.Add("id", "0");
            allDict.Add("text", "全部");
            enumWFProcessInstanceStateList.Insert(0, allDict);
            ViewData["WFProcessInstanceStateByAll"] = JsonHelper.Encode(enumWFProcessInstanceStateList);
            return View();
        }

        /// <summary>
        /// 获取分页公文列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWFProcessInstancesPage()
        {
            if (Request["PageIndex"] == null)
            {
                throw new ArgumentNullException("PageIndex");
            }
            if (Request["PageSize"] == null)
            {
                throw new ArgumentNullException("PageSize");
            }
            if (Request["BeginDate"] == null)
            {
                throw new ArgumentNullException("BeginDate");
            }
            if (Request["EndDate"] == null)
            {
                throw new ArgumentNullException("EndDate");
            }
            if (Request["WFProcessInstanceCode"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceCode");
            }
            if (Request["WFProcessInstanceName"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceName");
            }
            if (Request["WFProcessId"] == null)
            {
                throw new ArgumentNullException("WFProcessId");
            }
            if (Request["WFProcessInstanceState"] == null)
            {
                throw new ArgumentNullException("WFProcessInstanceState");
            }
            if (Request["CreateUserId"] == null)
            {
                throw new ArgumentNullException("CreateUserId");
            }

            using (ServiceProxy<IWFInstanceService> proxy = new ServiceProxy<IWFInstanceService>())
            {
                return await Task.Factory.StartNew(() => proxy.Channel.GetWFProcessInstancesPage(int.Parse(Request["PageIndex"]), int.Parse(Request["PageSize"]),
                    DateTime.Parse(Request["BeginDate"]), DateTime.Parse(Request["EndDate"]).AddDays(1), Request["WFProcessInstanceCode"].Trim(), Request["WFProcessInstanceName"].Trim(),
                    Guid.Parse(Request["WFProcessId"]), int.Parse(Request["WFProcessInstanceState"]), Guid.Parse(Request["CreateUserId"])));
            }
        }

        #endregion
    }
}